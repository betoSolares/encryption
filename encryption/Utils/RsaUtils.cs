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
    }
}