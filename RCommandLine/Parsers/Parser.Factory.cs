namespace RCommandLine.Parsers
{
    using System.Runtime.InteropServices.ComTypes;
    using Fluent;
    using ModelConversion;
    using Models;

    public class Parser
    {

        /* Factory methods */

        private static Parser<T> FromCommand<T>(ParserOptions options, Command root) where T : class
        {
            return new Parser<T>(options ?? ParserOptions.Templates.Default, root);
        }

        public static Parser<T> FromAttributes<T>(ParserOptions options = null) where T : class
        {
            var root = new AttributeModelBuilder<T>().Build();
            return FromCommand<T>(options, root);
        }

        public static FluentModelBuilder<T> CreateFluently<T>(ParserOptions options = null) where T : class
        {
            return new FluentModelBuilder<T>(options);
        }

    }
}