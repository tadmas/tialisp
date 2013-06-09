using System;
using System.Collections.Generic;
using TiaLisp.Values;

namespace TiaLisp.Environment
{
    internal class GlobalEnvironment : ILispEnvironment
    {
        private Dictionary<Symbol, ILispValue> _Variables = new Dictionary<Symbol, ILispValue>();

        public ILispValue Lookup(Symbol name)
        {
            ILispValue value;
            if (!_Variables.TryGetValue(name, out value))
            {
                throw new SymbolNotBoundException(name);
            }
            return value;
        }

        public void Set(Symbol name, ILispValue value)
        {
            _Variables[name] = value;
        }
     }
}
