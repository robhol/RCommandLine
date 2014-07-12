using Microsoft.VisualStudio.TestTools.UnitTesting;
using RCommandLine;

namespace TestRCommandLine
{

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

            [Parameter(0)]
            public string RequiredArgument { get; set; }

            [Parameter(1), Optional(Default = "nothing")]
            public string OptionalArgument { get; set; }
        }

        private readonly ParameterParser<DefaultValueOptions> _parameterParser;
        public DefaultValueTests()
        {
            _parameterParser = new ParameterParser<DefaultValueOptions>();
        }

        [TestMethod]
        public void DefaultValueTest()
        {
            var optsDefault = _parameterParser.Parse("reqArg -s reqString");
            var optsGiven = _parameterParser.Parse("reqArg -s reqString -Ai hello 42 something");

            Assert.AreEqual("none", optsDefault.OptionalString);
            Assert.AreEqual(1024, optsDefault.OptionalInteger);
            Assert.AreEqual("nothing", optsDefault.OptionalArgument);

            Assert.AreEqual("hello", optsGiven.OptionalString);
            Assert.AreEqual(42, optsGiven.OptionalInteger);
            Assert.AreEqual("something", optsGiven.OptionalArgument);
        }

    }
}
