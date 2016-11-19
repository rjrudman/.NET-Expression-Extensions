[![Build Status](https://travis-ci.org/rjrudman/.NET-Expression-Utilities.svg?branch=master)](https://travis-ci.org/rjrudman/.NET-Expression-Utilities)

# InvokeInliner

A tool to parse an expression tree and inline any `InvokeExpression`s it encountered. This is primarily useful when using EntityFramework, as `InvokeExpression`s are explicitly *not supported*. Also may come in useful when writing an expression tree parser, as invocations aren't necessarily part of the logic in the tree.


### Examples

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