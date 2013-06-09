using TiaLisp.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TiaLisp.Tests.Forms
{
    [TestClass]
    public class SymbolTest
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorThrowsOnNull()
        {
            new Symbol(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorThrowsOnEmpty()
        {
            new Symbol("");
        }

        [TestMethod]
        public void EqualsTest()
        {
            Assert.AreEqual(new Symbol("test"), new Symbol("test"));
            Assert.AreNotEqual(new Symbol("test2"), new Symbol("test"));
            // Symbols are case-sensitive
            Assert.AreNotEqual(new Symbol("TEST"), new Symbol("test"));

            for (int i = 0; i < 100; i++)
            {
                string name = Guid.NewGuid().ToString();
                Assert.AreEqual(new Symbol(name), new Symbol(name));
            }
        }

        [TestMethod]
        public void EqualsNonSymbolIsFalse()
        {
            Symbol test = new Symbol("test");
            Assert.IsFalse(test.Equals(null));
            Assert.IsFalse(test.Equals("test"));
            Assert.IsFalse(test.Equals(4));
        }

        [TestMethod]
        public void GetHashCodeTest()
        {
            for (int i = 0; i < 100; i++)
            {
                string name = Guid.NewGuid().ToString();
                Assert.AreEqual(new Symbol(name).GetHashCode(), new Symbol(name).GetHashCode());
            }
        }

        [TestMethod]
        public void Name()
        {
            Symbol test = new Symbol("test");
            Assert.AreEqual("test", test.Name);
        }
    }
}
