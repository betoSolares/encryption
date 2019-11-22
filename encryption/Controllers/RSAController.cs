using encryption.Utils;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
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

        // Decrypt the file uploaded
        [HttpPost]
        public ActionResult Decrypt(HttpPostedFileBase file, HttpPostedFileBase key)
        {
            string error = string.Empty;
            string path = string.Empty;
            if (DidDecryption(file, key, ref error, ref path))
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
        public ActionResult Encrypt(HttpPostedFileBase file, HttpPostedFileBase key)
        {
            string error = string.Empty;
            string path = string.Empty;
            if (DidEncryption(file, key, ref error, ref path))
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

        //Generate the keys for the RSA algorithm
        [HttpPost]
        public ActionResult Keys(int p, int q)
        {
            RsaUtils rsaUtils = new RsaUtils();
            List<string> files = new List<string>();
            if (rsaUtils.GenerateKeys(p, q, ref files))
            {
                string temp = Server.MapPath("~/App_Data/RSA-Keys/temp/");
                string archive = Server.MapPath("~/App_Data/RSA-Keys/archive.zip");
                if (Directory.Exists(temp))
                {
                    Directory.Delete(temp, true);
                    Directory.CreateDirectory(temp);
                }
                else
                {
                    Directory.CreateDirectory(temp);
                }
                if (System.IO.File.Exists(archive))
                    System.IO.File.Delete(archive);
                foreach (string file in files)
                    System.IO.File.Copy(file, temp + Path.GetFileName(file));
                ZipFile.CreateFromDirectory(temp, archive);
                return File(archive, "application/zip", "keys.zip");
            }
            else
            {
                ViewBag.Message = "Bad Keys";
                return View("Index");
            }
        }

        /// <summary>Check if the decryption was correct</summary>
        /// <param name="file">The file to decrypt</param>
        /// <param name="key">The key file</param>
        /// <param name="error">A error to return</param>
        /// <param name="path">The path for the new file</param>
        /// <returns>True if is correct, otherwise false</returns>
        private bool DidDecryption(HttpPostedFileBase file, HttpPostedFileBase key, ref string error, ref string path)
        {
            if (fileUtils.IsFileTypeCorrect(file, ".rsacif", ref error) && fileUtils.IsFileTypeCorrect(key, ".key", ref error))
            {
                string uploadedFile = fileUtils.SaveFile(file, "~/App_Data/Uploads");
                string uploadedKey = fileUtils.SaveFile(key, "~/App_Data/Uploads");
                RsaUtils rsa = new RsaUtils();
                if (rsa.Decrypt(uploadedFile, uploadedKey, ref path))
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

        /// <summary>Check if the encryption was correct</summary>
        /// <param name="file">The file to decrypt</param>
        /// <param name="key">The key file</param>
        /// <param name="error">A error to return</param>
        /// <param name="path">The path for the new file</param>
        /// <returns>True if is correct, otherwise false</returns>
        private bool DidEncryption(HttpPostedFileBase file, HttpPostedFileBase key, ref string error, ref string path)
        {
            if (fileUtils.IsFileTypeCorrect(file, ".txt", ref error) && fileUtils.IsFileTypeCorrect(key, ".key", ref error))
            {
                string uploadedFile = fileUtils.SaveFile(file, "~/App_Data/Uploads");
                string uploadedKey = fileUtils.SaveFile(key, "~/App_Data/Uploads");
                RsaUtils rsa = new RsaUtils();
                if (rsa.Encrypt(uploadedFile, uploadedKey, ref path))
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
    }
}