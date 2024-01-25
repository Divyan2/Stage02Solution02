using Libraries.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libraries.Data
{
    public class DefaultDbContext : DbContext
    {
        public DefaultDbContext() : base("DBCS")
        {
            Database.SetInitializer<DefaultDbContext>(null);
        }

        public DbSet<StudentTable> Students { get; set; }
        public DbSet<AdminTable> Admins { get; set; }
        public DbSet<SecurityQuestionTable> SecurityQuestions { get; set; }
        public DbSet<UserLoginAndSecurityTable> Users { get; set; }
        public DbSet<ExcelFileModel> ExcelFiles { get; set; }
    }
}
