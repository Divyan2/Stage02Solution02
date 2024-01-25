using Libraries.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.Web.UI.WebControls;
using Question04.Models;

namespace Question04.Controllers
{
    public class HomeController : Controller
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
        // GET: Home
        public ActionResult Index(ViewModel model)
        {
            if (model.ErrorMessage != "" && model.ErrorMessage !=null)
            {
                ViewBag.errormessage = model.ErrorMessage;
            }
            if(model.SuccessMessage!= "" && model.SuccessMessage != null)
            {
                ViewBag.SuccessMessage = model.SuccessMessage;
            }
            return View();
        }

        [HttpPost]
        public ActionResult UploadCsv(HttpPostedFileBase file)
        {
            try
            {
                var model = new ViewModel();
                if (file == null || file.ContentLength == 0)
                {
                    model.ErrorMessage = "Please select a file to upload.";
                    
                    return RedirectToAction("Index",model);
                }

                if (file == null || Path.GetExtension(file.FileName).ToLower() != ".csv")
                {
                    model.ErrorMessage = "Only CSV files are allowed.";
                    return RedirectToAction("Index", model);
                }

                using (var reader = new StreamReader(file.InputStream))
                {
                    var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture);
                    csvConfig.Delimiter = ",";
                    csvConfig.HasHeaderRecord = true;

                    var csvReader = new CsvReader(reader, csvConfig);

                    var records = csvReader.GetRecords<DynamicCsvDataModel>();

                    DataTable dataTable = records.ToDataTable();

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                        {
                            bulkCopy.DestinationTableName = "CsvDataTable";
                            bulkCopy.WriteToServer(dataTable);
                        }
                    }
                }

                model.SuccessMessage = "CSV file uploaded successfully!";
                return RedirectToAction("Index", model);
            }
            catch (Exception ex)
            {
                var model = new ViewModel();
                // Log the exception or handle it as needed
                model.ErrorMessage = $"Error during CSV upload: {ex.Message}";
                return RedirectToAction("Index", model);
            }
        }

        public ActionResult DownloadCsv()
        {
            var model = new ViewModel();
            try
            {
                List<DynamicCsvDataModel> csvData;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("SELECT * FROM CsvDataTable", connection))
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        var dataList = new List<DynamicCsvDataModel>();
                        while (reader.Read())
                        {
                            var dataModel = new DynamicCsvDataModel();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var columnName = reader.GetName(i);
                                var columnValue = reader.GetValue(i);

                                var property = dataModel.GetType().GetProperty(columnName);
                                if (property != null)
                                {
                                    if (property.PropertyType == typeof(int))
                                    {
                                        int intValue;
                                        if (int.TryParse(columnValue.ToString(), out intValue))
                                        {
                                            property.SetValue(dataModel, intValue);
                                        }
                                    }
                                    else
                                    {
                                        property.SetValue(dataModel, columnValue);
                                    }
                                }
                            }

                            dataList.Add(dataModel);
                        }

                        csvData = dataList;
                    }
                }

                string csvContent = csvData.ToCsv();

                byte[] fileBytes = System.Text.Encoding.UTF8.GetBytes(csvContent);
                return File(fileBytes, "text/csv", "CsvData.csv");
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                model.ErrorMessage = $"Error during CSV download: {ex.Message}";
                return RedirectToAction("Index", model);
            }
        }
    }
}