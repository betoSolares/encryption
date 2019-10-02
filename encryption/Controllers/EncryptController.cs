using System.Web.Mvc;

namespace encryption.Controllers
{
    public class EncryptController : Controller
    {
        // Default view for the encription
        // GET: /Encrypt , /Encrypt/Index
        public ActionResult Index()
        {
            ViewBag.Message = string.Empty;
            return View();
        }
    }
}