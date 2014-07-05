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
            _parser = new Parser<OptionalValueOptions>();
        }

        [TestMethod]
        [ExpectedException(typeof(MissingValuesException))]
        public void RequiredArgTest()
        {
            _parser.Parse("-s flag");
        }

        [TestMethod]
        [ExpectedException(typeof(MissingValuesException))]
        public void RequiredFlagTest()
        {
            _parser.Parse("arg");
        }

        [TestMethod]
        public void OptionalValueTest()
        {
            var optsNull = _parser.Parse("reqArg -s reqString");
            var optsGiven = _parser.Parse("reqArg -s reqString -Ai hello 42 something");

            Assert.IsNull(optsNull.OptionalString);
            Assert.IsNull(optsNull.OptionalInteger);
            Assert.IsNull(optsNull.OptionalArgument);

            Assert.AreEqual("hello", optsGiven.OptionalString);
            Assert.AreEqual(42, optsGiven.OptionalInteger);
            Assert.AreEqual("something", optsGiven.OptionalArgument);
        }


    }
}
