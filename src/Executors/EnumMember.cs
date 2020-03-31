using System;
using System.Diagnostics;

namespace FluentExecutors.Executors
{
	[DebuggerDisplay("Text = {Text}, Value = {Value}, EnumValue= {EnumValue}")]
	public sealed class EnumMember
	{
		public string Text { get; private set; }

		public object Value { get; private set; }

		public object OtherValue { get; private set; }

		public object EnumValueType { get; private set; }

		public EnumMember() { }

		public EnumMember(string text, object value, object otherValue, object enumValueType)
		{
			if (!(enumValueType is Enum))
				throw new InvalidCastException(value.ToString() + " is not a Enum");

			this.Text = text;
			this.Value = value;
			this.OtherValue = otherValue;
			this.EnumValueType = enumValueType;
		}
	}
}
