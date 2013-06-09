using System;
using System.Text;

namespace TiaLisp.Forms
{
    public sealed class ConsBox : ILispForm, IMaybeImproperList
    {
        public ILispForm Head { get; internal set; }
        public ILispForm Tail { get; internal set; }

        FormType ILispForm.Type
        {
            get { return FormType.Cons; }
        }

        bool IEquatable<ILispForm>.Equals(ILispForm other)
        {
            if (other == null)
                return false;
            if (!(other is ConsBox))
                return false;
            ConsBox otherConsBox = (ConsBox)other;
            return (this.Head.Equals(otherConsBox.Head) && this.Tail.Equals(otherConsBox.Tail));
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("(");

            ConsBox current = this;
            do
            {
                sb.Append(current.Head.ToString());
                switch (current.Tail.Type)
                {
                    case FormType.Nil:
                        current = null;
                        break;
                    case FormType.Cons:
                        sb.Append(" ");
                        current = current.Tail as ConsBox;
                        break;
                    default:
                        sb.Append(" . ");
                        sb.Append(current.Tail.ToString());
                        current = null;
                        break;
                }
            } while (current != null);

            sb.Append(")");
            return sb.ToString();
        }
    }
}
