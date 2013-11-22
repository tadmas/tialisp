using TiaLisp.Execution;
using TiaLisp.Environment;
using TiaLisp.Values;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace TiaLisp.Tests.Execution
{
    [TestClass]
    public class PredefinedPredicates
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void BooleanP()
        {
            LispAssert.EvaluatesTo(Lisp.Constant(true), Lisp.List(Lisp.Symbol("boolean?"), Lisp.Constant(true)));
            LispAssert.EvaluatesTo(Lisp.Constant(true), Lisp.List(Lisp.Symbol("boolean?"), Lisp.Constant(false)));
            LispAssert.EvaluatesTo(Lisp.Constant(true), Lisp.List(Lisp.Symbol("boolean?"), Lisp.Quote(Lisp.Constant(false))));

            LispAssert.EvaluatesTo(Lisp.Constant(false), Lisp.List(Lisp.Symbol("boolean?"), Lisp.Constant('x')));
            LispAssert.EvaluatesTo(Lisp.Constant(false), Lisp.List(Lisp.Symbol("boolean?"), Nil.Instance));
            LispAssert.EvaluatesTo(Lisp.Constant(false), Lisp.List(Lisp.Symbol("boolean?"), Lisp.Constant(5)));
            LispAssert.EvaluatesTo(Lisp.Constant(false), Lisp.List(Lisp.Symbol("boolean?"), Lisp.Constant("test")));
            LispAssert.EvaluatesTo(Lisp.Constant(false), Lisp.List(Lisp.Symbol("boolean?"), Lisp.Quote(Lisp.Symbol("test"))));
            LispAssert.EvaluatesTo(Lisp.Constant(false), Lisp.List(Lisp.Symbol("boolean?"), new UndefinedValue()));
        }

        [TestMethod]
        public void CharP()
        {
            LispAssert.EvaluatesTo(Lisp.Constant(true), Lisp.List(Lisp.Symbol("char?"), Lisp.Constant('x')));
            LispAssert.EvaluatesTo(Lisp.Constant(true), Lisp.List(Lisp.Symbol("char?"), Lisp.Quote(Lisp.Constant('x'))));

            LispAssert.EvaluatesTo(Lisp.Constant(false), Lisp.List(Lisp.Symbol("char?"), Lisp.Constant(true)));
            LispAssert.EvaluatesTo(Lisp.Constant(false), Lisp.List(Lisp.Symbol("char?"), Nil.Instance));
            LispAssert.EvaluatesTo(Lisp.Constant(false), Lisp.List(Lisp.Symbol("char?"), Lisp.Constant(5)));
            LispAssert.EvaluatesTo(Lisp.Constant(false), Lisp.List(Lisp.Symbol("char?"), Lisp.Constant("test")));
            LispAssert.EvaluatesTo(Lisp.Constant(false), Lisp.List(Lisp.Symbol("char?"), Lisp.Quote(Lisp.Symbol("test"))));
            LispAssert.EvaluatesTo(Lisp.Constant(false), Lisp.List(Lisp.Symbol("char?"), new UndefinedValue()));
        }

        [TestMethod]
        public void LambdaP()
        {
            LispAssert.EvaluatesTo(Lisp.Constant(true), Lisp.List(Lisp.Symbol("lambda?"), Lisp.Symbol("lambda?")));
            // TODO: add test for user-defined function

            LispAssert.EvaluatesTo(Lisp.Constant(false), Lisp.List(Lisp.Symbol("lambda?"), Lisp.Constant(true)));
            LispAssert.EvaluatesTo(Lisp.Constant(false), Lisp.List(Lisp.Symbol("lambda?"), Lisp.Constant('x')));
            LispAssert.EvaluatesTo(Lisp.Constant(false), Lisp.List(Lisp.Symbol("lambda?"), Nil.Instance));
            LispAssert.EvaluatesTo(Lisp.Constant(false), Lisp.List(Lisp.Symbol("lambda?"), Lisp.Constant(5)));
            LispAssert.EvaluatesTo(Lisp.Constant(false), Lisp.List(Lisp.Symbol("lambda?"), Lisp.Constant("test")));
            LispAssert.EvaluatesTo(Lisp.Constant(false), Lisp.List(Lisp.Symbol("lambda?"), Lisp.Quote(Lisp.Symbol("test"))));
            LispAssert.EvaluatesTo(Lisp.Constant(false), Lisp.List(Lisp.Symbol("lambda?"), new UndefinedValue()));
        }

        [TestMethod]
        public void ListP()
        {
            LispAssert.EvaluatesTo(Lisp.Constant(true), Lisp.List(Lisp.Symbol("list?"), Lisp.Nil));
            LispAssert.EvaluatesTo(Lisp.Constant(true), Lisp.List(Lisp.Symbol("list?"), Lisp.Quote(Lisp.List(Lisp.Constant(1), Lisp.Symbol("a")))));

            LispAssert.EvaluatesTo(Lisp.Constant(false), Lisp.List(Lisp.Symbol("list?"), Lisp.Constant(true)));
            LispAssert.EvaluatesTo(Lisp.Constant(false), Lisp.List(Lisp.Symbol("list?"), Lisp.Constant('x')));
            LispAssert.EvaluatesTo(Lisp.Constant(false), Lisp.List(Lisp.Symbol("list?"), Lisp.Constant(5)));
            LispAssert.EvaluatesTo(Lisp.Constant(false), Lisp.List(Lisp.Symbol("list?"), Lisp.Constant("test")));
            LispAssert.EvaluatesTo(Lisp.Constant(false), Lisp.List(Lisp.Symbol("list?"), Lisp.Quote(Lisp.Symbol("test"))));
            LispAssert.EvaluatesTo(Lisp.Constant(false), Lisp.List(Lisp.Symbol("list?"), new UndefinedValue()));
        }

        [TestMethod]
        public void NumberP()
        {
            LispAssert.EvaluatesTo(Lisp.Constant(true), Lisp.List(Lisp.Symbol("number?"), Lisp.Constant(5)));
            LispAssert.EvaluatesTo(Lisp.Constant(true), Lisp.List(Lisp.Symbol("number?"), Lisp.Constant(9.95m)));

            LispAssert.EvaluatesTo(Lisp.Constant(false), Lisp.List(Lisp.Symbol("number?"), Lisp.Constant(true)));
            LispAssert.EvaluatesTo(Lisp.Constant(false), Lisp.List(Lisp.Symbol("number?"), Lisp.Constant('x')));
            LispAssert.EvaluatesTo(Lisp.Constant(false), Lisp.List(Lisp.Symbol("number?"), Nil.Instance));
            LispAssert.EvaluatesTo(Lisp.Constant(false), Lisp.List(Lisp.Symbol("number?"), Lisp.Constant("test")));
            LispAssert.EvaluatesTo(Lisp.Constant(false), Lisp.List(Lisp.Symbol("number?"), Lisp.Quote(Lisp.Symbol("test"))));
            LispAssert.EvaluatesTo(Lisp.Constant(false), Lisp.List(Lisp.Symbol("number?"), new UndefinedValue()));
        }

        [TestMethod]
        public void StringP()
        {
            LispAssert.EvaluatesTo(Lisp.Constant(true), Lisp.List(Lisp.Symbol("string?"), Lisp.Constant("test")));

            LispAssert.EvaluatesTo(Lisp.Constant(false), Lisp.List(Lisp.Symbol("string?"), Lisp.Constant(true)));
            LispAssert.EvaluatesTo(Lisp.Constant(false), Lisp.List(Lisp.Symbol("string?"), Lisp.Constant('x')));
            LispAssert.EvaluatesTo(Lisp.Constant(false), Lisp.List(Lisp.Symbol("string?"), Nil.Instance));
            LispAssert.EvaluatesTo(Lisp.Constant(false), Lisp.List(Lisp.Symbol("string?"), Lisp.Constant(5)));
            LispAssert.EvaluatesTo(Lisp.Constant(false), Lisp.List(Lisp.Symbol("string?"), Lisp.Quote(Lisp.Symbol("test"))));
            LispAssert.EvaluatesTo(Lisp.Constant(false), Lisp.List(Lisp.Symbol("string?"), new UndefinedValue()));
        }

        [TestMethod]
        public void SymbolP()
        {
            LispAssert.EvaluatesTo(Lisp.Constant(true), Lisp.List(Lisp.Symbol("symbol?"), Lisp.Quote(Lisp.Symbol("test"))));

            LispAssert.EvaluatesTo(Lisp.Constant(false), Lisp.List(Lisp.Symbol("symbol?"), Lisp.Constant(true)));
            LispAssert.EvaluatesTo(Lisp.Constant(false), Lisp.List(Lisp.Symbol("symbol?"), Lisp.Constant('x')));
            LispAssert.EvaluatesTo(Lisp.Constant(false), Lisp.List(Lisp.Symbol("symbol?"), Nil.Instance));
            LispAssert.EvaluatesTo(Lisp.Constant(false), Lisp.List(Lisp.Symbol("symbol?"), Lisp.Constant(5)));
            LispAssert.EvaluatesTo(Lisp.Constant(false), Lisp.List(Lisp.Symbol("symbol?"), Lisp.Constant("test")));
            LispAssert.EvaluatesTo(Lisp.Constant(false), Lisp.List(Lisp.Symbol("symbol?"), new UndefinedValue()));
        }
    }
}
