using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Framework
{
	public class DatabaseEx : SqlDatabase
	{
		static readonly ParameterCache parameterCache = new ParameterCache();

		static ISqlCache _sqlCache;

		public DatabaseEx(string connectionString, ISqlCache sqlCache) : base(connectionString)
		{
			_sqlCache = sqlCache;
		}

		public virtual int ExecuteNonQuery(string sp, BaseEntity entity)
		{
			using (DbCommand command = GetStoredProcCommand(sp, entity))
			{
				int rtn = ExecuteNonQuery(command);

				AssignResult(command, entity);

				return rtn;
			}
		}

		public virtual IDataReader ExecuteReaderDic(string sqlString, IDictionary parameterDic)
		{
			using (DbCommand command = GetStoredProcCommand(sqlString, parameterDic))
			{
				return ExecuteReader(command);
			}
		}

		public virtual IDataReader ExecuteReaderEntity(string sqlString, BaseEntity entity)
		{
			using (DbCommand command = GetStoredProcCommand(sqlString, entity))
			{
				return ExecuteReader(command);
			}
		}

		public virtual DataSet ExecuteStringDataSet(string sqlString, params object[] parames)
		{
			using (DbCommand command = GetSqlStringCommand(sqlString, parames))
			{
				DataSet ds = ExecuteDataSet(command);

				return ds;
			}
		}

		public virtual DataSet ExecuteStringDataSet(string sqlString, IDictionary parameterDic)
		{
			using (DbCommand command = GetSqlStringCommand(sqlString, parameterDic))
			{
				DataSet ds = ExecuteDataSet(command);

				return ds;
			}
		}

		public virtual DataSet ExecuteStringDataSet(string sqlString, BaseEntity entity)
		{
			using (DbCommand command = GetSqlStringCommand(sqlString, entity))
			{
				DataSet ds = ExecuteDataSet(command);

				return ds;
			}
		}

		public virtual int ExecuteStringNonQuery(string sqlString, params object[] parames)
		{
			using (DbCommand command = GetSqlStringCommand(sqlString, parames))
			{
				return ExecuteNonQuery(command);
			}
		}

		public virtual int ExecuteStringNonQuery(string sqlString, IDictionary parameterDic)
		{
			using (DbCommand command = GetSqlStringCommand(sqlString, parameterDic))
			{
				return ExecuteNonQuery(command);
			}
		}

		public virtual int ExecuteStringNonQuery(string sqlString, BaseEntity entity)
		{
			using (DbCommand command = GetSqlStringCommand(sqlString, entity))
			{
				return ExecuteNonQuery(command);
			}
		}



		public virtual T ExecuteStringScalar<T>(string sqlString, params object[] parames)
		{
			using (DbCommand command = GetSqlStringCommand(sqlString, parames))
			{
				return ConvertEx.ConvertTo<T>(ExecuteScalar(command), default(T));
			}
		}

		public virtual T ExecuteStringScalar<T>(string sqlString, IDictionary parameterDic)
		{
			using (DbCommand command = GetSqlStringCommand(sqlString, parameterDic))
			{
				return ConvertEx.ConvertTo<T>(ExecuteScalar(command), default(T));
			}
		}

		public virtual T ExecuteStringScalar<T>(string sqlString, BaseEntity entity)
		{
			using (DbCommand command = GetSqlStringCommand(sqlString, entity))
			{
				return ConvertEx.ConvertTo<T>(ExecuteScalar(command), default(T));
			}
		}

		public virtual IDataReader ExecuteStringReader(string sqlString, params object[] parames)
		{
			using (DbCommand command = GetSqlStringCommand(sqlString, parames))
			{
				return ExecuteReader(command);
			}
		}

		public virtual IDataReader ExecuteStringReaderDic(string sqlString, IDictionary parameterDic)
		{
			using (DbCommand command = GetSqlStringCommand(sqlString, parameterDic))
			{
				return ExecuteReader(command);
			}
		}

		public virtual IDataReader ExecuteStringReaderEntity(string sqlString, BaseEntity entity)
		{
			using (DbCommand command = GetSqlStringCommand(sqlString, entity))
			{
				return ExecuteReader(command);
			}
		}

		public List<T> EntityList<TParam, T>(CommandType commandType, string commandText, TParam parames)
			where T : new()
		{
			using (IDataReader reader = ExecuteReader(commandType, commandText, parames))
			{
				return Map<T>(reader).ToList();
			}
		}

		public Tuple<List<T1>, List<T2>> EntityList<TParam, T1, T2>(CommandType commandType, string commandText, TParam parames)
			where T1 : new()
			where T2 : new()
		{
			using (IDataReader reader = ExecuteReader(commandType, commandText, parames))
			{
				var t1 = Map<T1>(reader).ToList();
				reader.NextResult();
				var t2 = Map<T2>(reader).ToList();

				return new Tuple<List<T1>, List<T2>>(t1.ToList(), t2.ToList());
			}
		}

		public Tuple<List<T1>, List<T2>, List<T3>> EntityList<TParam, T1, T2, T3>(CommandType commandType, string commandText, TParam parames)
			where T1 : new()
			where T2 : new()
			where T3 : new()
		{
			using (IDataReader reader = ExecuteReader(commandType, commandText, parames))
			{
				var t1 = Map<T1>(reader).ToList();
				reader.NextResult();
				var t2 = Map<T2>(reader).ToList();
				reader.NextResult();
				var t3 = Map<T3>(reader).ToList();

				return new Tuple<List<T1>, List<T2>, List<T3>>(t1.ToList(), t2.ToList(), t3.ToList());
			}
		}

		public Tuple<List<T1>, List<T2>, List<T3>, List<T4>> EntityList<TParam, T1, T2, T3, T4>(CommandType commandType, string commandText, TParam parames)
			where T1 : new()
			where T2 : new()
			where T3 : new()
			where T4 : new()
		{
			using (IDataReader reader = ExecuteReader(commandType, commandText, parames))
			{
				var t1 = Map<T1>(reader).ToList();
				reader.NextResult();
				var t2 = Map<T2>(reader).ToList();
				reader.NextResult();
				var t3 = Map<T3>(reader).ToList();
				reader.NextResult();
				var t4 = Map<T4>(reader).ToList();

				return new Tuple<List<T1>, List<T2>, List<T3>, List<T4>>(t1.ToList(), t2.ToList(), t3.ToList(), t4.ToList());
			}
		}

		public Tuple<List<T1>, List<T2>, List<T3>, List<T4>, List<T5>> EntityList<TParam, T1, T2, T3, T4, T5>(CommandType commandType, string commandText, TParam parames)
			where T1 : new()
			where T2 : new()
			where T3 : new()
			where T4 : new()
			where T5 : new()
		{
			using (IDataReader reader = ExecuteReader(commandType, commandText, parames))
			{
				var t1 = Map<T1>(reader).ToList();
				reader.NextResult();
				var t2 = Map<T2>(reader).ToList();
				reader.NextResult();
				var t3 = Map<T3>(reader).ToList();
				reader.NextResult();
				var t4 = Map<T4>(reader).ToList();
				reader.NextResult();
				var t5 = Map<T5>(reader).ToList();

				return new Tuple<List<T1>, List<T2>, List<T3>, List<T4>, List<T5>>(t1.ToList(), t2.ToList(), t3.ToList(), t4.ToList(), t5.ToList());
			}
		}

		public Tuple<List<T1>, List<T2>, List<T3>, List<T4>, List<T5>, List<T6>> EntityList<TParam, T1, T2, T3, T4, T5, T6>(CommandType commandType, string commandText, TParam parames)
			where T1 : new()
			where T2 : new()
			where T3 : new()
			where T4 : new()
			where T5 : new()
			where T6 : new()
		{
			using (IDataReader reader = ExecuteReader(commandType, commandText, parames))
			{
				var t1 = Map<T1>(reader).ToList();
				reader.NextResult();
				var t2 = Map<T2>(reader).ToList();
				reader.NextResult();
				var t3 = Map<T3>(reader).ToList();
				reader.NextResult();
				var t4 = Map<T4>(reader).ToList();
				reader.NextResult();
				var t5 = Map<T5>(reader).ToList();
				reader.NextResult();
				var t6 = Map<T6>(reader).ToList();

				return new Tuple<List<T1>, List<T2>, List<T3>, List<T4>, List<T5>, List<T6>>(t1.ToList(), t2.ToList(), t3.ToList(), t4.ToList(), t5.ToList(), t6.ToList());
			}
		}

		public Tuple<List<T1>, List<T2>, List<T3>, List<T4>, List<T5>, List<T6>, List<T7>> EntityList<TParam, T1, T2, T3, T4, T5, T6, T7>(CommandType commandType, string commandText, TParam parames)
			where T1 : new()
			where T2 : new()
			where T3 : new()
			where T4 : new()
			where T5 : new()
			where T6 : new()
			where T7 : new()
		{
			using (IDataReader reader = ExecuteReader(commandType, commandText, parames))
			{
				var t1 = Map<T1>(reader).ToList();
				reader.NextResult();
				var t2 = Map<T2>(reader).ToList();
				reader.NextResult();
				var t3 = Map<T3>(reader).ToList();
				reader.NextResult();
				var t4 = Map<T4>(reader).ToList();
				reader.NextResult();
				var t5 = Map<T5>(reader).ToList();
				reader.NextResult();
				var t6 = Map<T6>(reader).ToList();
				reader.NextResult();
				var t7 = Map<T7>(reader).ToList();

				return new Tuple<List<T1>, List<T2>, List<T3>, List<T4>, List<T5>, List<T6>, List<T7>>(t1.ToList(), t2.ToList(), t3.ToList(), t4.ToList(), t5.ToList(), t6.ToList(), t7.ToList());
			}
		}

		public Tuple<List<T1>, List<T2>, List<T3>, List<T4>, List<T5>, List<T6>, List<T7>, List<List<TRest>>> EntityList<TParam, T1, T2, T3, T4, T5, T6, T7, TRest>(CommandType commandType, string commandText, TParam parames)
			where T1 : new()
			where T2 : new()
			where T3 : new()
			where T4 : new()
			where T5 : new()
			where T6 : new()
			where T7 : new()
			where TRest : new()
		{
			using (IDataReader reader = ExecuteReader(commandType, commandText, parames))
			{
				var t1 = Map<T1>(reader).ToList();
				reader.NextResult();
				var t2 = Map<T2>(reader).ToList();
				reader.NextResult();
				var t3 = Map<T3>(reader).ToList();
				reader.NextResult();
				var t4 = Map<T4>(reader).ToList();
				reader.NextResult();
				var t5 = Map<T5>(reader).ToList();
				reader.NextResult();
				var t6 = Map<T6>(reader).ToList();
				reader.NextResult();
				var t7 = Map<T7>(reader).ToList();
				reader.NextResult();
				var trest = new List<List<TRest>>();
				do
					trest.Add(Map<TRest>(reader).ToList());
				while (reader.NextResult());

				return new Tuple<List<T1>, List<T2>, List<T3>, List<T4>, List<T5>, List<T6>, List<T7>, List<List<TRest>>>(t1.ToList(), t2.ToList(), t3.ToList(), t4.ToList(), t5.ToList(), t6.ToList(), t7.ToList(), trest);
			}
		}

		public IDataReader ExecuteReader<TParam>(CommandType commandType, string commandText, TParam parames)
		{
			switch (commandType)
			{
				case CommandType.Text:
					switch (parames)
					{
						case IDictionary dic:
							return ExecuteStringReaderDic(commandText, dic);
						case BaseEntity entity:
							return ExecuteStringReaderEntity(commandText, entity);
						case object[] objects:
							return ExecuteStringReader(commandText, objects);
						default:
							return null;
					}
				case CommandType.StoredProcedure:
					switch (parames)
					{
						case IDictionary dic:
							return ExecuteReaderDic(commandText, dic);
						case BaseEntity entity:
							return ExecuteReaderEntity(commandText, entity);
						case object[] objects:
							return ExecuteReader(commandText, objects);
						default:
							return null;
					}
				case CommandType.TableDirect:
					throw new ApplicationException("TableDirect 사용 하지 않음");
			}

			return null;
		}

		public IEnumerable<T> Map<T>(IDataReader reader)
			where T : new()
		{
			var colList = DiscoverColumnList(reader);

			var properties =
				from property in typeof(T).GetPublic()
				where IsAutoMappableProperty(property) && IsMap(property.Name, colList)
				select property;

			while (reader.Read())
				yield return MapRow<T>(reader, properties, colList);
		}

		public T MapRow<T>(IDataRecord row, IEnumerable<PropertyInfo> list, IEnumerable<string> colList)
			where T : new()
		{
			T t = new T();

			foreach (var pi in list)
			{
				pi.SetValue(t, GetValue(pi, row, colList), new object[0]);
			}

			return t;
		}

		private object GetValue(PropertyInfo pi, IDataRecord row, IEnumerable<string> colList)
		{
			int i = 0;
			var u = UtilEx.ToUpper(pi.Name);
			if (colList.Contains(u))
				i = row.GetOrdinal(u);
			else
				i = row.GetOrdinal(pi.Name);

			var val = row[i];

			return ConvertValue(val, pi.PropertyType);
		}

		private IEnumerable<string> DiscoverColumnList(IDataReader reader)
		{
			for (int i = 0; i < reader.FieldCount; i++)
				yield return reader.GetName(i);
		}

		private bool IsMap(string name, IEnumerable<string> list)
		{
			return list.Contains(UtilEx.ToUpper(name)) || list.Contains(name);
		}

		public virtual DbCommand GetStoredProcCommand(string sp, BaseEntity entity)
		{
			if (string.IsNullOrEmpty(sp)) throw new ArgumentException("storedProcedureName is null");

			DbCommand command = CreateCommandByCommandType(CommandType.StoredProcedure, sp);

			parameterCache.SetParameters(command, this);

			AssignParameterEntity(command, entity);

			return command;
		}

		public DbCommand GetSqlStringCommand(string sqlString, params object[] parames)
		{
			if (string.IsNullOrEmpty(sqlString)) throw new ArgumentException("sqlString is null");

			DbCommand command = CreateCommandByCommandType(CommandType.Text, sqlString);

			AddStringParameters(command, parames);

			return command;
		}

		public DbCommand GetSqlStringCommand(string sqlString, IDictionary parameterDic = null)
		{
			if (string.IsNullOrEmpty(sqlString)) throw new ArgumentException("sqlString is null");

			DbCommand command = CreateCommandByCommandType(CommandType.Text, sqlString, parameterDic as ParameterDictionary);

			if (parameterDic != null)
			{
				AddStringParameters(command, parameterDic);
			}

			return command;
		}

		public DbCommand GetSqlStringCommand(string sqlString, BaseEntity entity)
		{
			if (string.IsNullOrEmpty(sqlString)) throw new ArgumentException("sqlString is null");

			DbCommand command = CreateCommandByCommandType(CommandType.Text, sqlString);

			AddStringParameters(command, entity);

			return command;
		}

		private void AssignParameterEntity(DbCommand command, BaseEntity entity)
		{
			for (int i = command.Parameters.Count - 1; i >= UserParametersStartIndex(command); i--)
			{
				IDataParameter parameter = command.Parameters[i];

				string name = RefinePropertyName(parameter.ParameterName);
				PropertyInfo pi = entity.GetType().GetPublic(name);

				if (pi != null)
					SetParameterValue(command, parameter.ParameterName, pi.GetValue(entity, null));
				else
				{
					name = parameter.ParameterName;
					name = name.StartsWith("@") ? name.Remove(0, 1) : name;

					pi = entity.GetType().GetPublic(name);
					if (pi != null)
						SetParameterValue(command, parameter.ParameterName, pi.GetValue(entity, null));
				}
			}
		}

		DbCommand CreateCommandByCommandType(CommandType commandType, string commandText, ParameterDictionary parameterDic = null)
		{
			DbCommand command = base.DbProviderFactory.CreateCommand();
			command.CommandType = commandType;
			command.CommandText = GetSql(commandText, parameterDic);

			return command;
		}

		string RefinePropertyName(string parameterName)
		{
			if (parameterName.StartsWith("@"))
				parameterName = parameterName.Remove(0, 1);

			return UtilEx.ToTitle(parameterName);
		}

		string RefineDictionaryKey(string parameterName)
		{
			if (parameterName.StartsWith("@"))
				parameterName = parameterName.Remove(0, 1);

			return UtilEx.ToCamel(parameterName);
		}

		string ToParameterName(string parameterName)
		{
			if (parameterName.StartsWith("@"))
				return parameterName;

			return $"@{parameterName}";
		}

		public virtual void AssignResult(DbCommand command, BaseEntity entity)
		{
			foreach (DbParameter param in command.Parameters)
			{
				if (IsOutput(param))
				{
					string p = this.RefinePropertyName(param.ParameterName);
					PropertyInfo pi = entity.GetType().GetPublic(p);

					if (pi != null)
						pi.SetValue(entity, param.Value, null);
				}
			}

			PropertyInfo rtn = entity.GetType().GetPublic("ReturnValue");
			if (rtn != null)
				rtn.SetValue(entity, command.Parameters[0].Value, null);
		}

		bool IsOutput(DbParameter param)
		{
			return param.Direction == ParameterDirection.InputOutput ||
				param.Direction == ParameterDirection.Output;
		}

		public virtual void AddStringParameters(DbCommand command, params object[] parames)
		{
			if (command.CommandType != CommandType.Text)
				throw new InvalidOperationException("파라미터 쿼리에만 사용 가능");

			var parameterList = ExtractParameters(command.CommandText);

			if (parameterList.Count != parames.Length)
				throw new InvalidOperationException($"파라미터 숫자가 일치하지 않음 {command.CommandText} <> {string.Join(",", parames)}");

			for (int i = 0; i < parameterList.Count; i++)
			{
				var name = parameterList[i];
				object value = parames[i];
				DbType type = FindParamterType(value);

				SafeAddInParameter(command, name, type, value);
			}
		}

		public virtual void AddStringParameters(DbCommand command, IDictionary parameterDic)
		{
			if (command.CommandType != CommandType.Text)
				throw new InvalidOperationException("파라미터 쿼리에만 사용 가능");

			var parameterList = ExtractParameters(command.CommandText);

			foreach (string name in parameterList)
			{
				object value = parameterDic[RefineDictionaryKey(name)];
				DbType type = FindParamterType(value);

				SafeAddInParameter(command, name, type, value);
			}
		}

		public virtual void AddStringParameters(DbCommand command, BaseEntity entity)
		{
			if (command.CommandType != CommandType.Text)
				throw new InvalidOperationException("파라미터 쿼리에만 사용 가능");

			var parameterList = ExtractParameters(command.CommandText);

			for (int i = 0; i < parameterList.Count; i++)
			{
				var name = parameterList[i];
				PropertyInfo pi = entity.GetType().GetPublic(RefinePropertyName(name));

				if (pi != null)
				{
					object value = pi.GetValue(entity, null);
					DbType type = FindParamterType(value, pi.PropertyType);

					SafeAddInParameter(command, parameterList[i], type, value);
				}
				else
				{
					pi = entity.GetType().GetPublic(RefinePropertyName(name));
					if (pi != null)
					{
						object value = pi.GetValue(entity, null);
						DbType type = FindParamterType(value);

						SafeAddInParameter(command, parameterList[i], type, value);
					}
				}
			}
		}

		public void SafeAddInParameter(DbCommand command, string name, DbType dbType, object value)
		{
			if (command.Parameters.Contains(ToParameterName(name)))
				return;

			if (dbType == DbType.Xml && value is XDocument)
				AddInParameter(command, name, dbType, value.ToString());
			else if (dbType == DbType.String && value is JArray)
				AddInParameter(command, name, dbType, JsonConvert.SerializeObject(value));
			else
				AddInParameter(command, name, dbType, value);
		}

		public static List<string> ExtractParameters(string sql)
		{
			var regex = new Regex(@"[:?@](?<Params>[A-Z0-9_-]+)");
			var match = regex.Matches(sql);
			return match.Cast<Match>().Select(x => x.Groups["Params"].Value
				.Replace(";", string.Empty)
				.Replace(",", string.Empty)
				.Replace(")", string.Empty)).Distinct().ToList();
		}

		public virtual DbType FindParamterType(object value, Type type = null)
		{
			switch (value)
			{
				case string:
					return DbType.String;
				case int:
					return DbType.Int32;
				case decimal:
					return DbType.Decimal;
				case double:
					return DbType.Double;
				case long:
					return DbType.Int64;
				case char:
					return DbType.String;
				case DateTime:
					return DbType.DateTime;
				case byte[]:
					return DbType.Binary;
				case XDocument:
					return DbType.Xml;
				case IEnumerable:
					return DbType.String;
				default:
					switch (type)
					{
						case Type when type == typeof(string):
							return DbType.String;
						case Type when type == typeof(int):
							return DbType.Int32;
						case Type when type == typeof(decimal):
							return DbType.Decimal;
						case Type when type == typeof(double):
							return DbType.Double;
						case Type when type == typeof(char):
							return DbType.String;
						case Type when type == typeof(DateTime):
							return DbType.DateTime;
					}

					return DbType.Object;
			}
		}

		public static bool IsAutoMappableProperty(PropertyInfo property)
		{
			return property.CanWrite
			  && property.GetIndexParameters().Length == 0
			  && !IsCollectionType(property.PropertyType)
			;
		}

		public static bool IsCollectionType(Type type)
		{
			if (type == typeof(string)) return false;

			if (type == typeof(byte[])) return false;

			var interfaces = from inf in type.GetInterfaces()
							 where inf == typeof(IEnumerable) ||
								 (inf.IsGenericType && inf.GetGenericTypeDefinition() == typeof(IEnumerable<>))
							 select inf;
			return interfaces.Count() != 0;
		}

		public static object ConvertValue(object value, Type conversionType)
		{
			if (IsNullableType(conversionType))
			{
				return ConvertNullableValue(value, conversionType);
			}
			return ConvertNonNullableValue(value, conversionType);
		}

		public static bool IsNullableType(Type t)
		{
			return t.IsGenericType &&
				   t.GetGenericTypeDefinition() == typeof(Nullable<>);
		}

		public static object ConvertNullableValue(object value, Type conversionType)
		{
			if (value != DBNull.Value)
			{
				var converter = new NullableConverter(conversionType);
				return converter.ConvertFrom(value);
			}
			return null;
		}

		public static object ConvertNonNullableValue(object value, Type conversionType)
		{
			object convertedValue = null;

			if (value != DBNull.Value)
			{
				convertedValue = Convert.ChangeType(value, conversionType, CultureInfo.CurrentCulture);
			}
			else if (conversionType.IsValueType)
			{
				convertedValue = Activator.CreateInstance(conversionType);
			}

			return convertedValue;
		}

		public string GetSql(string sql, ParameterDictionary parameterDic)
		{
			if (sql.StartsWith("@"))
				sql = _sqlCache.GetSingleSql(ToSqlId(sql));

			return SqlPhraseEx.BuildSql(sql, parameterDic);
		}

		string ToSqlId(string sql)
		{
			return sql.Substring(1);
		}
	}
}
