using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RCommandLine;
using System.Text;
using System.Linq;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace TestRCommandLine
{

    [TestClass]
    public class BasicOptionsTests
    {
        class BasicOptions
        {

            [Flag('b')]
            public bool BooleanFlag { get; set; }

            [Flag('i')]
            public int IntegerFlag { get; set; }

            [Flag('s', Name = "str")]
            public string StringFlag { get; set; }

            [Argument(0)]
            public string StringArgument { get; set; }

            [Argument(1)]
            public int IntegerArgument { get; set; }

        }

        readonly Parser<BasicOptions> _parser;

        public BasicOptionsTests()
        {
            _parser = new Parser<BasicOptions>();
        }

        [TestMethod]
        public void BasicFlagTest()
        {
            var opts = _parser.Parse("--str -bi grell 8 arg 9001"); //=>  --str grell -b -i 8  , args are "arg" and 9001

            Assert.AreEqual(opts.BooleanFlag, true);
            Assert.AreEqual(opts.IntegerFlag, 8);
            Assert.AreEqual(opts.StringFlag, "grell");
        }

        [TestMethod]
        public void BasicArgumentTest()
        {
            var opts = _parser.Parse("argOne -s flagValue 42 -bi 44");

            Assert.AreEqual("argOne", opts.StringArgument);
            Assert.AreEqual(42, opts.IntegerArgument);
        }

        [TestMethod]
        public void ParserReusabilityTest()
        {
            const string args = "-is 1 a b 2";
            Assert.AreNotSame(_parser.Parse(args), _parser.Parse(args));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void FlagTypeTest()
        {
            var opts = _parser.Parse("--str required --integer-flag definitelyNotAnInteger");
        }

    }
}
