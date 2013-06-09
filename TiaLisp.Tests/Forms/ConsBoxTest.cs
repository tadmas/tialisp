using TiaLisp.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace TiaLisp.Tests.Forms
{
    [TestClass]
    public class ConsBoxTest
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void ProperListToString()
        {
            ILispForm properList = LispForms.List(LispForms.Symbol("test"), LispForms.Constant(1), LispForms.Constant("foo"), LispForms.List(LispForms.Constant(1), LispForms.Constant(2), LispForms.Constant(3)), LispForms.Constant(true));
            Assert.IsInstanceOfType(properList, typeof(ConsBox));
            Assert.AreEqual("(test 1 \"foo\" (1 2 3) #t)", properList.ToString());
        }

        [TestMethod]
        public void ImproperListToString()
        {
            ConsBox improperList = new ConsBox { Head = LispForms.Symbol("test"), Tail = LispForms.Constant(1) };
            Assert.AreEqual("(test . 1)", improperList.ToString());
        }

        [TestMethod]
        public void MixedImproperListToString()
        {
            ConsBox improperList = new ConsBox
            {
                Head = LispForms.Symbol("test"),
                Tail = new ConsBox
                {
                    Head = LispForms.Constant(1),
                    Tail = new ConsBox
                    {
                        Head = LispForms.Constant("foo"),
                        Tail = new ConsBox
                        {
                            Head = LispForms.List(LispForms.Constant(1), LispForms.Constant(2), LispForms.Constant(3)),
                            Tail = LispForms.Constant(true)
                        }
                    }
                }
            };

            Assert.AreEqual("(test 1 \"foo\" (1 2 3) . #t)", improperList.ToString());
        }

        [TestMethod]
        public void ProperListConstruction()
        {
            ILispForm helperConstructedList = LispForms.List(
                LispForms.Symbol("test"),
                LispForms.Constant(1),
                LispForms.Constant("foo"),
                LispForms.List(LispForms.Constant(1), LispForms.Constant(2), LispForms.Constant(3)),
                LispForms.Constant(true));

            ILispForm manuallyConstructedList = new ConsBox
            {
                Head = LispForms.Symbol("test"),
                Tail = new ConsBox
                {
                    Head = LispForms.Constant(1),
                    Tail = new ConsBox
                    {
                        Head = LispForms.Constant("foo"),
                        Tail = new ConsBox
                        {
                            Head = new ConsBox
                            {
                                Head = LispForms.Constant(1),
                                Tail = new ConsBox
                                {
                                    Head = LispForms.Constant(2),
                                    Tail = new ConsBox
                                    {
                                        Head = LispForms.Constant(3),
                                        Tail = Nil.Instance
                                    }
                                }
                            },
                            Tail = new ConsBox
                            {
                                Head = LispForms.Constant(true),
                                Tail = Nil.Instance
                            }
                        }
                    }
                }
            };

            // Do not use Assert.AreEqual here since it doesn't use IEquatable.
            Assert.IsTrue(EqualityComparer<ILispForm>.Default.Equals(manuallyConstructedList, helperConstructedList));
        }
    }
}
