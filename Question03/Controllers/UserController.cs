using Libraries.Data;
using Libraries.Interface;
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
    [Authorize(Roles = "User")]
    public class UserController : Controller
    {
        private readonly IDatabaseRepository _repo;
        public UserController()
        {
            _repo = new DatabaseRepository(new DefaultDbContext());
        }

        // GET: User
        public ActionResult Dashboard()
        {
            string username  = User.Identity.Name;
            var user = _repo.GetUserByUserName(username);
            var student = _repo.GetStudentById(user.UserID);
            if (user == null || student == null)
            {
                return HttpNotFound();
            }

            var viewModel = new UserDashboardViewModel
            {
                User = user,
                Student = student
            };

            return View(viewModel);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }
    }
}