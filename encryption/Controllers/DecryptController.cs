using encryption.Utils;
using System.Web;
using System.Web.Mvc;

namespace encryption.Controllers
{
    public class DecryptController : Controller
    {
        private readonly FileUtils fileUtils = new FileUtils();

        // Default view for the decription
        // GET: /Decrypt , /Decrypt/Index
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.Message = string.Empty;
            return View();
        }

        // Decrypt the file uploaded
        [HttpPost]
        public ActionResult Decrypt(HttpPostedFileBase file, string cipher, string key)
        {
            string error = string.Empty;
            string path = string.Empty;
            if (DidDecription(file, ref error, ref path, cipher, key))
            {
                ViewBag.Message = "SUCCESS";
                return fileUtils.DownloadFile(path);
            }
            else
            {
                ViewBag.Message = error;
                return View("Index");
            }
        }

        /// <summary>Try to decrypt the file with the specific cipher algorithm</summary>
        /// <param name="file">The file uploaded</param>
        /// <param name="error">The error to send back</param>
        /// <param name="path">The path to the encrypted file</param>
        /// <param name="cipher">The cipher algorithm to use</param>
        /// <param name="key">The key for the decompretion</param>
        /// <returns>True if the file was decompresed, otherwise false</returns>
        private bool DidDecription(HttpPostedFileBase file, ref string error, ref string path, string cipher, string key)
        {
            if (fileUtils.IsFileTypeCorrect(file, ".cif", ref error))
            {
                string uploadedPath = fileUtils.SaveFile(file, "~/App_Data/Uploads");
                if (fileUtils.IsFileEmpty(uploadedPath))
                {
                    error = "Empty file";
                    return false;
                }
                else
                {
                    if (cipher.Equals("Caesar"))
                    {
                        // Caesar decription
                    }
                    else
                    {
                        int numericKey = int.Parse(key);
                        if (cipher.Equals("ZigZag"))
                        {
                            // ZigZag decription
                        }
                        else
                        {
                            // Spiral decription
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