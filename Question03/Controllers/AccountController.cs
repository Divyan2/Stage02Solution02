using Libraries.Data;
using Libraries.Interface;
using Libraries.Model;
using Libraries.Repository;
using Question03.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Question03.Controllers
{
    public class AccountController : Controller
    {
        private readonly IDatabaseRepository _repo;
        private readonly DefaultDbContext _dbContext;
        public AccountController()
        {
            _repo = new DatabaseRepository(new DefaultDbContext());
            _dbContext = new DefaultDbContext();
        }

        //Generating Unique Id
        private string GenerateStudentId()
        {
            var lastStudent = _dbContext.Students
                        .OrderByDescending(s => s.ID)
                        .FirstOrDefault();

            if (lastStudent != null)
            {
                string lastId = lastStudent.ID.Substring(3);

                if (int.TryParse(lastId, out int lastIdValue))
                {
                    int nextId = lastIdValue + 1;
                    return "STD" + nextId.ToString("D7");
                }
                else
                {
                    //return "STD0001001";
                }
            }
            return "STD0001001";
        }

        //getting list of question to populate dropdown
        [HttpGet]
        public ActionResult GetSecurityQuestions()
        {
            try
            {
                var securityQuestions = _repo.GetSecurityQuestions().ToList();
                return Json(securityQuestions, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                
                return Json(new { error = "Error retrieving security questions." });
            }
        }

        //user login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model) 
        {
            if (ModelState.IsValid) 
            {
                var isAuthenticated  = _repo.LoginUser(model.Email, model.Password);    
                if (isAuthenticated)
                {
                    FormsAuthentication.SetAuthCookie(model.Email, false);
                    return RedirectToAction("Dashboard", "User");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid email or password");
                }
            }
            return View(model);
        }

        //admin login
        public ActionResult AdminLogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AdminLogin(AdminLoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var isAuthenticated = _repo.LoginAdmin(model.Name, model.Password);
                if (isAuthenticated)
                {
                    FormsAuthentication.SetAuthCookie(model.Name, false);
                    
                    if (!Roles.RoleExists("Admin"))
                    {
                        Roles.CreateRole("Admin");
                    }
                    if(!Roles.IsUserInRole(model.Name, "Admin"))
                    {
                        Roles.AddUserToRole(model.Name, "Admin");
                    }
                    return RedirectToAction("AdminDashboard", "Admin");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid admin name or password");
                }
            }
            return View(model);
        }

        //user register
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            //To-do = generate Id
            if (ModelState.IsValid)
            {
                try
                {
                    var studentid = GenerateStudentId();
                    var newStudent = new StudentTable
                    {
                        ID = studentid,
                        Name = model.Name,
                        Age = model.Age,
                        Address = model.Address,
                        Is_Active = true
                    };
                    var newUser = new UserLoginAndSecurityTable
                    {
                        UserID = studentid,
                        UserName = model.Email,
                        Password = model.Password,
                        QuestionCode = model.SecurityQuestionCode,
                        SecurityAnswer = model.SecurityAnswer,
                        Is_Active = true
                    };

                    _repo.RegisterUser(newUser,newStudent);

                    if (!Roles.IsUserInRole(newUser.UserName, "User"))
                    {
                        Roles.AddUserToRole(newUser.UserName, "User");
                    }

                    return RedirectToAction("Login");
                }
                catch (InvalidOperationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            return View(model);
        }

        public ActionResult RecoverPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RecoverPassword(RecoverPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var recoverySuccess = _repo.ChangePassword(model.Email, model.SecurityCode, model.SecurityAnswer, model.NewPassword);

                if (recoverySuccess)
                {
                    // Password recovery successful, you can redirect to login page or display a success message
                    TempData["RecoveryMessage"] = "Password recovery successful!";
                    return RedirectToAction("Login");
                }
                else
                {
                    ModelState.AddModelError("", "Password recovery failed. Please check your information and try again.");
                }
            }
            return View(model);
        }

        

    }
}