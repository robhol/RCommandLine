using Microsoft.VisualStudio.TestTools.UnitTesting;
using RCommandLine;

namespace TestRCommandLine
{

    
    /// <summary>
    /// Tests the function of OptionalAttribute (no default value given) and Nullable values.
    /// </summary>
    [TestClass]
    public class OptionalValueTests
    {
        class OptionalValueOptions
        {
            [Flag('s')]
            public string RequiredString { get; set; }

            [Flag('A'), Optional]
            public string OptionalString { get; set; }

            [Flag('i')]
            public int? OptionalInteger { get; set; }

            [OrderedParameter(0)]
            public string RequiredArgument { get; set; }

            [OrderedParameter(1), Optional]
            public string OptionalArgument { get; set; }
        }

        private readonly ParameterParser<OptionalValueOptions> _parameterParser;
        public OptionalValueTests()
        {
            _parameterParser = new ParameterParser<OptionalValueOptions>();
        }

        [TestMethod]
        [ExpectedException(typeof(MissingArgumentException))]
        public void RequiredArgTest()
        {
            _parameterParser.Parse("-s flag");
        }

        [TestMethod]
        [ExpectedException(typeof(MissingArgumentException))]
        public void RequiredFlagTest()
        {
            _parameterParser.Parse("arg");
        }

        [TestMethod]
        [ExpectedException(typeof(MissingValueException))]
        public void DanglingFlagTest()
        {
            _parameterParser.Parse("arg --required-string reqstr --optional-string");
            Assert.Fail("Expected MissingValueException. MissingMissingValuesExceptionException?");
        }

        [TestMethod]
        public void OptionalValueTest()
        {
            var optsNull = _parameterParser.Parse("reqArg -s reqString");
            var optsGiven = _parameterParser.Parse("reqArg -s reqString -Ai hello 42 something");

            Assert.IsNull(optsNull.OptionalString);
            Assert.IsNull(optsNull.OptionalInteger);
            Assert.IsNull(optsNull.OptionalArgument);

            Assert.AreEqual("hello", optsGiven.OptionalString);
            Assert.AreEqual(42, optsGiven.OptionalInteger);
            Assert.AreEqual("something", optsGiven.OptionalArgument);
        }


    }
}
