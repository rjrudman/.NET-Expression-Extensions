[![Build Status](https://travis-ci.org/rjrudman/.NET-Expression-Utilities.svg?branch=master)](https://travis-ci.org/rjrudman/.NET-Expression-Utilities)

# InvokeInliner

A tool to parse an expression tree and inline any `InvokeExpression`s it encountered. This is primarily useful when using EntityFramework, as `InvokeExpression`s are explicitly *not supported*. Also may come in useful when writing an expression tree parser, as invocations aren't necessarily part of the logic in the tree.

Find us on [NuGet](https://www.nuget.org/packages/InvokeInliner/1.0.0)

### Example transformations

<pre>
Invoke(i => (i + 1), 3)
Becomes 
(3 + 1)
</pre>

<pre>
i => Invoke((i, j) => (i * j), Invoke(i => (i + 1), i), Invoke(i => (i + 2), i))
Becomes
i => ((i + 1) * (i + 2))
</pre>
<pre>
b => Invoke((d, e) => (d * e), Invoke(b => (50 + Invoke(z => (25 + Invoke(h => (h * 8), z)), b)), b), Invoke(c => (c + 2), b))
Becomes
b => ((50 + (25 + (b * 8))) * (b + 2))  
</pre>

### Example usage:

```cs
Expression<Func<int, int>> f1 = i => i + 1;
Expression<Func<int, int>> f2 = i => i + 2;
Expression<Func<int, int, int>> f3 = (i, j) => i * j;

var p = Expression.Parameter(typeof(int), "i");
var r = Expression
    .Invoke(f3, new[] { 
        Expression.Invoke(f1, p), 
        Expression.Invoke(f2, p) }) 
    .InlineInvokes();

Expression<Func<int, int>> lam = Expression.Lambda<Func<int, int>>(r, p);
```