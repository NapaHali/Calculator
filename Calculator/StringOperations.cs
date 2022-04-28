using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    internal static class StringOperations
    {
        public static bool isNumeric(char ch)
        {
            return int.TryParse(ch.ToString(), out _);
        }

        public static bool isLastNumeric(string text)
        {
            if (text.Length == 0) return false;
            return int.TryParse(text[text.Length - 1].ToString(), out _);
        }

        public static bool lastEquals(string text, char ch)
        {
            if (text.Length == 0) return false;
            return text[text.Length - 1] == ch;
        }
    }
}
