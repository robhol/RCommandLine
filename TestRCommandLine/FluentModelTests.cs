using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RCommandLine.Parsers;

namespace TestRCommandLine
{
    [TestClass]
    public class FluentModelTests
    {
        private Parser<CommandTests.MyOptions> _attributeParser;
        private Parser<CommandTests.MyOptions> _fluentParser;

        public FluentModelTests()
        {
            _attributeParser = Parser.FromAttributes<CommandTests.MyOptions>();
            _fluentParser = GetFluentParser();
        }

        [TestMethod]
        public void Should_Consider_Fluent_And_Attribute_Models_Equal()
        {
            Assert.IsTrue(_attributeParser.Equals(_fluentParser));
        }

        Parser<CommandTests.MyOptions> GetFluentParser()
        {
            return Parser.CreateFluently<CommandTests.MyOptions>(builder => builder

                .BaseCommand<CommandTests.CommonOptions>(c => c
                    .Flag(x => x.CommonBool, x => x.ShortName('v'))
                    .Argument(x => x.CommonStringArg))

                .Command<CommandTests.FooOptions>(fc => fc
                    .LabelExtraArguments("ExtraArgsName", "ExtraArgsDescription")
                    .Flag(x => x.FooDouble, x => x.ShortName('X'))
                    .Argument(x => x.FooIntArg))

                .Command<CommandTests.BarOptions>(bc => bc
                    .Flag(x => x.BarString, x => x.ShortName('X'))
                    .Argument(x => x.BarStringArg, x => x.Optional())

                    .Command<CommandTests.BarBazOptions>(bbc => bbc
                        .Name("baz")
                        .Argument(x => x.BazIntegerArg))

                    .Command<CommandTests.BarHiddenCmdOptions>(bhc => bhc
                        .Name("(HIDDEN)")
                        .Hidden())
                ));
        }

    }
}
