using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Framework
{
    public class FileSqlCache : ISqlCache
    {
        string _path;
        IDictionary<string, string> _sqlDic;

        public FileSqlCache()
        {

        }

        public FileSqlCache(string path)
        {
            _path = path;

            LoadAllSql();
        }

        public void LoadAllSql()
        {
            _sqlDic = new Dictionary<string, string>();

            var fileList = Directory.GetFiles(_path, "*.sql", SearchOption.AllDirectories);

            foreach (var file in fileList)
            {
                var regex = new Regex(Regex.Escape($"{_path}\\"));
                string subPath = regex.Replace(file, string.Empty, 1);

                var pathList = subPath.Split(Path.DirectorySeparatorChar);

                pathList[pathList.Length - 1] = Path.GetFileNameWithoutExtension(pathList[pathList.Length - 1]);
                var sqlId = string.Join(".", pathList);
                var sqlString = File.ReadAllText(file);

                _sqlDic.Add(sqlId, sqlString);
            }
        }

        IDictionary<string, string> ISqlCache.GetAllSql()
        {
            return _sqlDic;
        }

        string ISqlCache.GetSingleSql(string sqlId)
        {
            if (!_sqlDic.ContainsKey(sqlId))
                throw new ApplicationException($"sqlId:{sqlId} not found");

            return _sqlDic[sqlId];
        }

        void ISqlCache.RefreshAllSql()
        {
            LoadAllSql();
        }

        void ISqlCache.RefreshSingleSql(string sqlId)
        {
            throw new NotImplementedException();
        }
    }
}
