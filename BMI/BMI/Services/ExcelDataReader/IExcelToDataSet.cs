using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace BMI.Services.ExcelDataReader
{
    public interface IExcelToDataSet
    {
        /// <summary>
        /// Преобразует файл в набор данных типа DataSet
        /// </summary>
        /// <param name="path">Путь до файла</param>
        /// <returns></returns>
        DataSet ToDataSet(string path);
    }
}