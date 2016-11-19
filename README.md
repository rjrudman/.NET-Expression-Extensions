[![Build Status](https://travis-ci.org/rjrudman/.NET-Expression-Utilities.svg?branch=master)](https://travis-ci.org/rjrudman/.NET-Expression-Utilities)

# InvokeInliner

A tool to parse an expression tree and inline any `InvokeExpression`s it encountered. This is primarily useful when using EntityFramework, as `InvokeExpression`s are explicitly *not supported*. Also may come in useful when writing an expression tree parser, as invocations aren't necessarily part of the logic in the tree.

Find us on [NuGet](https://www.nuget.org/packages/InvokeInliner/1.0.0). See [here](https://github.com/rjrudman/.NET-Expression-Utilities/tree/master/InvokeInliner/InvokeInliner) for more information
