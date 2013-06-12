using TiaLisp.Execution;
using TiaLisp.Environment;
using TiaLisp.Values;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace TiaLisp.Tests.Execution
{
    [TestClass]
    public class PredefinedListFunctions
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void Car()
        {
            LispAssert.EvaluatesTo(Lisp.Constant(1),
                Lisp.List(Lisp.Symbol("car"), Lisp.Quote(Lisp.List(Lisp.Constant(1), Lisp.Constant(2), Lisp.Constant(3)))));
            LispAssert.EvaluatesTo(Lisp.Constant(1),
                Lisp.List(Lisp.Symbol("car"), Lisp.Quote(Lisp.List(Lisp.Constant(1)))));
            LispAssert.EvaluatesTo(Lisp.Constant(1),
                Lisp.List(Lisp.Symbol("car"), Lisp.Quote(Lisp.Cons(Lisp.Constant(1), Lisp.Constant(2)))));

            LispAssert.ThrowsWhenEvaluated(Lisp.List(Lisp.Symbol("car"), Lisp.Nil));
            LispAssert.ThrowsWhenEvaluated<TypeMismatchException>(Lisp.List(Lisp.Symbol("car"), Lisp.Constant(3)));
            LispAssert.ThrowsWhenEvaluated<SignatureMismatchException>(Lisp.List(Lisp.Symbol("car")));
        }

        [TestMethod]
        public void Cdr()
        {
            LispAssert.EvaluatesTo(Lisp.List(Lisp.Constant(2), Lisp.Constant(3)),
                Lisp.List(Lisp.Symbol("cdr"), Lisp.Quote(Lisp.List(Lisp.Constant(1), Lisp.Constant(2), Lisp.Constant(3)))));
            LispAssert.EvaluatesTo(Lisp.Nil,
                Lisp.List(Lisp.Symbol("cdr"), Lisp.Quote(Lisp.List(Lisp.Constant(1)))));
            LispAssert.EvaluatesTo(Lisp.Constant(2),
                Lisp.List(Lisp.Symbol("cdr"), Lisp.Quote(Lisp.Cons(Lisp.Constant(1), Lisp.Constant(2)))));

            LispAssert.ThrowsWhenEvaluated(Lisp.List(Lisp.Symbol("cdr"), Lisp.Nil));
            LispAssert.ThrowsWhenEvaluated<TypeMismatchException>(Lisp.List(Lisp.Symbol("cdr"), Lisp.Constant(3)));
            LispAssert.ThrowsWhenEvaluated<SignatureMismatchException>(Lisp.List(Lisp.Symbol("cdr")));
        }

        [TestMethod]
        public void Cons()
        {
            LispAssert.EvaluatesTo(Lisp.List(Lisp.Constant(1)),
                Lisp.List(Lisp.Symbol("cons"), Lisp.Constant(1), Lisp.Nil));
            LispAssert.EvaluatesTo(Lisp.List(Lisp.Constant(1), Lisp.Constant(2)),
                Lisp.List(Lisp.Symbol("cons"), Lisp.Constant(1), Lisp.Quote(Lisp.List(Lisp.Constant(2)))));
            LispAssert.EvaluatesTo(Lisp.Cons(Lisp.Constant(1), Lisp.Constant(2)),
                Lisp.List(Lisp.Symbol("cons"), Lisp.Constant(1), Lisp.Constant(2)));
        }

        [TestMethod]
        public void List()
        {
            LispAssert.EvaluatesTo(Lisp.Nil,
                Lisp.List(Lisp.Symbol("list")));
            LispAssert.EvaluatesTo(Lisp.List(Lisp.Constant(1)),
                Lisp.List(Lisp.Symbol("list"), Lisp.Constant(1)));
            LispAssert.EvaluatesTo(Lisp.List(Lisp.Constant(1), Lisp.Constant(2)),
                Lisp.List(Lisp.Symbol("list"), Lisp.Constant(1), Lisp.Constant(2)));
        }

        [TestMethod]
        public void Reverse()
        {
            LispAssert.EvaluatesTo(Lisp.Nil,
                Lisp.List(Lisp.Symbol("reverse"), Lisp.Nil));
            LispAssert.EvaluatesTo(Lisp.List(Lisp.Constant(1)),
                Lisp.List(Lisp.Symbol("reverse"), Lisp.Quote(Lisp.List(Lisp.Constant(1)))));
            LispAssert.EvaluatesTo(Lisp.List(Lisp.Constant(2), Lisp.Constant(1)),
                Lisp.List(Lisp.Symbol("reverse"), Lisp.Quote(Lisp.List(Lisp.Constant(1), Lisp.Constant(2)))));
            LispAssert.EvaluatesTo(Lisp.List(Lisp.Constant(5), Lisp.Constant(4), Lisp.Constant(3), Lisp.Constant(2), Lisp.Constant(1)),
                Lisp.List(Lisp.Symbol("reverse"), Lisp.Quote(Lisp.List(Lisp.Constant(1), Lisp.Constant(2), Lisp.Constant(3), Lisp.Constant(4), Lisp.Constant(5)))));
        }

        [TestMethod]
        public void Length()
        {
            LispAssert.EvaluatesTo(Lisp.Constant(0),
                Lisp.List(Lisp.Symbol("length"), Lisp.Nil));
            LispAssert.EvaluatesTo(Lisp.Constant(1),
                Lisp.List(Lisp.Symbol("length"), Lisp.Quote(Lisp.List(Lisp.Constant(1)))));
            LispAssert.EvaluatesTo(Lisp.Constant(2),
                Lisp.List(Lisp.Symbol("length"), Lisp.Quote(Lisp.List(Lisp.Constant(1), Lisp.Constant(2)))));
            LispAssert.EvaluatesTo(Lisp.Constant(5),
                Lisp.List(Lisp.Symbol("length"), Lisp.Quote(Lisp.List(Lisp.Constant(1), Lisp.Constant(2), Lisp.Constant(3), Lisp.Constant(4), Lisp.Constant(5)))));
        }
    }
}
