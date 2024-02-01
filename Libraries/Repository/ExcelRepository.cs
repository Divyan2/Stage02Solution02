using Libraries.Data;
using Libraries.Interface;
using Libraries.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libraries.Repository
{
    public class ExcelRepository : IDatabaseRepository
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

        public bool ChangePassword(string userName, int securityCode, string securityAnswer, string newPassword)
        {
            throw new NotImplementedException();
        }

        public void DeleteUser(string userId)
        {
            throw new NotImplementedException();
        }

        public void DeleteUserAndStudent(string userId, string studentId)
        {
            throw new NotImplementedException();
        }

        public List<ExcelDataModel> GetAllExcelData()
        {
            return _context.ExcelData.ToList();
        }

        public List<StudentTable> GetAllStudents()
        {
            throw new NotImplementedException();
        }

        public List<UserLoginAndSecurityTable> GetAllUsers()
        {
            throw new NotImplementedException();
        }

        public List<UserAndStudentViewModel> GetAllUsersAndStudents()
        {
            throw new NotImplementedException();
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

        public List<SecurityQuestionTable> GetSecurityQuestions()
        {
            throw new NotImplementedException();
        }

        public StudentTable GetStudentById(string userID)
        {
            throw new NotImplementedException();
        }

        public UserLoginAndSecurityTable GetUserById(string userId)
        {
            throw new NotImplementedException();
        }

        public UserLoginAndSecurityTable GetUserByUserName(string userName)
        {
            throw new NotImplementedException();
        }

        public bool LoginAdmin(string userName, string password)
        {
            throw new NotImplementedException();
        }

        public bool LoginUser(string userName, string password)
        {
            throw new NotImplementedException();
        }

        public void RegisterUser(UserLoginAndSecurityTable user, StudentTable student)
        {
            throw new NotImplementedException();
        }

        public void UpdateExcelData(ExcelDataModel data)
        {
            _context.ExcelData.Add(data);
            _context.SaveChanges();
        }

        public void UpdateUser(UserLoginAndSecurityTable user, StudentTable student)
        {
            throw new NotImplementedException();
        }
    }
}
