using System;
using System.IO;

namespace encryption.Utils
{
    public class SpiralUtils
    {
        /// <summary>Encrypt the message in the file</summary>
        /// <param name="path">The path to the file</param>
        /// <param name="key">The key for the encription</param>
        /// <param name="newPath">The path of the new file</param>
        /// <param name="direction">The direction to read the matrix</param>
        public void Encrypt(string path, int key, ref string newPath, string direction)
        {
            int length = GetLength(path);
            int n = CalculateN(key, length);
            char[,] matrix = CreateMatrix(key, n);
            matrix = FillMatrix(matrix, n, path);
            if (direction.Equals("Right"))
                newPath = WriteRightEncryptedMessage(matrix, key, n);
            else
                newPath = WriteLeftEncryptedMessage(matrix, key, n);
        }

        /// <summary>Decrypt the message in the file</summary>
        /// <param name="path">The path to the file</param>
        /// <param name="key">The key for the encription</param>
        /// <param name="newPath">The path of the new file</param>
        /// <param name="direction">The direction to read the matrix</param>
        public void Decrypt(string path, int key, ref string newPath, string direction)
        {
            int length = GetLength(path);
            int n = CalculateN(key, length);
            char[,] matrix = CreateMatrix(n, key);
            if (direction.Equals("Right"))
                matrix = RegenerateMatrixRight(matrix, path, key, n);
            else
                matrix = RegenerateMatrixLeft(matrix, path, key, n);
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

        /// <summary>Fill the matrix</summary>
        /// <param name="matrix">The matrix to fill</param>
        /// <param name="n">The n length of the matrix</param>
        /// <param name="path">The path of the file to read</param>
        /// <returns>A matrix with all the chars in the file</returns>
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

        /// <summary>Fill the matrix for the decryption in the left direction</summary>
        /// <param name="matrix">The matrix to fill</param>
        /// <param name="path">The path of the file to read</param>
        /// <param name="m">The m length of the matrix</param>
        /// <param name="n">The n length of the matrix</param>
        /// <returns>A matrix filled with all the chars</returns>
        private char[,] RegenerateMatrixLeft(char[,] matrix, string path, int m, int n)
        {
            int count = 0;
            string text = File.ReadAllText(path);
            int k = 0;
            int l = 0;
            while (k < n && l < m)
            {
                // Down
                for (int i = k; i < n; i++)
                {
                    matrix[i, k] = text[count];
                    count++;
                }
                k++;

                // Right
                for (int i = k; i < m; i++)
                {
                    matrix[n - 1, i] = text[count];
                    count++;
                }
                n--;

                if (k < m)
                {
                    // Up
                    for (int i = n - 1; i >= l; i--)
                    {
                        matrix[i, m - 1] = text[count];
                        count++;
                    }
                    m--;
                }

                if (l < n)
                {
                    // Left
                    for (int i = m - 1; i >= k; i--)
                    {
                        matrix[l, i] = text[count];
                        count++;
                    }
                    l++;
                }
            }
            return matrix;
        }

        /// <summary>Fill the matrix for the decryption in the right direction</summary>
        /// <param name="matrix">The matrix to fill</param>
        /// <param name="path">The path of the file to read</param>
        /// <param name="m">The m length of the matrix</param>
        /// <param name="n">The n length of the matrix</param>
        /// <returns>A matrix filled with all the chars</returns>
        private char[,] RegenerateMatrixRight(char[,] matrix, string path, int m, int n)
        {
            int count = 0;
            string text = File.ReadAllText(path);
            int k = 0;
            int l = 0;
            while (k < n && l < m)
            {
                // Right
                for (int i = l; i < m; i++)
                {
                    matrix[k, i] = text[count];
                    count++;
                }
                k++;

                // Down
                for (int i = k; i < n; i++)
                {
                    matrix[i, m - 1] = text[count];
                    count++;
                }
                m--;

                if (k < n)
                {
                    // Left
                    for (int i = m - 1; i >= l; i--)
                    {
                        matrix[n - 1, i] = text[count];
                        count++;
                    }
                    n--;
                }

                if (l < m)
                {
                    // Up
                    for (int i = n - 1; i >= k; i--)
                    {
                        matrix[i, l] = text[count];
                        count++;
                    }
                    l++;
                }
            }
            return matrix;
        }

        /// <summary>Iterate through the matrix and write the message</summary>
        /// <param name="matrix">The matrix to iterate</param>
        /// <param name="m">The m length of the matrix</param>
        /// <param name="n">The n length og the matrix</param>
        /// <returns>The path of the new file</returns>
        private string WriteDecryptedMessage(char[,] matrix, int m, int n)
        {
            string path = new FileUtils().CreateFile("DecryptedMessage", ".txt", "~/App_Data/Downloads");
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (!matrix[j, i].Equals(Convert.ToChar(1)))
                    {
                        StreamWriter writer = new StreamWriter(path, true);
                        writer.Write(matrix[j, i]);
                        writer.Close();
                    }
                }
            }
            return path;
        }

        /// <summary>Write the encrypted message to the left</summary>
        /// <param name="matrix">The matrix with the chars</param>
        /// <param name="m">The m length of the matrix</param>
        /// <param name="n">The n length of the matrix</param>
        /// <returns>The path of the new file encrypted</returns>
        private string WriteLeftEncryptedMessage(char[,] matrix, int m, int n)
        {
            string path = new FileUtils().CreateFile("EncryptedMessage", ".cif", "~/App_Data/Downloads");
            int k = 0;
            int l = 0;
            while (k < m && l < n)
            {
                // Down
                for (int i = k; i < m; i++)
                {
                    StreamWriter writer = new StreamWriter(path, true);
                    writer.Write(matrix[i, k]);
                    writer.Close();
                }
                k++;

                // Right
                for (int i = k; i < n; i++)
                {
                    StreamWriter writer = new StreamWriter(path, true);
                    writer.Write(matrix[m - 1, i]);
                    writer.Close();
                }
                m--;
                
                if (k < n)
                {
                    // Up
                    for (int i = m - 1; i >= l; i--)
                    {
                        StreamWriter writer = new StreamWriter(path, true);
                        writer.Write(matrix[i, n - 1]);
                        writer.Close();
                    }
                    n--;
                }

                if (l < m)
                {
                    // Left
                    for (int i = n - 1; i >= k; i--)
                    {
                        StreamWriter writer = new StreamWriter(path, true);
                        writer.Write(matrix[l, i]);
                        writer.Close();
                    }
                    l++;
                }
            }
            return path;
        }

        /// <summary>Write the encrypted message to the rigth</summary>
        /// <param name="matrix">The matrix with the chars</param>
        /// <param name="m">The m length of the matrix</param>
        /// <param name="n">The n length of the matrix</param>
        /// <returns>The path of the new file encrypted</returns>
        private string WriteRightEncryptedMessage(char[,] matrix, int m, int n)
        {
            string path = new FileUtils().CreateFile("EncryptedMessage", ".cif", "~/App_Data/Downloads");
            int k = 0;
            int l = 0;
            while (k < m && l < n)
            {
                // Right
                for (int i = l; i < n; i++)
                {
                    StreamWriter writer = new StreamWriter(path, true);
                    writer.Write(matrix[k, i]);
                    writer.Close();
                }
                k++;

                // Down
                for (int i = k; i < m; i++)
                {
                    StreamWriter writer = new StreamWriter(path, true);
                    writer.Write(matrix[i, n - 1]);
                    writer.Close();
                }
                n--;

                if (k < m)
                {
                    // Left
                    for (int i = n - 1; i >= l; i--)
                    {
                        StreamWriter writer = new StreamWriter(path, true);
                        writer.Write(matrix[m - 1, i]);
                        writer.Close();
                    }
                    m--;
                }

                if (l < n)
                {
                    // Up
                    for (int i = m - 1; i >= k; i--)
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