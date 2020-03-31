using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace FluentExecutors.Executors
{
	public class ReflectionHelper
	{
		public static bool IsNullable(Type type)
		{
			if (type == null)
				return false;

			return Nullable.GetUnderlyingType(type) != null;
		}

		public static bool ContainsProperty(Type type, string propertyName, bool includeField = false)
		{
			var property = GetProperty(type, propertyName, false);

			if (property != null)
				return true;

			if (includeField)
			{
				var field = GetField(type, propertyName, false);

				if (field != null)
					return true;
			}

			return false;
		}

		public static FieldInfo GetField(Type type, string fieldName, bool throwNoField = true)
		{
			if (type == null)
				throw new ArgumentException("Type argument must not be null");

			if (string.IsNullOrWhiteSpace(fieldName))
				throw new ArgumentException("FieldName argument must not be null");

			var field = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic).FirstOrDefault(x => x.Name.ToString().Equals(fieldName, StringComparison.InvariantCultureIgnoreCase));

			if (field == null && type.BaseType != null)
				field = GetField(type.BaseType, fieldName, throwNoField);

			if (field == null && throwNoField)
				throw new Exception(string.Format("Type {0} does not contain the field {1}", type.Name, fieldName));

			return field;
		}

		public static PropertyInfo GetProperty(Type type, string propertyName, bool throwNoProperty = true)
		{
			if (type == null)
				throw new ArgumentException("Type argument must not be null");

			if (string.IsNullOrWhiteSpace(propertyName))
				throw new ArgumentException("PropertyName argument must not be null");

			var property = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).FirstOrDefault(x => x.Name.Equals(propertyName, StringComparison.InvariantCultureIgnoreCase));

			if (property == null && throwNoProperty)
				throw new Exception(string.Format("Type {0} does not contain the property {1}", type.Name, propertyName));

			return property;
		}

		public static Type GetPropertyType(Type type, string propertyName)
		{
			var property = GetProperty(type, propertyName, false);

			if (property != null)
				return property.PropertyType;

			return null;
		}

		public static void SetValue<T>(object propertyObject, string propertyName, T value, bool includeField = false, bool throwNoProperty = true, Type baseType = null)
		{
			if (propertyObject == null)
				throw new ArgumentException("PropertyObject argument must not be null");

			if (includeField)
			{
				throwNoProperty = false;
			}

			var type = baseType == null ? propertyObject.GetType() : baseType;
			var property = GetProperty(type, propertyName, throwNoProperty);

			if (includeField && ((property == null) || (property != null && !property.CanWrite)))
			{
				var field = GetField(type, propertyName, throwNoProperty);

				if (field != null)
				{
					field.SetValue(propertyObject, value);
				}
			}
			else
			{
				if (type.IsValueType)
				{
					var delDecltype = Expression.GetDelegateType(new Type[] { type.MakeByRefType(), property.PropertyType, typeof(void) });
					var del = Delegate.CreateDelegate(delDecltype, property.GetSetMethod()) as dynamic;

					del(ref propertyObject, value);
				}
				else
				{
					var isNullable = property.PropertyType.IsGenericType ? property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? true : false : false;
					var nullableType = isNullable ? property.PropertyType.GetGenericArguments()[0] : null;
					var propertyType = !isNullable ? property.PropertyType : nullableType;

					if (EnumTextValueHelper.IsEnumMember(propertyType) && value != null && propertyType != value.GetType())
						property.SetValue(propertyObject, EnumTextValueHelper.GetEnumValue(propertyType, value.ToString()), BindingFlags.Public | BindingFlags.Instance, null, null, null);
					else
					{
						var valueType = value != null ? value.GetType() : null;

						if (valueType != null && valueType != propertyType)
						{
							if (isNullable && (value == null || object.Equals(value, string.Empty)))
								property.SetValue(propertyObject, null, BindingFlags.Public | BindingFlags.Instance, null, null, null);
							else
								property.SetValue(propertyObject, TypeHelper.ChangeType(propertyType, value), BindingFlags.Public | BindingFlags.Instance, null, null, null);
						}
						else
							property.SetValue(propertyObject, value, BindingFlags.Public | BindingFlags.Instance, null, null, null);
					}
				}
			}
		}

		public static void SetValue(object propertyObject, string propertyName, object value, bool includeField = false, bool throwNoProperty = true, Type baseType = null)
		{
			SetValue<object>(propertyObject, propertyName, value, includeField, throwNoProperty, baseType);
		}

		public static object GetValue(object propertyObject, string propertyName, bool includeField = false, bool throwNoProperty = true)
		{
			if (propertyObject == null)
				throw new ArgumentException("PropertyObject argument must not be null");

			var property = default(PropertyInfo);

			if (propertyObject is IDictionary)
			{
				var dictionary = propertyObject as IDictionary;
				var key = dictionary.Keys.OfType<object>().FirstOrDefault(x => (x ?? string.Empty).ToString().Equals(propertyName, StringComparison.InvariantCultureIgnoreCase));

				if (key != null && !string.IsNullOrWhiteSpace(key.ToString()))
					return dictionary[key];
			}
			else
			{
				if (includeField)
					property = GetProperty(propertyObject.GetType(), propertyName, false);
				else
					property = GetProperty(propertyObject.GetType(), propertyName, throwNoProperty);

				if (property != null)
					return property.GetValue(propertyObject, BindingFlags.Public | BindingFlags.Instance, null, null, null);

				var field = GetField(propertyObject.GetType(), propertyName, throwNoProperty);

				if (field != null)
					return field.GetValue(propertyObject);
			}

			return null;
		}

		public static T GetValue<T>(object propertyObject, string propertyName, bool includeField = false, bool throwNoProperty = true)
		{
			var value = GetValue(propertyObject, propertyName, includeField, throwNoProperty);

			if (value is T)
				return (T)value;

			var changedValue = TypeHelper.ChangeType(typeof(T), value);

			if (changedValue != null)
				return (T)changedValue;

			return default(T);
		}

		public static IEnumerable<Type> FindDerivedTypes(Assembly assembly, Type baseType)
		{
			return assembly.GetTypes().Where(t => t != baseType && baseType.IsAssignableFrom(t) && !t.IsAbstract);
		}

		public static IEnumerable<Type> FindDerivedTypes(Type baseType)
		{
			return Assembly.GetAssembly(baseType).GetTypes().Where(t => t != baseType && baseType.IsAssignableFrom(t) && !t.IsAbstract);
		}

		public static IEnumerable<Type> FindDerivedTypes<T>()
		{
			Type baseType = typeof(T);

			return Assembly.GetAssembly(baseType).GetTypes().Where(t => t != baseType && baseType.IsAssignableFrom(t) && !t.IsAbstract);
		}

		public static object GetDefaultValue(Type type)
		{
			object defaultValue = null;

			if (type.IsValueType)
			{
				defaultValue = Activator.CreateInstance(type);
			}

			return defaultValue;
		}
	}
}
