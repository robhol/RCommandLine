using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCommandLine
{
    public static class ArgumentConverters
    {
        private static Dictionary<string, bool> _booleanValueNames;

        public static Func<string, Type, object> DefaultConverter { get; private set; }
        public static Func<string, bool> BooleanConverter { get; private set; }

        static ArgumentConverters()
        {
            DefaultConverter =
                (str, type) => Convert.ChangeType(str, type, System.Globalization.CultureInfo.InvariantCulture);

            _booleanValueNames = new Dictionary<string, bool>
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

            BooleanConverter =
                (str) =>
                {
                    try
                    {
                        return _booleanValueNames[str.ToLowerInvariant()];
                    }
                    catch (KeyNotFoundException)
                    {
                        throw new ArgumentException("Could not convert " + str + "into a boolean value.");
                    }
                };
        }




    }
}
