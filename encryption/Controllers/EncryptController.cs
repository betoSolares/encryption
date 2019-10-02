using System.Web.Mvc;
using System.Web;
using encryption.Utils;


namespace encryption.Controllers
{
    public class EncryptController : Controller
    {
        private readonly CesarUtils cesarUtils = new CesarUtils();
        private readonly FileUtils fileUtils = new FileUtils();

        // Default view for the encription
        // GET: /Encrypt , /Encrypt/Index
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.Message = string.Empty;
            return View();
        }

        [HttpPost]
        public ActionResult Encrypt(HttpPostedFileBase file, string key)
        {
            string error = string.Empty;
            string encryptedPath = string.Empty;
            cesarUtils.AssignAlphabet(key);
            return fileUtils.DownloadFile(encryptedPath);
        }
    }
}