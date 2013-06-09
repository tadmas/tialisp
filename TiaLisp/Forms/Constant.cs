using System;

namespace TiaLisp.Forms
{
    public sealed class Constant<T> : ILispForm
    {
        public T Value { get; internal set; }

        FormType ILispForm.Type
        {
            get
            {
                if (typeof(T) == typeof(int))
                    return FormType.Number;
                else if (typeof(T) == typeof(double))
                    return FormType.Number;
                else if (typeof(T) == typeof(decimal))
                    return FormType.Number;
                else if (typeof(T) == typeof(string))
                    return FormType.String;
                else if (typeof(T) == typeof(char))
                    return FormType.Char;
                else if (typeof(T) == typeof(bool))
                    return FormType.Boolean;
                else if (typeof(T) == typeof(Symbol))
                    return FormType.Symbol;
                else
                    return FormType.Unknown;
            }
        }
    }
}
