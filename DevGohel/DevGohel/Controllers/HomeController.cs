using System.Web.Mvc;

namespace DevGohel.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Sample()
        {
            return View();

        }

        public ActionResult Error()
        {
            return View();
        }
    }
}