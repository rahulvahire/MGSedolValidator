using NUnit.Framework;
using SedolValidatorInterfaces;

namespace SedolValidator.Tests
{
    [TestFixture]
    public class SedolValidatorTests
    {
        [TestCase(null)]
        public void SedolsNotSevenCharacters(string sedol)
        {
            var actual = new SedolValidator().ValidateSedol(sedol);
            var expected = new SedolValidationResult(sedol, false, false, Constants.INPUT_STRING_NOT_VALID_LENGTH);
            AssertValidationResult(expected, actual);
        }

        [TestCase("éz-^&**")]
        public void SedolsContainingNonAlphanumericCharacters(string sedol)
        {
            var actual = new SedolValidator().ValidateSedol(sedol);
            var expected = new SedolValidationResult(sedol, false, false, Constants.INPUT_STRING_NOT_ALPHANUMERIC);
            AssertValidationResult(expected, actual);
        }

        [TestCase("9123458")]
        public void UserDefinedSedolsWithCorrectChecksum(string sedol)
        {
            var actual = new SedolValidator().ValidateSedol(sedol);
            var expected = new SedolValidationResult(sedol, true, true, null);
            AssertValidationResult(expected, actual);
        }

        [TestCase("9123457")]
        public void UserDefinedSedolsWithIncorrectChecksum(string sedol)
        {
            var actual = new SedolValidator().ValidateSedol(sedol);
            var expected = new SedolValidationResult(sedol, false, true, Constants.CHECKSUM_NOT_VALID);
            AssertValidationResult(expected, actual);
        }

        [TestCase("1234567")]
        public void SedolsWithIncorrectChecksum(string sedol)
        {
            var actual = new SedolValidator().ValidateSedol(sedol);
            var expected = new SedolValidationResult(sedol, false, false, Constants.CHECKSUM_NOT_VALID);
            AssertValidationResult(expected, actual);
        }

        [Test]
        [TestCase("0709954")]
        [TestCase("0709954")]
        [TestCase("B0YBKJ7")]
        [TestCase("B0Ybkj7")]
        [TestCase("4065663")]
        [TestCase("B0YBLH2")]
        [TestCase("2282765")]
        [TestCase("B0YBKL9")]
        public void ValidSedols(string sedol)
        {
            var actual = new SedolValidator().ValidateSedol(sedol);
            var expected = new SedolValidationResult(sedol, true, false, null);
            AssertValidationResult(expected, actual);
        }

        [TestCase("AEIOUAE")]
        public void StringContainingVowelsWillReturnExpectedValidationDetails(string sedol)
        {
            var actual = new SedolValidator().ValidateSedol(sedol);
            var expected = new SedolValidationResult(sedol, false, false, Constants.INPUT_STRING_CONTAINS_VOWELS);
            AssertValidationResult(expected, actual);
        }

        private static void AssertValidationResult(ISedolValidationResult expected, ISedolValidationResult actual)
        {
            Assert.AreEqual(expected.InputString, actual.InputString, "Input String Failed");
            Assert.AreEqual(expected.IsValidSedol, actual.IsValidSedol, "Is Valid Failed");
            Assert.AreEqual(expected.IsUserDefined, actual.IsUserDefined, "Is User Defined Failed");
            Assert.AreEqual(expected.ValidationDetails, actual.ValidationDetails, "Validation Details Failed");
        }
    }
}

