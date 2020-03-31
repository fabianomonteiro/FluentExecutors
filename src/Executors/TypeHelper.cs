using System;
using System.Globalization;
using System.Linq;

namespace FluentExecutors.Executors
{
	public static class TypeHelper
	{
		public static Type GetNullableType(Type type)
		{
			if (type == null) throw new ArgumentNullException("The argument type can not be null");

			// Use Nullable.GetUnderlyingType() to remove the Nullable<T> wrapper if type is already nullable.
			var internalType = Nullable.GetUnderlyingType(type);

			if (internalType != null)
				return typeof(Nullable<>).MakeGenericType(internalType);
			else if (type.IsValueType)
				return typeof(Nullable<>).MakeGenericType(type);
			else
				return type;
		}

		public static T ChangeType<T>(object value)
		{
			return (T)ChangeType(typeof(T), value);
		}

		public static object ChangeType(Type type, object value)
		{
			if (type.IsEnum)
			{
				if (EnumTextValueHelper.IsEnumMember(type))
				{
					var listEnum = EnumTextValueHelper.GetEnums(type);

					if (listEnum != null)
					{
						var enumMember = listEnum.FirstOrDefault(x => x.Value != null && object.Equals(x.Value, value));

						if (enumMember != null)
							return enumMember.EnumValueType;
					}
				}
				else if (value is string)
				{
					object returnValue = null;

					returnValue = Enum.Parse(type, value as string);

					return returnValue;
				}
				else
					return Enum.ToObject(type, value);
			}
			else if (!type.IsInterface && type.IsGenericType)
			{
				Type innerType = type.GetGenericArguments()[0];
				var innerValue = default(object);

				if (type.GetGenericTypeDefinition() == typeof(Nullable<>) && (value == null || value.ToString() == ""))
				{
					innerValue = null;

					return Activator.CreateInstance(typeof(Nullable<>).MakeGenericType(innerType), new[] { innerValue });
				}
				else
				{
					if (innerType.IsEnum)
					{
						if (EnumTextValueHelper.IsEnumMember(innerType))
						{
							var listEnum = EnumTextValueHelper.GetEnums(innerType);

							if (listEnum != null)
							{
								var enumMember = listEnum.FirstOrDefault(x => x.Value != null && object.Equals(x.Value, value));

								if (enumMember != null)
									return enumMember.EnumValueType;
							}
						}
						else if (value is string)
							return Enum.Parse(innerType, value as string);
						else
							return Enum.ToObject(innerType, value);
					}
					else
						innerValue = Convert.ChangeType(value, innerType);
				}

				return Activator.CreateInstance(type, new object[] { innerValue });
			}
			else if (type == typeof(bool) && !(value is bool))
			{
				if (value == null)
					return null;
				else if (value.Equals("1"))
					return true;
				else if (value.Equals("0"))
					return false;
				else if (value.Equals(true))
					return true;
				else if (value.Equals(false))
					return false;
				else
					throw new Exception($"Value not correponding a type bool ({value})");
			}
			else if (type == typeof(double))
			{
				if (value == null)
					return null;
				else if (value.ToString().Contains("."))
				{
					return Convert.ToDouble(value, new CultureInfo("en-US"));
				}
				else
				{
					return System.Convert.ChangeType(value, type);
				}
			}

			if (value is System.Drawing.Bitmap && type.Equals(typeof(System.Drawing.Image)))
				return value;

			if (value is IConvertible)
				value = System.Convert.ChangeType(value, type);

			if (type.Equals(typeof(DBNull)) && value == null)
				return DBNull.Value;

			var defaultValue = ReflectionHelper.GetDefaultValue(type);
			if (value == null && value != defaultValue)
				return defaultValue;

			return value;
		}
	}
}
