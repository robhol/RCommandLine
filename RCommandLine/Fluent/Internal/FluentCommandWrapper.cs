using System;

namespace RCommandLine.Fluent
{
    using ModelConversion;
    using Models;

    internal class FluentCommandWrapper<T, TOptions> : ParameterOwner<T, TOptions>, IFluentCommand<T, TOptions>, IMixinInjectionTarget where T : class where TOptions : class
    {

        private readonly FluentModelBuilder<TOptions> _builder;

        private readonly Command _command;

        internal FluentCommandWrapper(FluentModelBuilder<TOptions> builder, Command parent = null) : base(builder)
        {
            _builder = builder;

            _command = new Command(parent != null ? parent.OutputType : _builder.TopType, typeof (T), parentCommand: parent);
        }

        public void AddArgument(Argument argument)
        {
            _command.AddArgument(argument);
        }

        public void AddFlag(Flag flag)
        {
            _command.AddFlag(flag);
        }

        internal Command GetCommand()
        {
            return _command;
        }

        public IFluentCommand<T, TOptions> Name(NameType nameType)
        {
            switch (nameType)
            {
                case NameType.None:
                    return Name("");
                case NameType.Default:
                    return Name(null);
            }

            return this;
        }

        public IFluentCommand<T, TOptions> Name(string name)
        {
            _command.Name = name;
            return this;
        }

        public IFluentCommand<T, TOptions> Description(string description)
        {
            _command.Description = description;
            return this;
        }

        public IFluentCommand<T, TOptions> Hidden(bool hidden = true)
        {
            _command.Hidden = true;
            return this;
        }

        public IFluentCommand<T, TOptions> LabelExtraArguments(string name, string description)
        {
            _command.ExtraArgumentName = name;
            _command.ExtraArgumentDescription = description;
            return this;
        }

        public IFluentCommand<T, TOptions> Command<TCommand>(Action<IFluentCommand<TCommand, TOptions>> configurator) where TCommand : class
        {
            var newWrapper = _builder.NewWrapper<TCommand>(_command);
            configurator(newWrapper);

            return this;
        }

        private int _usageOrder = 0;
        public IFluentCommand<T, TOptions> Usage(string invocation, string description = null)
        {
            _command.AddUsageExample(new CommandUsage(invocation) {Description = description, Order = _usageOrder++});
            return this;
        }

        private int _remarkOrder = 0;
        public IFluentCommand<T, TOptions> Remark(string remark)
        {
            _command.AddRemark(new CommandRemark(remark) {Order = _remarkOrder++});
            return this;
        }
    }



}
