using System;
using System.Collections.Generic;
using System.IO;

namespace encryption.Utils
{
    public class RsaUtils
    {
        private readonly FileUtils fileUtils = new FileUtils();

        /// <summary>Encrypt the message in the file</summary>
        /// <param name="file">The file to encrypt</param>
        /// <param name="key">The file with the key</param>
        /// <param name="newPath">The path of the new file</param>
        /// <returns>True if the file was encrypted correct, otherwise false</returns>
        public bool Decrypt(string file, string key, ref string newPath)
        {
            int d = 0;
            int n = 0;
            if (ParseKeyFile(key, ref d, ref n))
            {
                List<byte> bytes = ReadFile(file);
                List<byte> newByteList = DecryptMessage(bytes, d, n);
                string name = Path.GetFileNameWithoutExtension(file);
                newPath = new FileUtils().CreateFile(name, ".txt", "~/App_Data/Downloads");
                WriteFile(newByteList, newPath);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>Encrypt the message in the file</summary>
        /// <param name="file">The file to encrypt</param>
        /// <param name="key">The file with the key</param>
        /// <param name="newPath">The path of the new file</param>
        /// <returns>True if the file was encrypted correct, otherwise false</returns>
        public bool Encrypt(string file, string key, ref string newPath)
        {
            int e = 0;
            int n = 0;
            if (ParseKeyFile(key, ref e, ref n))
            {
                List<byte> bytes = ReadFile(file);
                List<byte> encryptedBytes = EncryptMessage(bytes, e, n);
                string name = Path.GetFileNameWithoutExtension(file);
                newPath = new FileUtils().CreateFile(name, ".rsacif", "~/App_Data/Downloads");
                WriteFile(encryptedBytes, newPath);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>Generate the public and private key</summary>
        /// <param name="p">The first value for the key generation</param>
        /// <param name="q">The second value for the key generation</param>
        /// <param name="files">The list of the paths for the key files</param>
        /// <returns>True if the keys are ok, otherwise false</returns>
        public bool GenerateKeys(int p, int q, ref List<string> files)
        {
            if (IsPrime(p) && IsPrime(q))
            {
                int n = p * q;
                int totient = (p - 1) * (q - 1);
                int e = GeneratePublicKey(totient);
                int d = GeneratePrivateKey(e, totient);
                files.Add(WriteKeyFile("~/App_Data/RSA-Keys", "public", e, n));
                files.Add(WriteKeyFile("~/App_Data/RSA-Keys", "private", d, n));
                return true;
            }
            return false;
        }

        /// <summary>Decrypt each character of the file</summary>
        /// <param name="Byte">The byte to encrypt</param>
        /// <param name="d">The private key</param>
        /// <param name="n">The modular number</param>
        /// <returns>The new decrypted byte</returns>
        private List<byte> DecryptMessage(List<byte> bytes, int d, int n)
        {
            List<byte> result = new List<byte>();
            foreach (byte Byte in bytes)
            {
                ulong encrypted;
                if (Byte - 1 == 0)
                    encrypted = Convert.ToUInt64(n);
                else if (Byte - 1 == 1)
                    encrypted = Convert.ToUInt64(n + 1);
                else
                    encrypted = Convert.ToUInt64((Math.Pow(Byte, d) % n));
                byte newByte = Convert.ToByte(encrypted);
                result.Add(newByte);
            }
            return result;
        }

        /// <summary>Encrypt each character of the file</summary>
        /// <param name="Byte">The byte to encrypt</param>
        /// <param name="e">The public key</param>
        /// <param name="n">The modular number</param>
        /// <param name="maxValue">The max value ofr the binary</param>
        /// <returns>The new cipher byte</returns>
        private List<byte> EncryptMessage(List<byte> bytes, int e, int n)
        {
            List<byte> result = new List<byte>();
            foreach (byte Byte in bytes)
            {
                byte encrypted = Convert.ToByte((Math.Pow(Byte, e) % n) + 1);
                result.Add(encrypted);
            }
            return result;
        }

        /// <summary>Calculate the greatest common divisor of two integers</summary>
        /// <param name="a">The first integer</param>
        /// <param name="b">The second integer</param>
        /// <returns>The greatest common divisor</returns>
        private int GCD(int a, int b)
        {
            bool isFound = false;
            int temporal;
            while (!isFound)
            {
                temporal = a % b;
                if (temporal == 0)
                {
                    isFound = true;
                }
                else
                {
                    a = b;
                    b = temporal;
                }
            }
            return b;
        }

        /// <summary>Calculate the private key</summary>
        /// <param name="e">The e value for the modular inverse</param>
        /// <param name="totient">The totient value for the modulat inverse</param>
        /// <returns>The modular inverse of e and totient</returns>
        private int GeneratePrivateKey(int e, int totient)
        {
            int inv;
            int u1 = 1;
            int u3 = e;
            int v1 = 0;
            int v3 = totient;
            int iter = 1;
            while (v3 != 0)
            {
                int q = u3 / v3;
                int t3 = u3 % v3;
                int t1 = u1 + q * v1;
                u1 = v1; v1 = t1; u3 = v3; v3 = t3;
                iter -= iter;
            }
            if (u3 != 1)
                return 0;
            if (iter < 0)
                inv = totient - u1;
            else
                inv = u1;
            return inv;
        }

        /// <summary>Generate the public key</summary>
        /// <param name="totient">The number of coprimes between p and q</param>
        /// <returns>A numberbetween 1 and totient that the greatest commom divisor is equals to 1</returns>
        private int GeneratePublicKey(int totient)
        {
            int e = 2;
            bool isFound = false;
            while (e < totient && !isFound)
            {
                if (GCD(e, totient) == 1 && e < totient)
                    isFound = true;
                else
                    e++;
            }
            return e;
        }

        /// <summary>Check if a number is prime or not</summary>
        /// <param name="number">The number to check</param>
        /// <returns>True if it's prime, otherwise false</returns>
        private bool IsPrime(int number)
        {
            int cont = 0;
            for (int i = 1; i <= number; i++)
            {
                if (number % i == 0)
                    cont++;
            }
            if (cont == 2)
                return true;
            return false;
        }

        /// <summary>Tries to parse the key file</summary>
        /// <param name="path">The path of the file</param>
        /// <param name="key">Th value of the key</param>
        /// <param name="mod">The value of the mod</param>
        /// <returns>True if the file was parsed succesful, otherwise false</returns>
        private bool ParseKeyFile(string path, ref int key, ref int mod)
        {
            StreamReader reader = new StreamReader(path);
            bool correctParse;
            try
            {
                string[] line = reader.ReadLine().Split(',');
                key = int.Parse(line[0]);
                mod = int.Parse(line[1]);
                correctParse = true;
            }
            catch
            {
                correctParse = false;
            }
            reader.Close();
            return correctParse;
        }

        /// <summary>Read all the bytes in the file</summary>
        /// <param name="path">The path of the file to read</param>
        /// <returns>A list with all the bytes</returns>
        private List<byte> ReadFile(string path)
        {
            List<byte> bytes = new List<byte>();
            BinaryReader reader = new BinaryReader(new FileStream(path, FileMode.Open));
            while (reader.BaseStream.Position != reader.BaseStream.Length)
            {
                byte[] buffer = reader.ReadBytes(1000);
                bytes.AddRange(buffer);
            }
            reader.Close();
            return bytes;
        }

        /// <summary>Write the specified set of bytes to a file</summary>
        /// <param name="bytes">The bytes to write</param>
        /// <param name="path">The path to the file</param>
        private void WriteFile(List<byte> bytes, string path)
        {
            BinaryWriter writer = new BinaryWriter(new FileStream(path, FileMode.OpenOrCreate));
            writer.Write(bytes.ToArray());
            writer.Close();
        }

        /// <summary>Write to a file the key information</summary>
        /// <param name="path">The path of the file</param>
        /// <param name="name">The name of the file</param>
        /// <param name="extension">The extension of the file</param>
        /// <param name="value">The value of the key</param>
        /// <param name="module">The modular number</param>
        /// <returns>The path of the new file</returns>
        private string WriteKeyFile(string path, string name, int value, int module)
        {
            string newFile = fileUtils.CreateFile(name, ".key", path);
            StreamWriter writer = new StreamWriter(newFile);
            writer.WriteLine(value + "," + module);
            writer.Close();
            return newFile;
        }
    }
}