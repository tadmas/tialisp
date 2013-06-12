using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TiaLisp.Environment;
using TiaLisp.Execution;
using TiaLisp.Values;

namespace TiaLisp.Tests
{
    internal static class LispAssert
    {
        public static void AreEqual(ILispValue expected, ILispValue actual)
        {
            Assert.IsNotNull(expected);
            Assert.IsNotNull(actual);

            if (!EqualityComparer<ILispValue>.Default.Equals(expected, actual))
                throw new AssertFailedException(string.Format(
                    "Values are different.{0}Expected: {1}, Actual: {2}",
                    System.Environment.NewLine,
                    expected,
                    actual));
        }

        public static void EvaluatesTo(ILispValue expected, ILispValue expression, ILispEnvironment environment = null)
        {
            Assert.IsNotNull(expected);
            Assert.IsNotNull(expression);

            ILispValue actual = Evaluator.Evaluate(expression, environment ?? new GlobalEnvironment());

            if (!EqualityComparer<ILispValue>.Default.Equals(expected, actual))
                throw new AssertFailedException(string.Format(
                    "Computed incorrect value while evaluating {1}.{0}Expected: {2}, Actual: {3}",
                    System.Environment.NewLine,
                    expression,
                    expected,
                    actual));
        }

        public static void ThrowsWhenEvaluated(ILispValue expression, ILispEnvironment environment = null)
        {
            ThrowsWhenEvaluated<LispException>(expression, environment);
        }

        public static void ThrowsWhenEvaluated<TException>(ILispValue expression, ILispEnvironment environment = null) where TException : LispException
        {
            Assert.IsNotNull(expression);

            try
            {
                ILispValue actual = Evaluator.Evaluate(expression, environment ?? new GlobalEnvironment());
                throw new AssertFailedException(string.Format(
                    "Expression returned value instead of throwing: {1}{0}Expected: {2}, Actual: {3}",
                    System.Environment.NewLine,
                    expression,
                    typeof(TException).FullName,
                    actual));
            }
            catch (LispException ex)
            {
                if (!(ex is TException))
                {
                    throw new AssertFailedException(string.Format(
                        "Expression threw incorrect exception type: {1}{0}Expected: {2}{0}Actual: {3}",
                        System.Environment.NewLine,
                        expression,
                        typeof(TException).FullName,
                        ex));
                }
            }
        }
    }
}
