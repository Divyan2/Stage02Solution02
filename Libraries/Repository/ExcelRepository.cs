using Libraries.Data;
using Libraries.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libraries.Repository
{
    public class ExcelRepository
    {
        private readonly DefaultDbContext _context;

        public ExcelRepository()
        {
            _context = new DefaultDbContext();
        }

        public ExcelRepository(DefaultDbContext context)
        {
            _context = context;
        }

        public void AddExcelFile(ExcelFileModel excelFile)
        {
            _context.ExcelFiles.Add(excelFile);
            _context.SaveChanges();
        }

        public ExcelFileModel GetExcelFile()
        {
            return _context.ExcelFiles.OrderByDescending(e => e.Id).FirstOrDefault();
        }

        public ExcelFileModel GetLatestExcelFile()
        {
            return _context.ExcelFiles
                .OrderByDescending(x => x.Id)
                .FirstOrDefault();
        }
    }
}
