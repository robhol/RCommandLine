using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            [Argument(0)]
            public string RequiredArgument { get; set; }

            [Argument(1), Optional(Default = "nothing")]
            public string OptionalArgument { get; set; }
        }

        private readonly Parser<DefaultValueOptions> _parser;
        public DefaultValueTests()
        {
            _parser = new Parser<DefaultValueOptions>();
        }

        [TestMethod]
        public void DefaultValueTest()
        {
            var optsDefault = _parser.Parse("reqArg -s reqString");
            var optsGiven = _parser.Parse("reqArg -s reqString -Ai hello 42 something");

            Assert.AreEqual("none", optsDefault.OptionalString);
            Assert.AreEqual(1024, optsDefault.OptionalInteger);
            Assert.AreEqual("nothing", optsDefault.OptionalArgument);

            Assert.AreEqual("hello", optsGiven.OptionalString);
            Assert.AreEqual(42, optsGiven.OptionalInteger);
            Assert.AreEqual("something", optsGiven.OptionalArgument);
        }

    }
}
