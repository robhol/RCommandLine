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

            [OrderedParameter(1, Description = "Maybe")]
            public string Maybe { get; set; }
        }

        public ParserFrontendTests()
        {

        }

        [TestMethod]
        public void Bugfix_CommandListOnEmptyInput()
        {
            var parser = new Parser<SimpleOptions> { Output = new InMemoryOutputChannel() };
            var parser2 = new Parser<CommandTests.MyOptions> { Output = new InMemoryOutputChannel() };
            parser.Parse("");
            parser2.Parse("");

            var op = parser.Output.ToString();
            Assert.IsFalse(op.Contains("Available commands"));
            Assert.IsTrue(op.Contains("--yes"));
            Assert.IsTrue(op.Contains("Maybe"));

            Assert.IsTrue(parser2.Output.ToString().Contains("Available commands"));
        }

    }

    internal class InMemoryOutputChannel : RCommandLine.Output.IOutput
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
