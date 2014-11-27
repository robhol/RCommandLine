using System;
using System.Collections.Generic;

namespace RCommandLine
{
    public static class ArgumentConverters
    {
        private static readonly Dictionary<string, bool> BooleanValueNames = new Dictionary<string, bool>
        {
            {"0", false},
            {"1", true},
            {"false", false},
            {"true", true},
            {"no", false},
            {"yes", true},
            {"off", false},
            {"on", true}
        };

        public static Func<string, Type, object> DefaultConverter { get; private set; }
        public static Func<string, bool> BooleanConverter { get; private set; }

        static ArgumentConverters()
        {
            DefaultConverter = (str, type) => 
                Convert.ChangeType(str, type, System.Globalization.CultureInfo.InvariantCulture);

            BooleanConverter = str =>
            {
                try
                {
                    return BooleanValueNames[str.ToLowerInvariant()];
                }
                catch (KeyNotFoundException)
                {
                    throw new ArgumentException("Could not convert " + str + "into a boolean value.");
                }
            };
        }




    }
}
