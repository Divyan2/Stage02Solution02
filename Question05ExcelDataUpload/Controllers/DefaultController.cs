using Libraries.Interface;
using Libraries.Repository;
using Libraries.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Libraries.Model;
using ClosedXML.Excel;
using System.IO;
using DocumentFormat.OpenXml.EMMA;
using Question05ExcelDataUpload.Models;

namespace Question05ExcelDataUpload.Controllers
{
    public class DefaultController : Controller
    {
        private readonly IDatabaseRepository _repo;

        public DefaultController()
        {
            _repo = new ExcelRepository(new DefaultDbContext());
        }
        public DefaultController(IDatabaseRepository repo)
        {
            _repo = repo;
        }

        public ActionResult Index(ViewModel model)
        {
            if (model.ErrorMessage != "" && model.ErrorMessage != null)
            {
                ViewBag.errormessage = model.ErrorMessage;
            }
            if (model.SuccessMessage != "" && model.SuccessMessage != null)
            {
                ViewBag.SuccessMessage = model.SuccessMessage;
            }
            var data = _repo.GetAllExcelData();
            return View(data);      
        }

        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase file)
        {
            try
            {
                var errormodel = new ViewModel();
                if (file == null || Path.GetExtension(file.FileName).ToLower() != ".xlsx")
                {
                    ViewBag.ErrorMessage = "Only Excel files are allowed.";
                    return RedirectToAction("Index",errormodel);
                }
                if (file != null && file.ContentLength > 0)
                {
                    var DataList = ParseFile(file);

                    foreach (var model in DataList)
                    {
                        _repo.UpdateExcelData(model);
                    }
                }
                errormodel.SuccessMessage = "CSV file uploaded successfully!";
                return RedirectToAction("Index", errormodel);
            }
            catch (Exception ex)
            {

                var model = new ViewModel();
                model.ErrorMessage = $"Error during Excel upload: {ex.Message}";
                return RedirectToAction("Index", model);
            }
        }

        private List<ExcelDataModel> ParseFile(HttpPostedFileBase file)
        {
            var errormodel = new ViewModel();
            var DataList = new List<ExcelDataModel>();

            using (var workbook = new XLWorkbook(file.InputStream))
            {
                var worksheet = workbook.Worksheet(1);

                int rowCount = worksheet.RowsUsed().Count();

                for (int row = 2; row <= rowCount; row++)
                {
                    try
                    {
                        var ecxeldata = new ExcelDataModel
                        {
                            Id = int.Parse(worksheet.Cell(row, 1).Value.ToString()),
                            FirstName = worksheet.Cell(row, 2).Value.ToString(),
                            LastName = worksheet.Cell(row, 3).Value.ToString(),
                            Gender = worksheet.Cell(row, 4).Value.ToString(),
                            Country = worksheet.Cell(row, 5).Value.ToString(),
                            Age = int.Parse(worksheet.Cell(row, 6).Value.ToString())
                        };

                        DataList.Add(ecxeldata);
                    }
                    catch (Exception ex)
                    {
                        
                        ViewBag.ErrorMessage = $"Error parsing data in row {row}. Please check your file.";
                        return DataList;
                    }
                }
            }
            return DataList;
        }

        public ActionResult DownloadFile()
        {
            var model = new ViewModel();
            try
            {
                var data = _repo.GetAllExcelData();

                var fileStream = GenerateExcelFile(data);

                var fileName = "ExcelData.xlsx";
                var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                return File(fileStream, contentType, fileName);
            }
            catch (Exception ex)
            {

                model.ErrorMessage = $"Error during Excelfile download: {ex.Message}";
                return RedirectToAction("Index", model);
            }
        }

        private MemoryStream GenerateExcelFile(IEnumerable<ExcelDataModel> data)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("PeopleData");

                worksheet.Cell(1, 1).Value = "Id";
                worksheet.Cell(1, 2).Value = "FirstName";
                worksheet.Cell(1, 3).Value = "LastName";
                worksheet.Cell(1, 4).Value = "Gender";
                worksheet.Cell(1, 5).Value = "Country";
                worksheet.Cell(1, 6).Value = "Age";

                int row = 2;
                foreach (var exceldata in data)
                {
                    worksheet.Cell(row, 1).Value = exceldata.Id;
                    worksheet.Cell(row, 2).Value = exceldata.FirstName;
                    worksheet.Cell(row, 3).Value = exceldata.LastName;
                    worksheet.Cell(row, 4).Value = exceldata.Gender;
                    worksheet.Cell(row, 5).Value = exceldata.Country;
                    worksheet.Cell(row, 6).Value = exceldata.Age;
                    row++;
                }

                var memoryStream = new MemoryStream();
                workbook.SaveAs(memoryStream);

                memoryStream.Position = 0;

                return memoryStream;
            }
        }
    }
}