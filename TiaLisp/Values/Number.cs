using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace TiaLisp.Values
{
    public sealed class Number : SimpleValue<decimal>, ILispValue
    {
        public Number(decimal value) : base(value)
        {
        }

        public Number(long value) : base(value)
        {
        }

        public Number(float value) : base((decimal)value)
        {
        }

        public LispValueType Type
        {
            get { return LispValueType.Number; }
        }

        public bool IsInteger
        {
            get { return this.Value == Math.Truncate(this.Value); }
        }

        public override string ToString()
        {
            return this.Value.ToString();
        }

        public static Number Add(Number left, Number right)
        {
            return new Number(left.Value + right.Value);
        }

        public static Number Subtract(Number left, Number right)
        {
            return new Number(left.Value - right.Value);
        }

        public static Number Multiply(Number left, Number right)
        {
            return new Number(left.Value * right.Value);
        }

        public static Number Divide(Number numerator, Number denominator)
        {
            if (denominator.Value == 0)
                throw new LispException("cannot divide by zero");
            return new Number(numerator.Value / denominator.Value);
        }
    }
}
