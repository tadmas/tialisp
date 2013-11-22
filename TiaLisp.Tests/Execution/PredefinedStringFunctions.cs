using TiaLisp.Execution;
using TiaLisp.Environment;
using TiaLisp.Values;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace TiaLisp.Tests.Execution
{
    [TestClass]
    public class PredefinedStringFunctions
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void String()
        {
            LispAssert.EvaluatesTo(Lisp.Constant("test"),
                Lisp.List(Lisp.Symbol("string"), Lisp.Constant('t'), Lisp.Constant('e'), Lisp.Constant('s'), Lisp.Constant('t')));
            LispAssert.EvaluatesTo(Lisp.Constant(""),
                Lisp.List(Lisp.Symbol("string")));
        }

        [TestMethod]
        public void ListToString()
        {
            LispAssert.EvaluatesTo(Lisp.Constant("test"),
                Lisp.List(Lisp.Symbol("list->string"), Lisp.Quote(Lisp.List(Lisp.Constant('t'), Lisp.Constant('e'), Lisp.Constant('s'), Lisp.Constant('t')))));
            LispAssert.EvaluatesTo(Lisp.Constant(""),
                Lisp.List(Lisp.Symbol("list->string"), Lisp.Nil));
        }

        [TestMethod]
        public void StringToList()
        {
            LispAssert.EvaluatesTo(Lisp.List(Lisp.Constant('t'), Lisp.Constant('e'), Lisp.Constant('s'), Lisp.Constant('t')),
                Lisp.List(Lisp.Symbol("string->list"), Lisp.Constant("test")));
            LispAssert.EvaluatesTo(Lisp.Nil,
                Lisp.List(Lisp.Symbol("string->list"), Lisp.Constant("")));
        }

        [TestMethod]
        public void MakeString()
        {
            LispAssert.EvaluatesTo(Lisp.Constant("xxxxxxxxxx"),
                Lisp.List(Lisp.Symbol("make-string"), Lisp.Constant(10), Lisp.Constant('x')));
            LispAssert.ThrowsWhenEvaluated(
                Lisp.List(Lisp.Symbol("make-string"), Lisp.Constant(9.3m), Lisp.Constant('x')));
            LispAssert.EvaluatesTo(Lisp.Constant(8),
                Lisp.List(Lisp.Symbol("string-length"), Lisp.List(Lisp.Symbol("make-string"), Lisp.Constant(8))));
        }

        [TestMethod]
        public void StringLength()
        {
            LispAssert.EvaluatesTo(Lisp.Constant(4),
                Lisp.List(Lisp.Symbol("string-length"), Lisp.Constant("test")));
            LispAssert.EvaluatesTo(Lisp.Constant(0),
                Lisp.List(Lisp.Symbol("string-length"), Lisp.Constant("")));
        }

        [TestMethod]
        public void StringRef()
        {
            LispAssert.EvaluatesTo(Lisp.Constant('t'),
                Lisp.List(Lisp.Symbol("string-ref"), Lisp.Constant("test"), Lisp.Constant(0)));
            LispAssert.EvaluatesTo(Lisp.Constant('s'),
                Lisp.List(Lisp.Symbol("string-ref"), Lisp.Constant("test test test"), Lisp.Constant(12)));
            LispAssert.ThrowsWhenEvaluated(
                Lisp.List(Lisp.Symbol("string-ref"), Lisp.Constant("test"), Lisp.Constant(-1)));
            LispAssert.ThrowsWhenEvaluated(
                Lisp.List(Lisp.Symbol("string-ref"), Lisp.Constant("test"), Lisp.Constant(4)));
            LispAssert.ThrowsWhenEvaluated(
                Lisp.List(Lisp.Symbol("string-ref"), Lisp.Constant("test"), Lisp.Constant(2.2m)));
        }
    }
}
