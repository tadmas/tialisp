using System;

namespace TiaLisp.Values
{
    public interface ILispValue : IEquatable<ILispValue>
    {
        LispValueType Type { get; }
    }

    public enum LispValueType
    {
        Unknown = 0,
        List,
        Symbol,
        Number,
        String,
        Char,
        Boolean,
        Lambda,
    }
}
