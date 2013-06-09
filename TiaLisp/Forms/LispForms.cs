using System;

namespace TiaLisp.Forms
{
    public static class LispForms
    {
        public static Constant<T> Constant<T>(T value) where T : struct, IEquatable<T>
        {
            return new Constant<T> { Value = value };
        }

        public static Constant<string> Constant(string value)
        {
            return new Constant<string> { Value = value };
        }

        public static Constant<Symbol> Symbol(string name)
        {
            return new Constant<Symbol> { Value = new Symbol(name) };
        }

        public static ILispForm List(params ILispForm[] items)
        {
            ILispForm currentForm = Nil.Instance;

            for (int i = items.Length - 1; i >= 0; i--)
            {
                currentForm = new ConsBox { Head = items[i], Tail = currentForm };
            }

            return currentForm;
        }
    }
}
