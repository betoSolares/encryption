using System;
using System.IO;

namespace encryption.Utils
{
    public class ZigZagUtils
    {
        public void Encrypt(string path, int key)
        {
            int length = GetLength(path);
        }

        /// <summary>Create the matrix</summary>
        /// <param name="key">The key for the encryption</param>
        /// <param name="length">The length of the text</param>
        /// <returns>A matrix with the specific dimensions</returns>
        private char[,] CreateMatrix(int key, int length)
        {
            char[,] matrix = new char[key, length];
            for (int i = 0; i < key; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    matrix[i, j] = Convert.ToChar(1);
                }
            }
            return matrix;
        }

        /// <summary>Get the length of the string</summary>
        /// <param name="path">The path of the file</param>
        /// <returns>The length of the file</returns>
        private int GetLength(string path)
        {
            StreamReader reader = new StreamReader(path);
            int length = reader.ReadToEnd().Length;
            reader.Close();
            return length;
        }
    }
}
