using System.Web.Mvc;

namespace encryption.Controllers
{
    public class DecryptController : Controller
    {
        // Default view for the decription
        // GET: /Decrypt , /Decrypt/Index
        public ActionResult Index()
        {
            ViewBag.Message = string.Empty;
            return View();
        }
    }
}