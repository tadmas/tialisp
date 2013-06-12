using System;
using System.Collections.Generic;
using TiaLisp.Environment;

namespace TiaLisp.Values
{
    public interface ILispLambda
    {
        IList<LambdaParameter> Parameters { get; }
        ILispEnvironment Environment { get; }
        bool IsNative { get; }
        ILispValue Execute(Dictionary<string, ILispValue> parameters);
    }

    public sealed class Lambda : ILispValue, ILispLambda
    {
        private List<LambdaParameter> _Parameters;
        public IList<LambdaParameter> Parameters
        {
            get
            {
                if (_Parameters == null)
                    _Parameters = new List<LambdaParameter>();

                return _Parameters;
            }
        }

        public ILispValue Body { get; internal set; }
        public ILispEnvironment Environment { get; internal set; }

        public LispValueType Type
        {
            get { return LispValueType.Lambda; }
        }

        bool ILispLambda.IsNative
        {
            get { return false; }
        }

        bool IEquatable<ILispValue>.Equals(ILispValue other)
        {
            // TODO: implement
            return object.ReferenceEquals(this, other);
        }

        public ILispValue Execute(Dictionary<string, ILispValue> parameters)
        {
            throw new NotImplementedException();
        }
    }

    public sealed class NativeLambda : ILispValue, ILispLambda
    {
        private IList<LambdaParameter> _Parameters;
        public IList<LambdaParameter> Parameters
        {
            get
            {
                if (_Parameters == null)
                    _Parameters = new List<LambdaParameter>();

                return _Parameters;
            }
            internal set
            {
                _Parameters = value;
            }
        }

        public Func<Dictionary<string, ILispValue>, ILispValue> Body { get; internal set; }
        public ILispEnvironment Environment { get; internal set; }

        public LispValueType Type
        {
            get { return LispValueType.Lambda; }
        }

        bool ILispLambda.IsNative
        {
            get { return true; }
        }

        bool IEquatable<ILispValue>.Equals(ILispValue other)
        {
            return object.ReferenceEquals(this, other);
        }

        public ILispValue Execute(Dictionary<string, ILispValue> parameters)
        {
            return Body(parameters);
        }
    }

    public enum LambdaParameterType
    {
        Normal = 0,
        Optional,
        Rest,
    }

    public struct LambdaParameter
    {
        private readonly Symbol _Name;
        private readonly LispValueType _ValueType;
        private readonly LambdaParameterType _ParameterType;

        public LambdaParameter(Symbol name, LispValueType valueType = LispValueType.Unknown, LambdaParameterType parameterType = LambdaParameterType.Normal)
        {
            this._Name = name;
            this._ValueType = valueType;
            this._ParameterType = parameterType;
        }

        public LambdaParameter(string name, LispValueType valueType = LispValueType.Unknown, LambdaParameterType parameterType = LambdaParameterType.Normal)
        {
            this._Name = new Symbol(name);
            this._ValueType = valueType;
            this._ParameterType = parameterType;
        }

        public Symbol Name
        {
            get { return _Name; }
        }

        public LispValueType ValueType
        {
            get { return _ValueType; }
        }

        public LambdaParameterType ParameterType
        {
            get { return _ParameterType; }
        }
    }
}
