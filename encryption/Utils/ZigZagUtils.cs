using System;
using System.IO;

namespace encryption.Utils
{
    public class ZigZagUtils
    {
        public void Encrypt(string path, int key)
        {
            int length = GetLength(path);
            char[,] matrix = CreateMatrix(key, length);
            matrix = FillMatrix(matrix, length, key, path);
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

        /// <summary>Fill the matrix with the chars in the file</summary>
        /// <param name="matrix">The matrix to fill</param>
        /// <param name="length">The length of the text</param>
        /// <param name="key">The key for the encryption</param>
        /// <param name="path">The path to the file to read</param>
        /// <returns>A matrix filled with the characters in the file</returns>
        private char[,] FillMatrix(char[,] matrix, int length, int key, string path)
        {
            bool isDirectionDown = false;
            int row = 0;
            int columns = 0;
            StreamReader reader = new StreamReader(path);
            do
            {
                if (row == 0 || row == key - 1)
                    isDirectionDown = !isDirectionDown;
                char character = (char)reader.Read();
                matrix[row, columns] = character;
                if (isDirectionDown)
                    row++;
                else
                    row--;
                columns++;
            } while (!reader.EndOfStream);
            reader.Close();
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
