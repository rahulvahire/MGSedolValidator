using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace SedolValidator
{
    public class Sedol
    {
        private readonly string _value;
        private readonly List<int> _weights;
        private const int CHECK_DIGIT_IDX = 6;
        private const int SEDOL_LENGTH = 7;
        private const int USER_DEFINED_IDX = 0;
        private const char USER_DEFINED_CHAR = '9';
        
        /// <summary>
        /// Representation of a sedol.  No need to actually expose the input string so it is kept private
        /// character/index Weights are 1,3,1,7,3,9
        /// </summary>
        /// <param name="input"></param>
        public Sedol(string input)
        {
            // Could potentially inject weights
            _weights = new List<int> { 1, 3, 1, 7, 3, 9 };
            _value = input;
        }

        /// <summary>
        /// Returns the Sedol character index for the supplied Character.  Alphabet position + 11
        /// Implemented as Upper ASCII code - 55 (for letters)
        /// ASCII Code - 48 (for numbers)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static int Code(char input)
        {
            if (Char.IsLetter(input))
                return Char.ToUpper(input) - 55;
            return input - 48;
        }

        /// <summary>
        /// Returns the Sedol check digit for a the Sedol.
        /// Calculation is ((10 - (weightedSum Mod 10)) Mod 10)
        /// </summary>
        /// <returns></returns>
        public char CheckDigit
        {
            get
            {
                var codes = _value.Take(SEDOL_LENGTH - 1).Select(Code).ToList();
                var weightedSum = _weights.Zip(codes, (w, c) => w*c).Sum();
                return Convert.ToChar(((10 - (weightedSum%10))%10).ToString(CultureInfo.InvariantCulture));
            }
        }

        /// <summary>
        /// Returns true if the input string only contains AlphaNumeric characters
        /// </summary>
        /// <returns></returns>
        public bool IsAlphaNumeric
        {
            get { return Regex.IsMatch(_value, "^[a-zA-Z0-9]*$"); }
        }

        /// <summary>
        /// Returns true if the input string is the correct length for a Sedol (7 characters)
        /// </summary>
        /// <returns></returns>
        public bool IsValidLength
        {
            get { return !String.IsNullOrEmpty(_value) && _value.Length == SEDOL_LENGTH; }
        }

        /// <summary>
        /// Returns true if the character at the specified index is the current "User Defined" char
        /// No validation of string length, if it's less than the USER_DEFINED_IDX it will throw an
        /// IndexOutOfRangeException.
        /// </summary>
        /// <returns></returns>
        public bool IsUserDefined
        {
            get { return _value[USER_DEFINED_IDX] == USER_DEFINED_CHAR; }
        }

        /// <summary>
        /// Returns true if the existing check digit on a sedol matches the calculated check digit.
        /// No validation of string length, if it's less than the CHECK_DIGIT_IDX it will throw an
        /// IndexOutOfRangeException.
        /// </summary>
        /// <returns></returns>
        public bool HasValidCheckDigit
        {
            get { return _value[CHECK_DIGIT_IDX] == CheckDigit; }
        }

        /// <summary>
        /// Returns true if the input string contains a vowel character
        /// </summary>
        /// <returns></returns>
        public bool ContainsVowel
        {
            get { return Regex.IsMatch(_value.ToUpper(), "[AEUIO]"); }
        }
    }
}
