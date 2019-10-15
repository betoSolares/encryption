using encryption.Models;
using System;

namespace encryption.Utils
{
    public class SdesUtils
    {
        /// <summary>Encrypt the message in the file</summary>
        /// <param name="path">The path to the file</param>
        /// <param name="key">The key for the encription</param>
        /// <param name="newPath">The path of the new file</param>
        public void Encrypt(string path, int key, ref string newPath)
        {
            Permutations permutations = new Permutations("8537926014", "79358216", "0321", "01323210", "63572014");
            if (permutations.CheckPermutations())
            {
                string binaryKey = Convert.ToString(key, 2).PadLeft(10, '0');
                (string K1, string K2) keys = GenerateKeys(binaryKey, permutations.P10, permutations.P8);
            }
        }

        /// <summary>Generate the keys following the S-DES algorithm</summary>
        /// <param name="key">The 10 bits key</param>
        /// <param name="p10">The P10 permutation</param>
        /// <param name="p8">The P8 permutation</param>
        /// <returns>A tuple with the K1 and K2</returns>
        private (string K1, string K2) GenerateKeys(string key, string p10, string p8)
        {
            string P10 = string.Empty;
            foreach(char index in p10)
            {
                P10 += key[int.Parse(index.ToString())];
            }
            string LS1 = LeftShift(P10);
            string K1 = string.Empty;
            foreach (char index in p8)
            {
                K1 += LS1[int.Parse(index.ToString())];
            }
            string LS2 = LeftShift(LeftShift(LS1));
            string K2 = string.Empty;
            foreach (char index in p8)
            {
                K2 += LS2[int.Parse(index.ToString())];
            }
            return (K1, K2);
        }

        /// <summary>Shift the string in one position to the left</summary>
        /// <param name="input">The string to shift</param>
        /// <returns>A new string shifted</returns>
        private string LeftShift(string input)
        {
            string firstBlock = input.Substring(0, 5);
            string secondBlock = input.Substring(5, 5);
            string firstShift = firstBlock.Substring(1, firstBlock.Length - 1) + firstBlock.Substring(0, 1);
            string secondShift = secondBlock.Substring(1, secondBlock.Length - 1) + secondBlock.Substring(0, 1);
            return firstShift + secondShift;
        }
    }
}