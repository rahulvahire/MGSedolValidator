using SedolValidatorInterfaces;

namespace SedolValidator
{
    /// <summary>
    /// A Sedol Validator
    /// </summary>
    public class SedolValidator : ISedolValidator
    {
        /// <summary>
        /// Accepts an input string and returns an appropriate ISedolValidationResult
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public ISedolValidationResult ValidateSedol(string input)
        {
            // should/could be injected to separate functionality of validator with sedol class
            // sedol class knows about sedols, validator class knows how to combine the properties of a sedol
            // to generate the expected validation result.
            var sedol = new Sedol(input);

            var result = new SedolValidationResult{
                InputString = input,
                IsUserDefined = false,
                IsValidSedol = false,
                ValidationDetails = null
            };

            if (!sedol.IsValidLength)
            {
                result.ValidationDetails = Constants.INPUT_STRING_NOT_VALID_LENGTH;
                return result;
            }
            if (!sedol.IsAlphaNumeric)
            {
                result.ValidationDetails = Constants.INPUT_STRING_NOT_ALPHANUMERIC;
                return result;
            }
            if (sedol.IsUserDefined)
            {
                result.IsUserDefined = true;
                if (sedol.HasValidCheckDigit)
                {
                    result.IsValidSedol = true;
                    return result;
                }
                result.ValidationDetails = Constants.CHECKSUM_NOT_VALID;
                return result;
            }

            if (sedol.ContainsVowel)
            {
                result.ValidationDetails = Constants.INPUT_STRING_CONTAINS_VOWELS;
                return result;
            }

            if (sedol.HasValidCheckDigit)
                result.IsValidSedol = true;
            else
                result.ValidationDetails = Constants.CHECKSUM_NOT_VALID;
            
            return result;
        }
    }
}
