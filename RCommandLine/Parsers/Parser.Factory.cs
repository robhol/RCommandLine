namespace RCommandLine.Parsers
{
    using ModelConversion;

    public class Parser
    {

        /* Factory methods */

        public static Parser<T> FromAttributes<T>(ParserOptions options = null) where T : class, new()
        {
            var root = new AttributeModelBuilder<T>().Build();
            return new Parser<T>(options ?? ParserOptions.Templates.Default, root);
        }

    }
}