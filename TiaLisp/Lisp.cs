﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TiaLisp.Values;

namespace TiaLisp
{
    public static class Lisp
    {
        public static Number Constant(long value)
        {
            return new Number(value);
        }

        public static Number Constant(decimal value)
        {
            return new Number(value);
        }

        public static TiaLisp.Values.Boolean Constant(bool value)
        {
            return new TiaLisp.Values.Boolean(value);
        }

        public static Character Constant(char value)
        {
            return new Character(value);
        }

        public static TiaLisp.Values.String Constant(string value)
        {
            return new TiaLisp.Values.String(value);
        }

        public static Symbol Symbol(string name)
        {
            return new Symbol(name);
        }

        public static List List(params ILispValue[] items)
        {
            List current = Nil.Instance;
            for (int i = items.Length - 1; i >= 0; i--)
            {
                current = new ConsBox { Head = items[i], Tail = current };
            }
            return current;
        }

        public static ConsBox Cons(ILispValue head, ILispValue tail)
        {
            return new ConsBox { Head = head, Tail = tail };
        }

        public static Nil Nil
        {
            get { return Nil.Instance; }
        }

        public static List Quote(ILispValue value)
        {
            return Lisp.List(Lisp.Symbol("quote"), value);
        }
    }
}
