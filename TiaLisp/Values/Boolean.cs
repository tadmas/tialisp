using System;

namespace TiaLisp.Values
{
    public sealed class Boolean : SimpleValue<bool>, ILispValue
    {
        public Boolean(bool value) : base(value)
        {
        }

        public LispValueType Type
        {
            get { return LispValueType.Boolean; }
        }

        public override string ToString()
        {
            return this.Value ? "#t" : "#f";
        }
    }
}
