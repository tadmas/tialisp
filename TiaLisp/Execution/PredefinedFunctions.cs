using System;
using System.Collections.Generic;
using System.Linq;
using TiaLisp.Values;

namespace TiaLisp.Execution
{
    internal static class PredefinedFunctions
    {
        public static Dictionary<Symbol, ILispValue> GetSymbols()
        {
            Dictionary<Symbol, ILispValue> dict = new Dictionary<Symbol, ILispValue>();

            GetSymbols_Predicates(dict);
            GetSymbols_Lists(dict);
            GetSymbols_Strings(dict);
            GetSymbols_IO(dict);

            return dict;
        }

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
            Body = parameters => new TiaLisp.Values.Boolean(parameters["arg"].Type == LispValueType.Boolean)
        };

        public static NativeLambda CharP = new NativeLambda
        {
            Parameters = new List<LambdaParameter>() { new LambdaParameter("arg") },
            Body = parameters => new TiaLisp.Values.Boolean(parameters["arg"].Type == LispValueType.Char)
        };

        public static NativeLambda LambdaP = new NativeLambda
        {
            Parameters = new List<LambdaParameter>() { new LambdaParameter("arg") },
            Body = parameters => new TiaLisp.Values.Boolean(parameters["arg"].Type == LispValueType.Lambda)
        };

        public static NativeLambda ListP = new NativeLambda
        {
            Parameters = new List<LambdaParameter>() { new LambdaParameter("arg") },
            Body = parameters => new TiaLisp.Values.Boolean(parameters["arg"].Type == LispValueType.List)
        };

        public static NativeLambda NumberP = new NativeLambda
        {
            Parameters = new List<LambdaParameter>() { new LambdaParameter("arg") },
            Body = parameters => new TiaLisp.Values.Boolean(parameters["arg"].Type == LispValueType.Number)
        };

        public static NativeLambda StringP = new NativeLambda
        {
            Parameters = new List<LambdaParameter>() { new LambdaParameter("arg") },
            Body = parameters => new TiaLisp.Values.Boolean(parameters["arg"].Type == LispValueType.String)
        };

        public static NativeLambda SymbolP = new NativeLambda
        {
            Parameters = new List<LambdaParameter>() { new LambdaParameter("arg") },
            Body = parameters => new TiaLisp.Values.Boolean(parameters["arg"].Type == LispValueType.Symbol)
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
            Body = parameters => ((List)parameters["list"]).GetHead()
        };

        public static NativeLambda Cdr = new NativeLambda
        {
            Parameters = new List<LambdaParameter>() { new LambdaParameter("list", LispValueType.List) },
            Body = parameters => ((List)parameters["list"]).GetTail()
        };

        public static NativeLambda Cons = new NativeLambda
        {
            Parameters = new List<LambdaParameter>() { new LambdaParameter("head"), new LambdaParameter("tail") },
            Body = parameters => new ConsBox { Head = parameters["head"], Tail = parameters["tail"] }
        };

        public static NativeLambda List = new NativeLambda
        {
            Parameters = new List<LambdaParameter>() { new LambdaParameter("args", LispValueType.Unknown, LambdaParameterType.Rest) },
            Body = parameters => parameters["args"]
        };

        public static NativeLambda Reverse = new NativeLambda
        {
            Parameters = new List<LambdaParameter>() { new LambdaParameter("list", LispValueType.List) },
            Body = parameters =>
                {
                    IList<ILispValue> items = ((List)parameters["list"]).CollectProperList();
                    return Lisp.List(items.Reverse().ToArray());
                }
        };

        public static NativeLambda Length = new NativeLambda
        {
            Parameters = new List<LambdaParameter>() { new LambdaParameter("list", LispValueType.List) },
            Body = parameters => new Integer(((List)parameters["list"]).CollectProperList().Count)
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
                    string s = new string(((List)parameters["chars"]).CollectProperList().Cast<Character>().Select(c => c.Value).ToArray());
                    return new TiaLisp.Values.String(s);
                }
        };

        public static NativeLambda ListToString = new NativeLambda
        {
            Parameters = new List<LambdaParameter>() { new LambdaParameter("chars", LispValueType.List) },
            Body = parameters =>
                {
                    IList<ILispValue> chars = ((List)parameters["chars"]).CollectProperList();
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
                    string s = ((TiaLisp.Values.String)parameters["string"]).Value;
                    return Lisp.List(s.Select(c => new Character(c)).ToArray());
                }
        };

        public static NativeLambda MakeString = new NativeLambda
        {
            Parameters = new List<LambdaParameter>() { new LambdaParameter("length", LispValueType.Number), new LambdaParameter("char", LispValueType.Char, LambdaParameterType.Optional) },
            Body = parameters =>
                {
                    if (!(parameters["length"] is Integer))
                    {
                        throw new LispException("length must be an integer");
                    }
                    long length = ((Integer)parameters["length"]).Value;
                    if (length < 0 || length > Int32.MaxValue)
                        throw new LispException("invalid string length: " + length.ToString());
                    char c = ((Character)parameters["char"]).Value;
                    return new TiaLisp.Values.String(new string(c, (int)length));
                }
        };

        public static NativeLambda StringLength = new NativeLambda
        {
            Parameters = new List<LambdaParameter>() { new LambdaParameter("string", LispValueType.String) },
            Body = parameters =>
                {
                    string s = ((TiaLisp.Values.String)parameters["string"]).Value;
                    return new TiaLisp.Values.Integer(s.Length);
                }
        };

        public static NativeLambda StringRef = new NativeLambda
        {
            Parameters = new List<LambdaParameter>() { new LambdaParameter("string", LispValueType.String), new LambdaParameter("index", LispValueType.Number) },
            Body = parameters =>
                {
                    string s = ((TiaLisp.Values.String)parameters["string"]).Value;
                    if (!(parameters["index"] is Integer))
                    {
                        throw new LispException("index must be an integer");
                    }
                    long index = ((Integer)parameters["index"]).Value;
                    if (index < 0 || index >= s.Length)
                        throw new LispException("invalid index " + index.ToString() + " to string: " + parameters["string"].ToString());
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
                    Console.WriteLine(((TiaLisp.Values.String)parameters["text"]).Value);
                    return parameters["text"];
                }
        };

        #endregion
    }
}
