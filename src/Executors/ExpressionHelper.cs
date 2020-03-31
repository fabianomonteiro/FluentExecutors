using System;
using System.Linq.Expressions;

namespace FluentExecutors.Executors
{
	public static class ExpressionHelper
	{
		public static string GetBodyMemberName<T>(Expression<Func<T>> memberExpression)
		{
			if (memberExpression == null)
				throw new ArgumentNullException("memberExpression");

			MemberExpression body = (memberExpression.Body.NodeType == ExpressionType.Convert) ? (MemberExpression)((UnaryExpression)memberExpression.Body).Operand : (MemberExpression)memberExpression.Body;
			if (body == null)
				throw new ArgumentException("Lambda must return a property.");

			return body.Member.Name;
		}

		public static string GetBodyMemberName<T, TResult>(Expression<Func<T, TResult>> memberExpression)
		{
			if (memberExpression == null)
				throw new ArgumentNullException("memberExpression");

			MemberExpression body = (memberExpression.Body.NodeType == ExpressionType.Convert) ? (MemberExpression)((UnaryExpression)memberExpression.Body).Operand : (MemberExpression)memberExpression.Body;
			if (body == null)
				throw new ArgumentException("Lambda must return a property.");

			return body.Member.Name;
		}
	}
}
