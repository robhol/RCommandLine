namespace TestRCommandLine
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using RCommandLine.Attributes;
    using RCommandLine.Exceptions;
    using RCommandLine.Parsers;

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

            [Argument(0)]
            public string RequiredArgument { get; set; }

            [Argument(1), Optional]
            public string OptionalArgument { get; set; }
        }

        private readonly Parser<OptionalValueOptions> _parser;
        public OptionalValueTests()
        {
            _parser = Parser.FromAttributes<OptionalValueOptions>(
                new ParserOptions
                {
                    AutomaticHelp = false,
                    AutomaticUsage = false
                });
        }

        [TestMethod]
        [ExpectedException(typeof(MissingArgumentException))]
        public void Should_Throw_On_MissingRequiredArgument()
        {
            _parser.Parse("-s flag");
        }

        [TestMethod]
        [ExpectedException(typeof(MissingArgumentException))]
        public void Should_Throw_On_MissingRequiredFlag()
        {
            _parser.Parse("arg");
        }

        [TestMethod]
        [ExpectedException(typeof(MissingValueException))]
        public void Should_Throw_On_MissingFlagValue()
        {
            _parser.Parse("arg --required-string reqstr --optional-string");
            Assert.Fail("Expected MissingValueException. MissingMissingValuesExceptionException?");
        }

        [TestMethod]
        public void Should_MapTypeDefaultValues()
        {
            var optsNull = _parser.Parse("reqArg -s reqString").Options;
            var optsGiven = _parser.Parse("reqArg -s reqString -Ai hello 42 something").Options;

            Assert.IsNull(optsNull.OptionalString);
            Assert.IsNull(optsNull.OptionalInteger);
            Assert.IsNull(optsNull.OptionalArgument);

            Assert.AreEqual("hello", optsGiven.OptionalString);
            Assert.AreEqual(42, optsGiven.OptionalInteger);
            Assert.AreEqual("something", optsGiven.OptionalArgument);
        }
    }
}
