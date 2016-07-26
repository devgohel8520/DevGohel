using DevGohel.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
namespace DevGohel.Controllers.Admin
{
    public class ProfileController : Controller
    {
        private BookDbContext _db;
        public HttpCookie userCookie;
        public int UserId;

        public ProfileController()
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

        public ActionResult PIndex()
        {
            if (!GetCookiesInformation())
                return RedirectToAction("AIndex", "Author", null);
            Topic topic = _db.Topics.Include(d => d.TDatas).FirstOrDefault(d => d.TopicType == TopicType.Author && d.AuthorId == UserId);

            return View(topic);

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
            topic.TopicType = TopicType.Author;
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
            topic.TopicType = TopicType.Author;

            if (ModelState.IsValid)
            {
                if (userCookie == null)
                {
                    return RedirectToAction("Login", "User", null);
                }
                _db.Entry(topic).State = EntityState.Modified;
                _db.SaveChanges();
                return Json("");
            }
            ViewBag.LabelId = new SelectList(_db.Labels, "LabelId", "Name", topic.LabelId);
            return View(topic);
        }

        public ActionResult CreateProfile(int TopicId)
        {
            if (!GetCookiesInformation())
                return RedirectToAction("Login", "Admin", null);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateProfile([Bind(Include = "TdataId,Name,Body,OrderId,ExtraText,Type,TopicId")] TData tData)
        {
            if (!GetCookiesInformation())
                return RedirectToAction("AIndex", "Author", null);

            tData.IsActive = true;
            tData.Created = DateTime.Now;

            if (tData.Type == TDataType.HtmlText)
                tData.ExtraText = "";

            if (ModelState.IsValid)
            {
                _db.TDatas.Add(tData);
                _db.SaveChanges();
                return Json("");
            }

            return Json("Model is not valid.");
        }

        [HttpGet]
        public ActionResult GetMaxOrderNo(int? topicId)
        {
            if (topicId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List<TData> tData = _db.TDatas.Where(t => t.TopicId == topicId).ToList();
            int maxOrderId = 0;
            if (tData.Count > 0)
            {
                maxOrderId = tData.Max(e => e.OrderId);
            }
            maxOrderId += 1;

            return Json(maxOrderId, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditProfile(long? id)
        {
            if (!GetCookiesInformation())
                return RedirectToAction("AIndex", "Author", null);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            TData model = _db.TDatas.Find(id);
            if (model == null)
            {
                return HttpNotFound("No records found.");
            }

            ViewBag.TopicId = new SelectList(_db.Topics, "TopicId", "Name", model.TopicId);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProfile([Bind(Include = "TdataId,Name,Body,OrderId,ExtraText,Type,TopicId")] TData tData)
        {
            if (!GetCookiesInformation())
                return RedirectToAction("Login", "Admin", null);

            tData.IsActive = true;
            tData.Created = DateTime.Now;
            if (ModelState.IsValid)
            {
                _db.Entry(tData).State = EntityState.Modified;
                _db.SaveChanges();
                return Json("");
            }

            return Json("Model is not valid.");
        }

        public ActionResult DeleteP(long? id)
        {
            if (!GetCookiesInformation())
                return RedirectToAction("AIndex", "Author", null);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TData tData = _db.TDatas.Find(id);
            if (tData == null)
            {
                return HttpNotFound();
            }
            return View(tData);
        }

        [HttpPost, ActionName("DeleteP")]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePConfirmed([Bind(Include = "TdataId")]TData tData)
        {
            if (!GetCookiesInformation())
                return RedirectToAction("AIndex", "Author", null);

            TData tDataGet = _db.TDatas.Find(tData.TdataId);
            _db.TDatas.Remove(tDataGet);
            _db.SaveChanges();
            return Json("");
        }
    }
}