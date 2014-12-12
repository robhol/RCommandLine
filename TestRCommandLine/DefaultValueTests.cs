namespace TestRCommandLine
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using RCommandLine;
    using RCommandLine.Attributes;
    using RCommandLine.Parsers;

    /// <summary>
    /// Tests the function of explicitly defaulted optional values.
    /// </summary>
    [TestClass]
    public class DefaultValueTests
    {
        class DefaultValueOptions
        {
            [Flag('s')]
            public string RequiredString { get; set; }

            [Flag('A'), Optional(Default = "none")]
            public string OptionalString { get; set; }

            [Flag('i'), Optional(Default = 1024)]
            public int OptionalInteger { get; set; }

            [Argument(0)]
            public string RequiredArgument { get; set; }

            [Argument(1), Optional(Default = "nothing")]
            public string OptionalArgument { get; set; }
        }

        private readonly Parser<DefaultValueOptions> _parameterParser;
        public DefaultValueTests()
        {
            _parameterParser = Parser.FromAttributes<DefaultValueOptions>();
        }

        [TestMethod]
        public void Should_MapGivenDefaultValues()
        {
            var optsDefault = _parameterParser.Parse("reqArg -s reqString").Options;
            var optsGiven = _parameterParser.Parse("reqArg -s reqString -Ai hello 42 something").Options;

            Assert.AreEqual("none", optsDefault.OptionalString);
            Assert.AreEqual(1024, optsDefault.OptionalInteger);
            Assert.AreEqual("nothing", optsDefault.OptionalArgument);

            Assert.AreEqual("hello", optsGiven.OptionalString);
            Assert.AreEqual(42, optsGiven.OptionalInteger);
            Assert.AreEqual("something", optsGiven.OptionalArgument);
        }

    }
}
