using System;
using System.Collections.Generic;
using TiaLisp.Execution;
using TiaLisp.Values;

namespace TiaLisp.Environment
{
    internal class GlobalEnvironment : ILispEnvironment
    {
        private Dictionary<Symbol, ILispValue> _Variables = new Dictionary<Symbol, ILispValue>();
        private Dictionary<Symbol, ILispValue> _Predefined = PredefinedFunctions.GetSymbols();

        public ILispValue Lookup(Symbol name)
        {
            ILispValue value;
            if (!_Variables.TryGetValue(name, out value))
            {
                if (!_Predefined.TryGetValue(name, out value))
                {
                    throw new SymbolNotBoundException(name);
                }
            }
            return value;
        }

        public void Set(Symbol name, ILispValue value)
        {
            _Variables[name] = value;
        }
     }
}
