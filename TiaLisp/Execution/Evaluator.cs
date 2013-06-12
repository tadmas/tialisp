using System;
using System.Collections.Generic;
using System.Linq;
using TiaLisp.Environment;
using TiaLisp.Values;

namespace TiaLisp.Execution
{
    public static class Evaluator
    {
        public static ILispValue Evaluate(ILispValue value, ILispEnvironment environment)
        {
            switch (value.Type)
            {
                case LispValueType.Boolean:
                case LispValueType.Char:
                case LispValueType.Lambda:
                case LispValueType.Number:
                case LispValueType.String:
                default:
                    return value;
                case LispValueType.Symbol:
                    return environment.Lookup((Symbol)value);
                case LispValueType.List:
                    List list = (List)value;
                    if (list.IsEmpty)
                        return value;
                    else
                        return EvaluateConsBox((ConsBox)value, environment);
            }
        }

        private static ILispValue EvaluateConsBox(ConsBox cons, ILispEnvironment environment)
        {
            ILispValue head = cons.Head;
            IList<ILispValue> parameters = cons.CollectProperList().Skip(1).ToList();

            if (head.Type == LispValueType.Symbol)
            {
                // Look for special forms and process them, um..., specially.
                switch (((Symbol)head).Name)
                {
                    case "quote":
                        return EvaluateQuote(parameters, environment);
                    case "if":
                        return EvaluateIf(parameters, environment);
                    case "progn":
                        return EvaluateProgn(parameters, environment);
                    case "set!":
                        return EvaluateSet(parameters, environment);
                    case "lambda":
                        return EvaluateLambdaDefinition(parameters, environment);
                    default:
                        return EvaluateInvocation((Symbol)head, parameters, environment);
                }
            }
            else
            {
                head = Evaluate(head, environment);
                if (head.Type != LispValueType.Symbol)
                    throw new TypeMismatchException(new Symbol("function"), LispValueType.Symbol, cons.Head.Type);
                return EvaluateInvocation((Symbol)head, parameters, environment);
            }
        }

        private static ILispValue EvaluateInvocation(Symbol funcName, IList<ILispValue> args, ILispEnvironment environment)
        {
            ILispValue func = environment.Lookup(funcName);
            if (func.Type != LispValueType.Lambda)
                throw new TypeMismatchException(new Symbol("function"), LispValueType.Lambda, func.Type);

            ILispLambda lambda = (ILispLambda)func;
            IList<ILispValue> parameterValues = args.Select(arg => Evaluate(arg, environment)).ToList();

            return lambda.Execute(BindParameters(lambda, parameterValues));
        }

        private static ILispValue EvaluateQuote(IList<ILispValue> args, ILispEnvironment environment)
        {
            if (args.Count == 0)
                throw new SignatureMismatchException("missing argument to 'quote'");
            if (args.Count > 1)
                throw new SignatureMismatchException("too many arguments supplied");
            return args[0];
        }

        private static ILispValue EvaluateIf(IList<ILispValue> args, ILispEnvironment environment)
        {
            if (args.Count < 3)
                throw new SignatureMismatchException("too few arguments supplied");
            if (args.Count > 3)
                throw new SignatureMismatchException("too many arguments supplied");
            ILispValue test = Evaluate(args[0], environment);
            switch (test.Type)
            {
                case LispValueType.Boolean:
                    if (!((TiaLisp.Values.Boolean)test).Value)
                        return Evaluate(args[2], environment);
                    else
                        return Evaluate(args[1], environment);
                case LispValueType.List:
                    if (((List)test).IsEmpty)
                        return Evaluate(args[2], environment);
                    else
                        return Evaluate(args[1], environment);
                default:
                    // Any other value than #f or () is considered true:
                    return Evaluate(args[1], environment);
            }
        }

        private static ILispValue EvaluateProgn(IList<ILispValue> args, ILispEnvironment environment)
        {
            ILispValue result = Nil.Instance;
            foreach (ILispValue arg in args)
            {
                result = Evaluate(arg, environment);
            }
            return result;
        }

        private static ILispValue EvaluateSet(IList<ILispValue> args, ILispEnvironment environment)
        {
            if (args.Count < 2)
                throw new SignatureMismatchException("too few arguments supplied");
            if (args.Count > 2)
                throw new SignatureMismatchException("too many arguments supplied");
            // this is a special form since we do not evaluate the first argument, which must be a symbol
            if (args[0].Type != LispValueType.Symbol)
                throw new TypeMismatchException(new Symbol("variable"), LispValueType.Symbol, args[0].Type);
            ILispValue value = Evaluate(args[1], environment);
            environment.Set((Symbol)args[0], value);
            return value;
        }

        private static ILispValue EvaluateLambdaDefinition(IList<ILispValue> args, ILispEnvironment environment)
        {
            throw new NotImplementedException();
        }

        private static Dictionary<string, ILispValue> BindParameters(ILispLambda lambda, IList<ILispValue> parameters)
        {
            Dictionary<string, ILispValue> bindings = new Dictionary<string, ILispValue>();

            Queue<ILispValue> actualParameters = new Queue<ILispValue>(parameters);
            foreach (LambdaParameter definedParameter in lambda.Parameters)
            {
                switch (definedParameter.ParameterType)
                {
                    case LambdaParameterType.Normal:
                        {
                            if (actualParameters.Count == 0)
                                throw new SignatureMismatchException("missing required argument: " + definedParameter.Name);
                            ILispValue value = actualParameters.Dequeue();
                            if (definedParameter.ValueType != LispValueType.Unknown && definedParameter.ValueType != value.Type)
                                throw new TypeMismatchException(definedParameter.Name, definedParameter.ValueType, value.Type);
                            bindings[definedParameter.Name.Name] = value;
                        }
                        break;
                    case LambdaParameterType.Optional:
                        {
                            if (actualParameters.Count > 0)
                            {
                                goto case LambdaParameterType.Normal;
                            }
                            switch (definedParameter.ValueType)
                            {
                                case LispValueType.Unknown:
                                case LispValueType.List:
                                    bindings[definedParameter.Name.Name] = Nil.Instance;
                                    break;
                                case LispValueType.Boolean:
                                    bindings[definedParameter.Name.Name] = new TiaLisp.Values.Boolean(false);
                                    break;
                                case LispValueType.String:
                                    bindings[definedParameter.Name.Name] = new TiaLisp.Values.String(string.Empty);
                                    break;
                                case LispValueType.Number:
                                    bindings[definedParameter.Name.Name] = new Integer(0);
                                    break;
                                case LispValueType.Char:
                                    bindings[definedParameter.Name.Name] = new Character(default(char));
                                    break;
                                default:
                                    throw new LispException("do not know how to construct a default value for type " + definedParameter.ValueType);
                            }
                        }
                        break;
                    case LambdaParameterType.Rest:
                        {
                            foreach (ILispValue actualParameter in actualParameters)
                            {
                                if (definedParameter.ValueType != LispValueType.Unknown && definedParameter.ValueType != actualParameter.Type)
                                    throw new TypeMismatchException(definedParameter.Name, definedParameter.ValueType, actualParameter.Type);
                            }
                            bindings[definedParameter.Name.Name] = Lisp.List(actualParameters.ToArray());
                            actualParameters.Clear();
                        }
                        break;
                }
            }

            if (actualParameters.Count > 0)
                throw new SignatureMismatchException("too many arguments supplied");

            return bindings;
        }
    }
}
