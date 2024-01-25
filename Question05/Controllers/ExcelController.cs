using Libraries.Repository;
using Libraries.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Libraries.Model;
using System.IO;
using OfficeOpenXml;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.EMMA;
using Question05.Models;

namespace Question05.Controllers
{
    public class ExcelController : Controller
    {
        private readonly ExcelRepository _repo;

        public ExcelController()
        {
            _repo = new ExcelRepository(new DefaultDbContext());
        }
        public ExcelController(ExcelRepository excelRepository)
        {
            _repo = excelRepository;
        }
        // GET: Excel
        public ActionResult Upload(ViewModel model)
        {
            if (model.ErrorMessage != "" && model.ErrorMessage != null)
            {
                ViewBag.errormessage = model.ErrorMessage;
            }
            if (model.SuccessMessage != "" && model.SuccessMessage != null)
            {
                ViewBag.SuccessMessage = model.SuccessMessage;
            }
            return View();
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            var model = new ViewModel();
            if(file == null)
            {
                model.ErrorMessage = "Please a select a Excel file...";
                return RedirectToAction("Upload", model);
            }
            if (Path.GetExtension(file.FileName).ToLower() != ".xlsx")
            {
                model.ErrorMessage = "Only xlsx files are allowed.";
                return RedirectToAction("Upload", model);
            }
            if (file != null && file.ContentLength > 0)
            {
                var excelFile = new ExcelFileModel
                {
                    Filename = Path.GetFileName(file.FileName)
                };

                using (var reader = new BinaryReader(file.InputStream))
                {
                    excelFile.Files = reader.ReadBytes(file.ContentLength);
                }

                _repo.AddExcelFile(excelFile);
            }
            model.SuccessMessage = "File Updated Successfully.";
            return RedirectToAction("Upload", model);
        }

        [HttpGet]
        public ActionResult Download()
        {
            var model = new ViewModel();
            try
            {
                var file = _repo.GetExcelFile();

                if (file != null)
                {
                    return File(file.Files, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", file.Filename);
                }

                return HttpNotFound();
            }
            catch (Exception ex)
            {
                model.ErrorMessage = $"Error during file download: {ex.Message}";
                return RedirectToAction("Upload", model);
            }
        }

        [HttpGet]
        public JsonResult GetExcelData()
        {
            try
            {
                var file = _repo.GetExcelFile();

                if (file != null)
                {
                    var excelData = ProcessExcelFile(file.Files);
                    return Json(excelData, JsonRequestBehavior.AllowGet);
                }

                return Json(new { error = "File not found" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        private List<List<string>> ProcessExcelFile(byte[] excelFileData)
        {
            using (MemoryStream memoryStream = new MemoryStream(excelFileData))
            {
                using (XLWorkbook workbook = new XLWorkbook(memoryStream))
                {
                    IXLWorksheet worksheet = workbook.Worksheets.FirstOrDefault();

                    if (worksheet != null)
                    {
                        List<List<string>> excelData = new List<List<string>>();

                        foreach (var row in worksheet.RowsUsed())
                        {
                            List<string> rowData = new List<string>();

                            foreach (var cell in row.Cells())
                            {
                                rowData.Add(cell.Value.ToString());
                            }

                            excelData.Add(rowData);
                        }

                        return excelData;
                    }
                }
            }

            return new List<List<string>>();
        }
    }
}