namespace TestRCommandLine
{
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using RCommandLine;
    using RCommandLine.Parsers;

    [TestClass]
    public class ParserFrontendTests
    {
        private Parser<CommandTests.MyOptions> _parser;
        private InMemoryOutputChannel _output;

        public ParserFrontendTests()
        {
            _output = new InMemoryOutputChannel();
            var parserOptions = new ParserOptions { OutputTarget = _output };
            _parser = Parser.FromAttributes<CommandTests.MyOptions>(parserOptions);

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
            _parser.Parse("-?");

            var op = GetLastOutput();
            Assert.IsTrue(op.Contains("ExtraArgsName"));
            Assert.IsTrue(op.Contains("ExtraArgsDescription"));
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
