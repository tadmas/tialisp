using System;

namespace TiaLisp.Forms
{
    public sealed class Constant<T> : ILispForm where T : IEquatable<T>
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

        bool IEquatable<ILispForm>.Equals(ILispForm other)
        {
            if (other == null)
                return false;
            if (!(other is Constant<T>))
                return false;
            Constant<T> otherConstant = (Constant<T>)other;
            if ((this.Value == null) == (otherConstant.Value == null))
                return true;
            return this.Value.Equals(otherConstant.Value);
        }

        public override string ToString()
        {
            switch (((ILispForm)this).Type)
            {
                case FormType.String:
                    // TODO: Add escaping...
                    return "\"" + this.Value.ToString() + "\"";
                case FormType.Char:
                    // TODO: Handle special chars (e.g., space)
                    return "#\\" + this.Value.ToString();
                case FormType.Boolean:
                    if ((this.Value as bool?).GetValueOrDefault())
                        return "#t";
                    else
                        return "#f";
                case FormType.Number:
                case FormType.Symbol:
                    return this.Value.ToString();
                default:
                    return "#<Unknown>";
            }
        }
    }
}
