using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace Framework
{
    public class BaseService
    {
        static readonly string _cachePrefix = "_cache";
        static readonly int _cacheMin = 10;

        static public string BuildCacheKey(string userContext, [CallerFilePath] string filePath = "", [CallerMemberName] string memberName = "")
        {
            return $"{_cachePrefix}_{Path.GetFileNameWithoutExtension(filePath)}_{memberName}_{userContext}";
        }

        static public int GetCacheMin(int cacheMin = -1)
        {
            if (cacheMin >= 0)
                return cacheMin;

            return _cacheMin;
        }
    }
}
