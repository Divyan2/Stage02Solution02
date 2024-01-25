using CsvHelper.Configuration;
using CsvHelper;
using Libraries.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Globalization;

namespace Question04.Controllers
{
    public static class CsvExtensions
    {
        public static string ToCsv(this IEnumerable<DynamicCsvDataModel> items)
        {
            try
            {
                using (var writer = new StringWriter())
                using (var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)))
                {
                    csv.Context.RegisterClassMap<DynamicCsvDataModelMap>();
                    csv.WriteRecords(items);
                    return writer.ToString();
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error during CSV conversion: " + ex.Message, ex);
            }

        }
    }

    public sealed class DynamicCsvDataModelMap : ClassMap<DynamicCsvDataModel>
    {
        public DynamicCsvDataModelMap()
        {
            Map(m => m.Username).Name("Username");
            Map(m => m.Identifier).Name("Identifier");
            Map(m => m.FirstName).Name("First name");
            Map(m => m.LastName).Name("Last name");
        }
    }
}