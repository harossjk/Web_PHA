using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Text.Json;

namespace Framework
{
	public static class BaseListEx
	{
		public static int TotalCnt<T>(this IEnumerable<T> list)
		{
			if (list.Count() <= 0)
				return 0;

			if (typeof(T).BaseType == typeof(BaseEntity))
				return (list.First<T>() as BaseEntity).TotalCount;

			return 0;
		}
	}

	[Serializable]
	public class BaseEntity
	{
		public BaseEntity()
		{
		}

		[XmlIgnore]
		[JsonIgnore]
		public int TotalCount { get; set; }

		public string ToDate8(DateTime dt)
		{
			return dt.ToString("yy.MM.dd");
		}

		public string ToDate10(DateTime dt)
		{
			return dt.ToString("yyyy-MM-dd");
		}

		public string ToDate10Local(DateTime dt)
		{
			return dt.ToString("yyyy-MM-dd");
		}

		public string ToDate10(DateTime? dt)
		{
			if (dt == null)
				return string.Empty;

			return dt.Value.ToString("yyyy-MM-dd");
		}

		public string ToDate16(DateTime dt)
		{
			return dt.ToString("yyyy-MM-dd HH:mm");
		}

		public string ToDate16(DateTime? dt)
		{
			if (dt == null)
				return string.Empty;

			return dt.Value.ToString("yyyy-MM-dd HH:mm");
		}

		public string ToDate19(DateTime dt)
		{
			return dt.ToString("yyyy-MM-dd HH:mm:ss");
		}

		public string ToDate19(DateTime? dt)
		{
			if (dt == null)
				return string.Empty;

			return dt.Value.ToString("yyyy-MM-dd HH:mm:ss");
		}

		public override string ToString()
		{
			return JsonSerializer.Serialize(this);
		}

		protected string Len(string s, int l)
		{
			if (string.IsNullOrWhiteSpace(s)) return string.Empty;

			return UtilEx.RemoveHtml(s).Length(l);
		}

		protected string ToText(string value)
		{
			if (string.IsNullOrWhiteSpace(value))
				return value;

			return UtilEx.RemoveHtml(value.Replace("\0", string.Empty));
		}

		protected string ToHtml(string value)
		{
			if (string.IsNullOrWhiteSpace(value))
				return value;

			return value.Replace("\r\n", "<br />").Replace("\n", "<br />");
		}

		protected XDocument ToXml(string xml)
		{
			if (string.IsNullOrWhiteSpace(xml))
				return null;

			return XDocument.Parse(xml);
		}

		protected string FromXml(XDocument xDoc)
		{
			if(xDoc.IsEmpty())
				return null;

			return xDoc.ToString();
		}

		[XmlIgnore]
		[JsonIgnore]
		public object ReturnValue { get; set; }

		public T TypeReturn<T>() {
			return ConvertEx.ConvertTo<T>(ReturnValue, default(T));
		}
	}

	[Serializable()]
	public class SearchEntity : BaseEntity
	{
		public SearchEntity()
		{
			this.Page       = 1;
			this.PageSize   = 10;
			this.SearchKey  = string.Empty;
			this.SearchVal  = string.Empty;
			this.SortKey    = string.Empty;
			this.IsDesc     = true;
			this.CacheMin   = 5;
		}

		public SearchEntity(int page, int pageSize)
		{
			this.Page      = page;
			this.PageSize  = pageSize;
			this.SearchKey  = string.Empty;
			this.SearchVal  = string.Empty;
			this.SortKey    = string.Empty;
			this.IsDesc     = true;
			this.CacheMin   = 5;
		}

		public SearchEntity(int page, int pageSize, string searchKey, string searchVal)
		{
			this.Page       = page;
			this.PageSize   = pageSize;
			this.SearchKey  = searchKey;
			this.SearchVal  = searchVal;
			this.SortKey    = string.Empty;
			this.IsDesc     = true;
			this.CacheMin   = 5;
		}

		public int Page { get; set; }

		public int PageSize { get; set; }

		public string SearchKey { get; set; }

		public string SearchVal { get; set; }

		public string SortKey { get; set; }

		public bool IsDesc { get; set; }

		public int CacheMin { get; set; }
	}

	[Serializable()]
	public class PermissionEntity : BaseEntity
	{
		public char Type { get; set; }

		[XmlIgnore]
		public string TypeName
		{
			get
			{
				switch (Type)
				{
					case 'G':
						return "Group";
					case 'D':
						return "Dept";
				}

				return "User";					
			}
		}

		public string Item { get; set; }

		public string Name { get; set; }
	}
}