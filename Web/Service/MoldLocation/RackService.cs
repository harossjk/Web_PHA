using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

using Framework;

namespace Web
{
    public class RackService : BaseService, Map.IMap
    {

        #region Cache

        public static RackList ListAll(string corpCode)
        {
            ParameterDictionary param = new ParameterDictionary()
            {
                { "corpCode", corpCode }
            };

            return new RackList(DataContext.StringEntityList<RackEntity>("@Mold.MoldList", param));
        }

        public static RackList ListAllCache(string corpCode)
        {

            var list = UtilEx.FromCache(
                "MOLD",
                DateTime.Now.AddMinutes(GetCacheMin()),
                ListAll,
                corpCode
             );

            return list;
        }

        public static void RemoveCache(string corpCode)
        {
            UtilEx.RemoveCache("MOLD");
        }

        public static RackList List(string corpCode)
        {
            return ListAllCache(corpCode);
        }

        #endregion

        public static Dictionary<string, List<RackEntity>> Select()
        {
            List<RackEntity> list = DataContext.StringEntityList<RackEntity>("@Rack.RackSelect", new HybridDictionary());
            var grop = list.GroupBy(x => x.LocationName);
            Dictionary<string, List<RackEntity>> dicRackS = grop.ToDictionary(x=>x.Key, x=>x.ToList());

            foreach (string key in dicRackS.Keys)
            {
                List<RackEntity> lstRack = dicRackS[key];
                List<string> barcodeS = lstRack.Select(x => x.Barcode.Substring(0,3)).ToList();
                var barcodeGroup = barcodeS.GroupBy(x => x, (key, value) => new { RackName = key, Count = value.Count() });

                foreach (RackEntity rack in lstRack)
                {
                    string sSubBarcode = rack.Barcode.Substring(0, 3);
                    var rackObj = barcodeGroup.ToList().Find(x => x.RackName == sSubBarcode);
                    if (rackObj != null)
                        rack.Floor = rackObj.Count.ToString();
                }
            }

            return dicRackS;
        }

        public static List<MoldPosEntity> SelectOne()
        {
        
            List<MoldPosEntity> list = DataContext.StringEntityList<MoldPosEntity>("@Rack.SelectMoldLocationInfo", new HybridDictionary());
            return new List<MoldPosEntity>(list);
        }

        public static Dictionary<string, MachineEntity> SelectMachine()
        {
            List<MachineEntity> list = DataContext.StringEntityList<MachineEntity>("@Rack.SelectMachine", new HybridDictionary());
            Dictionary<string, MachineEntity> dicMachineS = new Dictionary<string, MachineEntity>();
            for (int index = 0; index < list.Count; index++)
            {
                if(list[index].MachineType== "LinkI")
                {
                    string key = "mId_h_"+list[index].MachineId;
                    list[index].MachineType = "Horizontal";
                    dicMachineS.Add(key, list[index]);
                }
                else
                {
                    string key = "mId_v_" + list[index].MachineId;
                    list[index].MachineType = "Vertical";
                    dicMachineS.Add(key, list[index]);
                }
            }

            return dicMachineS;
        }

        public static Dictionary<string, LocationEntity> SelectLocation()
        {
            List<LocationEntity> list = DataContext.StringEntityList<LocationEntity>("@Rack.SelectLocation", new HybridDictionary());
            Dictionary<string, LocationEntity> dicLocationS = new Dictionary<string, LocationEntity>();
            list = list.FindAll(x => x.Barcode != null).ToList();

            foreach (LocationEntity item in list)
            {
                if (item.Barcode != null)
                {
                   string sID = item.LocationName.Replace("호", "");


                    string sHeader = item.Barcode.Substring(0, 1);
                    if (!string.IsNullOrEmpty(sHeader))
                    {
                        if (sHeader == "H")
                        {
                            item.LocationLegend = "수평 " + item.LocationName + "기";
                            string key = "mId_h_" + sID;
                            dicLocationS.Add(key, item);
                        }
                        else if (sHeader == "V")
                        {
                            item.LocationLegend = "수직 " + item.LocationName + "기";
                            string key = "mId_v_" + sID;
                            dicLocationS.Add(key, item);
                        }
                        else if (item.Barcode == "MOLD")
                        {
                            item.LocationLegend = item.LocationName;
                            dicLocationS.Add("MOLD", item);
                        }
                    }
                }
            }

            return dicLocationS;
        }

        #region Category
        public static Map GetMap(string corpCode, string category = null)
        {
            //RackList molds = ListAllCache(corpCode);
            //return ListAll(corpCode).AsEnumerable().Select(y =>
            //{
            //    return new MapEntity(y.Barcode, y.BarcodeName, category, 'Y');
            //}).ToMap();
            return null;
        }

        Map Map.IMap.GetMap(string corpCode, string category)
        {
            return GetMap(corpCode, category);
        }

        public static void RefreshMap(string corpCode)
        {
        }

        void Map.IMap.RefreshMap(string corpCode)
        {
            RefreshMap(corpCode);
        }
        #endregion
    }
}
