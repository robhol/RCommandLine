using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RCommandLine;

namespace TestRCommandLine
{
    using System.Collections.Generic;

    [TestClass]
    public class FlagHeaderTests
    {
        private readonly Parser<BasicOptionsTests.BasicOptions> _defaultParser;
        private readonly Parser<BasicOptionsTests.BasicOptions> _windowsStyleParser;
        private readonly Parser<BasicOptionsTests.BasicOptions> _unixStyleParser;

        public FlagHeaderTests()
        {
            //default is to accept any character
            _defaultParser = new Parser<BasicOptionsTests.BasicOptions>();

            _windowsStyleParser = new Parser<BasicOptionsTests.BasicOptions>(new ParserOptions
            {
                ShortFlagHeaders = new List<string> {"/"},
                LongFlagHeaders = new List<string> {"/"}
            });
            _unixStyleParser = new Parser<BasicOptionsTests.BasicOptions>(new ParserOptions
            {
                ShortFlagHeaders = new List<string> {"-"},
                LongFlagHeaders = new List<string> {"--"}
            });
        }

        [TestMethod]
        public void Should_ReturnExpectedData_For_DefaultControlCharacterSet()
        {
            var opt = _defaultParser.Parse("--str required /b -i 33 DUMMY_REQ_ARGUMENTS-> 9001").Options;

            Assert.AreEqual(opt.BooleanFlag, true);
            Assert.AreEqual(opt.StringFlag, "required");
            Assert.AreEqual(opt.IntegerFlag, 33);
        }

        [TestMethod]
        public void Should_ReturnExpectedData_For_WindowsControlCharacterSet()
        {
            var opt = _windowsStyleParser.Parse("/str required /b /i 33 DUMMY_REQ_ARGUMENTS-> 9001").Options;

            Assert.AreEqual(opt.BooleanFlag, true);
            Assert.AreEqual(opt.StringFlag, "required");
            Assert.AreEqual(opt.IntegerFlag, 33);
        }

        [TestMethod]
        public void Should_ReturnExpectedData_For_WindowsControlCharacterSet_On_CombinedShortFlags()
        {
            var opt = _windowsStyleParser.Parse("/str required /bi 33 DUMMY_REQ_ARGUMENTS-> 9001").Options;

            Assert.AreEqual(opt.BooleanFlag, true);
            Assert.AreEqual(opt.StringFlag, "required");
            Assert.AreEqual(opt.IntegerFlag, 33);
        }

        [TestMethod]
        public void Should_Throw_On_UnknownCombinedShortFlag_For_WindowsControlCharacterSet()
        {

            _windowsStyleParser.Options.AutomaticUsage = false;
            
            try
            {
                var opt = _windowsStyleParser.Parse("/str required /bi9 33 DUMMY_REQ_ARGUMENTS-> 9001").Options;
                Assert.Fail("Expected exception was not thrown.");
            }
            catch (UnrecognizedFlagException e)
            {
                Assert.IsTrue(e.Flag.Contains("/bi9"), "Expected reference to unknown flag.");
            }
            finally
            {
                _windowsStyleParser.Options.AutomaticUsage = true;
            }

        }

        [TestMethod]
        public void Should_ReturnExpectedData_For_UnixControlCharacterSet()
        {
            var opt = _unixStyleParser.Parse("--str required -bi 33 DUMMY_REQ_ARGUMENTS-> 9001").Options;

            Assert.AreEqual(opt.BooleanFlag, true);
            Assert.AreEqual(opt.StringFlag, "required");
            Assert.AreEqual(opt.IntegerFlag, 33);
        }
    }
}
