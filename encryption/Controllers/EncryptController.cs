﻿using System.Web.Mvc;
using System.Web;
using encryption.Utils;
using System.Linq;


namespace encryption.Controllers
{
    public class EncryptController : Controller
    {
        private readonly CesarUtils caesarUtils = new CesarUtils();
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
        public ActionResult Encrypt(HttpPostedFileBase file, string key, string algorithm)
        {
            string error = string.Empty;
            string encryptedPath = string.Empty;
            string keyWord = key;            
            if (DidEncryptationCorrect(file, ref error, ref encryptedPath, algorithm, keyWord))
            {
                ViewBag.Message = "SUCCESS";
                return fileUtils.DownloadFile(encryptedPath);
            }
            else
            {
                ViewBag.Message = error;
                return View("Index");
            }           
        }

        private bool DidEncryptationCorrect(HttpPostedFileBase file, ref string error, ref string encryptedPath, string algorithm, string keyWord)
        {
            if (fileUtils.IsFileTypeCorrect(file, ".txt",ref error))
            {
                string originalPath = fileUtils.SaveFile(file, "~/App_Data/Uploads");
                if (fileUtils.IsFileEmpty(originalPath))
                {
                    error = "Empty file";
                    return false;
                }
                else
                {
                    if (algorithm.Equals("Caesar"))
                    {
                        if (isKeyWordCorrect(keyWord))
                        {
                            return caesarUtils.EncryptFile(originalPath, ref error, ref encryptedPath, keyWord);
                        }
                        else
                        {

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

        private bool isKeyWordCorrect(string keyWord)
        {
            foreach (char c in keyWord)
            {
                int count = keyWord.ToCharArray().Count(x => x == c);

                if (count != 1)
                {
                    return false;
                }
            }
            return true;
        }
    }
}