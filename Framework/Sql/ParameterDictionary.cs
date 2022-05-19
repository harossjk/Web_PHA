using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Practices.EnterpriseLibrary.Common.Utility;

namespace Framework
{
    public class ParameterDictionary : Dictionary<string, object>
    {
        public ParameterDictionary ToUpperCase()
        {
            ParameterDictionary dic = new ParameterDictionary();

            this.ForEach(x => dic[UtilEx.ToUpper(x.Key)] = this[x.Key]);
            
            return dic;
        }
    }
}
