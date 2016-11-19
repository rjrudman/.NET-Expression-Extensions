/* Copyright (C) 2007 - 2008  Versant Inc.  http://www.db4o.com */

using System.Linq.Expressions;

namespace ExpressionComparer
{
	public class ExpressionEqualityComparer
	{
		public void AssertEqual(Expression a, Expression b)
		{
			var comparer = new ExpressionComparison(b);
			comparer.AssertEqual(a);		
		}
	}
}