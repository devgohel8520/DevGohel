using System.Web.Mvc;

namespace DevGohel.Controllers
{
    public class SearchController : Controller
    {

        public ActionResult Index(string search)
        {
            return View();
        }
    }
}