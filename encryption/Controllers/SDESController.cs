using encryption.Utils;
using System.Web;
using System.Web.Mvc;

namespace encryption.Controllers
{
    public class SDESController : Controller
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

        // Decrypt the file uploaded
        [HttpPost]
        public ActionResult Decrypt(HttpPostedFileBase file, string key)
        {
            string error = string.Empty;
            string path = string.Empty;
            if (DidDecription(file, ref error, ref path, key))
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

        // Encrypt the file uploaded
        [HttpPost]
        public ActionResult Encrypt(HttpPostedFileBase file, string key)
        {
            string error = string.Empty;
            string path = string.Empty;
            if (DidEncription(file, ref error, ref path, key))
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
        /// <param name="direction">The direction for the spiral cipher</param>
        /// <returns>True if the file was decompresed, otherwise false</returns>
        private bool DidDecription(HttpPostedFileBase file, ref string error, ref string path, string key)
        {
            if (fileUtils.IsFileTypeCorrect(file, ".scif", ref error))
            {
                string uploadedPath = fileUtils.SaveFile(file, "~/App_Data/Uploads");
                int numericKey = int.Parse(key);
                SdesUtils sdes = new SdesUtils();
                if (sdes.Decrypt(uploadedPath, numericKey, ref path))
                {
                    return true;
                }
                else
                {
                    error = "Bad Encryption";
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>Try to encrypt the file with the specific cipher algorithm</summary>
        /// <param name="file">The file uploaded</param>
        /// <param name="error">The error to send back</param>
        /// <param name="path">The path to the encrypted file</param>
        /// <param name="cipher">The cipher algorithm to use</param>
        /// <param name="key">The key for the encription</param>
        /// <param name="direction">The direction for the spiral cipher</param>
        /// <returns>True if the file was encripted, otherwise false</returns>
        private bool DidEncription(HttpPostedFileBase file, ref string error, ref string path, string key)
        {
            if (fileUtils.IsFileTypeCorrect(file, ".txt", ref error))
            {
                string uploadedPath = fileUtils.SaveFile(file, "~/App_Data/Uploads");
                if (fileUtils.IsFileEmpty(uploadedPath))
                {
                    error = "Empty file";
                    return false;
                }
                else
                {
                    int numericKey = int.Parse(key);
                    SdesUtils sdes = new SdesUtils();
                    if (sdes.Encrypt(uploadedPath, numericKey, ref path))
                    {
                        return true;
                    }
                    else
                    {
                        error = "Bad Encryption";
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }
        }
    }
}