using System;

namespace TiaLisp.Forms
{
    public struct Symbol : IEquatable<Symbol>
    {
        private readonly string _Name;

        public Symbol(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException();

            this._Name = name;
        }

        public string Name
        {
            get { return _Name; }
        }

        public bool Equals(Symbol other)
        {
            return string.Equals(this.Name, other.Name);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            else if (obj is Symbol)
                return Equals((Symbol)obj);
            else
                return false;
        }

        public override int GetHashCode()
        {
            return (this.Name ?? string.Empty).GetHashCode();
        }

        public override string ToString()
        {
            return (this.Name ?? string.Empty);
        }
    }
}
