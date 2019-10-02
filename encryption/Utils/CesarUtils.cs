using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace encryption.Utils
{
    public class CesarUtils
    {
        private readonly FileUtils fileUtils = new FileUtils();

        public bool AssignAlphabet(string keyWord)
        {
            string Alphabet = "abcdefghijklmnñopqrstuvwxyz";
            Dictionary<string, string> EncryptationDictionary = new Dictionary<string, string>();
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
            return true;
        }
    }
}