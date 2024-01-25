using Libraries.Model;
using Libraries.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libraries.Interface
{
    public interface IDatabaseRepository
    {
        //User read-only
        UserLoginAndSecurityTable GetUserById(string userId);

        //Admin Read,Update and Delete
        void DeleteUserAndStudent(string userId, string studentId);
        List<UserAndStudentViewModel> GetAllUsersAndStudents();
        List<UserLoginAndSecurityTable> GetAllUsers();
        List<StudentTable> GetAllStudents();
        void UpdateUser(UserLoginAndSecurityTable user, StudentTable student);
        void DeleteUser(string userId);
        UserLoginAndSecurityTable GetUserByUserName(string userName);
        // User Registration and Login
        void RegisterUser(UserLoginAndSecurityTable user, StudentTable student);
        bool LoginUser(string userName, string password);
        bool ChangePassword(string userName, int securityCode, string securityAnswer,string newPassword);
        List<SecurityQuestionTable> GetSecurityQuestions();
        StudentTable GetStudentById(string userID);
        bool LoginAdmin(string userName, string password);
    }
}
