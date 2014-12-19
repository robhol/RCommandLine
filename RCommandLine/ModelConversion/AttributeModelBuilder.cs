namespace RCommandLine.ModelConversion
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Attributes;
    using Models;
    using Util;

    internal class AttributeModelBuilder<TOptions>
    {

        private readonly Type _topType;

        public AttributeModelBuilder()
        {
            _topType = typeof (TOptions);
        }

        static IEnumerable<T> GetAttributes<T>(Type t, bool inherit = true)
        {
            return t.GetCustomAttributes(typeof(T), inherit).Cast<T>();
        }

        public Command Build()
        {
            var topCommand = new Command(_topType, _topType, "(root)", false);

            Action<Command> visit = null;
            visit = command =>
            {
                var ctype = command.OutputType;

                foreach (var ncmd in GetAttributes<HasCommandAttribute>(ctype, inherit: false)
                    .Select(attr => new Command(ctype, attr.CommandOptionsType, attr.Name, attr.Hidden, command)))
                {
                    command.Children.Add(ncmd);
                    visit(ncmd);
                }

                foreach (var u in GetAttributes<UsageAttribute>(ctype, inherit: false)
                    .Select(attr => new CommandUsage(attr.Usage, attr.Description)))
                    command.AddUsageExample(u);

                var extraAttrib = GetAttributes<LabelExtraArgumentsAttribute>(ctype, false).SingleOrDefault();
                if (extraAttrib != null)
                {
                    command.ExtraArgumentName = extraAttrib.Name;
                    command.ExtraArgumentDescription = extraAttrib.Description;
                }

                var props = ctype.GetProperties();
                foreach (var property in props)
                {
                    var attribs = property.GetCustomAttributes(true);
                    var propertyOptionalAttribute = attribs.OfType<OptionalAttribute>().SingleOrDefault();

                    var maybeDefaultValue = new Maybe<object>();
                    if (propertyOptionalAttribute != null)
                        maybeDefaultValue.Value = propertyOptionalAttribute.Default;

                    foreach (var attrib in attribs.Except(attribs.OfType<OptionalAttribute>()))
                    {
                        FlagAttribute flagAttribute;
                        ArgumentAttribute argumentAttribute;

                        if ((flagAttribute = attrib as FlagAttribute) != null)
                        {
                            command.AddFlag(new Flag(flagAttribute.GetShortName(), flagAttribute.Name, property, maybeDefaultValue)
                            {
                                Description = flagAttribute.Description
                            });
                        }
                        else if ((argumentAttribute = attrib as ArgumentAttribute) != null)
                        {
                            command.AddArgument(new Argument(argumentAttribute.GetOrder(), argumentAttribute.Name, property, maybeDefaultValue)
                            {
                                Description = argumentAttribute.Description
                            });
                        }
                    }
                }

            };

            visit(topCommand);

            return topCommand;
        }



    }
}