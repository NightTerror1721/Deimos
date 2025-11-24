#nullable enable

using System;

namespace Deimos.Lexer
{
    internal static class NumberUtils
    {
        internal static bool IsDigit(this char c) => char.IsDigit(c);

        internal static bool IsBinaryDigit(this char c)
        {
            return c == '0' || c == '1';
        }

        internal static bool IsOctalDigit(this char c)
        {
            return c >= '0' && c <= '7';
        }

        internal static bool IsHexDigit(this char c)
        {
            return (c >= '0' && c <= '9') ||
                   (c >= 'a' && c <= 'f') ||
                   (c >= 'A' && c <= 'F');
        }

        internal static string ExtractDigits(string text, int start, int end)
        {
            char[] arr = Array.FindAll(text[start..end].ToCharArray(),
                c => char.IsDigit(c) || char.IsLetter(c) || c == '.');

            return new string(arr);
        }

        internal static string ExtractDigitsExceptP(string text, int start, int end)
        {
            char[] arr = Array.FindAll(text[start..end].ToCharArray(), c =>
                char.IsDigit(c) ||
                IsHexDigit(c) ||
                c == '.' ||
                c == 'p' || c == 'P' ||
                c == '+' || c == '-' ||
                c == 'x' || c == 'X'
            );
            return new string(arr);
        }

        internal static double HexFloatToDouble(string literal)
        {
            literal = literal.ToLowerInvariant();

            bool negative = literal.StartsWith("-");
            if (negative) literal = literal[1..];

            // remove possible +
            if (literal.StartsWith("+"))
                literal = literal[1..];

            // remove 0x
            if (literal.StartsWith("0x"))
                literal = literal[2..];

            int idx = literal.IndexOf('p');
            string mantStr = literal[..idx];
            string expStr = literal[(idx + 1)..];

            int exp = int.Parse(expStr);

            string[] parts = mantStr.Split('.');
            long intPart = parts[0] == "" ? 0 : Convert.ToInt64(parts[0], 16);

            double frac = 0;
            if (parts.Length > 1)
            {
                string f = parts[1];
                for (int k = 0; k < f.Length; k++)
                {
                    int digit = Convert.ToInt32(f[k].ToString(), 16);
                    frac += digit / Math.Pow(16, k + 1);
                }
            }

            double mantissa = intPart + frac;
            double result = mantissa * Math.Pow(2, exp);

            return negative ? -result : result;
        }
    }
}
