using System;
using System.Linq.Expressions;
using ExpressionComparer;
using InvokeInliner;
using NUnit.Framework;

namespace Tests
{
	[TestFixture]
    public class General
	{
		private static int TestFunction() => 5;

		// ReSharper disable UnusedParameter.Local
	    private static void AssertExpressionsEquals(Expression expected, Expression actual)
		// ReSharper enable UnusedParameter.Local
		{
			var comparer = ExpressionEqualityComparer.Instance;
			Assert.True(comparer.Equals(expected, actual), "Expressions are not equal");
	    }

	    [Test]
	    public void TestConstantFunction()
	    {
			var expected = Expression.Invoke(Expression.Constant((Func<int>)TestFunction));
			var actual = Expression.Invoke(Expression.Constant((Func<int>)TestFunction)).InlineInvokes();
		    
		    AssertExpressionsEquals(expected, actual);
	    }

		[Test]
		public void TestInnerInvokeReferencingAncestorParameter()
		{
			var expected = Expression.Add(Expression.Constant(1), Expression.Constant(2));

			var iParam = Expression.Parameter(typeof(int));
			var lParam = Expression.Parameter(typeof(int));

			var actual = Expression.Invoke(
				Expression.Lambda(
					Expression.Invoke(
							Expression.Lambda(
								Expression.Add(iParam, lParam),
								iParam),
							Expression.Constant(1)
					),
					lParam),
				Expression.Constant(2)
			).InlineInvokes();

			AssertExpressionsEquals(expected, actual);
		}

		[Test]
		public void TestInnerInvokeOverridingAncestorParameter()
		{
			var expected = Expression.Add(Expression.Constant(1), Expression.Constant(3));

			var iParam = Expression.Parameter(typeof(int));
			var lParam = Expression.Parameter(typeof(int));

			var actual = Expression.Invoke(
				Expression.Lambda(
					Expression.Invoke(
						Expression.Lambda(
							Expression.Add(iParam, lParam),
							iParam, lParam),
						Expression.Constant(1), Expression.Constant(3)
					),
					lParam),
				Expression.Constant(2)
			).InlineInvokes();

			AssertExpressionsEquals(expected, actual);
		}
	}
}
