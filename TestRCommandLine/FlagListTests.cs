using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestRCommandLine
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using RCommandLine;

    [TestClass]
    public class FlagListTests
    {
        private Parser<ListOptions> _parser;

        class ListOptions
        {
            [Flag('a', "add")]
            public List<string> Added { get; set; }

            [Flag('i', "number")]
            public List<int> Numbers { get; set; } 
        }

        public FlagListTests()
        {
            _parser = Parser.FromAttributes<ListOptions>();
        }

        [TestMethod]
        public void Should_MapArguments_To_List()
        {
            var opt = _parser.Parse("-aaiia foo bar 32 128 baz").Options;

            Assert.IsNotNull(opt);
            Assert.IsNotNull(opt.Added);
            Assert.IsNotNull(opt.Numbers);

            Assert.IsTrue(opt.Added.SequenceEqual(new[] {"foo", "bar", "baz"}));
            Assert.IsTrue(opt.Numbers.SequenceEqual(new[] {32, 128}));
        }

        [TestMethod, ExpectedException(typeof(InvalidCastException))]
        public void Should_Throw_On_InvalidListItemType()
        {
            _parser.Parse("-i bork");
        }

        public void Should_Throw_On_InvalidListTypeParameter()
        {
            Assert.Inconclusive("Todo");
        }


    }
}
