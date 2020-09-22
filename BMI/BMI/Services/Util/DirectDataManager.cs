using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;

namespace BMI.Services.Util
{
    /// <summary>
	/// Менеджер прямого доступа к данным 
	/// </summary>
	public class DirectDataManager
    {
        /// <summary>
        /// Соединение
        /// </summary>
        private readonly SqlConnection _connection;
        /// <summary>
        /// Команда
        /// </summary>
        private readonly SqlCommand _sqlCommand;
        /// <summary>
        /// Запрос к БД
        /// </summary>
        private readonly string _query;
        /// <summary>
        /// Адаптер данных
        /// </summary>
        private readonly SqlDataAdapter _dataAdapter;
        /// <summary>
        /// Набор данных
        /// </summary>
        private readonly DataSet _dataSet;
        private static T CreateItemFromRow<T>(DataRow row, IEnumerable<PropertyInfo> properties) where T : new()
        {
            var item = new T();
            foreach (var property in properties.Where(property => row.Table.Columns.Contains(property.Name)).Where(property => row[property.Name].GetType() != typeof(DBNull)))
            {
                property.SetValue(item, row[property.Name], null);
            }
            return item;
        }
        /// <summary>
        /// Параметры запроса
        /// </summary>
        public List<SqlParameter> SqlParameters;
        /// <summary>
        /// Конструктор
        /// </summary>
        public DirectDataManager()
        {
            _connection = new SqlConnection(DirectDataManagerConnection.ConnectionString);
            _sqlCommand = new SqlCommand(_query, _connection);
            _dataAdapter = new SqlDataAdapter(_sqlCommand);
            _dataSet = new DataSet();
        }
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="query"></param>
        public DirectDataManager(string query)
        {
            _query = query;
            _connection = new SqlConnection(DirectDataManagerConnection.ConnectionString);
            _sqlCommand = new SqlCommand(_query, _connection);
            _dataAdapter = new SqlDataAdapter(_sqlCommand);
            _dataSet = new DataSet();
        }
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="query"></param>
        /// <param name="connectionString"></param>
        public DirectDataManager(string query, string connectionString)
        {
            _query = query;
            _connection = new SqlConnection(DirectDataManagerConnection.GetConnectionString(connectionString));
            _sqlCommand = new SqlCommand(_query, _connection);
            _dataAdapter = new SqlDataAdapter(_sqlCommand);
            _dataSet = new DataSet();
        }
        /// <summary>
        /// Набор данных
        /// </summary>
        /// <returns></returns>
        public DataSet DataSet()
        {
            using (_connection)
            {
                using (_sqlCommand)
                {
                    _connection.CreateCommand();
                    if (SqlParameters != null && SqlParameters.Count > 0)
                    {
                        foreach (var sqlParameter in SqlParameters)
                        {
                            _sqlCommand.Parameters.Add(sqlParameter);
                        }
                    }
                    _sqlCommand.CommandText = string.Format(CultureInfo.InvariantCulture, _query);
                    _connection.Open();
                    _dataAdapter.Fill(_dataSet);
                    return _dataSet;
                }
            }
        }
        /// <summary>
        /// Набор данных
        /// </summary>
        /// <param name="sqlParameters"></param>
        /// <returns></returns>
        public DataSet DataSet(List<SqlParameter> sqlParameters)
        {
            using (_connection)
            {
                using (_sqlCommand)
                {
                    _connection.CreateCommand();
                    if (sqlParameters != null)
                    {
                        foreach (var sqlParameter in sqlParameters)
                        {
                            _sqlCommand.Parameters.Add(sqlParameter);
                        }
                    }
                    _sqlCommand.CommandText = string.Format(CultureInfo.InvariantCulture, _query);
                    _connection.Open();
                    _dataAdapter.Fill(_dataSet);
                    return _dataSet;
                }
            }
        }
        /// <summary>
        /// Выполнить запрос
        /// </summary>
        public void ExecuteQuery()
        {
            using (_connection)
            {
                _connection.Open();
                using (_sqlCommand)
                {
                    _connection.CreateCommand();
                    _sqlCommand.CommandText = _query;
                    _sqlCommand.ExecuteNonQuery();
                }
            }
        }
        /// <summary>
        /// Массив строк
        /// </summary>
        /// <returns></returns>
        public ArrayList Rows()
        {
            using (_connection)
            {
                using (_sqlCommand)
                {
                    _connection.CreateCommand();
                    _sqlCommand.CommandText = string.Format(CultureInfo.InvariantCulture, _query);
                    _connection.Open();
                    _dataAdapter.Fill(_dataSet);
                    var rows = new ArrayList();
                    foreach (DataRow dataRow in _dataSet.Tables[0].Rows)
                    {
                        rows.Add(string.Join(";", dataRow.ItemArray.Select(item => item.ToString())));
                    }
                    return rows;
                }
            }
        }
        /// <summary>
        /// DataView
        /// </summary>
        /// <returns></returns>
        public DataView DataView(DataSet dataSet)
        {
            return dataSet.Tables[0].DefaultView;
        }
        public DataView DataView()
        {
            return DataSet().Tables[0].DefaultView;
        }
        /// <summary>
        /// DataTale
        /// </summary>
        /// <returns></returns>
        public DataTable DataTable()
        {
            return _dataSet.Tables[0];
        }
        /// <summary>
        /// DataTale
        /// </summary>
        /// <param name="dataset"></param>
        /// <returns></returns>
        public DataTable DataTable(DataSet dataset)
        {
            return dataset.Tables[0];
        }
        public IEnumerable<T> ToList<T>() where T : new()
        {
            DataSet();
            IList<PropertyInfo> properties = typeof(T).GetProperties().ToList();
            return (from object row in DataTable().Rows select CreateItemFromRow<T>((DataRow)row, properties));
        }
        public List<T> ToList<T>(DataSet dataset) where T : new()
        {

            IList<PropertyInfo> properties = typeof(T).GetProperties().ToList();
            var result = new List<T>();

            foreach (var row in DataTable(dataset).Rows)
            {
                var item = CreateItemFromRow<T>((DataRow)row, properties);
                result.Add(item);
            }

            return result;
        }

        public object SingleObject(string colName)
        {

            DataView dv = null;

            dv = DataView();

            if (dv != null && dv.Count == 1)
            {
                var val = dv[0].Row[colName];
                return val;
            }


            return null;
        }
    }
}