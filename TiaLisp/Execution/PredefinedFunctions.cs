using System;
using System.Collections.Generic;
using System.Linq;
using TiaLisp.Values;

namespace TiaLisp.Execution
{
    internal static class PredefinedFunctions
    {
        #region GetSymbols

        public static Dictionary<Symbol, ILispValue> GetSymbols()
        {
            Dictionary<Symbol, ILispValue> dict = new Dictionary<Symbol, ILispValue>();

            GetSymbols_Predicates(dict);
            GetSymbols_Lists(dict);
            GetSymbols_Strings(dict);
            GetSymbols_IO(dict);

            return dict;
        }

        #endregion

        #region Helper methods

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

        #endregion

        #region Predicates

        private static void GetSymbols_Predicates(Dictionary<Symbol, ILispValue> dict)
        {
            dict.Add(new Symbol("boolean?"), BooleanP);
            dict.Add(new Symbol("char?"), CharP);
            dict.Add(new Symbol("lambda?"), LambdaP);
            dict.Add(new Symbol("list?"), ListP);
            dict.Add(new Symbol("number?"), NumberP);
            dict.Add(new Symbol("string?"), StringP);
            dict.Add(new Symbol("symbol?"), SymbolP);
        }

        public static NativeLambda BooleanP = new NativeLambda
        {
            Parameters = new List<LambdaParameter>() { new LambdaParameter("arg") },
            Body = parameters =>
                {
                    Dictionary<string, ILispValue> boundParams = BindParameters(BooleanP, parameters);
                    return new TiaLisp.Values.Boolean(boundParams["arg"].Type == LispValueType.Boolean);
                }
        };

        public static NativeLambda CharP = new NativeLambda
        {
            Parameters = new List<LambdaParameter>() { new LambdaParameter("arg") },
            Body = parameters =>
                {
                    Dictionary<string, ILispValue> boundParams = BindParameters(CharP, parameters);
                    return new TiaLisp.Values.Boolean(boundParams["arg"].Type == LispValueType.Char);
                }
        };

        public static NativeLambda LambdaP = new NativeLambda
        {
            Parameters = new List<LambdaParameter>() { new LambdaParameter("arg") },
            Body = parameters =>
                {
                    Dictionary<string, ILispValue> boundParams = BindParameters(LambdaP, parameters);
                    return new TiaLisp.Values.Boolean(boundParams["arg"].Type == LispValueType.Lambda);
                }
        };

        public static NativeLambda ListP = new NativeLambda
        {
            Parameters = new List<LambdaParameter>() { new LambdaParameter("arg") },
            Body = parameters =>
                {
                    Dictionary<string, ILispValue> boundParams = BindParameters(ListP, parameters);
                    return new TiaLisp.Values.Boolean(boundParams["arg"].Type == LispValueType.List);
                }
        };

        public static NativeLambda NumberP = new NativeLambda
        {
            Parameters = new List<LambdaParameter>() { new LambdaParameter("arg") },
            Body = parameters =>
                {
                    Dictionary<string, ILispValue> boundParams = BindParameters(NumberP, parameters);
                    return new TiaLisp.Values.Boolean(boundParams["arg"].Type == LispValueType.Number);
                }
        };

        public static NativeLambda StringP = new NativeLambda
        {
            Parameters = new List<LambdaParameter>() { new LambdaParameter("arg") },
            Body = parameters =>
                {
                    Dictionary<string, ILispValue> boundParams = BindParameters(StringP, parameters);
                    return new TiaLisp.Values.Boolean(boundParams["arg"].Type == LispValueType.String);
                }
        };

        public static NativeLambda SymbolP = new NativeLambda
        {
            Parameters = new List<LambdaParameter>() { new LambdaParameter("arg") },
            Body = parameters =>
                {
                    Dictionary<string, ILispValue> boundParams = BindParameters(SymbolP, parameters);
                    return new TiaLisp.Values.Boolean(boundParams["arg"].Type == LispValueType.Symbol);
                }
        };

        #endregion

        #region List manipulation

        private static void GetSymbols_Lists(Dictionary<Symbol, ILispValue> dict)
        {
            dict.Add(new Symbol("car"), Car);
            dict.Add(new Symbol("head"), Car);
            dict.Add(new Symbol("cdr"), Cdr);
            dict.Add(new Symbol("tail"), Cdr);
            dict.Add(new Symbol("cons"), Cons);
            dict.Add(new Symbol("list"), List);
            dict.Add(new Symbol("reverse"), Reverse);
            dict.Add(new Symbol("length"), Length);
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
                    Dictionary<string, ILispValue> boundParams = BindParameters(Cdr, parameters);
                    if (((List)boundParams["list"]).IsEmpty)
                        throw new LispException("cannot take the CDR of an empty list");
                    return ((ConsBox)boundParams["list"]).Tail;
                }
        };

        public static NativeLambda Cons = new NativeLambda
        {
            Parameters = new List<LambdaParameter>() { new LambdaParameter("head"), new LambdaParameter("tail") },
            Body = parameters =>
                {
                    Dictionary<string, ILispValue> boundParams = BindParameters(Cons, parameters);
                    return new ConsBox { Head = boundParams["head"], Tail = boundParams["tail"] };
                }
        };

        public static NativeLambda List = new NativeLambda
        {
            Parameters = new List<LambdaParameter>() { new LambdaParameter("args", LispValueType.Unknown, LambdaParameterType.Rest) },
            Body = parameters =>
                {
                    Dictionary<string, ILispValue> boundParams = BindParameters(Cons, parameters);
                    return boundParams["args"];
                }
        };

        public static NativeLambda Reverse = new NativeLambda
        {
            Parameters = new List<LambdaParameter>() { new LambdaParameter("list", LispValueType.List) },
            Body = parameters =>
                {
                    Dictionary<string, ILispValue> boundParams = BindParameters(Reverse, parameters);
                    IList<ILispValue> items = ((List)boundParams["list"]).CollectProperList();
                    return Lisp.List(items.Reverse().ToArray());
                }
        };

        public static NativeLambda Length = new NativeLambda
        {
            Parameters = new List<LambdaParameter>() { new LambdaParameter("list", LispValueType.List) },
            Body = parameters =>
                {
                    Dictionary<string, ILispValue> boundParams = BindParameters(Length, parameters);
                    return new Integer(((List)boundParams["list"]).CollectProperList().Count);
                }
        };

        #endregion

        #region String functions

        private static void GetSymbols_Strings(Dictionary<Symbol, ILispValue> dict)
        {
            dict.Add(new Symbol("string"), String);
            dict.Add(new Symbol("list->string"), ListToString);
            dict.Add(new Symbol("string->list"), StringToList);
            dict.Add(new Symbol("make-string"), MakeString);
            dict.Add(new Symbol("string-length"), StringLength);
            dict.Add(new Symbol("string-ref"), StringRef);
        }

        public static NativeLambda String = new NativeLambda
        {
            Parameters = new List<LambdaParameter>() { new LambdaParameter("chars", LispValueType.Char, LambdaParameterType.Rest) },
            Body = parameters =>
                {
                    Dictionary<string, ILispValue> boundParams = BindParameters(String, parameters);
                    string s = new string(((List)boundParams["chars"]).CollectProperList().Cast<Character>().Select(c => c.Value).ToArray());
                    return new TiaLisp.Values.String(s);
                }
        };

        public static NativeLambda ListToString = new NativeLambda
        {
            Parameters = new List<LambdaParameter>() { new LambdaParameter("chars", LispValueType.List) },
            Body = parameters =>
                {
                    Dictionary<string, ILispValue> boundParams = BindParameters(ListToString, parameters);
                    IList<ILispValue> chars = ((List)boundParams["chars"]).CollectProperList();
                    foreach (ILispValue c in chars)
                    {
                        if (c.Type != LispValueType.Char)
                            throw new TypeMismatchException(new Symbol("chars"), LispValueType.Char, c.Type);
                    }
                    string s = new string(chars.Cast<Character>().Select(c => c.Value).ToArray());
                    return new TiaLisp.Values.String(s);
                }
        };

        public static NativeLambda StringToList = new NativeLambda
        {
            Parameters = new List<LambdaParameter>() { new LambdaParameter("string", LispValueType.String) },
            Body = parameters =>
                {
                    Dictionary<string, ILispValue> boundParams = BindParameters(StringToList, parameters);
                    string s = ((TiaLisp.Values.String)boundParams["string"]).Value;
                    return Lisp.List(s.Select(c => new Character(c)).ToArray());
                }
        };

        public static NativeLambda MakeString = new NativeLambda
        {
            Parameters = new List<LambdaParameter>() { new LambdaParameter("length", LispValueType.Number), new LambdaParameter("char", LispValueType.Char, LambdaParameterType.Optional) },
            Body = parameters =>
                {
                    Dictionary<string, ILispValue> boundParams = BindParameters(MakeString, parameters);
                    if (!(boundParams["length"] is Integer))
                    {
                        throw new LispException("length must be an integer");
                    }
                    long length = ((Integer)boundParams["length"]).Value;
                    if (length < 0 || length > Int32.MaxValue)
                        throw new LispException("invalid string length: " + length.ToString());
                    char c = ((Character)boundParams["char"]).Value;
                    return new TiaLisp.Values.String(new string(c, (int)length));
                }
        };

        public static NativeLambda StringLength = new NativeLambda
        {
            Parameters = new List<LambdaParameter>() { new LambdaParameter("string", LispValueType.String) },
            Body = parameters =>
                {
                    Dictionary<string, ILispValue> boundParams = BindParameters(StringLength, parameters);
                    string s = ((TiaLisp.Values.String)boundParams["string"]).Value;
                    return new TiaLisp.Values.Integer(s.Length);
                }
        };

        public static NativeLambda StringRef = new NativeLambda
        {
            Parameters = new List<LambdaParameter>() { new LambdaParameter("string", LispValueType.String), new LambdaParameter("index", LispValueType.Number) },
            Body = parameters =>
                {
                    Dictionary<string, ILispValue> boundParams = BindParameters(StringRef, parameters);
                    string s = ((TiaLisp.Values.String)boundParams["string"]).Value;
                    if (!(boundParams["index"] is Integer))
                    {
                        throw new LispException("index must be an integer");
                    }
                    long index = ((Integer)boundParams["index"]).Value;
                    if (index < 0 || index >= s.Length)
                        throw new LispException("invalid index " + index.ToString() + " to string: " + boundParams["string"].ToString());
                    return new Character(s[(int)index]);
                }
        };

        #endregion

        #region Input / output

        private static void GetSymbols_IO(Dictionary<Symbol, ILispValue> dict)
        {
            dict.Add(new Symbol("write-line"), WriteLine);
        }

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

        #endregion
    }
}
