using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RCommandLine;

namespace TestRCommandLine
{
    [TestClass]
    public class ParserFrontendTests
    {

        public class SimpleOptions
        {
            [Flag('y')]
            public bool Yes { get; set; }

            [Argument(1, Description = "Maybe")]
            public string Maybe { get; set; }
        }

        public ParserFrontendTests()
        {

        }

        [TestMethod]
        public void Should_PrintArgumentList_On_ValidCommand_And_MissingArguments()
        {
            var parserOptions = new ParserOptions { OutputTarget = new InMemoryOutputChannel() };
            var parser = ConsolidatedParser.FromAttributes<CommandTests.MyOptions>(parserOptions);
            parser.Parse("bar-name baz");

            var op = parserOptions.OutputTarget.ToString();
            Assert.IsFalse(op.Contains("Available commands"));
            Assert.IsTrue(op.Contains("BazIntegerArg"));
        }

        [TestMethod]
        public void Should_PrintCommandList_On_AutoHelpFlag_And_NonTerminalCommand()
        {
            var parserOptions = new ParserOptions { OutputTarget = new InMemoryOutputChannel() };
            var parser = ConsolidatedParser.FromAttributes<CommandTests.MyOptions>(parserOptions);
            parser.Parse("");

            Assert.IsTrue(parserOptions.OutputTarget.ToString().Contains("Available commands"));
        }

        [TestMethod]
        public void Should_PrintArgumentList_On_AutoHelpFlag_And_TerminalCommand()
        {
            var parserOptions = new ParserOptions { OutputTarget = new InMemoryOutputChannel() };
            var parser = ConsolidatedParser.FromAttributes<CommandTests.MyOptions>(parserOptions);
            parser.Parse("bar-name baz --help");

            var op = parserOptions.OutputTarget.ToString();
            Assert.IsFalse(op.Contains("Available commands"));
            Assert.IsTrue(op.Contains("BazIntegerArg"));
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
