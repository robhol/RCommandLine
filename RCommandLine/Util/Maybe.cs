﻿namespace RCommandLine.Util
{
    using System;

    public class Maybe<T>
    {

        public Maybe()
        {
            
        }

        public Maybe(T value)
        {
            Value = value;
        }
        
        public bool HasValue { get; private set; }

        private T _value;
        public T Value 
        {
            get
            {
                if (!HasValue)
                    throw new InvalidOperationException("No value exists.");

                return _value;
            }

            set 
            { 
                _value = value;
                HasValue = true;
            }
        }

        public T Or(T t)
        {
            return HasValue ? Value : t;
        }

        public override string ToString()
        {
            return HasValue ? Value.ToString() : "(NONE)";
        }
    }

}
