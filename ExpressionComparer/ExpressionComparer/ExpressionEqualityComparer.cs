/* Copyright (C) 2007 - 2008  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ExpressionComparer
{
	public class ExpressionEqualityComparer : IEqualityComparer<Expression>
	{
		public static ExpressionEqualityComparer Instance = new ExpressionEqualityComparer();

		public bool Equals(Expression a, Expression b)
		{
			var comparer = new ExpressionComparison(b);
			comparer.Compare(a);
			return comparer.AreEqual;
		}

		public int GetHashCode(Expression expression)
		{
			throw new NotImplementedException();
		}
	}
}