using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;

namespace BMI.Services.ExcelDataReader
{
    public sealed class ExcelDataAdapter : IExcelToDataSet
    {
        private readonly ExcelReaderConfiguration _excelReaderConfiguration;
        private readonly ExcelDataSetConfiguration _exelDataSetConfiguration;
        private DataSet _dataset;
        public ExcelDataAdapter(ExcelReaderConfiguration excelReaderConfiguration = null, ExcelDataSetConfiguration excelDataSetConfiguration = null)
        {
            //см. https://github.com/ExcelDataReader/ExcelDataReader
            _excelReaderConfiguration = excelReaderConfiguration;
            _exelDataSetConfiguration = excelDataSetConfiguration;
        }
        /// <summary>
        /// Получить поток FileStream из файла
        /// </summary>
        /// <param name="path">Полный путь к файлу</param>
        /// <returns></returns>
        private static FileStream GetFileStream(string path)
        {
            return File.Open(path, FileMode.Open, FileAccess.Read);
        }
        /// <summary>
        /// Создать объект читателя данных из файла Excel
        /// </summary>
        /// <param name="fileStream"></param>
        /// <returns></returns>
        private IExcelDataReader CreateExcelReader(Stream fileStream)
        {
            return ExcelReaderFactory.CreateReader(fileStream, _excelReaderConfiguration);
        }
        /// <inheritdoc />
        public DataSet ToDataSet(string path)
        {
            DataSet dateSet;
            using (var fileStream = GetFileStream(path))
            {
                // Автоматическое распознавание форматов. 
                // Поддерживаются:
                // - Binary Excel files (2.0-2003 format; *.xls)
                // - OpenXml Excel files (2007 format; *.xlsx)
                using (var reader = CreateExcelReader(fileStream))
                {
                    // 2. Use the AsDataSet extension method
                    dateSet = reader.AsDataSet(_exelDataSetConfiguration);
                }
            }
            return dateSet;
        }

        public List<T> ToList<T>(DataSet dataset) where T : new()
        {

            IList<PropertyInfo> properties = typeof(T).GetProperties().ToList();
            var result = new List<T>();

            foreach (var row in dataset.Tables[0].Rows)
            {
                var item = CreateItemFromRow<T>((DataRow)row, properties);
                result.Add(item);
            }

            return result;
        }

        public List<T> ToList<T>(string path) where T : new()
        {
            _dataset = ToDataSet(path);

            IList<PropertyInfo> properties = typeof(T).GetProperties().ToList();
            var result = new List<T>();

            foreach (var row in _dataset.Tables[0].Rows)
            {
                var item = CreateItemFromRow<T>((DataRow)row, properties);
                result.Add(item);
            }

            return result;
        }

        public List<string> GetColumns()
        {
            if (_dataset == null)
            {
                return null;
            }
            return _dataset.Tables[0].Columns.Cast<DataColumn>()
                .Select(x => x.ColumnName).ToList();
        }

        private static T CreateItemFromRow<T>(DataRow row, IEnumerable<PropertyInfo> properties) where T : new()
        {
            var item = new T();
            foreach (var property in properties.Where(property => row.Table.Columns.Contains(property.Name)).Where(property => row[property.Name].GetType() != typeof(DBNull)))
            {
                var value = row[property.Name].ToString().Trim();
                property.SetValue(item, value, null);
            }
            return item;
        }
    }
}