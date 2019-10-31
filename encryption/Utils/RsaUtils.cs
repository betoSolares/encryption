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

    }
}