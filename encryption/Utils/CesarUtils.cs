using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace encryption.Utils
{
    public class CesarUtils
    {
        private readonly FileUtils fileUtils = new FileUtils();
        Dictionary<string, string> EncryptationDictionary = new Dictionary<string, string>();

        public bool EncryptFile(string path, ref string error, ref string newPath, string keyWord)
        {
            AssignAlphabet(keyWord);
            EncryptMessage(path);
            return true;
        }

        /// <summary>Assigns the normal alphabet to the key one in a dictionary</summary>
        /// <param name="keyWord">Entered by the user, will be used to create the new alphabet</param>
        /// <returns>true for the moment</returns>
        private void AssignAlphabet(string keyWord)
        {
            string Alphabet = "abcdefghijklmnñopqrstuvwxyz";            
            List<string> normalList = new List<string>();
            List<string> keyList = new List<string>();

            foreach (char c in keyWord)
            {
                keyList.Add(c.ToString());
            }

            foreach (char c in Alphabet)
            {
                normalList.Add(c.ToString());

                int index = keyList.FindIndex(x=> x == c.ToString());

                if (index == -1)
                {
                    keyList.Add(c.ToString());
                }
            }

            for (int i = 0; i < 27; i++)
            {
                EncryptationDictionary.Add(normalList[i], keyList[i]);
            }            
        }

        private void EncryptMessage(string path)
        {
            BinaryReader reader = new BinaryReader(new FileStream(path, FileMode.Open));
            BinaryWriter writer = new BinaryWriter(new FileStream(path, FileMode.Open, FileAccess.Write));
            while (reader.BaseStream.Position != reader.BaseStream.Length)
            {
                char character = reader.ReadChar();                
                
            }
            reader.Close();
            writer.Close();
        }
    }
}