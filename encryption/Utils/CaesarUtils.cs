using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace encryption.Utils
{
    public class CaesarUtils
    {
        private readonly FileUtils fileUtils = new FileUtils();
        private readonly Dictionary<char, char> EncryptationDictionary = new Dictionary<char, char>();

        /// <summary>Encrypt the file</summary>
        /// <param name="path">The path to the file</param>
        /// <param name="error">The error the send back</param>
        /// <param name="newPath">The path to download</param>
        /// <param name="keyWord">The key for the encryption</param>
        /// <returns>True if the encryption is successful, otherwise false</returns>
        public bool EncryptFile(string path, ref string error, ref string newPath, string keyWord)
        {
            string encryptedPath = fileUtils.CreateFile(Path.GetFileNameWithoutExtension(path) + "_encrypted", ".cif", "~/App_Data/Downloads");
            if (IsKeyWordCorrect(keyWord))
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

        /// <summary>Verifies and uses two methods that assign the alphabet and decrypt the message entered</summary>
        /// <param name="path">Path given to read the encrypetd message</param>
        /// <param name="error">Error that may occur</param>
        /// <param name="newPath">Path to download the new decrypted message</param>
        /// <param name="keyWord">Key entered by user to decrypt the message</param>
        /// <returns>True if the key doesn't have repeated letters, otherwise false</returns>
        public bool DecryptFile(string path, ref string error, ref string newPath, string keyWord)
        {
            string decryptedPath = fileUtils.CreateFile(Path.GetFileNameWithoutExtension(path), ".txt", "~/App_Data/Downloads");
            bool isEmpty = (EncryptationDictionary.Count == 0);
            if (IsKeyWordCorrect(keyWord))
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
        private void AssignAlphabet(string keyWord)
        {
            string Alphabet = "abcdefghijklmnñopqrstuvwxyz";            
            List<char> normalList = new List<char>();
            List<char> keyList = new List<char>();

            foreach (char c in keyWord)
            {
                if (char.IsUpper(c))
                {
                    char cLower = char.ToLower(c);
                    keyList.Add(cLower);
                }
                else
                {
                    keyList.Add(c);
                }                
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

        /// <summary>Reads original message and encrypts it, then writes it in a new file</summary>
        /// <param name="path">Path with the original message</param>
        /// <param name="newPath">Path to write the encrypted message</param>
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

        /// <summary>Reads encrypted message, then writes the decrypted message in a new file</summary>
        /// <param name="path">Path with encrypted message</param>
        /// <param name="newPath">Path to write the new decrypted message</param>
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

        /// <summary>Checks if the key doesn't have repeated letters</summary>
        /// <param name="keyWord">Key entered by the user</param>
        /// <returns>True if it doesn't have repeated letter, otherwise false</returns>
        private bool IsKeyWordCorrect(string keyWord)
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