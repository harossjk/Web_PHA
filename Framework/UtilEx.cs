using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Runtime.Caching;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using System.Collections;
using System.Collections.Specialized;
using Newtonsoft.Json.Linq;

namespace Framework
{
	public class UtilEx
	{
		static public MemoryCache CacheEx
		{
			get
			{
				return MemoryCache.Default;
			}
		}

		static public string RemoveHtml(string s)
		{
			Regex regHtml = new Regex("<[^>]*>");

			return regHtml.Replace(s, string.Empty)
				.Replace("<!--", string.Empty)
				.Replace("-->", string.Empty)
				.Replace("&nbsp;", string.Empty)
				.Replace("&nbsp", string.Empty);

		}

		static public string ToUpper(string s)
		{
			if (string.IsNullOrWhiteSpace(s))
				return string.Empty;

			return Regex.Replace(s, "(?<=.)([A-Z])", "_$0", RegexOptions.Compiled).ToUpper();
		}

		static public string ToTitle(string s)
		{
			return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(s.ToLower().Replace("_", " ")).Replace(" ", string.Empty);

		}

		static public string ToCamel(string s)
        {
			string t = ToTitle(s);
			return $"{char.ToLowerInvariant(t[0])}{t.Substring(1)}";
		}
		
		static public string SafeIndex(string s, char spt, int i, string d = null)
		{
			if (string.IsNullOrWhiteSpace(s))
				return d;

			var sp = s.Split(new char[] { spt }, StringSplitOptions.RemoveEmptyEntries);
			if (sp.Count() <= i)
				return d;

			return sp[i];
		}

		static public void RemoveCache(string key)
		{
			if (CacheEx[key] != null)
				CacheEx.Remove(key);
		}

		static public void ClearCache()
        {
			CacheEx.ForEach(x => CacheEx.Remove(x.Key));
		}

		static public R FromCache<R>(string key, DateTime expDT, Func<R> f)
		{
			if (CacheEx[key] == null)
				CacheEx.Set(key, f.Invoke(), expDT);

			return CacheEx.TypeKey<R>(key);
		}

		static public R FromCache<R, T1>(string key, DateTime expDT, Func<T1, R> f, T1 t1)
		{
			if (CacheEx[key] == null)
				CacheEx.Set(key, f.Invoke(t1), expDT);

			return CacheEx.TypeKey<R>(key);
		}

		static public R FromCache<R, T1, T2>(string key, DateTime expDT, Func<T1, T2, R> f, T1 t1, T2 t2)
		{
			if (CacheEx[key] == null)
				CacheEx.Set(key, f.Invoke(t1, t2), expDT);

			return CacheEx.TypeKey<R>(key);
		}

		static public R FromCache<R, T1, T2, T3>(string key, DateTime expDT, Func<T1, T2, T3, R> f, T1 t1, T2 t2, T3 t3)
		{
			if (CacheEx[key] == null)
				CacheEx.Set(key, f.Invoke(t1, t2, t3), expDT);

			return CacheEx.TypeKey<R>(key);
		}

		static public R FromCache<R, T1, T2, T3, T4>(string key, DateTime expDT, Func<T1, T2, T3, T4, R> f, T1 t1, T2 t2, T3 t3, T4 t4)
		{
			if (CacheEx[key] == null)
				CacheEx.Set(key, f.Invoke(t1, t2, t3, t4), expDT);

			return CacheEx.TypeKey<R>(key);
		}

		static public R FromCache<R, T1, T2, T3, T4, T5>(string key, DateTime expDT, Func<T1, T2, T3, T4, T5, R> f, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5)
		{
			if (CacheEx[key] == null)
				CacheEx.Set(key, f.Invoke(t1, t2, t3, t4, t5), expDT);

			return CacheEx.TypeKey<R>(key);
		}

		static public R FromCache<R, T1, T2, T3, T4, T5, T6>(string key, DateTime expDT, Func<T1, T2, T3, T4, T5, T6, R> f, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6)
		{
			if (CacheEx[key] == null)
				CacheEx.Set(key, f.Invoke(t1, t2, t3, t4, t5, t6), expDT);

			return CacheEx.TypeKey<R>(key);
		}

		static public R FromCache<R, T1, T2, T3, T4, T5, T6, T7>(string key, DateTime expDT, Func<T1, T2, T3, T4, T5, T6, T7, R> f, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7)
		{
			if (CacheEx[key] == null)
				CacheEx.Set(key, f.Invoke(t1, t2, t3, t4, t5, t6, t7), expDT);

			return CacheEx.TypeKey<R>(key);
		}

		static public R FromCache<R, T1, T2, T3, T4, T5, T6, T7, T8>(string key, DateTime expDT, Func<T1, T2, T3, T4, T5, T6, T7, T8, R> f, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8)
		{
			if (CacheEx[key] == null)
				CacheEx.Set(key, f.Invoke(t1, t2, t3, t4, t5, t6, t7, t8), expDT);

			return CacheEx.TypeKey<R>(key);
		}

		static public R FromCache<R, T1, T2, T3, T4, T5, T6, T7, T8, T9>(string key, DateTime expDT, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, R> f, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9)
		{
			if (CacheEx[key] == null)
				CacheEx.Set(key, f.Invoke(t1, t2, t3, t4, t5, t6, t7, t8, t9), expDT);

			return CacheEx.TypeKey<R>(key);
		}

		static public R FromCache<R, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string key, DateTime expDT, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, R> f, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10)
		{
			if (CacheEx[key] == null)
				CacheEx.Set(key, f.Invoke(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10), expDT);

			return CacheEx.TypeKey<R>(key);
		}

		static public R FromCache<R, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string key, DateTime expDT, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, R> f, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11)
		{
			if (CacheEx[key] == null)
				CacheEx.Set(key, f.Invoke(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11), expDT);

			return CacheEx.TypeKey<R>(key);
		}

		static public R FromCache<R, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string key, DateTime expDT, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, R> f, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12)
		{
			if (CacheEx[key] == null)
				CacheEx.Set(key, f.Invoke(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12), expDT);

			return CacheEx.TypeKey<R>(key);
		}

		static public R FromCache<R, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(string key, DateTime expDT, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, R> f, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13)
		{
			if (CacheEx[key] == null)
				CacheEx.Set(key, f.Invoke(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13), expDT);

			return CacheEx.TypeKey<R>(key);
		}

		static public R FromCache<R, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(string key, DateTime expDT, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, R> f, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14)
		{
			if (CacheEx[key] == null)
				CacheEx.Set(key, f.Invoke(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14), expDT);

			return CacheEx.TypeKey<R>(key);
		}

		public static XDocument XDocCreate(string root = "ROOT")
		{
			return new XDocument(new XElement(root));
		}

		public static bool IsImageExt(string ext)
		{
			if (string.IsNullOrWhiteSpace(ext))
				return false;

			ext = ext.ToLower();

			string[] extList = new string[] { ".gif", ".jpg", ".png", ".bmp" };
			return extList.Contains(ext);
		}

		public static bool IsAllowExt(string ext)
		{
			if (string.IsNullOrWhiteSpace(ext))
				return true;

			ext = ext.ToLower();

			string[] extList = new string[]{".jsp",
											".php",
											".asp",
											".aspx",
											".cs",
											".vb",
											".asmx",
											".db",
											".com",
											".bat",
											".exe",
											".vbs",
											".java",
											".config",
											".dll",
											".cmd",
											".msi",
											".cab",
											".master"};

			return !extList.Contains(ext);
		}

		public static Tuple<int, int> PrevNextCalc(int page, int pageSize)
		{
			int min = (page - 1) * pageSize;
			int max = page * pageSize + 1;

			int x = pageSize + 2;

			for (int i = x; i <= x * page; i++)
			{
				for (int j = 1; j <= page; j++)
				{
					if (i * (j - 1) < min && i * j >= max)
					{
						return new Tuple<int, int>(j, i);
					}
				}
			}

			return null;
		}
	}
}
