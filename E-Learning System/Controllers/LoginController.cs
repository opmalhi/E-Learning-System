using E_Learning_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace E_Learning_System.Controllers
{
    public class LoginController : Controller
    {
        ELearningDBContext db = new ELearningDBContext();
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User user)
        {
            if (!ModelState.IsValid) return View(user);

            user.Password = HashPassword(user.Password);

            bool isValidUser = db.Users.Any(c => c.Email.ToLower() == user.Email.ToLower()
                && c.Password == user.Password);

            if (isValidUser)
            {
                FormsAuthentication.SetAuthCookie(user.Email, false);

                var userRole = db.Users
                    .Include("UserRole")
                    .Where(c => c.Email.ToLower() == user.Email.ToLower())
                    .FirstOrDefault();

                if (userRole.Role_Id == 1)
                    return RedirectToAction("Subject", "Admin");

                if (userRole.Role_Id == 2)
                    return RedirectToAction("ShowTeacherSubject", "Admin");

                if (userRole.Role_Id == 3)
                    return RedirectToAction("Index", "Admin");

            }

            ModelState.AddModelError("", "Invalid username or password!");
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Login");
        }

        private string HashPassword(string password)
        {
            byte[] encData_byte = new byte[password.Length];
            encData_byte = System.Text.Encoding.UTF8.GetBytes(password);
            return Convert.ToBase64String(encData_byte);
        }
    }
}