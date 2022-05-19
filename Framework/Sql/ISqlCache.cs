using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Framework
{
    public interface ISqlCache
    {
        string GetSingleSql(string sqlId);

        IDictionary<string, string> GetAllSql();

        void RefreshSingleSql(string sqlId);

        void RefreshAllSql();
    }
}
