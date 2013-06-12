using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TiaLisp.Values
{
    public abstract class List : ILispValue
    {
        public LispValueType Type
        {
            get { return LispValueType.List; }
        }

        public abstract bool IsEmpty { get; }
        public abstract ILispValue GetHead();
        public abstract ILispValue GetTail();
        public abstract IList<ILispValue> CollectProperList();

        bool IEquatable<ILispValue>.Equals(ILispValue other)
        {
            if (other == null)
                return false;
            if (!(other is List))
                return false;

            List otherList = (List)other;

            if (this.IsEmpty != otherList.IsEmpty)
                return false;
            else if (this.IsEmpty && otherList.IsEmpty)
                return true;
            else
                return ((ConsBox)this).Equals((ConsBox)other);
        }
    }

    public sealed class Nil : List
    {
        public static Nil Instance = new Nil();

        private Nil()
        {
        }

        public override bool IsEmpty
        {
            get { return true; }
        }

        public override ILispValue GetHead()
        {
            throw new LispException("cannot take the CAR of an empty list");
        }

        public override ILispValue GetTail()
        {
            throw new LispException("cannot take the CDR of an empty list");
        }

        public override IList<ILispValue> CollectProperList()
        {
            return new ILispValue[0];
        }

        public override string ToString()
        {
            return "()";
        }
    }

    public sealed class ConsBox : List, IEquatable<ConsBox>
    {
        public ILispValue Head { get; internal set; }
        public ILispValue Tail { get; internal set; }

        public override bool IsEmpty
        {
            get { return false; }
        }

        public override ILispValue GetHead()
        {
            return this.Head;
        }

        public override ILispValue GetTail()
        {
            return this.Tail;
        }

        public override IList<ILispValue> CollectProperList()
        {
            List<ILispValue> items = new List<ILispValue>();

            WalkList(proper => items.Add(proper), improper => { throw new ImproperListException(this); });

            return items.AsReadOnly();
        }

        public override string ToString()
        {
            List<string> items = new List<string>();

            WalkList(proper => items.Add(proper.ToString()),
                improper => { items.Add("."); items.Add(improper.ToString()); });

            return "(" + string.Join(" ", items) + ")";
       }

        private void WalkList(Action<ILispValue> handleProperListItem, Action<ILispValue> handleImproperListTail)
        {
            for (ConsBox current = this; current != null; current = (ConsBox)current.Tail)
            {
                handleProperListItem(current.Head);

                if (current.Tail.Type != LispValueType.List)
                {
                    handleImproperListTail(current.Tail);
                    return;
                }

                if (((List)current.Tail).IsEmpty)
                    return;
            }
        }

        public bool Equals(ConsBox other)
        {
            if (other == null)
                return false;
            return this.Head.Equals(other.Head) && this.Tail.Equals(other.Tail);
        }
    }

    public class ImproperListException : LispException
    {
        public List List { get; private set; }

        public ImproperListException(List list)
        {
            this.List = list;
        }

        public override string Message
        {
            get { return string.Format("The specified list is not a proper list: " + this.List.ToString()); }
        }
    }

}
