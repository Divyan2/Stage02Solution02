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
    public class DatabaseRepository : IDatabaseRepository
    {
        private readonly DefaultDbContext _context;
        public DatabaseRepository()
        {
            _context = new DefaultDbContext();
        }
        public DatabaseRepository(DefaultDbContext context)
        {
            _context= context;
        }

        public List<UserAndStudentViewModel> GetAllUsersAndStudents()
        {
            var usersAndStudents = (from user in _context.Users
                                    join student in _context.Students on user.UserID equals student.ID
                                    select new UserAndStudentViewModel
                                    {
                                        Users = user,
                                        Students = student
                                    }).ToList();

            return usersAndStudents;
        }

        public void DeleteUserAndStudent(string userId, string studentId)
        {
            var user = _context.Users.SingleOrDefault(u => u.UserID == userId);
            var student = _context.Students.SingleOrDefault(s => s.ID == studentId);

            if (user != null && student != null)
            {
                _context.Users.Remove(user);
                _context.Students.Remove(student);
                _context.SaveChanges();
            }
        }

        public bool ChangePassword(string userName, int securityCode, string securityAnswer, string newPassword)
        {
            var user = _context.Users.SingleOrDefault(c => c.UserName == userName &&
                                                     c.SecurityAnswer == securityAnswer &&
                                                     c.QuestionCode == securityCode);

            if (user != null)
            { 
                user.Password = newPassword;
                _context.SaveChanges();
                return true;
            }

            return false;
        }

        public void DeleteUser(string userId)
        {
            var user = _context.Users.SingleOrDefault( u => u.UserID == userId);
            if(user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }

        public List<UserLoginAndSecurityTable> GetAllUsers()
        {
            return _context.Users.ToList();
        }

        public List<SecurityQuestionTable> GetSecurityQuestions()
        {
            return _context.SecurityQuestions.ToList();
        }

        public UserLoginAndSecurityTable GetUserById(string userId)
        {
            return _context.Users.SingleOrDefault(u => u.UserID == userId);
        }

        public StudentTable GetStudentById(string studentId)
        {
            return _context.Students.SingleOrDefault(s => s.ID == studentId);
        }

        public UserLoginAndSecurityTable GetUserByUserName(string userName)
        {
            return _context.Users.SingleOrDefault(u=> u.UserName == userName);
        }

        public bool LoginUser(string userName, string password)
        {
            var user = _context.Users.SingleOrDefault(x => x.UserName == userName && x.Password == password);
            return user != null;
        }

        public void RegisterUser(UserLoginAndSecurityTable user,StudentTable student)
        {
            if(_context.Users.Any(c => c.UserName == user.UserName))
            {
                throw new InvalidOperationException("Email is already registered");
            }

            var newUser = new UserLoginAndSecurityTable
            {
                UserID = user.UserID,
                UserName = user.UserName,
                Password = user.Password,
                QuestionCode = user.QuestionCode,
                SecurityAnswer = user.SecurityAnswer,
                Is_Active = true
            };

            var newStudent = new StudentTable
            {
                ID = student.ID,
                Name = student.Name,
                Age = student.Age,
                Address = student.Address,
                Is_Active = true,
            };
            _context.Students.Add(newStudent);
            _context.Users.Add(newUser);
            _context.SaveChanges();
        }

        public void UpdateUser(UserLoginAndSecurityTable user,StudentTable student)
        {
            var existuser = _context.Users.SingleOrDefault(u => u.UserID == user.UserID);
            var existstudent = _context.Students.SingleOrDefault(s => s.ID == student.ID);
            if (existuser != null && existstudent != null)
            {   //student context
                existstudent.Name = student.Name;
                existstudent.Age = student.Age; 
                existstudent.Address = student.Address;
                //user context
                existuser.UserName = user.UserName;
                existuser.Password = user.Password;
                existuser.QuestionCode = user.QuestionCode;
                existuser.SecurityAnswer = user.SecurityAnswer;

                _context.SaveChanges();
            }

            if (existuser != null)
            {
                existuser.UserName = user.UserName;
                existuser.Password = user.Password;
                existuser.QuestionCode = user.QuestionCode;
                existuser.SecurityAnswer = user.SecurityAnswer;

                _context.SaveChanges();
            }
        }

        public bool LoginAdmin(string userName, string password)
        {
            var user = _context.Admins.SingleOrDefault(x => x.AdminName == userName && x.Password == password);
            return user != null;
        }

        public List<StudentTable> GetAllStudents()
        {
            return _context.Students.ToList();
        }

        public List<ExcelDataModel> GetAllExcelData()
        {
            throw new NotImplementedException();
        }

        public void UpdateExcelData(ExcelDataModel data)
        {
            throw new NotImplementedException();
        }
    }
}
