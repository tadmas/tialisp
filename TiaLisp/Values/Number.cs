using System;
using System.Collections.Generic;

namespace TiaLisp.Values
{
    public abstract class Number<T> : SimpleValue<T>, ILispValue where T : struct, IEquatable<T>
    {
        public Number(T value) : base(value)
        {
        }

        public LispValueType Type
        {
            get { return LispValueType.Number; }
        }

        public override string ToString()
        {
            return this.Value.ToString();
        }
    }

    public sealed class Integer : Number<long>
    {
        public Integer(long value) : base(value)
        {
        }

        public static implicit operator FloatingPointNumber(Integer integer)
        {
            return new FloatingPointNumber(integer.Value);
        }

        public static implicit operator FixedPointNumber(Integer integer)
        {
            return new FixedPointNumber(integer.Value);
        }
    }

    public sealed class FloatingPointNumber : Number<double>
    {
        public FloatingPointNumber(double value) : base(value)
        {
        }

        public static explicit operator Integer(FloatingPointNumber number)
        {
            return new Integer((long)number.Value);
        }

        public static explicit operator FixedPointNumber(FloatingPointNumber number)
        {
            return new FixedPointNumber((decimal)number.Value);
        }
    }

    public sealed class FixedPointNumber : Number<decimal>
    {
        public FixedPointNumber(decimal value) : base(value)
        {
        }

        public static explicit operator Integer(FixedPointNumber number)
        {
            return new Integer((long)number.Value);
        }

        public static explicit operator FloatingPointNumber(FixedPointNumber number)
        {
            return new FloatingPointNumber((double)number.Value);
        }
    }
}
