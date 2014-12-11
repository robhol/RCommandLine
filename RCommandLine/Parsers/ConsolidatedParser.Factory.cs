namespace RCommandLine
{
    using System;
    using System.Linq;
    using System.Text;

    public class ConsolidatedParser
    {

        /* Factory methods */

        public static ConsolidatedParser<T> FromAttributes<T>(ParserOptions options = null) where T : class, new()
        {
            var root = new AttributeModelBuilder<T>().Build();
            return new ConsolidatedParser<T>(options ?? ParserOptions.Templates.Default, root);
        }

    }
}