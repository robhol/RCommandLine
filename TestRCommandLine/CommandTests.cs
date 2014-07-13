using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        [HasCommand(typeof(BarOptions))]
        class MyOptions : CommonOptions
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

        
        private readonly Parser<MyOptions> _parser;

        public CommandTests()
        {
            _parser = new Parser<MyOptions>();
        }

        [TestMethod]
        public void CommandBaseTest()
        {
            var opt = _parser.Parse("commonString").Options as MyOptions;

            Assert.IsNotNull(opt);
            Assert.AreEqual("commonString", opt.CommonStringArg);
        }

        [TestMethod]
        public void CommandFlatTest()
        {
            var fooResult = _parser.Parse("foo commonString 32 -X 55.4321");
            var foo = fooResult.Options as FooOptions;
            var barResult = _parser.Parse("bar commonString barString -X barFlag");
            var bar = barResult.Options as BarOptions;

            Assert.IsNotNull(foo);
            Assert.IsNotNull(bar);

            Assert.AreEqual("foo", fooResult.Command);
            Assert.AreEqual("commonString", foo.CommonStringArg);
            Assert.AreEqual(32, foo.FooIntArg);
            Assert.AreEqual(55.4321, foo.FooDouble);

            Assert.AreEqual("bar", barResult.Command);
            Assert.AreEqual("commonString", bar.CommonStringArg);
            Assert.AreEqual("barString", bar.BarStringArg);
            Assert.AreEqual("barFlag", bar.BarString);
        }

        [TestMethod]
        public void CommandNestedTest()
        {
            var bar = _parser.Parse("bar commonString barString -X 55.4321").Options as BarOptions;
            var bazResult = _parser.Parse("bar baz 999");
            var baz = bazResult.Options as BarBazOptions;

            Assert.IsNotNull(bar);
            Assert.IsNotNull(baz);

            Assert.AreEqual("bar baz", bazResult.Command);
            Assert.AreEqual("commonString", bar.CommonStringArg);
            Assert.AreEqual("barString", bar.BarStringArg);

            Assert.AreEqual(999, baz.BazIntegerArg);
        }
    }
}
