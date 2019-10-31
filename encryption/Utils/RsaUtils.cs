namespace encryption.Utils
{
    public class RsaUtils
    {
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

    }
}