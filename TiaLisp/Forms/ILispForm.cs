using System;

namespace TiaLisp.Forms
{
    public interface ILispForm : IEquatable<ILispForm>
    {
        FormType Type { get; }
    }
}
