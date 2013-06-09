using System;

namespace TiaLisp.Forms
{
    public class ConsBox : ILispForm, IMaybeImproperList
    {
        public ILispForm Head { get; internal set; }
        public ILispForm Tail { get; internal set; }

        FormType ILispForm.Type
        {
            get { return FormType.Cons; }
        }
    }
}
