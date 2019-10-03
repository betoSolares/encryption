using System;
using System.IO;

namespace encryption.Utils
{
    public class ZigZagUtils
    {
        /// <summary>Encrypt the message in the file</summary>
        /// <param name="path">The path to the file</param>
        /// <param name="key">The key for the encription</param>
        /// <param name="newPath">The path of the new file</param>
        public void Encrypt(string path, int key, ref string newPath)
        {
            int length = GetLength(path);
            char[,] matrix = CreateMatrix(key, length);
            matrix = FillMatrix(matrix, key, path);
            newPath = WriteEncryptedMessage(matrix, key, length);
        }

        /// <summary>Decrypt the message in the file</summary>
        /// <param name="path">The path to the file</param>
        /// <param name="key">The key for the encription</param>
        /// <param name="newPath">The path of the new file</param>
        public void Decrypt(string path, int key, ref string newPath)
        {
            int length = GetLength(path);
            char[,] matrix = CreateMatrix(key, length);
            matrix = MarkPlaces(matrix, length, key);
            matrix = ConstructMatrix(matrix, key, length, path);
            newPath = WriteDecryptedMessage(matrix, key, length);
        }

        /// <summary>Fill the matrix with the characters in the file</summary>
        /// <param name="matrix">The matrix to fill</param>
        /// <param name="key">The key for the encryption</param>
        /// <param name="length">The length of the text</param>
        /// <param name="path">The path to the file to read</param>
        /// <returns>A matrix filled with the characters in the file</returns>
        private char[,] ConstructMatrix(char[,] matrix, int key, int length, string path)
        {
            int index = 0;
            string text = File.ReadAllText(path);
            for (int i = 0; i < key; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    if (matrix[i, j].Equals(Convert.ToChar(2)) && index < length)
                    {
                        matrix[i, j] = text[index];
                        index++;
                    }
                }
            }
            return matrix;
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
        /// <param name="key">The key for the encryption</param>
        /// <param name="path">The path to the file to read</param>
        /// <returns>A matrix filled with the characters in the file</returns>
        private char[,] FillMatrix(char[,] matrix, int key, string path)
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

        /// <summary>Mark the places to put the characters</summary>
        /// <param name="matrix">The matrix to fill</param>
        /// <param name="length">The length of the text</param>
        /// <param name="key">The key for the decompretion</param>
        /// <returns>A matrix filled with the new characters</returns>
        private char[,] MarkPlaces(char[,] matrix, int length, int key)
        {
            bool isDirectionDown = false;
            int row = 0;
            int columns = 0;
            for (int i = 0; i < length; i++)
            {
                if (row == 0)
                    isDirectionDown = true;
                if (row == key - 1)
                    isDirectionDown = false;
                matrix[row, columns] = Convert.ToChar(2);
                if (isDirectionDown)
                    row++;
                else
                    row--;
                columns++;
            }
            return matrix;
        }

        /// <summary>Iterate through the matrix and write the message</summary>
        /// <param name="matrix">The matrix to iterate</param>
        /// <param name="key">The key for the decription</param>
        /// <param name="length">The length of the text</param>
        /// <returns>The path of the new file</returns>
        private string WriteDecryptedMessage(char[,] matrix, int key, int length)
        {
            string path = new FileUtils().CreateFile("DecryptedMessage", ".txt", "~/App_Data/Downloads");
            bool isDirectionDown = false;
            int row = 0;
            int columns = 0;
            for (int i = 0; i < length; i++)
            {
                if (row == 0)
                    isDirectionDown = true;
                if (row == key - 1)
                    isDirectionDown = false;
                if (!matrix[row, columns].Equals(Convert.ToChar(2)))
                {
                    StreamWriter writer = new StreamWriter(path, true);
                    writer.Write(matrix[row, columns]);
                    writer.Close();
                }
                if (isDirectionDown)
                    row++;
                else
                    row--;
                columns++;
            }
            return path;
        }

        /// <summary>Iterate through the matrix and write the message</summary>
        /// <param name="matrix">The matrix to iterate</param>
        /// <param name="key">The key for the encription</param>
        /// <param name="length">The length of the text</param>
        /// <returns>The path of the new file</returns>
        private string WriteEncryptedMessage(char[,] matrix, int key, int length)
        {
            string path = new FileUtils().CreateFile("EncryptedMessage", ".cif", "~/App_Data/Downloads");
            for (int i = 0; i < key; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    if (!matrix[i,j].Equals(Convert.ToChar(1)))
                    {
                        StreamWriter writer = new StreamWriter(path, true);
                        writer.Write(matrix[i, j]);
                        writer.Close();
                    }
                }
            }
            return path;
        }
    }
}
