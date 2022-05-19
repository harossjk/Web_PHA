using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using System.Collections;

using Microsoft.Extensions.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Framework
{
	public class DataContext
	{
		static IConfiguration _configuration;

		static ISqlCache _sqlCache;

		public DataContext(IConfiguration configuration, ISqlCache sqlCache)
        {
			_configuration = configuration;
			_sqlCache = sqlCache;
		}

		#region DataSet

		static public DataSet DataSetEx(string name, string sp, params object[] parames)
		{
			return Create(name).ExecuteDataSet(sp, parames);
		}

		static public DataSet DataSetEx(string name, string sp, IDictionary parameterDic)
		{
			return Create(name).ExecuteDataSet(sp, parameterDic);
		}

		static public DataSet DataSet(string sp, params object[] parames)
		{
			return DataSetEx(null, sp, parames);
		}

		static public DataSet DataSet(string sp, IDictionary parameterDic)
		{
			return DataSetEx(null, sp, parameterDic);
		}

        #endregion


        #region NonQuery

        static public int NonQueryEx(string name, string sp, IDictionary parameterDic)
		{
			return Create(name).ExecuteNonQuery(sp, parameterDic);
		}

		static public int NonQueryEx(string name, string sp, BaseEntity entity)
		{
			return Create(name).ExecuteNonQuery(sp, entity);
		}

		static public int NonQueryEx(string name, string sp, params object[] parames)
		{
			return Create(name).ExecuteNonQuery(sp, parames);
		}

		static public int NonQuery(string sp, IDictionary parameterDic)
		{
			return NonQueryEx(null, sp, parameterDic);
		}

		static public int NonQuery(string sp, BaseEntity entity)
		{
			return NonQueryEx(null, sp, entity);
		}

		static public int NonQuery(string sp, params object[] parames)
		{
			return NonQueryEx(null, sp, parames);
		}

		static public int NonQueryTransEx(string name, string sp, params object[] parames)
		{
			int result = 0;

			using (TransactionScope scope = new TransactionScope())
			{
				result = NonQueryEx(name, sp, parames);

				scope.Complete();
			}

			return result;
		}

		static public int NonQueryTransEx(string name, string sp, IDictionary parameterDic)
		{
			int result = 0;

			using (TransactionScope scope = new TransactionScope())
			{
				result = NonQueryEx(name, sp, parameterDic);

				scope.Complete();
			}

			return result;
		}

		static public int NonQueryTransEx(string name, string sp, BaseEntity entity)
		{
			int result = 0;

			using (TransactionScope scope = new TransactionScope())
			{
				result = NonQueryEx(name, sp, entity);

				scope.Complete();
			}

			return result;
		}

		static public int NonQueryTrans(string sp, params object[] parames)
		{
			return NonQueryTransEx(null, sp, parames);
		}

		static public int NonQueryTrans(string sp, IDictionary parameterDic)
		{
			return NonQueryTransEx(null, sp, parameterDic);
		}

		static public int NonQueryTrans(string sp, BaseEntity entity)
		{
			return NonQueryTransEx(null, sp, entity);
		}

        #endregion


        #region Value

        static public T ValueEx<T>(string name, string sp, params object[] parames)
		{
			return ConvertEx.ConvertTo<T>(Create(name).ExecuteScalar(sp, parames), default(T));
		}

		static public T ValueEx<T>(string name, string sp, IDictionary parameterDic)
		{
			return ConvertEx.ConvertTo<T>(Create(name).ExecuteScalar(sp, parameterDic), default(T));
		}

		static public T ValueEx<T, U>(string name, string sp, BaseEntity entity)
		{
			return ConvertEx.ConvertTo<T>(Create(name).ExecuteScalar(sp, entity), default(T));
		}

		static public T Value<T>(string sp, params object[] parames)
		{
			return ValueEx<T>(null, sp, parames);
		}

		static public T Value<T>(string sp, IDictionary parameterDic)
		{
			return ValueEx<T>(null, sp, parameterDic);
		}

		static public T Value<T, U>(string sp, BaseEntity entity)
		{
			return ValueEx<T, U>(null, sp, entity);
		}

		#endregion


		#region Entity - params

		static public List<T> EntityListEx<T>(string name, string sp, params object[] parames)
			where T : new()
		{
			DatabaseEx db = Create(name);

			return db.EntityList<object[], T>(CommandType.StoredProcedure, sp, parames);
		}

		static public Tuple<List<T1>, List<T2>> EntityListEx<T1, T2>(string name, string sp, params object[] parames)
			where T1 : new()
			where T2 : new()
		{
			DatabaseEx db = Create(name);

			return db.EntityList<object[], T1, T2>(CommandType.StoredProcedure, sp, parames);
		}

		static public Tuple<List<T1>, List<T2>, List<T3>> EntityListEx<T1, T2, T3>(string name, string sp, params object[] parames)
			where T1 : new()
			where T2 : new()
			where T3 : new()
		{
			DatabaseEx db = Create(name);

			return db.EntityList<object[], T1, T2, T3>(CommandType.StoredProcedure, sp, parames);
		}

		static public Tuple<List<T1>, List<T2>, List<T3>, List<T4>> EntityListEx<T1, T2, T3, T4>(string name, string sp, params object[] parames)
			where T1 : new()
			where T2 : new()
			where T3 : new()
			where T4 : new()
		{
			DatabaseEx db = Create(name);

			return db.EntityList<object[], T1, T2, T3, T4>(CommandType.StoredProcedure, sp, parames);
		}

		static public Tuple<List<T1>, List<T2>, List<T3>, List<T4>, List<T5>> EntityListEx<T1, T2, T3, T4, T5>(string name, string sp, params object[] parames)
			where T1 : new()
			where T2 : new()
			where T3 : new()
			where T4 : new()
			where T5 : new()
		{
			DatabaseEx db = Create(name);

			return db.EntityList<object[], T1, T2, T3, T4, T5>(CommandType.StoredProcedure, sp, parames);
		}

		static public Tuple<List<T1>, List<T2>, List<T3>, List<T4>, List<T5>, List<T6>> EntityListEx<T1, T2, T3, T4, T5, T6>(string name, string sp, params object[] parames)
			where T1 : new()
			where T2 : new()
			where T3 : new()
			where T4 : new()
			where T5 : new()
			where T6 : new()
		{
			DatabaseEx db = Create(name);

			return db.EntityList<object[], T1, T2, T3, T4, T5, T6>(CommandType.StoredProcedure, sp, parames);
		}

		static public Tuple<List<T1>, List<T2>, List<T3>, List<T4>, List<T5>, List<T6>, List<T7>> EntityListEx<T1, T2, T3, T4, T5, T6, T7>(string name, string sp, params object[] parames)
			where T1 : new()
			where T2 : new()
			where T3 : new()
			where T4 : new()
			where T5 : new()
			where T6 : new()
			where T7 : new()
		{
			DatabaseEx db = Create(name);

			return db.EntityList<object[], T1, T2, T3, T4, T5, T6, T7>(CommandType.StoredProcedure, sp, parames);
		}

		static public Tuple<List<T1>, List<T2>, List<T3>, List<T4>, List<T5>, List<T6>, List<T7>, List<List<TRest>>> EntityListEx<T1, T2, T3, T4, T5, T6, T7, TRest>(string name, string sp, params object[] parames)
			where T1 : new()
			where T2 : new()
			where T3 : new()
			where T4 : new()
			where T5 : new()
			where T6 : new()
			where T7 : new()
			where TRest : new()
		{
			DatabaseEx db = Create(name);

			return db.EntityList<object[], T1, T2, T3, T4, T5, T6, T7, TRest>(CommandType.StoredProcedure, sp, parames);
		}

		static public List<T> EntityList<T>(string sp, params object[] parames)
			where T : new()
		{
			return EntityListEx<T>(null, sp, parames);
		}

		static public Tuple<List<T1>, List<T2>> EntityList<T1, T2>(string sp, params object[] parames)
			where T1 : new()
			where T2 : new()
		{
			return EntityListEx<T1, T2>(null, sp, parames);
		}

		static public Tuple<List<T1>, List<T2>, List<T3>> EntityList<T1, T2, T3>(string sp, params object[] parames)
			where T1 : new()
			where T2 : new()
			where T3 : new()
		{
			return EntityListEx<T1, T2, T3>(null, sp, parames);
		}

		static public Tuple<List<T1>, List<T2>, List<T3>, List<T4>> EntityList<T1, T2, T3, T4>(string sp, params object[] parames)
			where T1 : new()
			where T2 : new()
			where T3 : new()
			where T4 : new()
		{
			return EntityListEx<T1, T2, T3, T4>(null, sp, parames);
		}

		static public Tuple<List<T1>, List<T2>, List<T3>, List<T4>, List<T5>> EntityList<T1, T2, T3, T4, T5>(string sp, params object[] parames)
			where T1 : new()
			where T2 : new()
			where T3 : new()
			where T4 : new()
			where T5 : new()
		{
			return EntityListEx<T1, T2, T3, T4, T5>(null, sp, parames);
		}

		static public Tuple<List<T1>, List<T2>, List<T3>, List<T4>, List<T5>, List<T6>> EntityList<T1, T2, T3, T4, T5, T6>(string sp, params object[] parames)
			where T1 : new()
			where T2 : new()
			where T3 : new()
			where T4 : new()
			where T5 : new()
			where T6 : new()
		{
			return EntityListEx<T1, T2, T3, T4, T5, T6>(null, sp, parames);
		}

		static public Tuple<List<T1>, List<T2>, List<T3>, List<T4>, List<T5>, List<T6>, List<T7>> EntityList<T1, T2, T3, T4, T5, T6, T7>(string sp, params object[] parames)
			where T1 : new()
			where T2 : new()
			where T3 : new()
			where T4 : new()
			where T5 : new()
			where T6 : new()
			where T7 : new()
		{
			return EntityListEx<T1, T2, T3, T4, T5, T6, T7>(null, sp, parames);
		}

		static public Tuple<List<T1>, List<T2>, List<T3>, List<T4>, List<T5>, List<T6>, List<T7>, List<List<TRest>>> EntityList<T1, T2, T3, T4, T5, T6, T7, TRest>(string sp, params object[] parames)
			where T1 : new()
			where T2 : new()
			where T3 : new()
			where T4 : new()
			where T5 : new()
			where T6 : new()
			where T7 : new()
			where TRest : new()
		{
			return EntityListEx<T1, T2, T3, T4, T5, T6, T7, TRest>(null, sp, parames);
		}

		static public T BaseEntity<T>(string name, string sp, params object[] parames)
			where T : new()
		{
			DatabaseEx db = Create(name);
			var rtn = db.EntityList<object[], T>(CommandType.StoredProcedure, sp, parames);

			if (rtn.Count() > 0)
				return rtn.First();
			else
				return default(T);
		}

		static public TResult EntityEx<TResult>(string name, string sp, params object[] parames)
			where TResult : new()
		{
			return BaseEntity<TResult>(name, sp, parames);
		}

		static public TResult Entity<TResult>(string sp, params object[] parames)
			where TResult : new()
		{
			return EntityEx<TResult>(null, sp, parames);
		}

        #endregion


        #region Entity - dic

        static public List<T> EntityListEx<T>(string name, string sp, IDictionary dic)
			where T : new()
		{
			DatabaseEx db = Create(name);

			return db.EntityList<IDictionary, T>(CommandType.StoredProcedure, sp, dic);
		}

		static public Tuple<List<T1>, List<T2>> EntityListEx<T1, T2>(string name, string sp, IDictionary dic)
			where T1 : new()
			where T2 : new()
		{
			DatabaseEx db = Create(name);

			return db.EntityList<IDictionary, T1, T2>(CommandType.StoredProcedure, sp, dic);
		}

		static public Tuple<List<T1>, List<T2>, List<T3>> EntityListEx<T1, T2, T3>(string name, string sp, IDictionary dic)
			where T1 : new()
			where T2 : new()
			where T3 : new()
		{
			DatabaseEx db = Create(name);

			return db.EntityList<IDictionary, T1, T2, T3>(CommandType.StoredProcedure, sp, dic);
		}

		static public Tuple<List<T1>, List<T2>, List<T3>, List<T4>> EntityListEx<T1, T2, T3, T4>(string name, string sp, IDictionary dic)
			where T1 : new()
			where T2 : new()
			where T3 : new()
			where T4 : new()
		{
			DatabaseEx db = Create(name);

			return db.EntityList<IDictionary, T1, T2, T3, T4>(CommandType.StoredProcedure, sp, dic);
		}

		static public Tuple<List<T1>, List<T2>, List<T3>, List<T4>, List<T5>> EntityListEx<T1, T2, T3, T4, T5>(string name, string sp, IDictionary dic)
			where T1 : new()
			where T2 : new()
			where T3 : new()
			where T4 : new()
			where T5 : new()
		{
			DatabaseEx db = Create(name);

			return db.EntityList<IDictionary, T1, T2, T3, T4, T5>(CommandType.StoredProcedure, sp, dic);
		}

		static public Tuple<List<T1>, List<T2>, List<T3>, List<T4>, List<T5>, List<T6>> EntityListEx<T1, T2, T3, T4, T5, T6>(string name, string sp, IDictionary dic)
			where T1 : new()
			where T2 : new()
			where T3 : new()
			where T4 : new()
			where T5 : new()
			where T6 : new()
		{
			DatabaseEx db = Create(name);

			return db.EntityList<IDictionary, T1, T2, T3, T4, T5, T6>(CommandType.StoredProcedure, sp, dic);
		}

		static public Tuple<List<T1>, List<T2>, List<T3>, List<T4>, List<T5>, List<T6>, List<T7>> EntityListEx<T1, T2, T3, T4, T5, T6, T7>(string name, string sp, IDictionary dic)
			where T1 : new()
			where T2 : new()
			where T3 : new()
			where T4 : new()
			where T5 : new()
			where T6 : new()
			where T7 : new()
		{
			DatabaseEx db = Create(name);

			return db.EntityList<IDictionary, T1, T2, T3, T4, T5, T6, T7>(CommandType.StoredProcedure, sp, dic);
		}

		static public Tuple<List<T1>, List<T2>, List<T3>, List<T4>, List<T5>, List<T6>, List<T7>, List<List<TRest>>> EntityListEx<T1, T2, T3, T4, T5, T6, T7, TRest>(string name, string sp, IDictionary dic)
			where T1 : new()
			where T2 : new()
			where T3 : new()
			where T4 : new()
			where T5 : new()
			where T6 : new()
			where T7 : new()
			where TRest : new()
		{
			DatabaseEx db = Create(name);

			return db.EntityList<IDictionary, T1, T2, T3, T4, T5, T6, T7, TRest>(CommandType.StoredProcedure, sp, dic);
		}

		static public List<T> EntityList<T>(string sp, IDictionary dic)
			where T : new()
		{
			return EntityListEx<T>(null, sp, dic);
		}

		static public Tuple<List<T1>, List<T2>> EntityList<T1, T2>(string sp, IDictionary dic)
			where T1 : new()
			where T2 : new()
		{
			return EntityListEx<T1, T2>(null, sp, dic);
		}

		static public Tuple<List<T1>, List<T2>, List<T3>> EntityList<T1, T2, T3>(string sp, IDictionary dic)
			where T1 : new()
			where T2 : new()
			where T3 : new()
		{
			return EntityListEx<T1, T2, T3>(null, sp, dic);
		}

		static public Tuple<List<T1>, List<T2>, List<T3>, List<T4>> EntityList<T1, T2, T3, T4>(string sp, IDictionary dic)
			where T1 : new()
			where T2 : new()
			where T3 : new()
			where T4 : new()
		{
			return EntityListEx<T1, T2, T3, T4>(null, sp, dic);
		}

		static public Tuple<List<T1>, List<T2>, List<T3>, List<T4>, List<T5>> EntityList<T1, T2, T3, T4, T5>(string sp, IDictionary dic)
			where T1 : new()
			where T2 : new()
			where T3 : new()
			where T4 : new()
			where T5 : new()
		{
			return EntityListEx<T1, T2, T3, T4, T5>(null, sp, dic);
		}

		static public Tuple<List<T1>, List<T2>, List<T3>, List<T4>, List<T5>, List<T6>> EntityList<T1, T2, T3, T4, T5, T6>(string sp, IDictionary dic)
			where T1 : new()
			where T2 : new()
			where T3 : new()
			where T4 : new()
			where T5 : new()
			where T6 : new()
		{
			return EntityListEx<T1, T2, T3, T4, T5, T6>(null, sp, dic);
		}

		static public Tuple<List<T1>, List<T2>, List<T3>, List<T4>, List<T5>, List<T6>, List<T7>> EntityList<T1, T2, T3, T4, T5, T6, T7>(string sp, IDictionary dic)
			where T1 : new()
			where T2 : new()
			where T3 : new()
			where T4 : new()
			where T5 : new()
			where T6 : new()
			where T7 : new()
		{
			return EntityListEx<T1, T2, T3, T4, T5, T6, T7>(null, sp, dic);
		}

		static public Tuple<List<T1>, List<T2>, List<T3>, List<T4>, List<T5>, List<T6>, List<T7>, List<List<TRest>>> EntityList<T1, T2, T3, T4, T5, T6, T7, TRest>(string sp, IDictionary dic)
			where T1 : new()
			where T2 : new()
			where T3 : new()
			where T4 : new()
			where T5 : new()
			where T6 : new()
			where T7 : new()
			where TRest : new()
		{
			return EntityListEx<T1, T2, T3, T4, T5, T6, T7, TRest>(null, sp, dic);
		}

		static public T BaseEntity<T>(string name, string sp, IDictionary dic)
			where T : new()
		{
			DatabaseEx db = Create(name);
			var rtn = db.EntityList<IDictionary, T>(CommandType.StoredProcedure, sp, dic);

			if (rtn.Count() > 0)
				return rtn.First();
			else
				return default(T);
		}

		static public TResult EntityEx<TResult>(string name, string sp, IDictionary dic)
			where TResult : new()
		{
			return BaseEntity<TResult>(name, sp, dic);
		}

		static public TResult Entity<TResult>(string sp, IDictionary dic)
			where TResult : new()
		{
			return EntityEx<TResult>(null, sp, dic);
		}

		#endregion


		#region Entity - entity

		static public List<T> EntityListEx<T>(string name, string sp, BaseEntity entity)
			where T : new()
		{
			DatabaseEx db = Create(name);

			return db.EntityList<BaseEntity, T>(CommandType.StoredProcedure, sp, entity);
		}

		static public Tuple<List<T1>, List<T2>> EntityListEx<T1, T2>(string name, string sp, BaseEntity entity)
			where T1 : new()
			where T2 : new()
		{
			DatabaseEx db = Create(name);

			return db.EntityList<BaseEntity, T1, T2>(CommandType.StoredProcedure, sp, entity);
		}

		static public Tuple<List<T1>, List<T2>, List<T3>> EntityListEx<T1, T2, T3>(string name, string sp, BaseEntity entity)
			where T1 : new()
			where T2 : new()
			where T3 : new()
		{
			DatabaseEx db = Create(name);

			return db.EntityList<BaseEntity, T1, T2, T3>(CommandType.StoredProcedure, sp, entity);
		}

		static public Tuple<List<T1>, List<T2>, List<T3>, List<T4>> EntityListEx<T1, T2, T3, T4>(string name, string sp, BaseEntity entity)
			where T1 : new()
			where T2 : new()
			where T3 : new()
			where T4 : new()
		{
			DatabaseEx db = Create(name);

			return db.EntityList<BaseEntity, T1, T2, T3, T4>(CommandType.StoredProcedure, sp, entity);
		}

		static public Tuple<List<T1>, List<T2>, List<T3>, List<T4>, List<T5>> EntityListEx<T1, T2, T3, T4, T5>(string name, string sp, BaseEntity entity)
			where T1 : new()
			where T2 : new()
			where T3 : new()
			where T4 : new()
			where T5 : new()
		{
			DatabaseEx db = Create(name);

			return db.EntityList<BaseEntity, T1, T2, T3, T4, T5>(CommandType.StoredProcedure, sp, entity);
		}

		static public Tuple<List<T1>, List<T2>, List<T3>, List<T4>, List<T5>, List<T6>> EntityListEx<T1, T2, T3, T4, T5, T6>(string name, string sp, BaseEntity entity)
			where T1 : new()
			where T2 : new()
			where T3 : new()
			where T4 : new()
			where T5 : new()
			where T6 : new()
		{
			DatabaseEx db = Create(name);

			return db.EntityList<BaseEntity, T1, T2, T3, T4, T5, T6>(CommandType.StoredProcedure, sp, entity);
		}

		static public Tuple<List<T1>, List<T2>, List<T3>, List<T4>, List<T5>, List<T6>, List<T7>> EntityListEx<T1, T2, T3, T4, T5, T6, T7>(string name, string sp, BaseEntity entity)
			where T1 : new()
			where T2 : new()
			where T3 : new()
			where T4 : new()
			where T5 : new()
			where T6 : new()
			where T7 : new()
		{
			DatabaseEx db = Create(name);

			return db.EntityList<BaseEntity, T1, T2, T3, T4, T5, T6, T7>(CommandType.StoredProcedure, sp, entity);
		}

		static public Tuple<List<T1>, List<T2>, List<T3>, List<T4>, List<T5>, List<T6>, List<T7>, List<List<TRest>>> EntityListEx<T1, T2, T3, T4, T5, T6, T7, TRest>(string name, string sp, BaseEntity entity)
			where T1 : new()
			where T2 : new()
			where T3 : new()
			where T4 : new()
			where T5 : new()
			where T6 : new()
			where T7 : new()
			where TRest : new()
		{
			DatabaseEx db = Create(name);

			return db.EntityList<BaseEntity, T1, T2, T3, T4, T5, T6, T7, TRest>(CommandType.StoredProcedure, sp, entity);
		}

		static public List<T> EntityList<T>(string sp, BaseEntity entity)
			where T : new()
		{
			return EntityListEx<T>(null, sp, entity);
		}

		static public Tuple<List<T1>, List<T2>> EntityList<T1, T2>(string sp, BaseEntity entity)
			where T1 : new()
			where T2 : new()
		{
			return EntityListEx<T1, T2>(null, sp, entity);
		}

		static public Tuple<List<T1>, List<T2>, List<T3>> EntityList<T1, T2, T3>(string sp, BaseEntity entity)
			where T1 : new()
			where T2 : new()
			where T3 : new()
		{
			return EntityListEx<T1, T2, T3>(null, sp, entity);
		}

		static public Tuple<List<T1>, List<T2>, List<T3>, List<T4>> EntityList<T1, T2, T3, T4>(string sp, BaseEntity entity)
			where T1 : new()
			where T2 : new()
			where T3 : new()
			where T4 : new()
		{
			return EntityListEx<T1, T2, T3, T4>(null, sp, entity);
		}

		static public Tuple<List<T1>, List<T2>, List<T3>, List<T4>, List<T5>> EntityList<T1, T2, T3, T4, T5>(string sp, BaseEntity entity)
			where T1 : new()
			where T2 : new()
			where T3 : new()
			where T4 : new()
			where T5 : new()
		{
			return EntityListEx<T1, T2, T3, T4, T5>(null, sp, entity);
		}

		static public Tuple<List<T1>, List<T2>, List<T3>, List<T4>, List<T5>, List<T6>> EntityList<T1, T2, T3, T4, T5, T6>(string sp, BaseEntity entity)
			where T1 : new()
			where T2 : new()
			where T3 : new()
			where T4 : new()
			where T5 : new()
			where T6 : new()
		{
			return EntityListEx<T1, T2, T3, T4, T5, T6>(null, sp, entity);
		}

		static public Tuple<List<T1>, List<T2>, List<T3>, List<T4>, List<T5>, List<T6>, List<T7>> EntityList<T1, T2, T3, T4, T5, T6, T7>(string sp, BaseEntity entity)
			where T1 : new()
			where T2 : new()
			where T3 : new()
			where T4 : new()
			where T5 : new()
			where T6 : new()
			where T7 : new()
		{
			return EntityListEx<T1, T2, T3, T4, T5, T6, T7>(null, sp, entity);
		}

		static public Tuple<List<T1>, List<T2>, List<T3>, List<T4>, List<T5>, List<T6>, List<T7>, List<List<TRest>>> EntityList<T1, T2, T3, T4, T5, T6, T7, TRest>(string sp, BaseEntity entity)
			where T1 : new()
			where T2 : new()
			where T3 : new()
			where T4 : new()
			where T5 : new()
			where T6 : new()
			where T7 : new()
			where TRest : new()
		{
			return EntityListEx<T1, T2, T3, T4, T5, T6, T7, TRest>(null, sp, entity);
		}

		static public T BaseEntity<T>(string name, string sp, BaseEntity entity)
			where T : new()
		{
			DatabaseEx db = Create(name);
			var rtn = db.EntityList<BaseEntity, T>(CommandType.StoredProcedure, sp, entity);

			if (rtn.Count() > 0)
				return rtn.First();
			else
				return default(T);
		}

		static public TResult EntityEx<TResult>(string name, string sp, BaseEntity entity)
			where TResult : new()
		{
			return BaseEntity<TResult>(name, sp, entity);
		}

		static public TResult Entity<TResult>(string sp, BaseEntity entity)
			where TResult : new()
		{
			return EntityEx<TResult>(null, sp, entity);
		}

		#endregion


		#region StringDataSet

		static public DataSet StringDataSetEx(string name, string sql, params object[] parames)
		{
			return Create(name).ExecuteStringDataSet(sql, parames);
		}

		static public DataSet StringDataSetEx(string name, string sql, IDictionary parameterDic = null)
		{
			return Create(name).ExecuteStringDataSet(sql, parameterDic);
		}

		static public DataSet StringDataSetEx(string name, string sql, BaseEntity entity)
		{
			return Create(name).ExecuteStringDataSet(sql, entity);
		}

		static public DataSet StringDataSet(string sql, params object[] parames)
		{
			return StringDataSetEx(null, sql, parames);
		}

		static public DataSet StringDataSet(string sql, IDictionary parameterDic = null)
		{
			return StringDataSetEx(null, sql, parameterDic);
		}

		static public DataSet StringDataSet(string sql, BaseEntity entity)
		{
			return StringDataSetEx(null, sql, entity);
		}

        #endregion


        #region StringNonQuery

        static public int StringNonQueryEx(string name, string sql, params object[] parames)
		{
			return Create(name).ExecuteStringNonQuery(sql, parames);
		}

		static public int StringNonQueryEx(string name, string sql, IDictionary parameterDic = null)
		{
			return Create(name).ExecuteStringNonQuery(sql, parameterDic);
		}

		static public int StringNonQueryEx(string name, string sql, BaseEntity entity)
		{
			return Create(name).ExecuteStringNonQuery(sql, entity);
		}

		static public int StringNonQuery(string sql, params object[] parames)
		{
			return StringNonQueryEx(null, sql, parames);
		}

		static public int StringNonQuery(string sql, IDictionary parameterDic = null)
		{
			return StringNonQueryEx(null, sql, parameterDic);
		}

		static public int StringNonQuery(string sql, BaseEntity entity)
		{
			return StringNonQueryEx(null, sql, entity);
		}

        #endregion


        #region StringValue

        static public T StringValueEx<T>(string name, string sql, params object[] parames)
		{
			return Create(name).ExecuteStringScalar<T>(sql, parames);
		}

		static public T StringValueEx<T>(string name, string sql, IDictionary parameterDic = null)
		{
			return Create(name).ExecuteStringScalar<T>(sql, parameterDic);
		}

		static public T StringValueEx<T>(string name, string sql, BaseEntity entity)
		{
			return Create(name).ExecuteStringScalar<T>(sql, entity);
		}

		static public T StringValue<T>(string sql, params object[] parames)
		{
			return Create(null).ExecuteStringScalar<T>(sql, parames);
		}

		static public T StringValue<T>(string sql, IDictionary parameterDic = null)
		{
			return Create(null).ExecuteStringScalar<T>(sql, parameterDic);
		}

		static public T StringValue<T>(string sql, BaseEntity entity)
		{
			return Create(null).ExecuteStringScalar<T>(sql, entity);
		}

		#endregion


		#region StringEntity - params

		static public List<T> StringEntityListEx<T>(string name, string sql, params object[] parames)
			where T : new()
		{
			DatabaseEx db = Create(name);

			return db.EntityList<object[], T>(CommandType.Text, sql, parames);
		}

		static public Tuple<List<T1>, List<T2>> StringEntityListEx<T1, T2>(string name, string sql, params object[] parames)
			where T1 : new()
			where T2 : new()
		{
			DatabaseEx db = Create(name);

			return db.EntityList<object[], T1, T2>(CommandType.Text, sql, parames);
		}

		static public Tuple<List<T1>, List<T2>, List<T3>> StringEntityListEx<T1, T2, T3>(string name, string sql, params object[] parames)
			where T1 : new()
			where T2 : new()
			where T3 : new()
		{
			DatabaseEx db = Create(name);

			return db.EntityList<object[], T1, T2, T3>(CommandType.Text, sql, parames);
		}

		static public Tuple<List<T1>, List<T2>, List<T3>, List<T4>> StringEntityListEx<T1, T2, T3, T4>(string name, string sql, params object[] parames)
			where T1 : new()
			where T2 : new()
			where T3 : new()
			where T4 : new()
		{
			DatabaseEx db = Create(name);

			return db.EntityList<object[], T1, T2, T3, T4>(CommandType.Text, sql, parames);
		}

		static public Tuple<List<T1>, List<T2>, List<T3>, List<T4>, List<T5>> StringEntityListEx<T1, T2, T3, T4, T5>(string name, string sql, params object[] parames)
			where T1 : new()
			where T2 : new()
			where T3 : new()
			where T4 : new()
			where T5 : new()
		{
			DatabaseEx db = Create(name);

			return db.EntityList<object[], T1, T2, T3, T4, T5>(CommandType.Text, sql, parames);
		}

		static public Tuple<List<T1>, List<T2>, List<T3>, List<T4>, List<T5>, List<T6>> StringEntityListEx<T1, T2, T3, T4, T5, T6>(string name, string sql, params object[] parames)
			where T1 : new()
			where T2 : new()
			where T3 : new()
			where T4 : new()
			where T5 : new()
			where T6 : new()
		{
			DatabaseEx db = Create(name);

			return db.EntityList<object[], T1, T2, T3, T4, T5, T6>(CommandType.Text, sql, parames);
		}

		static public Tuple<List<T1>, List<T2>, List<T3>, List<T4>, List<T5>, List<T6>, List<T7>> StringEntityListEx<T1, T2, T3, T4, T5, T6, T7>(string name, string sql, params object[] parames)
			where T1 : new()
			where T2 : new()
			where T3 : new()
			where T4 : new()
			where T5 : new()
			where T6 : new()
			where T7 : new()
		{
			DatabaseEx db = Create(name);

			return db.EntityList<object[], T1, T2, T3, T4, T5, T6, T7>(CommandType.Text, sql, parames);
		}

		static public Tuple<List<T1>, List<T2>, List<T3>, List<T4>, List<T5>, List<T6>, List<T7>, List<List<TRest>>> StringEntityListEx<T1, T2, T3, T4, T5, T6, T7, TRest>(string name, string sql, params object[] parames)
			where T1 : new()
			where T2 : new()
			where T3 : new()
			where T4 : new()
			where T5 : new()
			where T6 : new()
			where T7 : new()
			where TRest : new()
		{
			DatabaseEx db = Create(name);

			return db.EntityList<object[], T1, T2, T3, T4, T5, T6, T7, TRest>(CommandType.Text, sql, parames);
		}

		static public List<T> StringEntityList<T>(string sql, params object[] parames)
			where T : new()
		{
			return StringEntityListEx<T>(null, sql, parames);
		}

		static public Tuple<List<T1>, List<T2>> StringEntityList<T1, T2>(string sql, params object[] parames)
			where T1 : new()
			where T2 : new()
		{
			return StringEntityListEx<T1, T2>(null, sql, parames);
		}

		static public Tuple<List<T1>, List<T2>, List<T3>> StringEntityList<T1, T2, T3>(string sql, params object[] parames)
			where T1 : new()
			where T2 : new()
			where T3 : new()
		{
			return StringEntityListEx<T1, T2, T3>(null, sql, parames);
		}

		static public Tuple<List<T1>, List<T2>, List<T3>, List<T4>> StringEntityList<T1, T2, T3, T4>(string sql, params object[] parames)
			where T1 : new()
			where T2 : new()
			where T3 : new()
			where T4 : new()
		{
			return StringEntityListEx<T1, T2, T3, T4>(null, sql, parames);
		}

		static public Tuple<List<T1>, List<T2>, List<T3>, List<T4>, List<T5>> StringEntityList<T1, T2, T3, T4, T5>(string sql, params object[] parames)
			where T1 : new()
			where T2 : new()
			where T3 : new()
			where T4 : new()
			where T5 : new()
		{
			return StringEntityListEx<T1, T2, T3, T4, T5>(null, sql, parames);
		}

		static public Tuple<List<T1>, List<T2>, List<T3>, List<T4>, List<T5>, List<T6>> StringEntityList<T1, T2, T3, T4, T5, T6>(string sql, params object[] parames)
			where T1 : new()
			where T2 : new()
			where T3 : new()
			where T4 : new()
			where T5 : new()
			where T6 : new()
		{
			return StringEntityListEx<T1, T2, T3, T4, T5, T6>(null, sql, parames);
		}

		static public Tuple<List<T1>, List<T2>, List<T3>, List<T4>, List<T5>, List<T6>, List<T7>> StringEntityList<T1, T2, T3, T4, T5, T6, T7>(string sql, params object[] parames)
			where T1 : new()
			where T2 : new()
			where T3 : new()
			where T4 : new()
			where T5 : new()
			where T6 : new()
			where T7 : new()
		{
			return StringEntityListEx<T1, T2, T3, T4, T5, T6, T7>(null, sql, parames);
		}

		static public Tuple<List<T1>, List<T2>, List<T3>, List<T4>, List<T5>, List<T6>, List<T7>, List<List<TRest>>> StringEntityList<T1, T2, T3, T4, T5, T6, T7, TRest>(string sql, params object[] parames)
			where T1 : new()
			where T2 : new()
			where T3 : new()
			where T4 : new()
			where T5 : new()
			where T6 : new()
			where T7 : new()
			where TRest : new()
		{
			return StringEntityListEx<T1, T2, T3, T4, T5, T6, T7, TRest>(null, sql, parames);
		}

		static public T StringBaseEntity<T>(string name, string sql, params object[] parames)
			where T : new()
		{
			DatabaseEx db = Create(name);
			var rtn = db.EntityList<object[], T>(CommandType.Text, sql, parames);

			if (rtn.Count() > 0)
				return rtn.First();
			else
				return default(T);
		}

		static public TResult StringEntityEx<TResult>(string name, string sql, params object[] parames)
			where TResult : new()
		{
			return StringBaseEntity<TResult>(name, sql, parames);
		}

		static public TResult StringEntity<TResult>(string sql, params object[] parames)
			where TResult : new()
		{
			return StringEntityEx<TResult>(null, sql, parames);
		}

		#endregion


		#region StringEntity - dic

		static public List<T> StringEntityListEx<T>(string name, string sp, IDictionary dic)
			where T : new()
		{
			DatabaseEx db = Create(name);

			return db.EntityList<IDictionary, T>(CommandType.Text, sp, dic);
		}

		static public Tuple<List<T1>, List<T2>> StringEntityListEx<T1, T2>(string name, string sp, IDictionary dic)
			where T1 : new()
			where T2 : new()
		{
			DatabaseEx db = Create(name);

			return db.EntityList<IDictionary, T1, T2>(CommandType.Text, sp, dic);
		}

		static public Tuple<List<T1>, List<T2>, List<T3>> StringEntityListEx<T1, T2, T3>(string name, string sp, IDictionary dic)
			where T1 : new()
			where T2 : new()
			where T3 : new()
		{
			DatabaseEx db = Create(name);

			return db.EntityList<IDictionary, T1, T2, T3>(CommandType.Text, sp, dic);
		}

		static public Tuple<List<T1>, List<T2>, List<T3>, List<T4>> StringEntityListEx<T1, T2, T3, T4>(string name, string sp, IDictionary dic)
			where T1 : new()
			where T2 : new()
			where T3 : new()
			where T4 : new()
		{
			DatabaseEx db = Create(name);

			return db.EntityList<IDictionary, T1, T2, T3, T4>(CommandType.Text, sp, dic);
		}

		static public Tuple<List<T1>, List<T2>, List<T3>, List<T4>, List<T5>> StringEntityListEx<T1, T2, T3, T4, T5>(string name, string sp, IDictionary dic)
			where T1 : new()
			where T2 : new()
			where T3 : new()
			where T4 : new()
			where T5 : new()
		{
			DatabaseEx db = Create(name);

			return db.EntityList<IDictionary, T1, T2, T3, T4, T5>(CommandType.Text, sp, dic);
		}

		static public Tuple<List<T1>, List<T2>, List<T3>, List<T4>, List<T5>, List<T6>> StringEntityListEx<T1, T2, T3, T4, T5, T6>(string name, string sp, IDictionary dic)
			where T1 : new()
			where T2 : new()
			where T3 : new()
			where T4 : new()
			where T5 : new()
			where T6 : new()
		{
			DatabaseEx db = Create(name);

			return db.EntityList<IDictionary, T1, T2, T3, T4, T5, T6>(CommandType.Text, sp, dic);
		}

		static public Tuple<List<T1>, List<T2>, List<T3>, List<T4>, List<T5>, List<T6>, List<T7>> StringEntityListEx<T1, T2, T3, T4, T5, T6, T7>(string name, string sp, IDictionary dic)
			where T1 : new()
			where T2 : new()
			where T3 : new()
			where T4 : new()
			where T5 : new()
			where T6 : new()
			where T7 : new()
		{
			DatabaseEx db = Create(name);

			return db.EntityList<IDictionary, T1, T2, T3, T4, T5, T6, T7>(CommandType.Text, sp, dic);
		}

		static public Tuple<List<T1>, List<T2>, List<T3>, List<T4>, List<T5>, List<T6>, List<T7>, List<List<TRest>>> StringEntityListEx<T1, T2, T3, T4, T5, T6, T7, TRest>(string name, string sp, IDictionary dic)
			where T1 : new()
			where T2 : new()
			where T3 : new()
			where T4 : new()
			where T5 : new()
			where T6 : new()
			where T7 : new()
			where TRest : new()
		{
			DatabaseEx db = Create(name);

			return db.EntityList<IDictionary, T1, T2, T3, T4, T5, T6, T7, TRest>(CommandType.Text, sp, dic);
		}

		static public List<T> StringEntityList<T>(string sp, IDictionary dic)
			where T : new()
		{
			return StringEntityListEx<T>(null, sp, dic);
		}

		static public Tuple<List<T1>, List<T2>> StringEntityList<T1, T2>(string sp, IDictionary dic)
			where T1 : new()
			where T2 : new()
		{
			return StringEntityListEx<T1, T2>(null, sp, dic);
		}

		static public Tuple<List<T1>, List<T2>, List<T3>> StringEntityList<T1, T2, T3>(string sp, IDictionary dic)
			where T1 : new()
			where T2 : new()
			where T3 : new()
		{
			return StringEntityListEx<T1, T2, T3>(null, sp, dic);
		}

		static public Tuple<List<T1>, List<T2>, List<T3>, List<T4>> StringEntityList<T1, T2, T3, T4>(string sp, IDictionary dic)
			where T1 : new()
			where T2 : new()
			where T3 : new()
			where T4 : new()
		{
			return StringEntityListEx<T1, T2, T3, T4>(null, sp, dic);
		}

		static public Tuple<List<T1>, List<T2>, List<T3>, List<T4>, List<T5>> StringEntityList<T1, T2, T3, T4, T5>(string sp, IDictionary dic)
			where T1 : new()
			where T2 : new()
			where T3 : new()
			where T4 : new()
			where T5 : new()
		{
			return StringEntityListEx<T1, T2, T3, T4, T5>(null, sp, dic);
		}

		static public Tuple<List<T1>, List<T2>, List<T3>, List<T4>, List<T5>, List<T6>> StringEntityList<T1, T2, T3, T4, T5, T6>(string sp, IDictionary dic)
			where T1 : new()
			where T2 : new()
			where T3 : new()
			where T4 : new()
			where T5 : new()
			where T6 : new()
		{
			return StringEntityListEx<T1, T2, T3, T4, T5, T6>(null, sp, dic);
		}

		static public Tuple<List<T1>, List<T2>, List<T3>, List<T4>, List<T5>, List<T6>, List<T7>> StringEntityList<T1, T2, T3, T4, T5, T6, T7>(string sp, IDictionary dic)
			where T1 : new()
			where T2 : new()
			where T3 : new()
			where T4 : new()
			where T5 : new()
			where T6 : new()
			where T7 : new()
		{
			return StringEntityListEx<T1, T2, T3, T4, T5, T6, T7>(null, sp, dic);
		}

		static public Tuple<List<T1>, List<T2>, List<T3>, List<T4>, List<T5>, List<T6>, List<T7>, List<List<TRest>>> StringEntityList<T1, T2, T3, T4, T5, T6, T7, TRest>(string sp, IDictionary dic)
			where T1 : new()
			where T2 : new()
			where T3 : new()
			where T4 : new()
			where T5 : new()
			where T6 : new()
			where T7 : new()
			where TRest : new()
		{
			return StringEntityListEx<T1, T2, T3, T4, T5, T6, T7, TRest>(null, sp, dic);
		}

		static public T StringBaseEntity<T>(string name, string sp, IDictionary dic)
			where T : new()
		{
			DatabaseEx db = Create(name);
			var rtn = db.EntityList<IDictionary, T>(CommandType.Text, sp, dic);

			if (rtn.Count() > 0)
				return rtn.First();
			else
				return default(T);
		}

		static public TResult StringEntityEx<TResult>(string name, string sp, IDictionary dic)
			where TResult : new()
		{
			return StringBaseEntity<TResult>(name, sp, dic);
		}

		static public TResult StringEntity<TResult>(string sp, IDictionary dic)
			where TResult : new()
		{
			return StringEntityEx<TResult>(null, sp, dic);
		}

		#endregion


		#region StringEntity - entity

		static public List<T> StringEntityListEx<T>(string name, string sp, BaseEntity entity)
			where T : new()
		{
			DatabaseEx db = Create(name);

			return db.EntityList<BaseEntity, T>(CommandType.Text, sp, entity);
		}

		static public Tuple<List<T1>, List<T2>> StringEntityListEx<T1, T2>(string name, string sp, BaseEntity entity)
			where T1 : new()
			where T2 : new()
		{
			DatabaseEx db = Create(name);

			return db.EntityList<BaseEntity, T1, T2>(CommandType.Text, sp, entity);
		}

		static public Tuple<List<T1>, List<T2>, List<T3>> StringEntityListEx<T1, T2, T3>(string name, string sp, BaseEntity entity)
			where T1 : new()
			where T2 : new()
			where T3 : new()
		{
			DatabaseEx db = Create(name);

			return db.EntityList<BaseEntity, T1, T2, T3>(CommandType.Text, sp, entity);
		}

		static public Tuple<List<T1>, List<T2>, List<T3>, List<T4>> StringEntityListEx<T1, T2, T3, T4>(string name, string sp, BaseEntity entity)
			where T1 : new()
			where T2 : new()
			where T3 : new()
			where T4 : new()
		{
			DatabaseEx db = Create(name);

			return db.EntityList<BaseEntity, T1, T2, T3, T4>(CommandType.Text, sp, entity);
		}

		static public Tuple<List<T1>, List<T2>, List<T3>, List<T4>, List<T5>> StringEntityListEx<T1, T2, T3, T4, T5>(string name, string sp, BaseEntity entity)
			where T1 : new()
			where T2 : new()
			where T3 : new()
			where T4 : new()
			where T5 : new()
		{
			DatabaseEx db = Create(name);

			return db.EntityList<BaseEntity, T1, T2, T3, T4, T5>(CommandType.Text, sp, entity);
		}

		static public Tuple<List<T1>, List<T2>, List<T3>, List<T4>, List<T5>, List<T6>> StringEntityListEx<T1, T2, T3, T4, T5, T6>(string name, string sp, BaseEntity entity)
			where T1 : new()
			where T2 : new()
			where T3 : new()
			where T4 : new()
			where T5 : new()
			where T6 : new()
		{
			DatabaseEx db = Create(name);

			return db.EntityList<BaseEntity, T1, T2, T3, T4, T5, T6>(CommandType.Text, sp, entity);
		}

		static public Tuple<List<T1>, List<T2>, List<T3>, List<T4>, List<T5>, List<T6>, List<T7>> StringEntityListEx<T1, T2, T3, T4, T5, T6, T7>(string name, string sp, BaseEntity entity)
			where T1 : new()
			where T2 : new()
			where T3 : new()
			where T4 : new()
			where T5 : new()
			where T6 : new()
			where T7 : new()
		{
			DatabaseEx db = Create(name);

			return db.EntityList<BaseEntity, T1, T2, T3, T4, T5, T6, T7>(CommandType.Text, sp, entity);
		}

		static public Tuple<List<T1>, List<T2>, List<T3>, List<T4>, List<T5>, List<T6>, List<T7>, List<List<TRest>>> StringEntityListEx<T1, T2, T3, T4, T5, T6, T7, TRest>(string name, string sp, BaseEntity entity)
			where T1 : new()
			where T2 : new()
			where T3 : new()
			where T4 : new()
			where T5 : new()
			where T6 : new()
			where T7 : new()
			where TRest : new()
		{
			DatabaseEx db = Create(name);

			return db.EntityList<BaseEntity, T1, T2, T3, T4, T5, T6, T7, TRest>(CommandType.Text, sp, entity);
		}

		static public List<T> StringEntityList<T>(string sp, BaseEntity entity)
			where T : new()
		{
			return StringEntityListEx<T>(null, sp, entity);
		}

		static public Tuple<List<T1>, List<T2>> StringEntityList<T1, T2>(string sp, BaseEntity entity)
			where T1 : new()
			where T2 : new()
		{
			return StringEntityListEx<T1, T2>(null, sp, entity);
		}

		static public Tuple<List<T1>, List<T2>, List<T3>> StringEntityList<T1, T2, T3>(string sp, BaseEntity entity)
			where T1 : new()
			where T2 : new()
			where T3 : new()
		{
			return StringEntityListEx<T1, T2, T3>(null, sp, entity);
		}

		static public Tuple<List<T1>, List<T2>, List<T3>, List<T4>> StringEntityList<T1, T2, T3, T4>(string sp, BaseEntity entity)
			where T1 : new()
			where T2 : new()
			where T3 : new()
			where T4 : new()
		{
			return StringEntityListEx<T1, T2, T3, T4>(null, sp, entity);
		}

		static public Tuple<List<T1>, List<T2>, List<T3>, List<T4>, List<T5>> StringEntityList<T1, T2, T3, T4, T5>(string sp, BaseEntity entity)
			where T1 : new()
			where T2 : new()
			where T3 : new()
			where T4 : new()
			where T5 : new()
		{
			return StringEntityListEx<T1, T2, T3, T4, T5>(null, sp, entity);
		}

		static public Tuple<List<T1>, List<T2>, List<T3>, List<T4>, List<T5>, List<T6>> StringEntityList<T1, T2, T3, T4, T5, T6>(string sp, BaseEntity entity)
			where T1 : new()
			where T2 : new()
			where T3 : new()
			where T4 : new()
			where T5 : new()
			where T6 : new()
		{
			return StringEntityListEx<T1, T2, T3, T4, T5, T6>(null, sp, entity);
		}

		static public Tuple<List<T1>, List<T2>, List<T3>, List<T4>, List<T5>, List<T6>, List<T7>> StringEntityList<T1, T2, T3, T4, T5, T6, T7>(string sp, BaseEntity entity)
			where T1 : new()
			where T2 : new()
			where T3 : new()
			where T4 : new()
			where T5 : new()
			where T6 : new()
			where T7 : new()
		{
			return StringEntityListEx<T1, T2, T3, T4, T5, T6, T7>(null, sp, entity);
		}

		static public Tuple<List<T1>, List<T2>, List<T3>, List<T4>, List<T5>, List<T6>, List<T7>, List<List<TRest>>> StringEntityList<T1, T2, T3, T4, T5, T6, T7, TRest>(string sp, BaseEntity entity)
			where T1 : new()
			where T2 : new()
			where T3 : new()
			where T4 : new()
			where T5 : new()
			where T6 : new()
			where T7 : new()
			where TRest : new()
		{
			return StringEntityListEx<T1, T2, T3, T4, T5, T6, T7, TRest>(null, sp, entity);
		}

		static public T StringBaseEntity<T>(string name, string sp, BaseEntity entity)
			where T : new()
		{
			DatabaseEx db = Create(name);
			var rtn = db.EntityList<BaseEntity, T>(CommandType.Text, sp, entity);

			if (rtn.Count() > 0)
				return rtn.First();
			else
				return default(T);
		}

		static public TResult StringEntityEx<TResult>(string name, string sp, BaseEntity entity)
			where TResult : new()
		{
			return StringBaseEntity<TResult>(name, sp, entity);
		}

		static public TResult StringEntity<TResult>(string sp, BaseEntity entity)
			where TResult : new()
		{
			return StringEntityEx<TResult>(null, sp, entity);
		}

		#endregion

		static public void ClearParameterCache()
		{
			Database.ClearParameterCache();
		}

		static public DatabaseEx Create(string name = null)
		{
			return new DatabaseEx(GetConnection(name), _sqlCache);
		}

		static public string GetConnection(string name)
		{
			if (string.IsNullOrWhiteSpace(name))
				name = _configuration.GetSection(BaseStatic.DefaultDBConfigSection).GetSection(BaseStatic.DefaultDBValueSection).Value;


			return _configuration.GetConnectionString(name);
		}
		

	}
}
