namespace TestRCommandLine
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using RCommandLine;

    [TestClass]
    public class FlagDirectAssignment
    {
        private readonly Parser<Options> _parser;
        private readonly Parser<Options> _windowsStyleParser;
        private readonly Parser<BooleanOptions> _booleanOptionParser;

        class Options
        {

            [Flag('a')]
            public int AValue { get; set; }

            [Flag('b')]
            public int BValue { get; set; }

            [Flag('c')]
            public string CValue { get; set; }

            [Flag("ab"), Optional]
            public string AbValue { get; set; }

        }

        class BooleanOptions
        {
            [Flag('a')]
            public bool a { get; set; }

            [Flag('b')]
            public bool b { get; set; }

            [Flag('c')]
            public bool c { get; set; }

            [Flag('d')]
            public bool d { get; set; }

            [Flag('e')]
            public bool e { get; set; }

            [Flag('f')]
            public bool f { get; set; }
        }

        public FlagDirectAssignment()
        {
            _parser = new Parser<Options>();
            _windowsStyleParser = new Parser<Options>(ParserOptions.Template.Windows);
            _booleanOptionParser = new Parser<BooleanOptions>();
        }

        [TestMethod]
        public void Should_ReturnExpectedValue_On_ShortFlagAssignment()
        {
            var opts = _parser.Parse("-a:3 -b=4 -c=test").Options;

            Assert.AreEqual(3, opts.AValue);
            Assert.AreEqual(4, opts.BValue);
            Assert.AreEqual("test", opts.CValue);
        }

        [TestMethod, ExpectedException(typeof(FormatException))]
        public void Should_Throw_On_ShortFlagAssignment_If_TypeIsInvalid()
        {
            _parser.Parse("-a:3 -b=q -c=test");
        }

        [TestMethod]
        public void Should_ReturnExpectedValue_On_LongFlagAssignment()
        {
            var opts = _parser.Parse("--a-value:3 --b-value=4 --c-value=test").Options;

            Assert.AreEqual(3, opts.AValue);
            Assert.AreEqual(4, opts.BValue);
            Assert.AreEqual("test", opts.CValue);
        }

        [TestMethod]
        public void Should_Throw_On_AmbiguousShortFlagAssignment()
        {
            try
            {
                _parser.Parse("-ab:4 --c-value=test");
            }
            catch (AmbiguousDirectAssignmentException e)
            {
                Assert.IsTrue(e.Argument.Contains("-ab"), "Expected reference to ambiguous flags");
                return;
            }

            Assert.Fail("Expected AmbiguousDirectAssignmentException");
        }

        [TestMethod]
        public void Should_Assign_ExpectedValue_Given_BooleanAlias()
        {
            var result = _booleanOptionParser.Parse("/a:no /b:1 /c:ON /D=yes /e=0 /f=oFf").Options;
            
            Assert.IsFalse(result.a);
            Assert.IsTrue(result.b);
            Assert.IsTrue(result.c);
            Assert.IsTrue(result.d);
            Assert.IsFalse(result.e);
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void Should_Throw_On_BooleanFlagAssignment_If_ValueIsInvalid()
        {
            _booleanOptionParser.Parse("/a:foo_is_not_a_boolean");
        }


    }
}