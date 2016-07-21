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
    public class TopicController : Controller
    {
        private BookDbContext _db;
        public HttpCookie userCookie;
        public int UserId;

        public TopicController()
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

        public ActionResult TIndex(int? page, string Search)
        {
            if (!GetCookiesInformation())
                return RedirectToAction("AIndex", "Author", null);

            if (Search != null)
            {
                var model = _db.Topics.Where(d => d.Name.Contains(Search) && d.TopicType == TopicType.Topic && d.AuthorId == UserId).OrderByDescending(o => o.Name).ToList();
                return View(model.ToPagedList(page ?? 1, 10));
            }
            else
            {
                var model = _db.Topics.OrderByDescending(d => d.Name).ToList();
                return View(model.ToPagedList(page ?? 1, 10));
            }
        }

        public PartialViewResult _SearchTopic(int? page, string Search = null)
        {
            if (GetCookiesInformation())
            {
                var model = _db.Topics.Where(d => d.Name.Contains(Search) && d.TopicType == TopicType.Topic && d.AuthorId == UserId).OrderByDescending(o => o.Name).ToList();
                return PartialView(model.ToPagedList(page ?? 1, 10));
            }
            else
                return PartialView();
        }

        public ActionResult Create()
        {
            if (!GetCookiesInformation())
                return RedirectToAction("AIndex", "Author", null);
            ViewBag.LabelId = new SelectList(_db.Labels, "LabelId", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,Ename,BgColor,TxColor,LabelId")] Topic topic)
        {
            if (!GetCookiesInformation())
                return RedirectToAction("AIndex", "Author", null);

            topic.IsActive = true;
            topic.Created = DateTime.Now;
            topic.AuthorId = UserId;
            topic.TopicType = TopicType.Topic;
            if (ModelState.IsValid)
            {
                bool Exists = _db.Topics.Any(d => d.Name.Equals(topic.Name));
                if (!Exists)
                {
                    _db.Topics.Add(topic);
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

            Topic model = _db.Topics.Find(id);
            if (model == null)
            {
                return Redirect("~/Admin/Login");
            }
            if (model.AuthorId == UserId)
            {
                if (model == null)
                {
                    return HttpNotFound();
                }
                ViewBag.LabelId = new SelectList(_db.Labels, "LabelId", "Name", model.LabelId);
                return View(model);
            }
            else
            {
                return Redirect("~/Author/Login");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TopicId,AuthorId,Name,Ename,TopicType,BgColor,TxColor,LabelId,Created")] Topic topic)
        {
            if (!GetCookiesInformation())
                return RedirectToAction("Login", "Author", null);

            topic.IsActive = true;
            topic.TopicType = TopicType.Topic;

            if (ModelState.IsValid)
            {
                if (userCookie == null)
                {
                    return RedirectToAction("Login", "User", null);
                }
                bool Exists = _db.Topics.Any(d => d.Name == topic.Name && d.TopicId != topic.TopicId);
                if (Exists)
                {
                    return Json("Try another label name");
                }
                else
                {
                    _db.Entry(topic).State = EntityState.Modified;
                    _db.SaveChanges();
                    return Json("");
                }
            }
            ViewBag.LabelId = new SelectList(_db.Labels, "LabelId", "Name", topic.LabelId);
            return View(topic);
        }

        public ActionResult Delete(long? id)
        {
            if (!GetCookiesInformation())
                return RedirectToAction("AIndex", "Author", null);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Topic topic = _db.Topics.Find(id);
            ViewBag.LabelId = new SelectList(_db.Labels, "LabelId", "Name");
            if (topic == null)
            {
                return HttpNotFound();
            }
            return View(topic);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed([Bind(Include = "TopicId")]Topic topic)
        {
            if (!GetCookiesInformation())
                return RedirectToAction("AIndex", "Author", null);

            Topic topicGet = _db.Topics.Find(topic.TopicId);
            _db.Topics.Remove(topicGet);
            _db.SaveChanges();
            return Json("");
        }

        public ActionResult Detail(long? id)
        {
            if (!GetCookiesInformation())
                return RedirectToAction("Login", "Admin", null);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Topic topic = _db.Topics.Find(id);
            topic.TDatas = _db.TDatas.Where(d => d.TopicId == topic.TopicId).ToList();
            if (topic != null && topic.AuthorId == UserId)
            {
                ViewBag.LabelId = new SelectList(_db.Labels, "LabelId", "Name", topic.LabelId);
                ViewBag.TagId = new SelectList(_db.Topics, "TopicId", "Name", topic.TopicId);
                return View(topic);
            }
            else
            {
                return Redirect("~/Admin/Login");
            }
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