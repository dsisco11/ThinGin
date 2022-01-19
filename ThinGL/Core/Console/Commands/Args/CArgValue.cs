using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Numerics;
using System.Runtime.CompilerServices;

using ThinGin.Core.Common.Objects;
using ThinGin.Core.Common.Types;
using ThinGin.Core.Console.Commands.Selectors;
using ThinGin.Core.Console.Commands.Selectors.Expressions;
using ThinGin.Core.Console.Parsing;

namespace ThinGin.Core.Console
{
    /// <summary>
    /// Implements a console command argument which is defined as text but can be resolved to a concrete usable value
    /// </summary>
    public class CArgValue
    {
        #region Instances
        public static CArgValue Null = new CArgValue(ECArgType.NULL);
        public static CArgValue Auto = new CArgValue(ECArgType.AUTO);
        #endregion

        #region Values
        private readonly object Value;
        #endregion

        #region Properties
        public readonly ECArgType Type;
        public readonly System.Type ValueType;
        #endregion

        #region Accessors
        /// <summary>
        /// Returns whether the value is 'Auto'
        /// </summary>
        public bool IsAuto => (Type == ECArgType.AUTO);

        /// <summary>
        /// Returns whether the value is a definite Number or Integer
        /// </summary>
        public bool IsDefinite => (0 != (Type & (ECArgType.INTEGER | ECArgType.UNSIGNED | ECArgType.NUMBER)));

        /// <summary>
        /// Returns whether the value is a collection of sub-values
        /// </summary>
        public bool IsCollection => (Type == ECArgType.COLLECTION);

        /// <summary>
        /// Returns whether the value type is <see cref="ECArgType.NULL"/>
        /// </summary>
        public bool IsNull => (Type == ECArgType.NULL);

        /// <summary>
        /// Returns whether there is actually a set value
        /// </summary>
        public bool HasValue
        {
            get
            {
                if (Type == ECArgType.NULL)
                    return false;

                if (Value is null)
                    return false;

                return true;
            }
        }
        #endregion

        #region Constructors
        public CArgValue(ECArgType type)
        {
            Type = type;
        }

        public CArgValue(ECArgType type, object value)
        {
            Type = type;
            Value = value;
        }

        public CArgValue(CSelectorToken token)
        {
            if (token.Values.Count <= 0)
            {
                Type = ECArgType.NULL;
                Value = null;
                return;
            }
            else
            {
                Type = ECArgType.SELECTOR;
                Value = Selector.From(token);
            }
        }

        public CArgValue(CFunction token)
        {
            if (token is null)
            {
                Type = ECArgType.NULL;
                Value = null;
                return;
            }
            else
            {
                Type = ECArgType.SELECTOR;
                var parser = new ExpressionParser(new[] { token });
                Value = new Selector(parser.Parse());
            }
        }
        #endregion

        #region Converters
        /// <summary>
        /// Returns the value as the specified enum type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T AsEnum<T>() where T : struct, IConvertible => (T)Value;

        /// <summary>
        /// Returns the value as a Color4 if possible, or NULL if not possible.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3 AsPosition()
        {
            if (Type != ECArgType.POSITION) throw new Exception($"{nameof(CArgValue)} is not a Position! {this}");
            Contract.EndContractBlock();

            return (Vector3)Value;
        }

        /// <summary>
        /// Returns the value as a Rgba struct
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Rgba AsColor()
        {
            if (Type != ECArgType.COLOR) throw new Exception($"{nameof(CArgValue)} is not a Color! {this}");
            Contract.EndContractBlock();

            return new Rgba((uint)Value);
        }

        /// <summary>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlyCollection<CArgValue> AsCollection()
        {
            if (!IsCollection) throw new Exception($"{nameof(CArgValue)} is not a collection! {this}");
            Contract.EndContractBlock();

            return new ReadOnlyCollection<CArgValue>((CArgValue[])Value);
        }

        /// <summary>
        /// Returns the value as the preferred Integer type
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Int64 AsInteger() => (Int64)Value;

        /// <summary>
        /// Returns the value as the preferred (Nullable) Integer type
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Int64? AsIntegerN() => !HasValue ? null : (Int64?)Value;

        /// <summary>
        /// Returns the value as the preferred Integer type
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public UInt64 AsUnsigned() => (UInt64)Value;

        /// <summary>
        /// Returns the value as the preferred (Nullable) Integer type
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public UInt64? AsUnsignedN() => !HasValue ? null : (UInt64?)Value;

        /// <summary>
        /// Returns the value as the preferred Decimal type
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double AsDecimal() => (double)Value;

        /// <summary>
        /// Returns the value as the preferred (Nullable) Decimal type
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double? AsDecimalN() => !HasValue ? null : (double?)Value;

        /// <summary>
        /// Returns the value as a string
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string AsString() => (string)Value;

        /// <summary>
        /// Returns the value as a selector
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Selector AsSelector() => Value as Selector;
        #endregion



        #region Resolution

        /// <summary>
        /// Resolves the value to a decimal and returns it if possible, returns NULL otherwise
        /// </summary>
        public object Resolve(ConsoleContext Context, object defaultValue)
        {
            return Type switch
            {
                ECArgType.NULL => null,
                ECArgType.AUTO => defaultValue,
                ECArgType.KEYWORD => Value,
                ECArgType.STRING => Value,
                ECArgType.INTEGER => Value,
                ECArgType.UNSIGNED => Value,
                ECArgType.NUMBER => Value,
                ECArgType.PERCENT => null,
                ECArgType.COLOR => AsColor(),
                ECArgType.POSITION => Value,
                ECArgType.OBJECT_ID => Value,
                ECArgType.COLLECTION => Value,
                ECArgType.SELECTOR => AsSelector().Execute(Context),
                _ => throw new NotImplementedException()
            };
        }

        /// <summary>
        /// Resolves the value to a decimal and returns it if possible, returns NULL otherwise
        /// <para>Additionally takes an multiplier as input for resolving the value to a decimal if it's a percentage type</para>
        /// </summary>
        public object Resolve(ConsoleContext Context, double percentageMultiplier, object defaultValue)
        {
            switch (Type)
            {
                case ECArgType.PERCENT:
                    return ((AsDecimal() / 100.0) * percentageMultiplier);
                default:
                    return Resolve(Context, defaultValue);
            }
        }

        #endregion

        #region Utility
        public bool Is_Match(Type vType)
        {
            switch (this.Type)
            {
                case ECArgType.NULL:
                    return false;

                case ECArgType.AUTO:
                    return true;

                case ECArgType.COLOR:
                    return vType == typeof(Rgba);

                case ECArgType.POSITION:
                    return vType == typeof(Vector3);
                case ECArgType.OBJECT_ID:
                    return vType == typeof(ObjectID);

                case ECArgType.STRING:
                    return vType == typeof(string);

                case ECArgType.KEYWORD:
                    return vType.IsEnum;

                case ECArgType.COLLECTION:
                    return vType.IsArray;

                case ECArgType.INTEGER:
                    return vType.IsValueType;
                case ECArgType.UNSIGNED:
                    return vType.IsValueType;
                case ECArgType.NUMBER:
                    return vType.IsValueType;
                case ECArgType.PERCENT:
                    return vType.IsValueType;

                case ECArgType.SELECTOR:
                    return vType == (this.Value as Selector).Get_Return_Type();

                default:
                    throw new NotImplementedException();
            }
        }
        #endregion


        #region Operators
        public static bool operator ==(CArgValue A, CArgValue B)
        {
            // If either object is null return whether they are BOTH null
            if (A is null || B is null)
                return (A is null && B is null);

            if (A.Type != B.Type) return false;

            switch (A.Type)
            {
                case ECArgType.NULL:
                case ECArgType.AUTO:
                    return true;
                case ECArgType.COLOR:
                    return EqualityComparer<uint>.Default.Equals((uint)A.Value, (uint)B.Value);
                case ECArgType.POSITION:
                    return EqualityComparer<Vector3>.Default.Equals((Vector3)A.Value, (Vector3)B.Value);
                case ECArgType.INTEGER:
                    return EqualityComparer<Int64>.Default.Equals((Int64)A.Value, (Int64)B.Value);
                case ECArgType.UNSIGNED:
                case ECArgType.OBJECT_ID:
                    return EqualityComparer<UInt64>.Default.Equals((UInt64)A.Value, (UInt64)B.Value);
                case ECArgType.NUMBER:
                case ECArgType.PERCENT:
                    return EqualityComparer<double>.Default.Equals((double)A.Value, (double)B.Value);
                case ECArgType.STRING:
                    return EqualityComparer<string>.Default.Equals((string)A.Value, (string)B.Value);
                case ECArgType.KEYWORD:
                    return A.Value.Equals(B.Value);
                default:
                    throw new NotImplementedException($"Equality comparison logic not implemented for type: {Enum.GetName(typeof(ECArgType), A.Type)}");
            }
        }

        public static bool operator !=(CArgValue A, CArgValue B)
        {
            return !(A == B);
        }


        public override bool Equals(object o)
        {
            if (o is null)
                return false;

            if (o is CArgValue cssVal)
            {
                return this == cssVal;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        #endregion
    }
}
