using System;
using TiaLisp.Values;

namespace TiaLisp.Environment
{
    public class SymbolNotBoundException : LispException
    {
        public Symbol Symbol { get; private set; }

        public SymbolNotBoundException(Symbol symbol)
        {
            this.Symbol = symbol;
        }

        public override string Message
        {
            get { return string.Format("The specified symbol was not bound: " + this.Symbol.ToString()); }
        }
    }
}
