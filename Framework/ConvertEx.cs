using System;
using System.ComponentModel;
using System.Globalization;

namespace Framework
{
    public class ConvertEx
    {
        public static T ConvertTo<T>(object value, T d)
        {
            return (T)Convert(value, typeof(T), d);
        }

        public static object Convert(object value, Type destinationType, object d)
        {
            if (ValueRepresentsNull(value))
            {
                return d;
            }

            object result;
            if (TryConvert(value, destinationType, out result))
            {
                return result;
            }

            return d;
        }

        public static bool TryConvert(object value, Type destinationType, out object result)
        {
            if (destinationType == typeof(object))
            {
                result = value;
                return true;
            }

            if (destinationType.IsAssignableFrom(value.GetType()))
            {
                result = value;
                return true;
            }
            Type coreDestinationType = IsGenericNullable(destinationType) ? GetUnderlyingType(destinationType) : destinationType;
            object tmpResult = null;
            if (TryConvertCore(value, coreDestinationType, ref tmpResult))
            {
                result = tmpResult;
                return true;
            }
            result = null;
            return false;
        }

        private static bool TryConvertCore(object value, Type destinationType, ref object result)
        {
            if (value.GetType() == destinationType)
            {
                result = value;
                return true;
            }
            if (TryConvertByDefaultTypeConverters(value, destinationType, ref result))
            {
                return true;
            }
            if (TryConvertByIConvertibleImplementation(value, destinationType, ref result))
            {
                return true;
            }
            if (TryConvertByIntermediateConversion(value, destinationType, ref result))
            {
                return true;
            }
            if (destinationType.IsEnum)
            {
                if (TryConvertToEnum(value, destinationType, ref result))
                {
                    return true;
                }
            }
            return false;
        }

        private static bool TryConvertByDefaultTypeConverters(object value, Type destinationType, ref object result)
        {
            TypeConverter converter = TypeDescriptor.GetConverter(destinationType);
            if (converter != null)
            {
                if (converter.CanConvertFrom(value.GetType()))
                {
                    try
                    {
                        result = converter.ConvertFrom(null, CultureInfo.CurrentCulture, value);
                        return true;
                    }
                    catch
                    {
                    }
                }
            }

            converter = TypeDescriptor.GetConverter(value);
            if (converter != null)
            {
                if (converter.CanConvertTo(destinationType))
                {
                    try
                    {
                        result = converter.ConvertTo(null, CultureInfo.CurrentCulture, value, destinationType);
                        return true;

                    }
                    catch
                    {
                    }
                }
            }
            return false;
        }

        private static bool TryConvertByIConvertibleImplementation(object value, Type destinationType, ref object result)
        {
            CultureInfo formatProvider = CultureInfo.CurrentCulture;
            IConvertible convertible = value as IConvertible;
            if (convertible != null)
            {
                try
                {
                    if (destinationType == typeof(Boolean))
                    {
                        result = convertible.ToBoolean(formatProvider);
                        return true;
                    }
                    if (destinationType == typeof(Byte))
                    {
                        result = convertible.ToByte(formatProvider);
                        return true;
                    }
                    if (destinationType == typeof(Char))
                    {
                        result = convertible.ToChar(formatProvider);
                        return true;
                    }
                    if (destinationType == typeof(DateTime))
                    {
                        result = convertible.ToDateTime(formatProvider);
                        return true;
                    }
                    if (destinationType == typeof(Decimal))
                    {
                        result = convertible.ToDecimal(formatProvider);
                        return true;
                    }
                    if (destinationType == typeof(Double))
                    {
                        result = convertible.ToDouble(formatProvider);
                        return true;
                    }
                    if (destinationType == typeof(Int16))
                    {
                        result = convertible.ToInt16(formatProvider);
                        return true;
                    }
                    if (destinationType == typeof(Int32))
                    {
                        result = convertible.ToInt32(formatProvider);
                        return true;
                    }
                    if (destinationType == typeof(Int64))
                    {
                        result = convertible.ToInt64(formatProvider);
                        return true;
                    }
                    if (destinationType == typeof(SByte))
                    {
                        result = convertible.ToSByte(formatProvider);
                        return true;
                    }
                    if (destinationType == typeof(Single))
                    {
                        result = convertible.ToSingle(formatProvider);
                        return true;
                    }
                    if (destinationType == typeof(UInt16))
                    {
                        result = convertible.ToUInt16(formatProvider);
                        return true;
                    }
                    if (destinationType == typeof(UInt32))
                    {
                        result = convertible.ToUInt32(formatProvider);
                        return true;
                    }
                    if (destinationType == typeof(UInt64))
                    {
                        result = convertible.ToUInt64(formatProvider);
                        return true;
                    }
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }
        
        private static bool TryConvertByIntermediateConversion(object value, Type destinationType, ref object result)
        {
            if (value is char
                && (destinationType == typeof(double) || destinationType == typeof(float)))
            {
                return TryConvertCore(System.Convert.ToInt16(value), destinationType, ref result);
            }
            if ((value is double || value is float) && destinationType == typeof(char))
            {
                return TryConvertCore(System.Convert.ToInt16(value), destinationType, ref result);
            }
            return false;
        }

        private static bool TryConvertToEnum(object value, Type destinationType, ref object result)
        {
            try
            {
                result = Enum.ToObject(destinationType, value);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static bool ValueRepresentsNull(object value)
        {
            return value == null || value == DBNull.Value;
        }
        
        private static bool IsGenericNullable(Type type)
        {
            return type.IsGenericType &&
                   type.GetGenericTypeDefinition() == typeof(Nullable<>).GetGenericTypeDefinition();
        }

        private static Type GetUnderlyingType(Type type)
        {
            return Nullable.GetUnderlyingType(type);
        }        
    }
    
}
