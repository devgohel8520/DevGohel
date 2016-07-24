using DevGohel.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Xml.Linq;

namespace DevGohel.Controllers
{
    public class HomeController : Controller
    {
        string con = "";
        BookDbContext _db;

        public HomeController()
        {
            _db = new BookDbContext();
            con = ConfigurationManager.ConnectionStrings["BookDbContext"].ConnectionString;
        }

        public ActionResult Index()
        {
            List<Topic> lstTopics = new List<Topic>();
            SqlParameter @TType = new SqlParameter()
            {
                ParameterName = "@TType",
                DbType = DbType.Int32,
                Value = (int)TopicType.Topic
            };

            SqlParameter @Ename = new SqlParameter()
            {
                ParameterName = "@Ename",
                DbType = DbType.String,
                Value = ""
            };
            object[] topicParameters = new object[] { @TType, @Ename };


            lstTopics = _db.Database.SqlQuery<Topic>("exec spGetTopicByEName @TType, @Ename", topicParameters).OrderByDescending(d => d.Created).Take(12).ToList();

            foreach (var topic in lstTopics)
            {
                SqlParameter @LabelId = new SqlParameter()
                   {
                       ParameterName = "@LabelId",
                       DbType = DbType.Int64,
                       Value = topic.LabelId
                   };
                object[] labelParameters = new object[] { @LabelId };
                topic.Label = _db.Database.SqlQuery<Label>("exec spGetLabelById @LabelId", labelParameters).FirstOrDefault();

            }

            if (lstTopics != null)
                return View(lstTopics);

            return View(new List<Topic>());
        }

        public ActionResult Error()
        {
            return View();
        }

        public ActionResult Sitemap()
        {
            var sitemap = new XDocument();
            var ns = XNamespace.Get("http://www.sitemaps.org/schemas/sitemap/0.9");
            var root = new XElement(ns + "urlset");

            var model = _db.Topics.ToList();
            if (model.Any())
            {
                foreach (var t in model)
                {
                    var node = new XElement(ns + "url", new XElement(ns + "loc", Request.Url.GetLeftPart(UriPartial.Authority) + "/" + t.Ename), new XElement(ns + "lastmod", DateTime.Today.ToString("yyyy-MM-dd")), new XElement(ns + "changefreq", "always"), new XElement(ns + "priority", "1"));
                    root.Add(node);
                }
            }
            sitemap.Add(root);
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream, Encoding.UTF8);
            sitemap.Save(writer);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "text/xml");
        }


        public ActionResult Feedback()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Feedback(FeedBack model)
        {
            if (ModelState.IsValid)
            {
                model.Date = DateTime.Now;
                _db.FeedBacks.Add(model);
                _db.SaveChanges();
                return Json("Thanks " + model.FullName + ", I will contact you soon.");
            }
            else
            {
                var errors = ModelState.SelectMany(x => x.Value.Errors.Select(d => d.Exception));
                return Json("Error: " + errors);
            }
        }

        public ActionResult Contact()
        {
            return View();
        }



    }
}