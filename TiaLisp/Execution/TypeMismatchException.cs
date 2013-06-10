using System;
using TiaLisp.Values;

namespace TiaLisp.Execution
{
    public class TypeMismatchException : LispException
    {
        public Symbol Symbol { get; private set; }
        public LispValueType ExpectedType { get; private set; }
        public LispValueType ActualType { get; private set; }

        public TypeMismatchException(Symbol symbol, LispValueType expectedType, LispValueType actualType)
        {
            this.Symbol = symbol;
            this.ExpectedType = expectedType;
            this.ActualType = actualType;
        }

        public override string Message
        {
            get { return string.Format("Type mismatch for {0}: expected {1} but found {2}", this.Symbol, this.ExpectedType, this.ActualType); }
        }
    }
}
