using System;
using System.IO;

namespace encryption.Utils
{
    public class SpiralUtils
    {
        public void Encrypt(string path, int key, ref string newPath)
        {
            int length = GetLength(path);
            int n = CalculateN(key, length);
            char[,] matrix = CreateMatrix(key, n);
            matrix = FillMatrix(matrix, n, path);
            newPath = WriteDecryptedMessage(matrix, key, n);
        }

        /// <summary>Calculate the N value</summary>
        /// <param name="m">The key for the encryption</param>
        /// <param name="length">The length of the text</param>
        /// <returns>The N value</returns>
        private int CalculateN(int m, int length)
        {
            return (int) Math.Ceiling((double) length / m);
        }

        /// <summary>Create the matrix</summary>
        /// <param name="key">The key for the encryption</param>
        /// <param name="length">The length of the text</param>
        /// <returns>A matrix with the specific dimensions</returns>
        private char[,] CreateMatrix(int m, int n)
        {
            char[,] matrix = new char[m, n];
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    matrix[i, j] = Convert.ToChar(1);
                }
            }
            return matrix;
        }

        private char[,] FillMatrix(char[,] matrix, int n, string path)
        {
            int row = 0;
            int columns = 0;
            StreamReader reader = new StreamReader(path);
            do
            {
                char character = (char)reader.Read();
                matrix[row, columns] = character;
                if (columns == n -1)
                {
                    row++;
                    columns = 0;
                }
                else
                {
                    columns++;
                }
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

        private string WriteDecryptedMessage(char[,] matrix, int m, int n)
        {
            string path = new FileUtils().CreateFile("EncryptedMessage", ".txt", "~/App_Data/Downloads");
            int i, k = 0, l = 0;
            while (k < m && l < n)
            {
                for (i = l; i < n; ++i)
                {
                    StreamWriter writer = new StreamWriter(path, true);
                    writer.Write(matrix[k, i]);
                    writer.Close();
                }
                k++;

                for (i = k; i < m; ++i)
                {
                    StreamWriter writer = new StreamWriter(path, true);
                    writer.Write(matrix[i, n - 1]);
                    writer.Close();
                }
                n--;

                if (k < m)
                {
                    for (i = n - 1; i >= l; --i)
                    {
                        StreamWriter writer = new StreamWriter(path, true);
                        writer.Write(matrix[m - 1, i]);
                        writer.Close();
                    }
                    m--;
                }

                if (l < n)
                {
                    for (i = m - 1; i >= k; --i)
                    {
                        StreamWriter writer = new StreamWriter(path, true);
                        writer.Write(matrix[i, l]);
                        writer.Close();
                    }
                    l++;
                }
            }
            return path;
        }
    }
}