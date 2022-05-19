using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Reflection;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.Globalization;
using System.Collections.Specialized;
using System.Runtime.Caching;

using ProtoBuf;

namespace Framework
{
	public static class ExtensionEx
	{
		#region Var

		static private readonly List<Tuple<string, string>> RefineList
			= new List<Tuple<string, string>>()
		{
			{ new Tuple<string, string>("'", @"\'") }
		,   { new Tuple<string, string>("\"", "\\\"") }
		,   { new Tuple<string, string>("\r\n", @"\r\n") }
		};

		#endregion


		#region Type

		static public T TypeKey<T>(this IDictionary dic, object k, T d = default(T))
		{
			return ConvertEx.ConvertTo(dic[k], d);
		}

		static public TResult TypeKey<TKey, TValue, TResult>(this IDictionary<TKey, TValue> dic, TKey k, TResult d = default(TResult))
		{
			return ConvertEx.ConvertTo(dic[k], d);
		}

		static public T SafeTypeKey<T>(this IDictionary dic, object k, T d = default(T))
		{
			if (!dic.Contains(k) || dic[k] == null)
				return d;

			return ConvertEx.ConvertTo(dic[k], d);
		}

		static public TResult SafeTypeKey<TKey, TValue, TResult>(this IDictionary<TKey, TValue> dic, TKey k, TResult d = default(TResult))
		{
			if (!dic.ContainsKey(k) || dic[k] == null)
				return d;

			return ConvertEx.ConvertTo(dic[k], d);
		}

		//static public T TypeKey<T>(this AttributeCollection dic, string k, T d = default(T))
		//{
		//	return ConvertEx.ConvertTo(dic[k], d);
		//}

		public static T TypeKey<T>(this MemoryCache dic, string k, T d = default(T))
        {
            return ConvertEx.ConvertTo(dic[k], d);
        }

        //public static T TypeKey<T>(this HttpCookieCollection dic, string k, T d = default(T))
        //{
        //	return ConvertEx.ConvertTo(dic[k], d);
        //}

        static public T TypeCol<T>(this DataRow row, string k, T d = default(T))
		{
			return ConvertEx.ConvertTo(row[k], d);
		}

		static public T TypeColUpper<T>(this DataRow row, string k, T d = default(T))
		{
			return ConvertEx.ConvertTo(row[UtilEx.ToUpper(k)], d);
		}

		static public T TypeCol<T>(this DataRowView row, string k, T d = default(T))
		{
			return ConvertEx.ConvertTo(row[k], d);
		}

		static public T TypeVal<T>(this NameValueCollection n, string k, T d = default(T))
		{
			return ConvertEx.ConvertTo(n[k], d);
		}

		//static public T TypeKey<T>(this HttpCookie c, T d = default(T))
		//{
		//	if (c == null)
		//		return d;

		//	return ConvertEx.ConvertTo<T>(c.Value, d);
		//}

		#endregion


		#region Safe

		static public T SafeKey<T>(this IDictionary dic, object k, T d = default(T))
		{
			if (!dic.Contains(k)) return d;

			return ConvertEx.ConvertTo(dic[k], d);
		}

		#endregion


		#region List, Dictionary

		static public T SafeIndex<T>(this IList<T> list, int i, T d = default(T))
		{
			if (list.Count <= i)
				return d;

			return list[i];
		}

		static public bool IsEmpty<T>(this IList<T> list)
		{
			return list == null || list.Count <= 0;
		}

		static public bool IsEmpty<T, U>(this IDictionary<T, U> dic)
		{
			return dic == null || dic.Count <= 0;
		}

		static public bool IntersectAny(this IList<string> list1, IList<string> list2)
		{
			return list1.Any(x => list2.Contains(x));
		}

		public static T DeepClone<T>(this T original) where T : class
		{
			return Serializer.DeepClone(original);
		}

		public static int GetTotalCount<T>(this IEnumerable<T> list)
		{
			if (list.Count() <= 0)
				return 0;

			var entity = list.First();
			if (entity is BaseEntity)
				return (entity as BaseEntity).TotalCount;

			return 0;
		}

		public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
		{
			HashSet<TKey> seenKeys = new HashSet<TKey>();
			foreach (TSource element in source)
			{
				if (seenKeys.Add(keySelector(element)))
				{
					yield return element;
				}
			}
		}

		#endregion


		#region ETC

		static public PropertyInfo GetPublic(this Type type, string name)
		{
			return type.GetProperty(name, BindingFlags.Instance | BindingFlags.Public);
		}

		static public PropertyInfo[] GetPublic(this Type type)
		{
			return type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
		}
		
		public static string Length(this string v, int l, string postfix = "")
		{
			if (string.IsNullOrWhiteSpace(v))
				return string.Empty;

			int n = 0;
			string s = string.Empty;

			foreach (char ch in v)
			{
				if (IsUnicode(ch))
					n += 2;
				else
					n++;
				s += ch;
				if (n >= l)
					return (s + postfix);
			}

			return s;
		}

		public static bool IsUnicode(char c)
		{
			return (char.GetUnicodeCategory(c) == UnicodeCategory.OtherLetter);
		}

		public static bool IsEmpty(this DateTime dt)
		{
			return dt == DateTime.MinValue || dt == DateTime.MaxValue || dt == default(DateTime);
		}

		#endregion


		#region Tuple

		#region Tuple#2

		static public T1 Select1<T1, T2>(this Tuple<List<T1>, List<T2>> tuple)
		{
			if (tuple.Item1.Count > 0)
				return tuple.Item1[0];
			else
				return default(T1);
		}

		static public T2 Select2<T1, T2>(this Tuple<List<T1>, List<T2>> tuple)
		{
			if (tuple.Item2.Count > 0)
				return tuple.Item2[0];
			else
				return default(T2);
		}

		#endregion

		#region Tuple#3

		static public T1 Select1<T1, T2, T3>(this Tuple<List<T1>, List<T2>, List<T3>> tuple)
		{
			if (tuple.Item1.Count > 0)
				return tuple.Item1[0];
			else
				return default(T1);
		}

		static public T2 Select2<T1, T2, T3>(this Tuple<List<T1>, List<T2>, List<T3>> tuple)
		{
			if (tuple.Item2.Count > 0)
				return tuple.Item2[0];
			else
				return default(T2);
		}

		static public T3 Select3<T1, T2, T3>(this Tuple<List<T1>, List<T2>, List<T3>> tuple)
		{
			if (tuple.Item3.Count > 0)
				return tuple.Item3[0];
			else
				return default(T3);
		}

		#endregion

		#region Tuple#4

		static public T1 Select1<T1, T2, T3, T4>(this Tuple<List<T1>, List<T2>, List<T3>, List<T4>> tuple)
		{
			if (tuple.Item1.Count > 0)
				return tuple.Item1[0];
			else
				return default(T1);
		}

		static public T2 Select2<T1, T2, T3, T4>(this Tuple<List<T1>, List<T2>, List<T3>, List<T4>> tuple)
		{
			if (tuple.Item2.Count > 0)
				return tuple.Item2[0];
			else
				return default(T2);
		}

		static public T3 Select3<T1, T2, T3, T4>(this Tuple<List<T1>, List<T2>, List<T3>, List<T4>> tuple)
		{
			if (tuple.Item3.Count > 0)
				return tuple.Item3[0];
			else
				return default(T3);
		}

		static public T4 Select4<T1, T2, T3, T4>(this Tuple<List<T1>, List<T2>, List<T3>, List<T4>> tuple)
		{
			if (tuple.Item4.Count > 0)
				return tuple.Item4[0];
			else
				return default(T4);
		}

		#endregion

		#region Tuple#5

		static public T1 Select1<T1, T2, T3, T4, T5>(this Tuple<List<T1>, List<T2>, List<T3>, List<T4>, List<T5>> tuple)
		{
			if (tuple.Item1.Count > 0)
				return tuple.Item1[0];
			else
				return default(T1);
		}

		static public T2 Select2<T1, T2, T3, T4, T5>(this Tuple<List<T1>, List<T2>, List<T3>, List<T4>, List<T5>> tuple)
		{
			if (tuple.Item2.Count > 0)
				return tuple.Item2[0];
			else
				return default(T2);
		}

		static public T3 Select3<T1, T2, T3, T4, T5>(this Tuple<List<T1>, List<T2>, List<T3>, List<T4>, List<T5>> tuple)
		{
			if (tuple.Item3.Count > 0)
				return tuple.Item3[0];
			else
				return default(T3);
		}

		static public T4 Select4<T1, T2, T3, T4, T5>(this Tuple<List<T1>, List<T2>, List<T3>, List<T4>, List<T5>> tuple)
		{
			if (tuple.Item4.Count > 0)
				return tuple.Item4[0];
			else
				return default(T4);
		}

		static public T5 Select5<T1, T2, T3, T4, T5>(this Tuple<List<T1>, List<T2>, List<T3>, List<T4>, List<T5>> tuple)
		{
			if (tuple.Item5.Count > 0)
				return tuple.Item5[0];
			else
				return default(T5);
		}

		#endregion

		#region Tuple#6

		static public T1 Select1<T1, T2, T3, T4, T5, T6>(this Tuple<List<T1>, List<T2>, List<T3>, List<T4>, List<T5>, List<T6>> tuple)
		{
			if (tuple.Item1.Count > 0)
				return tuple.Item1[0];
			else
				return default(T1);
		}

		static public T2 Select2<T1, T2, T3, T4, T5, T6>(this Tuple<List<T1>, List<T2>, List<T3>, List<T4>, List<T5>, List<T6>> tuple)
		{
			if (tuple.Item2.Count > 0)
				return tuple.Item2[0];
			else
				return default(T2);
		}

		static public T3 Select3<T1, T2, T3, T4, T5, T6>(this Tuple<List<T1>, List<T2>, List<T3>, List<T4>, List<T5>, List<T6>> tuple)
		{
			if (tuple.Item3.Count > 0)
				return tuple.Item3[0];
			else
				return default(T3);
		}

		static public T4 Select4<T1, T2, T3, T4, T5, T6>(this Tuple<List<T1>, List<T2>, List<T3>, List<T4>, List<T5>, List<T6>> tuple)
		{
			if (tuple.Item4.Count > 0)
				return tuple.Item4[0];
			else
				return default(T4);
		}

		static public T5 Select5<T1, T2, T3, T4, T5, T6>(this Tuple<List<T1>, List<T2>, List<T3>, List<T4>, List<T5>, List<T6>> tuple)
		{
			if (tuple.Item5.Count > 0)
				return tuple.Item5[0];
			else
				return default(T5);
		}

		static public T6 Select6<T1, T2, T3, T4, T5, T6>(this Tuple<List<T1>, List<T2>, List<T3>, List<T4>, List<T5>, List<T6>> tuple)
		{
			if (tuple.Item6.Count > 0)
				return tuple.Item6[0];
			else
				return default(T6);
		}

		#endregion

		#region Tuple#7

		static public T1 Select1<T1, T2, T3, T4, T5, T6, T7>(this Tuple<List<T1>, List<T2>, List<T3>, List<T4>, List<T5>, List<T6>, List<T7>> tuple)
		{
			if (tuple.Item1.Count > 0)
				return tuple.Item1[0];
			else
				return default(T1);
		}

		static public T2 Select2<T1, T2, T3, T4, T5, T6, T7>(this Tuple<List<T1>, List<T2>, List<T3>, List<T4>, List<T5>, List<T6>, List<T7>> tuple)
		{
			if (tuple.Item2.Count > 0)
				return tuple.Item2[0];
			else
				return default(T2);
		}

		static public T3 Select3<T1, T2, T3, T4, T5, T6, T7>(this Tuple<List<T1>, List<T2>, List<T3>, List<T4>, List<T5>, List<T6>, List<T7>> tuple)
		{
			if (tuple.Item3.Count > 0)
				return tuple.Item3[0];
			else
				return default(T3);
		}

		static public T4 Select4<T1, T2, T3, T4, T5, T6, T7>(this Tuple<List<T1>, List<T2>, List<T3>, List<T4>, List<T5>, List<T6>, List<T7>> tuple)
		{
			if (tuple.Item4.Count > 0)
				return tuple.Item4[0];
			else
				return default(T4);
		}

		static public T5 Select5<T1, T2, T3, T4, T5, T6, T7>(this Tuple<List<T1>, List<T2>, List<T3>, List<T4>, List<T5>, List<T6>, List<T7>> tuple)
		{
			if (tuple.Item5.Count > 0)
				return tuple.Item5[0];
			else
				return default(T5);
		}

		static public T6 Select6<T1, T2, T3, T4, T5, T6, T7>(this Tuple<List<T1>, List<T2>, List<T3>, List<T4>, List<T5>, List<T6>, List<T7>> tuple)
		{
			if (tuple.Item6.Count > 0)
				return tuple.Item6[0];
			else
				return default(T6);
		}

		static public T7 Select7<T1, T2, T3, T4, T5, T6, T7>(this Tuple<List<T1>, List<T2>, List<T3>, List<T4>, List<T5>, List<T6>, List<T7>> tuple)
		{
			if (tuple.Item7.Count > 0)
				return tuple.Item7[0];
			else
				return default(T7);
		}

		#endregion

		#endregion


		#region XDocument

		public static void AddItem(this XDocument xDoc, string content, string itemName = "ITEM")
		{
			XElement r = xDoc.Root;

			XElement item = new XElement(itemName, content);

			r.Add(item);
		}

		public static void AddEntity<T>(this XDocument xDoc, BaseEntity entity, string cdataCol, string itemName = "ITEM")
		{
			var properties =
				from property in typeof(T).GetPublic()
				where DatabaseEx.IsAutoMappableProperty(property) && !property.GetCustomAttributes(false).Any(x => x is XmlIgnoreAttribute)
				select property;

			XElement r = xDoc.Root;

			XElement item = new XElement(itemName);

			foreach (var pi in properties)
			{
				object val = pi.GetValue(entity);
				if (val == null)
					continue;

				if (cdataCol == pi.Name)
					item.Add(new XCData(val.ToString()));
				else
					item.Add(new XAttribute(UtilEx.ToUpper(pi.Name), val));
			}

			r.Add(item);
		}

		public static bool IsEmpty(this XDocument xDoc)
		{
			return xDoc == null || xDoc.Root == null || xDoc.Root.IsEmpty;
		}

		public static T TypeVal<T>(this XElement x, T d = default(T))
		{
			if (x == null) return d;
			return ConvertEx.ConvertTo<T>(x.Value, d);
		}

		static public T AttrVal<T>(this XElement x, string k, T d = default(T))
		{
			var t = x.Attribute(k);
			if (t == null || string.IsNullOrWhiteSpace(t.Value)) return d;

			return ConvertEx.ConvertTo<T>(t.Value, d);
		}

		public static T PathVal<T>(this XElement x, string exp, T d = default(T))
		{
			XElement xe = x.XPathSelectElement(exp);
			if (xe == null) return d;

			return xe.TypeVal<T>(d);
		}

		public static T PathAttrVal<T>(this XElement x, string exp, string n, T d = default(T))
		{
			XElement xe = x.XPathSelectElement(exp);
			if (xe == null) return d;

			return xe.AttrVal<T>(n, d);
		}

		#endregion


		#region DataSet

		static public List<Dictionary<string, object>> ToDic(this DataTable dt, Func<string, string> columnNameFunc)
		{
			List<Dictionary<string, object>> rtn = new List<Dictionary<string, object>>();

			foreach (DataRow row in dt.Rows)
			{
				Dictionary<string, object> dic = new Dictionary<string, object>();

				foreach (DataColumn col in dt.Columns)
				{
					dic.Add(columnNameFunc(col.ColumnName), row[col]);
				}

				rtn.Add(dic);
			}

			return rtn;
		}

		#endregion
	}
}
