using System;

namespace TiaLisp.Values
{
    public sealed class Symbol : ILispValue, IEquatable<Symbol>
    {
        private readonly string _Name;

        public Symbol(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException();

            this._Name = name;
        }

        public string Name
        {
            get { return _Name; }
        }

        public LispValueType Type
        {
            get { return LispValueType.Symbol; }
        }

        public bool Equals(Symbol other)
        {
            if (other == null)
                return false;
            return string.Equals(this.Name, other.Name);
        }

        public override bool Equals(object obj)
        {
            if (obj is Symbol)
                return Equals((Symbol)obj);
            else
                return false;
        }

        bool IEquatable<ILispValue>.Equals(ILispValue other)
        {
            if (other is Symbol)
                return Equals((Symbol)other);
            else
                return false;
        }

        public override int GetHashCode()
        {
            return this.Name.GetHashCode();
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
