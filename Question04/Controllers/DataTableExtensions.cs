using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Question04.Controllers
{
    public static class DataTableExtensions
    {
        public static DataTable ToDataTable<T>(this IEnumerable<T> items)
        {
            try
            {
                var dataTable = new DataTable(typeof(T).Name);

                var headers = typeof(T).GetProperties().Select(propertyInfo => propertyInfo.Name).ToArray();

                foreach (var header in headers)
                {
                    dataTable.Columns.Add(new DataColumn(header));
                }

                foreach (var item in items)
                {
                    var values = new object[headers.Length];

                    for (var i = 0; i < values.Length; i++)
                    {
                        values[i] = typeof(T).GetProperties()[i].GetValue(item);
                    }

                    dataTable.Rows.Add(values);
                }

                return dataTable;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error during DataTable conversion: " + ex.Message, ex);
            }
        }
    }
}