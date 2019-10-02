using System.Web.Mvc;
using System.Web;


namespace encryption.Controllers
{
    public class EncryptController : Controller
    {
        // Default view for the encription
        // GET: /Encrypt , /Encrypt/Index
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.Message = string.Empty;
            return View();
        }

        [HttpPost]
        public ActionResult Encrypt(HttpPostedFileBase file, string algorithm)
        {
            string error = string.Empty;
            string encryptedPath = string.Empty;
            return View("Index");
        }
    }
}