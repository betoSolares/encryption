using encryption.Utils;
using System.Web;
using System.Web.Mvc;

namespace encryption.Controllers
{
    public class EncryptController : Controller
    {
        private readonly FileUtils fileUtils = new FileUtils();

        // Default view for the encription
        // GET: /Encrypt , /Encrypt/Index
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.Message = string.Empty;
            return View();
        }

        // Encrypt the file uploaded
        [HttpPost]
        public ActionResult Encrypt(HttpPostedFileBase file, string cipher, string key, string direction)
        {
            string error = string.Empty;
            string path = string.Empty;
            if (DidEncription(file, ref error, ref path, cipher, key, direction))
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

        /// <summary>Try to encrypt the file with the specific cipher algorithm</summary>
        /// <param name="file">The file uploaded</param>
        /// <param name="error">The error to send back</param>
        /// <param name="path">The path to the encrypted file</param>
        /// <param name="cipher">The cipher algorithm to use</param>
        /// <param name="key">The key for the encription</param>
        /// <param name="direction">The direction for the spiral cipher</param>
        /// <returns>True if the file was encripted, otherwise false</returns>
        private bool DidEncription(HttpPostedFileBase file, ref string error, ref string path, string cipher, string key, string direction)
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
                    if (cipher.Equals("Caesar"))
                    {
                        CaesarUtils caesarUtils = new CaesarUtils();
                        if (caesarUtils.EncryptFile(uploadedPath, ref error, ref path, key))
                        {
                            return true;
                        }
                        else
                        {
                            if (!error.Equals("Bad key"))
                                error = "Bad Encryption";
                            return false;
                        }
                    }
                    else
                    {
                        int numericKey = int.Parse(key);
                        if (cipher.Equals("ZigZag"))
                        {
                            ZigZagUtils zigZag = new ZigZagUtils();
                            try
                            {
                                zigZag.Encrypt(uploadedPath, numericKey, ref path);
                                return true;
                            }
                            catch
                            {
                                error = "Bad Encryption";
                                return false;
                            }
                        }
                        else
                        {                        
                            SpiralUtils spiral = new SpiralUtils();
                            try
                            {
                                spiral.Encrypt(uploadedPath, numericKey, ref path, direction);
                                return true;
                            }
                            catch 
                            {
                                error = "Bad Encryption";
                                return false;
                            }
                        }
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