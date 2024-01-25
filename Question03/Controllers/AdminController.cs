using Libraries.Interface;
using Libraries.Repository;
using Libraries.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Libraries.Model;
using Question03.Models;
using System.Web.Security;

namespace Question03.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IDatabaseRepository _repo;

        public AdminController()
        {
            _repo = new DatabaseRepository(new DefaultDbContext());
        }
        // GET: Admin
        public ActionResult AdminDashboard()
        {
            var userAndStudentList = _repo.GetAllUsersAndStudents();
            var viewModel = new AdminDashboardViewModel
            {
                UserAndStudentList = userAndStudentList
            };

            return View(viewModel);
        }

        [HttpGet]
        public ActionResult EditUserAndStudent(string userId)
        {
            var user = _repo.GetUserById(userId);
            var student = _repo.GetStudentById(userId);

            if (user == null || student == null)
            {
                return HttpNotFound();
            }

            var model = new UserAndStudentViewModel
            {
                Users = user,
                Students = student
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult EditUserAndStudent(UserAndStudentViewModel model)
        {
            if (ModelState.IsValid)
            {
                _repo.UpdateUser(model.Users, model.Students);
                return RedirectToAction("AdminDashboard");
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult DeleteUserAndStudent(string userId, string studentId)
        {
            _repo.DeleteUserAndStudent(userId, studentId);
            return RedirectToAction("AdminDashboard");
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }
    }
}