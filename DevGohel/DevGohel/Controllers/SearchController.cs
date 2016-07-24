using DevGohel.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

namespace DevGohel.Controllers
{
    public class SearchController : Controller
    {
        string con = "";
        BookDbContext _db;

        public SearchController()
        {
            _db = new BookDbContext();
            con = ConfigurationManager.ConnectionStrings["BookDbContext"].ConnectionString;
        }
        public ActionResult Index(string search)
        {
            if (search == null)
                return RedirectToAction("Index", "Home", null);

            ViewBag.Advertise = null;
            if (Session["Advertise"] == null)
            {
                Sponser sponser = _db.Database.SqlQuery<Sponser>("exec spGetSponser").FirstOrDefault();
                Session.Add("Advertise", sponser.Body);
            }
            ViewBag.Advertise = Session["Advertise"];

            //SqlParameter @TType = new SqlParameter()
            //{
            //    ParameterName = "@TType",
            //    DbType = DbType.Int32,
            //    Value = (int)TopicType.Topic
            //};

            SqlParameter @Ename = new SqlParameter()
            {
                ParameterName = "@Ename",
                DbType = DbType.String,
                Value = search
            };
            object[] topicParameters = new object[] { @Ename };


            Topic topic = _db.Database.SqlQuery<Topic>("exec spGetTopicByENameSearch @Ename", topicParameters).FirstOrDefault();

            SqlParameter @TopicId = new SqlParameter()
            {
                ParameterName = "@TopicId",
                DbType = DbType.Int64,
                Value = topic.TopicId
            };

            object[] tdataParameters = new object[] { @TopicId };
            topic.TDatas = _db.Database.SqlQuery<TData>("exec spGetTopicIdData @TopicId", tdataParameters).ToList();


            SqlParameter @LabelId = new SqlParameter()
            {
                ParameterName = "@LabelId",
                DbType = DbType.Int64,
                Value = topic.LabelId
            };

            object[] labelParameters = new object[] { @LabelId };
            topic.Label = _db.Database.SqlQuery<Label>("exec spGetLabelById @LabelId", labelParameters).FirstOrDefault();

            SqlParameter @AuthorId = new SqlParameter()
            {
                ParameterName = "@AuthorId",
                DbType = DbType.Int64,
                Value = topic.AuthorId
            };

            object[] authorParameters = new object[] { @AuthorId };
            topic.Author = _db.Database.SqlQuery<Author>("exec spGetAuthorById @AuthorId", authorParameters).FirstOrDefault();


            if (topic != null)
            {
                return View(topic);
            }

            return View(returnTopicsIFNull(search));
        }

        public Topic returnTopicsIFNull(string search)
        {
            Topic Empty = new Topic();

            List<TData> LstTdata = new List<TData>();
            TData tData = new TData() { TdataId = 0, Name = "No records related to " + Convert.ToString(search), Body = "Sorry for inconvenience we haven't information related to " + Convert.ToString(search) };
            LstTdata.Add(tData);

            Empty.AuthorId = 0;
            Empty.Name = String.Empty;
            Empty.TopicId = 0;
            Empty.Name = "Not Found";
            Empty.Ename = search;
            Empty.TDatas = LstTdata;

            return Empty;
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