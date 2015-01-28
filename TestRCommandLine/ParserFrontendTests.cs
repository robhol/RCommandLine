namespace TestRCommandLine
{
    using System;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using RCommandLine;
    using RCommandLine.Attributes;

    [TestClass]
    public class ParserFrontendTests
    {
        private readonly Parser<CommandTests.MyOptions> _parser;
        private readonly Parser<AbstractOptions> _abstractParser;
        private readonly InMemoryOutputChannel _output;

        [HasCommand(typeof(ActualOptions))]
        abstract class AbstractOptions
        {

            class ActualOptions
            {
            
            }

        }

        public ParserFrontendTests()
        {
            _output = new InMemoryOutputChannel();
            var parserOptions = new ParserOptions { OutputTarget = _output };
            _parser = Parser.FromAttributes<CommandTests.MyOptions>(parserOptions);
            _abstractParser = Parser.FromAttributes<AbstractOptions>(parserOptions);
        }

        string GetLastOutput()
        {
            var s = _output.ToString();
            _output.Reset();
            return s;
        }

        [TestMethod]
        public void Should_PrintArgumentList_On_ValidCommand_And_MissingArguments()
        {
            _parser.Parse("bar-name baz");

            var op = GetLastOutput();
            Assert.IsFalse(op.Contains("Available commands"));
            Assert.IsTrue(op.Contains("BazIntegerArg"));
        }

        [TestMethod]
        public void Should_PrintCommandList_On_AutoHelpFlag_And_NonTerminalCommand()
        {
            _parser.Parse("");

            Assert.IsTrue(GetLastOutput().Contains("Available commands"));
        }

        [TestMethod]
        public void Should_PrintArgumentList_On_AutoHelpFlag_And_TerminalCommand()
        {
            _parser.Parse("bar-name baz --help");

            var op = GetLastOutput();
            Assert.IsFalse(op.Contains("Available commands"));
            Assert.IsTrue(op.Contains("BazIntegerArg"));
        }

        [TestMethod]
        public void Should_ExposeExtraArgumentInformation_On_Help()
        {
            _parser.Parse("foo -?");

            var op = GetLastOutput();
            Assert.IsTrue(op.Contains("ExtraArgsName"));
            Assert.IsTrue(op.Contains("ExtraArgsDescription"));
        }

        [TestMethod]
        [ExpectedException(typeof(MissingMethodException))]
        public void Should_Throw_On_InstantiatingAbstractOptions_And_AutoUsageDisabled()
        {
            _abstractParser.Options.AutomaticUsage = false;
            try
            {
                _abstractParser.Parse("");
            }
            finally
            {
                _abstractParser.Options.AutomaticUsage = true;
                _output.Reset();
            }
        }

        [TestMethod]
        public void Should_PrintCommandList_On_InstantiatingAbstractOptions_And_AutoUsageEnabled()
        {
            _abstractParser.Parse("");
            Assert.IsTrue(GetLastOutput().Contains("Available commands"));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <seealso cref="OptionalValueTests.Should_Throw_On_MissingRequiredArgument"/>
        [TestMethod]
        public void Should_Not_Throw_On_MissingArgument_And_AutomaticUsageEnabled()
        {
            _parser.Parse("");
        }

        [TestMethod]
        public void Should_Not_Throw_On_UnrecognizedFlag_And_AutomaticUsageEnabled()
        {
            _parser.Parse("--unrecognized-flag");
        }

    }

    internal class InMemoryOutputChannel : RCommandLine.Output.IOutputTarget
    {
        private readonly StringBuilder _log = new StringBuilder();

        public void Write(string s)
        {
            _log.Append(s);
        }

        public void WriteLine(string s)
        {
            _log.AppendLine(s);
        }

        public void Reset()
        {
            _log.Clear();
        }

        public override string ToString()
        {
            return _log.ToString();
        }
    }

}
