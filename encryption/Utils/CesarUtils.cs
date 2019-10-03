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
        Dictionary<char, char> EncryptationDictionary = new Dictionary<char, char>();

        public bool EncryptFile(string path, ref string error, ref string newPath, string keyWord)
        {
            string encryptedPath = fileUtils.CreateFile(Path.GetFileNameWithoutExtension(path) + "_encrypted", ".cif", "~/App_Data/Downloads");
            if (isKeyWordCorrect(keyWord))
            {
                AssignAlphabet(keyWord);
                EncryptMessage(path, encryptedPath);
                newPath = encryptedPath;
                return true;
            }
            else
            {
                error = "Bad key";
                return false;
            }            
        }

        public bool DecryptFile(string path, ref string error, ref string newPath, string keyWord)
        {
            string decryptedPath = fileUtils.CreateFile(Path.GetFileNameWithoutExtension(path), ".txt", "~/App_Data/Downloads");
            bool isEmpty = (EncryptationDictionary.Count == 0);
            if (isKeyWordCorrect(keyWord))
            {
                if (isEmpty)
                {
                    AssignAlphabet(keyWord);
                }                
                DecryptMessage(path, decryptedPath);
                newPath = decryptedPath;
                return true;
            }
            else
            {
                error = "Bad key";
                return false;
            }
        }

        /// <summary>Assigns the normal alphabet to the key one in a dictionary</summary>
        /// <param name="keyWord">Entered by the user, will be used to create the new alphabet</param>
        /// <returns>true for the moment</returns>
        private void AssignAlphabet(string keyWord)
        {
            string Alphabet = "abcdefghijklmnñopqrstuvwxyz";            
            List<char> normalList = new List<char>();
            List<char> keyList = new List<char>();

            foreach (char c in keyWord)
            {
                keyList.Add(c);
            }

            foreach (char c in Alphabet)
            {
                normalList.Add(c);

                int index = keyList.FindIndex(x=> x == c);

                if (index == -1)
                {
                    keyList.Add(c);
                }
            }

            for (int i = 0; i < 27; i++)
            {
                EncryptationDictionary.Add(normalList[i], keyList[i]);
            }            
        }

        private void EncryptMessage(string path, string newPath)
        {
            BinaryReader reader = new BinaryReader(new FileStream(path, FileMode.Open));
            BinaryWriter writer = new BinaryWriter(new FileStream(newPath, FileMode.Open, FileAccess.Write));
            while (reader.BaseStream.Position != reader.BaseStream.Length)
            {
                char character = reader.ReadChar();
                bool isUpper = char.IsUpper(character);
                char index;
                if (isUpper)
                {
                    char lower = char.ToLower(character);
                    index = EncryptationDictionary.FirstOrDefault(x => x.Key == lower).Value;
                }
                else
                {
                    index = EncryptationDictionary.FirstOrDefault(x => x.Key == character).Value;
                }                                
                
                if (index != 0)
                {
                    if (isUpper)
                    {
                        writer.Write(char.ToUpper(index));
                    }
                    else
                    {
                        writer.Write(index);
                    }                    
                }                
                else
                {
                    writer.Write(character);
                }               
            }
            reader.Close();
            writer.Close();
        }

        private void DecryptMessage(string path, string newPath)
        {
            BinaryReader reader = new BinaryReader(new FileStream(path, FileMode.Open));
            BinaryWriter writer = new BinaryWriter(new FileStream(newPath, FileMode.Open, FileAccess.Write));
            while (reader.BaseStream.Position != reader.BaseStream.Length)
            {
                char character = reader.ReadChar();
                bool isUpper = char.IsUpper(character);
                char index;
                if (isUpper)
                {
                    char lower = char.ToLower(character);
                    index = EncryptationDictionary.FirstOrDefault(x => x.Value == lower).Key;
                }
                else
                {
                    index = EncryptationDictionary.FirstOrDefault(x => x.Value == character).Key;
                }

                if (index != 0)
                {
                    if (isUpper)
                    {
                        writer.Write(char.ToUpper(index));
                    }
                    else
                    {
                        writer.Write(index);
                    }
                }
                else
                {
                    writer.Write(character);
                }
            }
            reader.Close();
            writer.Close();
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