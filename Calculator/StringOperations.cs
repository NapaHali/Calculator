using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Main namespace of Calculator
/// </summary>
namespace Calculator
{
    /// <summary>
    /// StringOperations class used to determine whether we caught a number in specified place in the string
    /// </summary>
    internal static class StringOperations
    {
        /// <summary>
        /// Tries to parse the string into integer format
        /// </summary>
        /// <param name="ch">Character to be parsed</param>
        /// <returns>true if the string is number, false otherwise</returns>
        public static bool isNumeric(char ch)
        {
            return int.TryParse(ch.ToString(), out _);
        }

        /// <summary>
        /// Checks whether the last character in string is a number
        /// </summary>
        /// <param name="text">String where should the last character be checked</param>
        /// <returns>true if the last character is a number, false otherwise</returns>
        public static bool isLastNumeric(string text)
        {
            if (text.Length == 0) return false;
            return int.TryParse(text[text.Length - 1].ToString(), out _);
        }

        /// <summary>
        /// Evaluates if the string's last character equals to the given character as a second parameter
        /// </summary>
        /// <param name="text">String where should the last character be checked</param>
        /// <param name="ch">Character to look for in the string</param>
        /// <returns>true if last character in string equals the given character, false otherwise</returns>
        public static bool lastEquals(string text, char ch)
        {
            if (text.Length == 0) return false;
            return text[text.Length - 1] == ch;
        }
    }
}
