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
    }
}
