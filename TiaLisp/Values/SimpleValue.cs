using System;

namespace TiaLisp.Values
{
    public abstract class SimpleValue<T> : IEquatable<ILispValue> where T : struct, IEquatable<T>
    {
        private readonly T _Value;

        public SimpleValue(T value)
        {
            this._Value = value;
        }

        public T Value
        {
            get { return _Value; }
        }

        bool IEquatable<ILispValue>.Equals(ILispValue other)
        {
            if (other == null)
                return false;
            if (!(other is SimpleValue<T>))
                return false;
            return this.Value.Equals(((SimpleValue<T>)other).Value);
        }
    }
}
