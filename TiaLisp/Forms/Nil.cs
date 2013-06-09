using System;

namespace TiaLisp.Forms
{
    public sealed class Nil : ILispForm, IMaybeImproperList
    {
        public static Nil Instance = new Nil();

        private Nil()
        {
        }

        FormType ILispForm.Type
        {
            get { return FormType.Nil; }
        }

        bool IEquatable<ILispForm>.Equals(ILispForm other)
        {
            if (other == null)
                return false;
            else
                return other is Nil;
        }

        public override string ToString()
        {
            return "()";
        }
    }
}
