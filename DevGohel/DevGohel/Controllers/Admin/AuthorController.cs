using DevGohel.Models;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DevGohel.Controllers.Admin
{
    public class AuthorController : Controller
    {
        BookDbContext _db;

        public AuthorController()
        {
            _db = new BookDbContext();
        }

        public bool LoginStatus()
        {
            if (Request.Cookies["devgoheladmin"] == null)
            {
                return false;
            }
            return true;
        }

        public ActionResult Login()
        {
            if (LoginStatus())
                return RedirectToAction("AIndex", "Author", null);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Author author)
        {
            var query = _db.Authors.FirstOrDefault(s => s.EmailId == author.EmailId && s.Password == author.Password && s.IsActive == true);

            if (query != null)
            {
                var userName = query.Name;

                HttpCookie userInfo = new HttpCookie("devgoheladmin");
                userInfo.Expires = DateTime.Now.AddDays(1);
                userInfo.Values["AuthorId"] = query.AuthorId.ToString();
                userInfo.Values["Email"] = query.EmailId;
                userInfo.Values["Name"] = query.Name;
                Response.Cookies.Add(userInfo);
                return Json("");
            }
            else
            {
                return Json("Incorrect email id or password.");
            }
        }


        public ActionResult AIndex()
        {
            return View();
        }


        public ActionResult Logout()
        {
            if (Request.Cookies["devgoheladmin"] != null)
            {
                HttpCookie mybookUser = new HttpCookie("devgoheladmin");
                mybookUser.Expires = DateTime.Now.AddDays(-1d);
                Response.Cookies.Add(mybookUser);
                return RedirectToAction("Login", "Author", null);
            }
            return View();
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}