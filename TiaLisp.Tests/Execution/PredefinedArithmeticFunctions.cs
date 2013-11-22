using TiaLisp.Execution;
using TiaLisp.Environment;
using TiaLisp.Values;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace TiaLisp.Tests.Execution
{
    [TestClass]
    public class PredefinedArithmeticFunctions
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void Add()
        {
            LispAssert.EvaluatesTo(Lisp.Constant(1),
                Lisp.List(Lisp.Symbol("+"), Lisp.Constant(1)));
            LispAssert.EvaluatesTo(Lisp.Constant(3),
                Lisp.List(Lisp.Symbol("+"), Lisp.Constant(1), Lisp.Constant(2)));
            LispAssert.EvaluatesTo(Lisp.Constant(6),
                Lisp.List(Lisp.Symbol("+"), Lisp.Constant(1), Lisp.Constant(2), Lisp.Constant(3)));

            LispAssert.EvaluatesTo(Lisp.Constant(6.3m),
                Lisp.List(Lisp.Symbol("+"), Lisp.Constant(1.1m), Lisp.Constant(2.1m), Lisp.Constant(3.1m)));
            LispAssert.EvaluatesTo(Lisp.Constant(6.1m),
                Lisp.List(Lisp.Symbol("+"), Lisp.Constant(1.1m), Lisp.Constant(2), Lisp.Constant(3)));
            LispAssert.EvaluatesTo(Lisp.Constant(6.1m),
                Lisp.List(Lisp.Symbol("+"), Lisp.Constant(1), Lisp.Constant(2.1m), Lisp.Constant(3)));
            LispAssert.EvaluatesTo(Lisp.Constant(6.1m),
                Lisp.List(Lisp.Symbol("+"), Lisp.Constant(1), Lisp.Constant(2), Lisp.Constant(3.1m)));
        }

        [TestMethod]
        public void Subtract()
        {
            LispAssert.EvaluatesTo(Lisp.Constant(-1),
                Lisp.List(Lisp.Symbol("-"), Lisp.Constant(1)));
            LispAssert.EvaluatesTo(Lisp.Constant(-1),
                Lisp.List(Lisp.Symbol("-"), Lisp.Constant(1), Lisp.Constant(2)));
            LispAssert.EvaluatesTo(Lisp.Constant(-4),
                Lisp.List(Lisp.Symbol("-"), Lisp.Constant(1), Lisp.Constant(2), Lisp.Constant(3)));

            LispAssert.EvaluatesTo(Lisp.Constant(-0.1m),
                Lisp.List(Lisp.Symbol("-"), Lisp.Constant(3.1m), Lisp.Constant(2.1m), Lisp.Constant(1.1m)));
            LispAssert.EvaluatesTo(Lisp.Constant(0.1m),
                Lisp.List(Lisp.Symbol("-"), Lisp.Constant(3.1m), Lisp.Constant(2), Lisp.Constant(1)));
            LispAssert.EvaluatesTo(Lisp.Constant(-0.1m),
                Lisp.List(Lisp.Symbol("-"), Lisp.Constant(3), Lisp.Constant(2.1m), Lisp.Constant(1)));
            LispAssert.EvaluatesTo(Lisp.Constant(-0.1m),
                Lisp.List(Lisp.Symbol("-"), Lisp.Constant(3), Lisp.Constant(2), Lisp.Constant(1.1m)));
        }

        [TestMethod]
        public void Multiply()
        {
            LispAssert.EvaluatesTo(Lisp.Constant(1),
                Lisp.List(Lisp.Symbol("*"), Lisp.Constant(1)));
            LispAssert.EvaluatesTo(Lisp.Constant(2),
                Lisp.List(Lisp.Symbol("*"), Lisp.Constant(1), Lisp.Constant(2)));
            LispAssert.EvaluatesTo(Lisp.Constant(6),
                Lisp.List(Lisp.Symbol("*"), Lisp.Constant(1), Lisp.Constant(2), Lisp.Constant(3)));
            LispAssert.EvaluatesTo(Lisp.Constant(24),
                Lisp.List(Lisp.Symbol("*"), Lisp.Constant(1), Lisp.Constant(2), Lisp.Constant(3), Lisp.Constant(4)));

            LispAssert.EvaluatesTo(Lisp.Constant(3.1m*2.1m*1.1m),
                Lisp.List(Lisp.Symbol("*"), Lisp.Constant(3.1m), Lisp.Constant(2.1m), Lisp.Constant(1.1m)));
            LispAssert.EvaluatesTo(Lisp.Constant(2.1m*3*4),
                Lisp.List(Lisp.Symbol("*"), Lisp.Constant(2.1m), Lisp.Constant(3), Lisp.Constant(4)));
            LispAssert.EvaluatesTo(Lisp.Constant(2*3.1m*4),
                Lisp.List(Lisp.Symbol("*"), Lisp.Constant(2), Lisp.Constant(3.1m), Lisp.Constant(4)));
            LispAssert.EvaluatesTo(Lisp.Constant(2*3*4.1m),
                Lisp.List(Lisp.Symbol("*"), Lisp.Constant(2), Lisp.Constant(3), Lisp.Constant(4.1m)));
        }

        [TestMethod]
        public void Divide()
        {
            LispAssert.EvaluatesTo(Lisp.Constant(1),
                Lisp.List(Lisp.Symbol("/"), Lisp.Constant(1)));
            LispAssert.EvaluatesTo(Lisp.Constant(0.5m),
                Lisp.List(Lisp.Symbol("/"), Lisp.Constant(1), Lisp.Constant(2)));
            LispAssert.EvaluatesTo(Lisp.Constant(1m/2m/3m),
                Lisp.List(Lisp.Symbol("/"), Lisp.Constant(1), Lisp.Constant(2), Lisp.Constant(3)));
            LispAssert.EvaluatesTo(Lisp.Constant(1m/2m/3m/4m),
                Lisp.List(Lisp.Symbol("/"), Lisp.Constant(1), Lisp.Constant(2), Lisp.Constant(3), Lisp.Constant(4)));

            LispAssert.EvaluatesTo(Lisp.Constant(3.1m/2.1m/1.1m),
                Lisp.List(Lisp.Symbol("/"), Lisp.Constant(3.1m), Lisp.Constant(2.1m), Lisp.Constant(1.1m)));
            LispAssert.EvaluatesTo(Lisp.Constant(2.1m/3m/4m),
                Lisp.List(Lisp.Symbol("/"), Lisp.Constant(2.1m), Lisp.Constant(3), Lisp.Constant(4)));
            LispAssert.EvaluatesTo(Lisp.Constant(2m/3.1m/4m),
                Lisp.List(Lisp.Symbol("/"), Lisp.Constant(2), Lisp.Constant(3.1m), Lisp.Constant(4)));
            LispAssert.EvaluatesTo(Lisp.Constant(2m/3m/4.1m),
                Lisp.List(Lisp.Symbol("/"), Lisp.Constant(2), Lisp.Constant(3), Lisp.Constant(4.1m)));
        }
    }
}
