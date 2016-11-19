using System.Linq.Expressions;

namespace InvokeInliner
{
    public static class InvokeInlinerExtensions
    {
		public static TExpressionType InlineInvokes<TExpressionType>(this TExpressionType expression)
			where TExpressionType : Expression
		{
			return (TExpressionType)new InvokeInliner().Inline(expression);
		}

		public static Expression InlineInvokes(this InvocationExpression expression)
		{
			return new InvokeInliner().Inline(expression);
		}
	}
}
