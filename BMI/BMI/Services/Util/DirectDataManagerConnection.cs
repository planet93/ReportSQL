using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace BMI.Services.Util
{
    internal class DirectDataManagerConnection
    {
        /// <summary>
        /// Строка соединения, используемая по умолчанию
        /// </summary>
        private const string DbConnectionStringDefault = "DataContext";
        /// <summary>
        /// Получение строки соединения
        /// </summary>
        /// <returns></returns>
        public static string ConnectionString => ConfigurationManager.ConnectionStrings[DbConnectionStringDefault].ConnectionString;

        /// <summary>
        /// Получение строки соединения
        /// </summary>
        /// <param name="connectionStringName">Название строки соединения, из файла Web.Config</param>
        /// <returns></returns>
        public static string GetConnectionString(string connectionStringName)
        {
            return ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
        }
    }
}