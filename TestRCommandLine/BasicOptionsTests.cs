using System;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RCommandLine;

namespace TestRCommandLine
{
    using System.Collections.Generic;

    [TestClass]
    public class BasicOptionsTests
    {
        public class BasicOptions
        {

            [Flag('b')]
            public bool BooleanFlag { get; set; }

            [Flag('i')]
            public int IntegerFlag { get; set; }

            [Flag('s', "str")]
            public string StringFlag { get; set; }

            [Argument(0)]
            public string StringArgument { get; set; }

            [Argument(1)]
            public int IntegerArgument { get; set; }

        }

        private readonly ConsolidatedParser<BasicOptions> _parser;

        public BasicOptionsTests()
        {
            _parser = ConsolidatedParser.FromAttributes<BasicOptions>();
        }

        [TestMethod]
        public void Should_ParseFlags()
        {
            var result = _parser.Parse("--str -bi grell 8 arg 9001");
            var opts = result.Options;

            Assert.IsTrue(result.Success);
            Assert.IsNotNull(opts);

            Assert.AreEqual(opts.BooleanFlag, true);
            Assert.AreEqual(opts.IntegerFlag, 8);
            Assert.AreEqual(opts.StringFlag, "grell");
        }

        [TestMethod]
        public void Should_ParseArguments()
        {
            var opts = _parser.Parse("argOne -s flagValue 42 -bi 44").Options as BasicOptions;
            Assert.IsNotNull(opts);

            Assert.AreEqual("argOne", opts.StringArgument);
            Assert.AreEqual(42, opts.IntegerArgument);
        }

        [TestMethod]
        public void Should_ReturnSeparateOptionsObjects()
        {
            const string args = "-is 1 a b 2";
            Assert.AreNotSame(_parser.Parse(args).Options, _parser.Parse(args).Options);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void Should_Throw_On_InvalidFlagType()
        {
            _parser.Parse("--str required --integer-flag definitelyNotAnInteger");
        }

        [TestMethod]
        public void Should_Throw_On_InvalidFlagName()
        {
            _parser.Options.AutomaticUsage = false; // this option "swallows" the exception
            try
            {
                _parser.Parse("--str required --this-flag-doesnt-even-exist");
                Assert.Fail("Expected exception was not thrown.");
            }
            catch (UnrecognizedFlagException e)
            {
                Assert.AreEqual("--this-flag-doesnt-even-exist", e.Flag);
            }
            finally
            {
                _parser.Options.AutomaticUsage = true;
            }
        }

        [TestMethod]
        public void Should_ExposeExtraArguments()
        {
            var pr = _parser.Parse("argOne -s flagValue 42 -bi 44 extra1 extra2");
            var opts = pr.Options;

            Assert.IsNotNull(opts);

            Assert.AreEqual("argOne", opts.StringArgument);
            Assert.AreEqual(42, opts.IntegerArgument);

            Assert.AreEqual(2, pr.ExtraArguments.Count);
            Assert.IsTrue(pr.ExtraArguments.SequenceEqual(new[] {"extra1", "extra2"}));
        }

        [TestMethod]
        public void Should_Not_ProcessQuotedFlag()
        {
            _parser.Options.AutomaticUsage = false; // this option "swallows" the exception
            try
            {
                _parser.Parse("--str required -bi 33 \"--this-flag-doesnt-even-exist\" 22");
            }
            finally
            {
                _parser.Options.AutomaticUsage = true;
            }
        }

    }
}
