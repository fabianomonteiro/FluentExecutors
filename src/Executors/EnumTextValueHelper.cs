using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentExecutors.Executors
{
	public class EnumTextValueHelper
	{
		public static object GetValue(object enumValue)
		{
			var listEnums = GetEnums(enumValue.GetType());

			return listEnums.FirstOrDefault(x => object.Equals(x.EnumValueType, enumValue)).Value;
		}

		public static T GetValue<T>(object enumValue)
		{
			return (T)Convert.ChangeType(GetValue(enumValue), typeof(T));
		}

		public static List<EnumMember> GetEnums(Type type)
		{
			if (type == null)
				throw new ArgumentNullException("type");
			if (!type.IsEnum)
				throw new ArgumentException("The type is not an Enum", "type");

			List<EnumMember> enums = new List<EnumMember>();

			foreach (object enumValueType in Enum.GetValues(type))
			{
				var attributes = type.GetField(enumValueType.ToString()).GetCustomAttributes(typeof(EnumMemberAttribute), false);
				if (attributes == null || attributes.Length == 0)
					throw new InvalidOperationException("Enum must have EnumWrapperKey attribute");

				string text = ((EnumMemberAttribute)attributes[0]).Text;
				object value = ((EnumMemberAttribute)attributes[0]).Value;
				object otherValue = ((EnumMemberAttribute)attributes[0]).OtherValue;

				EnumMember enumWrapper = new EnumMember(text, value, otherValue, enumValueType);
				enums.Add(enumWrapper);
			}

			return enums.OrderBy(x => x.Text).ToList();
		}

		public static EnumMember GetEnumMember(object enumValue)
		{
			var list = GetEnums(enumValue.GetType());

			return list.FirstOrDefault(x => object.Equals(x.EnumValueType, enumValue));
		}

		public static EnumMember GetEnumMember(Type enumType, string text)
		{
			var list = GetEnums(enumType);

			return list.FirstOrDefault(x => x.Text.Equals(text, StringComparison.InvariantCultureIgnoreCase));
		}

		public static EnumMember GetEnumMember(Type enumType, object value)
		{
			var list = GetEnums(enumType);

			return list.FirstOrDefault(x => (x.Value ?? string.Empty).ToString().Equals((value ?? string.Empty).ToString(), StringComparison.InvariantCultureIgnoreCase));
		}

		public static object GetEnumValue(Type type, object textValue)
		{
			var listEnum = GetEnums(type);
			var enumMember = listEnum.FirstOrDefault(x => Object.Equals(x.Text, textValue) || Object.Equals(x.Value, textValue));

			if (enumMember != null)
				return enumMember.EnumValueType;

			return null;
		}

		public static bool IsEnumMember(Type type)
		{
			if (type == null)
				throw new ArgumentNullException("type");

			if (!type.IsEnum)
				return false;

			var enums = new List<EnumMember>();

			foreach (object enumValueType in Enum.GetValues(type))
			{
				var attributes = type.GetField(enumValueType.ToString()).GetCustomAttributes(typeof(EnumMemberAttribute), false);
				if (attributes == null || attributes.Length == 0)
					return false;
			}

			return true;
		}

		public static List<EnumMember> GetEnums(Type type, bool isFirstEmptyItem)
		{
			if (isFirstEmptyItem)
			{
				var list = GetEnums(type);

				list.Insert(0, new EnumMember());

				return list;
			}
			else
			{
				return GetEnums(type);
			}
		}
	}
}