using System.Collections.Generic;
using System.IO;
​
namespace encryption.Utils
{
    public class RsaUtils
    {
        private readonly FileUtils fileUtils = new FileUtils();
​
        /// <summary>Generate the public and private key</summary>
        /// <param name="p">The first value for the key generation</param>
        /// <param name="q">The second value for the key generation</param>
        /// <param name="files">The list of the paths for the key files</param>
        /// <returns>True if the keys are ok, otherwise false</returns>
        public bool GenerateKeys(ulong p, ulong q, ref List<string> files)
        {
            if (IsPrime(p) && IsPrime(q))
            {
                ulong n = p * q;
                ulong totient = (p - 1) * (q - 1);
                ulong e = GeneratePublicKey(p, totient);
                ulong d = GeneratePrivateKey(e, totient);
                files.Add(WriteKeyFile("~/App_Data/RSA-Keys", "public", e, n));
                files.Add(WriteKeyFile("~/App_Data/RSA-Keys", "private", d, n));
                return true;
            }
            return false;
        }
​
        /// <summary>Calculate the greatest common divisor of two integers</summary>
        /// <param name="a">The first integer</param>
        /// <param name="b">The second integer</param>
        /// <returns>The greatest common divisor</returns>
        private ulong GCD(ulong a, ulong b)
        {
            bool isFound = false;
            ulong temporal;
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
​
        /// <summary>Calculate the private key</summary>
        /// <param name="e">The e value for the modular inverse</param>
        /// <param name="totient">The totient value for the modulat inverse</param>
        /// <returns>The modular inverse of e and totient</returns>
        private ulong GeneratePrivateKey(ulong e, ulong totient)
        {
            ulong inv;
            ulong u1 = 1;
            ulong u3 = e;
            ulong v1 = 0;
            ulong v3 = totient;
            ulong iter = 1;
            while (v3 != 0)
            {
                ulong q = u3 / v3;
                ulong t3 = u3 % v3;
                ulong t1 = u1 + q * v1;
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
​
        /// <summary>Generate the public key</summary>
        /// <param name="totient">The number of coprimes between p and q</param>
        /// <returns>A numberbetween 1 and totient that the greatest commom divisor is equals to 1</returns>
        private ulong GeneratePublicKey(ulong p, ulong totient)
        {
            ulong e = 2;
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
​
        /// <summary>Check if a number is prime or not</summary>
        /// <param name="number">The number to check</param>
        /// <returns>True if it's prime, otherwise false</returns>
        private bool IsPrime(ulong number)
        {
            int cont = 0;
            for (ulong i = 1; i <= number; i++)
            {
                if (number % i == 0)
                    cont++;
            }
            if (cont == 2)
                return true;
            return false;
        }
​
        /// <summary>Write to a file the key information</summary>
        /// <param name="path">The path of the file</param>
        /// <param name="name">The name of the file</param>
        /// <param name="extension">The extension of the file</param>
        /// <param name="value">The value of the key</param>
        /// <param name="module">The modular number</param>
        /// <returns>The path of the new file</returns>
        private string WriteKeyFile(string path, string name, ulong value, ulong module)
        {
            string newFile = fileUtils.CreateFile(name, ".key", path);
            StreamWriter writer = new StreamWriter(newFile);
            writer.WriteLine(value + "," + module);
            writer.Close();
            return newFile;
        }
    }
}