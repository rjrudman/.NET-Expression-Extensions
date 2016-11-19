using System.Linq.Expressions;

namespace InvokeInliner
{
    public static class InvokeInlinerExtensions
    {
		public static TExpressionType InlineInvokes<TExpressionType>(this TExpressionType expression)
			where TExpressionType : Expression
		{
			return (TExpressionType)new InvokeInlinerVisitor().Inline(expression);
		}

		public static Expression InlineInvokes(this InvocationExpression expression)
		{
			return new InvokeInlinerVisitor().Inline(expression);
		}
	}
}
