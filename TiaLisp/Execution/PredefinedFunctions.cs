using System;
using System.Collections.Generic;
using TiaLisp.Values;

namespace TiaLisp.Execution
{
    internal static class PredefinedFunctions
    {
        private static Dictionary<string, ILispValue> BindParameters(NativeLambda lambda, IList<ILispValue> parameters)
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
                                default:
                                    throw new LispException("do not know how to construct a default value for type " + definedParameter.ValueType);
                            }
                        }
                        break;
                    case LambdaParameterType.Rest:
                        bindings[definedParameter.Name.Name] = Lisp.List(actualParameters.ToArray());
                        actualParameters.Clear();
                        break;
                }
            }

            if (actualParameters.Count > 0)
                throw new SignatureMismatchException("too many arguments supplied");

            return bindings;
        }

        public static NativeLambda Car = new NativeLambda
        {
            Parameters = new List<LambdaParameter>() { new LambdaParameter("list", LispValueType.List) },
            Body = parameters =>
                {
                    Dictionary<string, ILispValue> boundParams = BindParameters(Car, parameters);
                    if (((List)boundParams["list"]).IsEmpty)
                        throw new LispException("cannot take the CAR of an empty list");
                    return ((ConsBox)boundParams["list"]).Head;
                }
        };

        public static NativeLambda Cdr = new NativeLambda
        {
            Parameters = new List<LambdaParameter>() { new LambdaParameter("list", LispValueType.List) },
            Body = parameters =>
                {
                    Dictionary<string, ILispValue> boundParams = BindParameters(Car, parameters);
                     if (((List)boundParams["list"]).IsEmpty)
                        throw new LispException("cannot take the CDR of an empty list");
                    return ((ConsBox)boundParams["list"]).Tail;
                }
        };

        public static NativeLambda WriteLine = new NativeLambda
        {
            Parameters = new List<LambdaParameter>() { new LambdaParameter("text", LispValueType.String, LambdaParameterType.Optional) },
            Body = parameters =>
                {
                    Dictionary<string, ILispValue> boundParams = BindParameters(Car, parameters);
                    Console.WriteLine(((TiaLisp.Values.String)boundParams["text"]).Value);
                    return boundParams["text"];
                }
        };
    }
}
