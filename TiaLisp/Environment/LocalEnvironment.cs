using System;
using System.Collections.Generic;
using TiaLisp.Values;

namespace TiaLisp.Environment
{
    internal class LocalEnvironment : ILispEnvironment
    {
        private ILispEnvironment _ParentEnvironment;
        private Dictionary<Symbol, ILispValue> _Variables = new Dictionary<Symbol, ILispValue>();

        internal LocalEnvironment(ILispEnvironment parentEnvironment)
        {
            this._ParentEnvironment = parentEnvironment;
        }

        public ILispValue Lookup(Symbol name)
        {
            ILispValue value;
            if (!_Variables.TryGetValue(name, out value))
            {
                return _ParentEnvironment.Lookup(name);
            }
            return value;
        }

        public void Set(Symbol name, ILispValue value)
        {
            _Variables[name] = value;
        }
    }
}
