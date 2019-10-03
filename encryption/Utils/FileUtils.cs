using System;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace encryption.Utils
{
    public class FileUtils
    {
        /// <summary>Create an empty file</summary>
        /// <param name="filename">The name for the file</param>
        /// <param name="extension">The extension for the file</param>
<<<<<<< HEAD
        /// <param name="directory">The directory to save the file</param>        
=======
        /// <param name="directory">The directory to save the file</param>
        /// <param name="type">The type of the file, can be normal and compressed</param>
>>>>>>> e87068079eba544c1ed7f7975d57e03898ae6a63
        /// <returns>The path of the file, if ERROR check the type</returns>
        public string CreateFile(string filename, string extension, string directory)
        {
            string dir = HttpContext.Current.Server.MapPath(directory);
            string path = Path.Combine(dir, filename + extension);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            File.Create(path).Close();
            return path;
        }

        /// <summary>Return the file to download</summary>
        /// <param name="path">The path to the file</param>
        /// <returns>The file to download if exist, otherwise null</returns>
        public FileContentResult DownloadFile(string path)
        {
            if (File.Exists(path))
                return new FileContentResult(File.ReadAllBytes(path), "application/octet-stream") { FileDownloadName = Path.GetFileName(path) };
            else
                return null;
        }

        /// <summary>Check that the filetype is correct</summary>
        /// <param name="file">The file uploaded by the user</param>
        /// <param name="filetype">The extension of the file to check</param>
        /// <param name="error">The error message to get</param>
        /// <returns>True if the file type is correct, otherwise false</returns>
        public bool IsFileTypeCorrect(HttpPostedFileBase file, string filetype, ref string error)
        {
            if (file != null)
            {
                if (filetype.Equals(Path.GetExtension(file.FileName), StringComparison.OrdinalIgnoreCase))
                    return true;
                error = "Bad filetype";
                return false;
            }
            else
            {
                error = "Null file";
                return false;
            }
        }

        /// <summary>Check if the file is empty</summary>
        /// <param name="path">The path to the file to check</param>
        /// <returns>True if the file is empty, otherwise return false</returns>
        public bool IsFileEmpty(string path)
        {
            if (new FileInfo(path).Length == 0)
                return true;
            return false;
        }

        /// <summary>Save the specified file in the specified directory</summary>
        /// <param name="file">The file to save</param>
        /// <param name="directory">The directory to save the file</param>
        /// <returns>The path to the file saved</returns>
        public string SaveFile(HttpPostedFileBase file, string directory)
        {
            string name = Path.GetFileName(file.FileName);
            string dir = HttpContext.Current.Server.MapPath(directory);
            string path = Path.Combine(dir, name);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            file.SaveAs(path);
            return path;
        }
    }
<<<<<<< HEAD
}
=======
}
>>>>>>> e87068079eba544c1ed7f7975d57e03898ae6a63
