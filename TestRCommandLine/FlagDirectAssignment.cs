namespace TestRCommandLine
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using RCommandLine;

    [TestClass]
    public class FlagDirectAssignment
    {
        private readonly Parser<Options> _parser;
        private Parser<Options> _windowsStyleParser;

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

        public FlagDirectAssignment()
        {
            _parser = new Parser<Options>();
            _windowsStyleParser = new Parser<Options>(ParserOptions.Template.Windows);
        }

        [TestMethod]
        public void Should_ReturnExpectedValue_On_ShortFlagAssignment()
        {
            var opts = _parser.Parse("-a:3 -b=4 -c=test").Options;

            Assert.AreEqual(3, opts.AValue);
            Assert.AreEqual(4, opts.BValue);
            Assert.AreEqual("test", opts.CValue);
        }

        [TestMethod, ExpectedException(typeof(InvalidCastException))]
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




    }
}