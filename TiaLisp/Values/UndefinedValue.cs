using System;

namespace TiaLisp.Values
{
    public class UndefinedValue : ILispValue
    {
        public LispValueType Type
        {
            get { return LispValueType.Unknown; }
        }

        bool IEquatable<ILispValue>.Equals(ILispValue other)
        {
            return false;
        }

        public override string ToString()
        {
            return "#<undefined>";
        }
    }
}
