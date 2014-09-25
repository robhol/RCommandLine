using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RCommandLine;

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

            [Flag('s', "str")]
            public string StringFlag { get; set; }

            [OrderedParameter(0)]
            public string StringArgument { get; set; }

            [OrderedParameter(1)]
            public int IntegerArgument { get; set; }

        }

        private readonly Parser<BasicOptions> _parser;

        public BasicOptionsTests()
        {
            _parser = new Parser<BasicOptions>();
        }

        [TestMethod]
        public void BasicFlagTest()
        {
            var opts = _parser.Parse("--str -bi grell 8 arg 9001").Options as BasicOptions; //=>  --str grell -b -i 8  , args are "arg" and 9001
            Assert.IsNotNull(opts);

            Assert.AreEqual(opts.BooleanFlag, true);
            Assert.AreEqual(opts.IntegerFlag, 8);
            Assert.AreEqual(opts.StringFlag, "grell");
        }

        [TestMethod]
        public void BasicArgumentTest()
        {
            var opts = _parser.Parse("argOne -s flagValue 42 -bi 44").Options as BasicOptions;
            Assert.IsNotNull(opts);

            Assert.AreEqual("argOne", opts.StringArgument);
            Assert.AreEqual(42, opts.IntegerArgument);
        }

        [TestMethod]
        public void ParserReusabilityTest()
        {
            const string args = "-is 1 a b 2";
            Assert.AreNotSame(_parser.Parse(args).Options, _parser.Parse(args).Options);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void FlagTypeTest()
        {
            _parser.Parse("--str required --integer-flag definitelyNotAnInteger");
        }

        [TestMethod]
        public void ExtraArgumentsTest()
        {
            var pr = _parser.Parse("argOne -s flagValue 42 -bi 44 extra1 extra2");
            var opts = pr.Options as BasicOptions;
            Assert.IsNotNull(opts);

            Assert.AreEqual("argOne", opts.StringArgument);
            Assert.AreEqual(42, opts.IntegerArgument);

            Assert.AreEqual(2, pr.ExtraArguments.Count);
            Assert.IsTrue(pr.ExtraArguments.SequenceEqual(new[] {"extra1", "extra2"}));
        }

    }
}
