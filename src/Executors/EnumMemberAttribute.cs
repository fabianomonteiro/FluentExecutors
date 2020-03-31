using System;

namespace FluentExecutors.Executors
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
	public class EnumMemberAttribute : Attribute
	{
		public string Text { get; private set; }

		public object Value { get; private set; }

		public object OtherValue { get; private set; }

		public EnumMemberAttribute(string text, object value)
		{
			this.Text = text;
			this.Value = value;
		}

		public EnumMemberAttribute(string text, object value, object otherValue)
		{
			this.Text = text;
			this.Value = value;
			this.OtherValue = otherValue;
		}
	}
}
