using DevGohel.Models;
using PagedList;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace DevGohel.Controllers.Admin
{
    public class LabelController : Controller
    {
        BookDbContext _db;
        public HttpCookie userCookie;
        public int UserId;

        public LabelController()
        {
            _db = new BookDbContext();
        }

        private bool GetCookiesInformation()
        {
            if (Request.Cookies["devgoheladmin"] != null)
            {
                userCookie = HttpContext.Request.Cookies["devgoheladmin"];
                UserId = int.Parse(userCookie["AuthorId"].ToString());
                return true;
            }
            return false;
        }

        public ActionResult LIndex(int? page, string Search = null)
        {
            if (!GetCookiesInformation())
                return RedirectToAction("AIndex", "Author", null);

            if (Search != null)
            {
                var model = _db.Labels.Where(d => d.Name.Contains(Search)).OrderByDescending(o => o.Name).ToList();
                return View(model.ToPagedList(page ?? 1, 10));
            }
            else
            {
                var model = _db.Labels.OrderByDescending(d => d.Name).ToList();
                return View(model.ToPagedList(page ?? 1, 10));
            }

        }
        public PartialViewResult _SearchLabel(int? page, string Search = null)
        {
            if (GetCookiesInformation())
            {
                var model = _db.Labels.Where(d => d.Name.Contains(Search)).OrderByDescending(o => o.Name).ToList();
                return PartialView(model.ToPagedList(page ?? 1, 10));
            }
            else
                return PartialView();
        }

        public ActionResult Create()
        {
            if (!GetCookiesInformation())
                return RedirectToAction("AIndex", "Author", null);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,EName")] Label label)
        {
            if (!GetCookiesInformation())
                return RedirectToAction("AIndex", "Author", null);

            label.Created = DateTime.Now;
            label.IsActive = true;
            if (ModelState.IsValid)
            {
                bool Exists = _db.Labels.Any(d => d.Name.Equals(label.Name));
                if (!Exists)
                {
                    _db.Labels.Add(label);
                    _db.SaveChanges();
                    return Json("");
                }
                else
                {
                    return Json("Try another label name");
                }
            }
            else
            {
                return Json("Model is not valid.");
            }
        }

        public ActionResult Edit(long? id)
        {
            if (!GetCookiesInformation())
                return RedirectToAction("AIndex", "Author", null);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Label label = _db.Labels.Find(id);
            if (label == null)
            {
                return HttpNotFound();
            }
            return View(label);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LabelId,Name,EName,IsActive")] Label label)
        {
            if (!GetCookiesInformation())
                return RedirectToAction("AIndex", "Author", null);

            label.Created = DateTime.Now;
            if (ModelState.IsValid)
            {
                bool Exists = _db.Labels.Any(d => d.Name == label.Name && d.LabelId != label.LabelId);
                if (Exists)
                {
                    return Json("Try another label name");
                }
                else
                {
                    _db.Entry(label).State = EntityState.Modified;
                    _db.SaveChanges();
                    return Json("");
                }
            }
            else
            {
                return Json("Model is not valid.");
            }
        }

        public ActionResult Delete(long? id)
        {
            if (!GetCookiesInformation())
                return RedirectToAction("AIndex", "Author", null);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Label label = _db.Labels.Find(id);
            if (label == null)
            {
                return HttpNotFound();
            }
            return View(label);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed([Bind(Include = "LabelId")] Label label)
        {
            if (!GetCookiesInformation())
                return RedirectToAction("AIndex", "Author", null);

            Label labelGet = _db.Labels.Find(label.LabelId);
            _db.Labels.Remove(labelGet);
            _db.SaveChanges();
            return Json("");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }

        public System.Collections.Generic.List<Label> List { get; set; }
    }
}
