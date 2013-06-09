using System;
using TiaLisp.Values;

namespace TiaLisp.Environment
{
    public interface ILispEnvironment
    {
        ILispValue Lookup(Symbol name);
        void Set(Symbol name, ILispValue value);
    }
}
