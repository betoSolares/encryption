using System.Web.Mvc;
using encryption.Utils;
using System.Web;

namespace encryption.Controllers
{
    public class DecryptController : Controller
    {
        private readonly CesarUtils caesarUtils = new CesarUtils();
        private readonly FileUtils fileUtils = new FileUtils();

        // Default view for the decription
        // GET: /Decrypt , /Decrypt/Index
        public ActionResult Index()
        {
            ViewBag.Message = string.Empty;
            return View();
        }

        [HttpPost]
        public ActionResult Decrypt(HttpPostedFileBase file, string key, string cipher)
        {
            string error = string.Empty;
            string decryptedPath = string.Empty;
            string keyWord = key;
            if (DidDEcryptationCorrect(file, ref error, ref decryptedPath, cipher, keyWord))
            {
                ViewBag.Message = "SUCCESS";
                return fileUtils.DownloadFile(decryptedPath);
            }
            else
            {
                ViewBag.Message = error;
                return View("Index");
            }
        }

        private bool DidDEcryptationCorrect(HttpPostedFileBase file, ref string error, ref string decryptedPath, string cipher, string keyWord)
        {
            if (fileUtils.IsFileTypeCorrect(file, ".cif", ref error))
            {
                string originalPath = fileUtils.SaveFile(file, "~/App_Data/Uploads");
                if (fileUtils.IsFileEmpty(originalPath))
                {
                    error = "Empty file";
                    return false;
                }
                else
                {
                    if (cipher.Equals("Caesar"))
                    {
                        if (caesarUtils.DecryptFile(originalPath, ref error, ref decryptedPath, keyWord))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }                                                
                    }
                    return true;
                }
            }
            else
            {
                return false;
            }
        }
    }
}