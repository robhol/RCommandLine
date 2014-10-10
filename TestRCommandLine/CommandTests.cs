﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using RCommandLine;

namespace TestRCommandLine
{
    [TestClass]
    public class CommandTests
    {
        
        public class CommonOptions
        {
            [Flag('v')]
            public bool CommonBool { get; set; }

            [OrderedParameter(0)]
            public string CommonStringArg { get; set; }
        }

        [HasCommand(typeof(FooOptions))]
        [HasCommand(typeof(BarOptions), "bar-name")]
        public class MyOptions : CommonOptions
        {

        }

        public class FooOptions : CommonOptions
        {

            [Flag('X')]
            public double FooDouble { get; set; }

            [OrderedParameter(1)]
            public int FooIntArg { get; set; }

        }

        [HasCommand(typeof(BarBazOptions), "baz")]
        [HasCommand(typeof(BarHiddenCmdOptions), "(HIDDEN)", Hidden = true)]
        public class BarOptions : CommonOptions
        {

            [Flag('X')]
            public string BarString { get; set; }

            [OrderedParameter(1), Optional]
            public string BarStringArg { get; set; }

        }

        public class BarBazOptions
        {

            [OrderedParameter(0)]
            public int BazIntegerArg { get; set; }

        }

        public class BarHiddenCmdOptions
        {
            
        }
        
        private readonly Parser<MyOptions> _parser;

        public CommandTests()
        {
            _parser = new Parser<MyOptions>();
        }

        [TestMethod]
        public void Should_MapArguments_Without_Command()
        {
            var opt = _parser.Parse("commonString").Options as MyOptions;

            Assert.IsNotNull(opt);
            Assert.AreEqual("commonString", opt.CommonStringArg);
        }

        [TestMethod]
        public void Should_MapArguments_On_SimpleCommand()
        {
            var fooResult = _parser.Parse("foo commonString 32 -X 55.4321");
            var foo = fooResult.Options as FooOptions;
            var barResult = _parser.Parse("bar-name commonString barString -X barFlag");
            var bar = barResult.Options as BarOptions;

            Assert.IsNotNull(foo);
            Assert.IsNotNull(bar);

            Assert.AreEqual("foo", fooResult.Command);
            Assert.AreEqual("commonString", foo.CommonStringArg);
            Assert.AreEqual(32, foo.FooIntArg);
            Assert.AreEqual(55.4321, foo.FooDouble);

            Assert.AreEqual("bar-name", barResult.Command);
            Assert.AreEqual("commonString", bar.CommonStringArg);
            Assert.AreEqual("barString", bar.BarStringArg);
            Assert.AreEqual("barFlag", bar.BarString);
        }

        [TestMethod]
        public void Should_MapArguments_On_NestedCommands()
        {
            var bar = _parser.Parse("bar-name commonString barString -X 55.4321").Options as BarOptions;
            var bazResult = _parser.Parse("bar-name baz 999");
            var baz = bazResult.Options as BarBazOptions;

            Assert.IsNotNull(bar);
            Assert.IsNotNull(baz);

            Assert.AreEqual("bar-name baz", bazResult.Command);
            Assert.AreEqual("commonString", bar.CommonStringArg);
            Assert.AreEqual("barString", bar.BarStringArg);

            Assert.AreEqual(999, baz.BazIntegerArg);
        }

        [TestMethod]
        public void Should_OutputCorrectCommandList_On_EmptyInput()
        {
            var output = _parser.Parse().GetCommandList();
            Assert.IsTrue(output.Contains("foo"));
            Assert.IsTrue(output.Contains("bar-name")); //manually specified
            Assert.IsTrue(output.Contains("bar-name baz")); 

            Assert.IsFalse(output.Contains("(HIDDEN)"));
        }
    }
}
