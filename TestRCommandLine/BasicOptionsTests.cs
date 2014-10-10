﻿using System;
using System.Linq;
using System.Runtime.InteropServices;
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
        public void Should_ParseFlags()
        {
            var result = _parser.Parse("--str -bi grell 8 arg 9001");
            var opts = result.Options as BasicOptions;

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
        [ExpectedException(typeof(UnrecognizedFlagException))]
        public void Should_Throw_On_InvalidFlagName()
        {
            _parser.Options.AutomaticUsage = false; // this option "swallows" the exception
            try
            {
                _parser.Parse("--str required --this-flag-doesnt-even-exist");
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
            var opts = pr.Options as BasicOptions;
            Assert.IsNotNull(opts);

            Assert.AreEqual("argOne", opts.StringArgument);
            Assert.AreEqual(42, opts.IntegerArgument);

            Assert.AreEqual(2, pr.ExtraArguments.Count);
            Assert.IsTrue(pr.ExtraArguments.SequenceEqual(new[] {"extra1", "extra2"}));
        }

    }
}
