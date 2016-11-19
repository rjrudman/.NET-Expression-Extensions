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
			Assert.True(comparer.Equals(expected, actual), $"Expressions are not equal. Expected: {expected}, actual: {actual}");
	    }

		[Test]
		public void TestSimple()
		{
			var iParam = Expression.Parameter(typeof(int), "i");

			// Invoke(i => (i + 1), 3)
			var input = Expression.Invoke(Expression.Lambda(Expression.Add(iParam, Expression.Constant(1)), iParam), Expression.Constant(3));

			// (3 + 1)
			var expected = Expression.Add(Expression.Constant(3), Expression.Constant(1));

			var actual = input.InlineInvokes();
			AssertExpressionsEquals(expected, actual);
		}

		[Test]
		public void TestMultiple()
		{
			var i = Expression.Parameter(typeof(int), "i");
			
			//Invoke(ia => (ia + 1), i)
			var ia = Expression.Parameter(typeof(int), "ia");
			var firstInvoke = Expression.Invoke(
				Expression.Lambda(
					Expression.Add(
						ia,
						Expression.Constant(1)
					),
					ia
				),
				i
			);
			
			//Invoke(ib => (ib + 2), i)
			var ib = Expression.Parameter(typeof(int), "ib");
			var secondInvoke = Expression.Invoke(
				Expression.Lambda(
					Expression.Add(
						ib,
						Expression.Constant(2)
					),
					ib
				),
				i
			);

			var lExpr = Expression.Parameter(typeof(int), "lExpr");
			var rExpr = Expression.Parameter(typeof(int), "rExpr");
			// i => Invoke((lExpr, rExpr) => (lExpr * rExpr), Invoke(ia => (ia + 1), i), Invoke(ib => (ib + 2), i))
			var input =
				Expression.Lambda(
					Expression.Invoke(
						Expression.Lambda(
							Expression.Multiply(lExpr, rExpr),
							lExpr, rExpr
						),
						firstInvoke,
						secondInvoke
					),
					i);

			// i => ((i + 1) * (i + 2))
			var expected = Expression.Lambda(
				Expression.Multiply(
					Expression.Add(
						i,
						Expression.Constant(1)
					),
					Expression.Add(
						i,
						Expression.Constant(2)
					)
				), i);

			var actual = input.InlineInvokes();

			AssertExpressionsEquals(expected, actual);
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
