
using System.Web.Http.Cors;
using ArduinoAPI.Utils;
using System.Web.Mvc;

namespace ArduinoAPI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
