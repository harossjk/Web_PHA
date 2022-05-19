using Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Framework
{
    public class SqlPhraseEx
    {
        public SqlPhraseEx()
        {
        }

        public static string BuildSql(string sql, ParameterDictionary parameterDic = null)
        {
            if (parameterDic == null)
                return sql;

            var keyList = parameterDic.ToUpperCase().Keys.Cast<string>().ToList();

            if (keyList.Count <= 0)
                return sql;

            var lines = SplitByLine(sql);

            List<string> sqlList = new List<string>();

            Action<List<string>, string> safeAdd = (sqlList, line) =>
            {
                if (sqlList.Count <= 0)
                {
                    sqlList.Add(line);
                    return;
                }

                string last = sqlList[sqlList.Count - 1];

                switch (last.ToUpper())
                {
                    case "SET":
                        if (line.Trim().StartsWith(","))
                        {
                            var regex = new Regex(Regex.Escape(","));
                            line = regex.Replace(line, string.Empty, 1);
                        }
                        break;
                    case "WHERE":
                        if (line.Trim().StartsWith("AND"))
                        {
                            var regex = new Regex(Regex.Escape("AND"));
                            line = regex.Replace(line, string.Empty, 1);
                        }
                        break;
                    default:
                        break;
                }

                sqlList.Add(line);
            };

            foreach (string line in lines)
            {
                var paramList = DatabaseEx.ExtractParameters(line);
                if (paramList.Count > 0)
                {
                    if (keyList.Intersect(paramList).Count() != paramList.Count)
                        continue;
                }

                safeAdd(sqlList, line);
            }

            return string.Join(Environment.NewLine, sqlList);
        }

        public static string[] SplitByLine(string sqlString)
        {
            return Regex.Split(sqlString, "\r\n|\r|\n");
        }
    }
}
