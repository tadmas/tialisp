using System;

namespace TiaLisp.Values
{
    public sealed class Lambda : ILispValue
    {
        public LispValueType Type
        {
            get { return LispValueType.Lambda; }
        }

        bool IEquatable<ILispValue>.Equals(ILispValue other)
        {
            // TODO: implement
            return object.ReferenceEquals(this, other);
        }
    }
}
