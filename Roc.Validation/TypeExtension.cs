using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roc.Validation
{
    public static class TypeExtension
    {
        /// <summary>
        /// 判断实体属性是否是简单类型
        /// You can't insert or update complex types. Lets filter them out.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsSimpleType(this Type type)
        {
            var underlyingType = Nullable.GetUnderlyingType(type);
            type = underlyingType ?? type;
            var simpleTypes = new List<Type>
                               {
                                   typeof(byte),
                                   typeof(sbyte),
                                   typeof(short),
                                   typeof(ushort),
                                   typeof(int),
                                   typeof(uint),
                                   typeof(long),
                                   typeof(ulong),
                                   typeof(float),
                                   typeof(double),
                                   typeof(decimal),
                                   typeof(bool),
                                   typeof(string),
                                   typeof(char),
                                   typeof(Guid),
                                   typeof(DateTime),
                                   typeof(DateTimeOffset),
                                   typeof(byte[])
                               };
            return simpleTypes.Contains(type) || type.IsEnum;
        }

        public static bool IsNumeric(this Type type)
        {
            var underlyingType = Nullable.GetUnderlyingType(type);
            type = underlyingType ?? type;
            var simpleTypes = new List<Type>
                               {
                                   typeof(byte),
                                   typeof(sbyte),
                                   typeof(short),
                                   typeof(ushort),
                                   typeof(int),
                                   typeof(uint),
                                   typeof(long),
                                   typeof(ulong),
                                   typeof(float),
                                   typeof(double),
                                   typeof(decimal)
                               };
            return simpleTypes.Contains(type);
        }

        /// <summary>
        /// 是否是类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsType<T>(this Type type)
        {
            var underlyingType = Nullable.GetUnderlyingType(type);
            type = underlyingType ?? type;
            return type == typeof(T);
        }
    }
}
