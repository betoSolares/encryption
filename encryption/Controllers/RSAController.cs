using encryption.Utils;
using System.Web;
using System.Web.Mvc;

namespace encryption.Controllers
{
    public class RSAController : Controller
    {
        private readonly FileUtils fileUtils = new FileUtils();

        // Default view for the encription
        // GET: /SDES , /SDES/Index
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.Message = string.Empty;
            return View();
        }
    }
}