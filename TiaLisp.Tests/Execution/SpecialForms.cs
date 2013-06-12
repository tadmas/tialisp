using TiaLisp.Execution;
using TiaLisp.Environment;
using TiaLisp.Values;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace TiaLisp.Tests.Execution
{
    [TestClass]
    public class SpecialForms
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void Quote()
        {
            LispAssert.EvaluatesTo(Lisp.List(Lisp.Symbol("car"), Lisp.Nil),
                Lisp.Quote(Lisp.List(Lisp.Symbol("car"), Lisp.Nil)));
        }

        [TestMethod]
        public void If()
        {
            ILispEnvironment environment = new GlobalEnvironment();
            environment.Set(new Symbol("kablooey"), new NativeLambda { Body = parameters => { throw new LispException("kablooey!"); } });

            // Ensure only the "then" clause is evaluated on a true condition.
            LispAssert.EvaluatesTo(Lisp.Constant(1),
                Lisp.List(Lisp.Symbol("if"), Lisp.Constant(true), Lisp.Constant(1), Lisp.List(Lisp.Symbol("kablooey"))),
                environment);
            LispAssert.ThrowsWhenEvaluated(
                Lisp.List(Lisp.Symbol("if"), Lisp.Constant(true), Lisp.List(Lisp.Symbol("kablooey")), Lisp.Constant(1)),
                environment);

            // Ensure only the "else" clause is evaluated on a false condition.
            LispAssert.EvaluatesTo(Lisp.Constant(1),
                Lisp.List(Lisp.Symbol("if"), Lisp.Constant(false), Lisp.List(Lisp.Symbol("kablooey")), Lisp.Constant(1)),
                environment);
            LispAssert.ThrowsWhenEvaluated(
                Lisp.List(Lisp.Symbol("if"), Lisp.Constant(false), Lisp.Constant(1), Lisp.List(Lisp.Symbol("kablooey"))),
                environment);

            // Ensure the test condition clause is evaluated.  In this case, a non-empty list would be true if not
            // evaluated, but evaluating the expression will produce false.
            LispAssert.EvaluatesTo(Lisp.Constant(2),
                Lisp.List(Lisp.Symbol("if"), Lisp.List(Lisp.Symbol("number?"), Lisp.Symbol("car")), Lisp.Constant(1), Lisp.Constant(2)));

            // Test truth / falsiness of non-boolean - only #f and () should count as false.
            LispAssert.EvaluatesTo(Lisp.Constant(2), Lisp.List(Lisp.Symbol("if"), Lisp.Nil, Lisp.Constant(1), Lisp.Constant(2)));
            LispAssert.EvaluatesTo(Lisp.Constant(1), Lisp.List(Lisp.Symbol("if"), Lisp.Quote(Lisp.List(Lisp.Constant(false))), Lisp.Constant(1), Lisp.Constant(2)));
            LispAssert.EvaluatesTo(Lisp.Constant(1), Lisp.List(Lisp.Symbol("if"), Lisp.Constant(""), Lisp.Constant(1), Lisp.Constant(2)));
            LispAssert.EvaluatesTo(Lisp.Constant(1), Lisp.List(Lisp.Symbol("if"), Lisp.Symbol("cdr"), Lisp.Constant(1), Lisp.Constant(2)));
        }
    }
}
