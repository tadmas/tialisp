using TiaLisp.Values;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace TiaLisp.Tests.Values
{
    [TestClass]
    public class ConsBoxTest
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void ProperListToString()
        {
            ILispValue properList = Lisp.List(Lisp.Symbol("test"), Lisp.Constant(1), Lisp.Constant("foo"), Lisp.List(Lisp.Constant(1), Lisp.Constant(2), Lisp.Constant(3)), Lisp.Constant(true));
            Assert.IsInstanceOfType(properList, typeof(ConsBox));
            Assert.AreEqual("(test 1 \"foo\" (1 2 3) #t)", properList.ToString());
        }

        [TestMethod]
        public void ImproperListToString()
        {
            ConsBox improperList = new ConsBox { Head = Lisp.Symbol("test"), Tail = Lisp.Constant(1) };
            Assert.AreEqual("(test . 1)", improperList.ToString());
        }

        [TestMethod]
        public void MixedImproperListToString()
        {
            ConsBox improperList = new ConsBox
            {
                Head = Lisp.Symbol("test"),
                Tail = new ConsBox
                {
                    Head = Lisp.Constant(1),
                    Tail = new ConsBox
                    {
                        Head = Lisp.Constant("foo"),
                        Tail = new ConsBox
                        {
                            Head = Lisp.List(Lisp.Constant(1), Lisp.Constant(2), Lisp.Constant(3)),
                            Tail = Lisp.Constant(true)
                        }
                    }
                }
            };

            Assert.AreEqual("(test 1 \"foo\" (1 2 3) . #t)", improperList.ToString());
        }

        [TestMethod]
        public void ProperListConstruction()
        {
            ILispValue helperConstructedList = Lisp.List(
                Lisp.Symbol("test"),
                Lisp.Constant(1),
                Lisp.Constant("foo"),
                Lisp.List(Lisp.Constant(1), Lisp.Constant(2), Lisp.Constant(3)),
                Lisp.Constant(true));

            ILispValue manuallyConstructedList = new ConsBox
            {
                Head = Lisp.Symbol("test"),
                Tail = new ConsBox
                {
                    Head = Lisp.Constant(1),
                    Tail = new ConsBox
                    {
                        Head = Lisp.Constant("foo"),
                        Tail = new ConsBox
                        {
                            Head = new ConsBox
                            {
                                Head = Lisp.Constant(1),
                                Tail = new ConsBox
                                {
                                    Head = Lisp.Constant(2),
                                    Tail = new ConsBox
                                    {
                                        Head = Lisp.Constant(3),
                                        Tail = Nil.Instance
                                    }
                                }
                            },
                            Tail = new ConsBox
                            {
                                Head = Lisp.Constant(true),
                                Tail = Nil.Instance
                            }
                        }
                    }
                }
            };

            // Do not use Assert.AreEqual here since it doesn't use IEquatable.
            LispAssert.AreEqual(manuallyConstructedList, helperConstructedList);
        }
    }
}
