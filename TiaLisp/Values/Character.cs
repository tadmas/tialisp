using System;

namespace TiaLisp.Values
{
    public sealed class Character : SimpleValue<char>, ILispValue
    {
        public Character(char value) : base(value)
        {
        }

        public LispValueType Type
        {
            get { return LispValueType.Char; }
        }

        public override string ToString()
        {
            // TODO: Handle special characters (like space)
            return "#\\" + this.Value;
        }
    }
}
