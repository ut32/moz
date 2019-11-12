using System;

// ReSharper disable once CheckNamespace
namespace System
{
    public static class LongExtensions
    {
        public static string To36Base(this long num) 
        {
            if (num == 0) return "0";

            var values = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
            long temp = Math.Abs(num);
            var nb = "";
            while (temp != 0) 
            {
                long c = temp % 36;
                temp = temp / 36;
                nb = values[(int)c] + nb;
            }
            return nb;
        }

        public static string To36Base(this int num)
        {
            return To36Base((long)num);
        }
    }
}