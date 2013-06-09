using System;

namespace TiaLisp.Values
{
    public sealed class String : ILispValue
    {
        private readonly string _Value;

        public String(string value)
        {
            this._Value = value;
        }

        public string Value
        {
            get { return _Value; }
        }

        public LispValueType Type
        {
            get { return LispValueType.String; }
        }

        public override string ToString()
        {
            // TODO: Add escaping.
            return "\"" + this.Value + "\"";
        }

        bool IEquatable<ILispValue>.Equals(ILispValue other)
        {
            if (other == null)
                return false;
            else if (other is String)
                return string.Equals(this.Value, ((String)other).Value);
            else
                return false;
        }
    }
}
