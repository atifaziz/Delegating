# Delegating

[![Build Status][build-badge]][builds]
[![NuGet][nuget-badge]][nuget-pkg]
[![MyGet][myget-badge]][edge-pkgs]

Delegating is a .NET Standard Library that provides delegating implementations
of common interfaces, typically those with a single method only. The actual
implementation is delegated to an `Action` or a function (`Func<>`, `Func<,>`
and so on depending on the arity of the original method).

With version 1.2, Delegating also targets .NET Framework versions 3.5 and up.


## Motivation

The .NET Framework has many interfaces with a single method, like
[`IDisposable`][IDisposable], that require the same and tedious boilerplate
class declaration and code each time. With closures and lambdas, such
interfaces can be implemented once and which delegate the actual
implementation to, well (surprise, surprise), [a delegate][delegate]! When
such interfaces need to be supplied, an implementation can be expressed more
succinctly.

Yes, this will be somewhat useless the day C# gains [object expressions like
F#][fsobjexpr].


## Usage

Try the following example for [`IDisposable`][IDisposable] in a C# Interactive
(`csi`) session (assuming `Delegating.dll` is in the current directory):

```c#
#r "Delegating.dll"
using static Delegating.Delegate;
var disposable = Disposable(() => Console.WriteLine("Disposed!"));
disposable.Dispose();
disposable.Dispose(); // Try again!
```

The session should look like:

```
Microsoft (R) Visual C# Interactive Compiler version 2.1.0.61520
Copyright (C) Microsoft Corporation. All rights reserved.

Type "#help" for more information.
> #r "Delegating.dll"
> using static Delegating.Delegate;
> var disposable = Disposable(() => Console.WriteLine("Disposed!"));
> disposable.Dispose();
Disposed!
> disposable.Dispose(); // Disposing more than once is ineffective
>
```


## Implementations

Delegated implementations are available for the following interfaces:

- [`IComparer<T>`][IComparer]
- [`IDisposable`][IDisposable]
- [`IEqualityComparer<T>`][IEqualityComparer]
- [`IEnumerable<T>`][IEnumerable]
- [`IObservable<T>`][IObservable] (.NET Framework 4.0 and above)
- [`IObserver<T>`][IObserver] (.NET Framework 4.0 and above)
- [`IProgress`][IProgress] (.NET Framework 4.5 and above)
- [`IServiceProvider`][IServiceProvider]

The delegated implementation for an `IEnumerable<T>` becomes particularly
interesting when combined with [local functions introduced in
C# 7][cs-local-funcs]. It enables you to write in-line, ad-hoc and anonymous
iterators, e.g.:

```c#
var singleDigitEvens = Enumerable(() =>
{
    return _(); IEnumerator<int> _()
    {
        for (var x = 0; ; x < 10; x += 2)
            yield return x;
    }
});
```

Below is a more involved example that does several computations (runnning
aggregations) in a single iteration, all as part of a single LINQ
comprehension:

```c#
var q = 
    from x in Enumerable.Range(1, 20)
    group x by x % 5 == 0 into g
    select new
    {
        g.Key,
        Run =
            from x in Delegating.Delegate.Enumerable(() =>
            {
                return _(); IEnumerator<(int Num, int Min, int Max, int Count, int Sum)> _()
                {
                    var min = int.MaxValue;
                    var max = int.MinValue;
                    var sum = 0;
                    var count = 0;
                    using (var e = g.GetEnumerator())
                    while (e.MoveNext())
                    {
                        var x = e.Current;
                        if (x < min) min = e.Current;
                        if (x > max) max = e.Current;
                        sum += e.Current;
                        yield return (x, min, max, ++count, sum);
                    }
                }
            })
            select new
            {
                x.Num, x.Min, x.Max, x.Count, x.Sum,
                Average = (double) x.Sum / x.Count
            }
    };
```

The result will be two groups, where the first group will have a key of
`false` (numbers not divisible by 5) and the following sequnce as its run:

| Num | Min | Max | Count | Sum |   Average   | 
|----:|----:|----:|------:|----:|------------:| 
|   1 |   1 |   1 |     1 |   1 |           1 | 
|   2 |   1 |   2 |     2 |   3 |         1.5 | 
|   3 |   1 |   3 |     3 |   6 |           2 | 
|   4 |   1 |   4 |     4 |  10 |         2.5 | 
|   6 |   1 |   6 |     5 |  16 |         3.2 | 
|   7 |   1 |   7 |     6 |  23 | 3.833333333 | 
|   8 |   1 |   8 |     7 |  31 | 4.428571429 | 
|   9 |   1 |   9 |     8 |  40 |           5 | 
|  11 |   1 |  11 |     9 |  51 | 5.666666667 | 
|  12 |   1 |  12 |    10 |  63 |         6.3 | 
|  13 |   1 |  13 |    11 |  76 | 6.909090909 | 
|  14 |   1 |  14 |    12 |  90 |         7.5 | 
|  16 |   1 |  16 |    13 | 106 | 8.153846154 | 
|  17 |   1 |  17 |    14 | 123 | 8.785714286 | 
|  18 |   1 |  18 |    15 | 141 |         9.4 | 
|  19 |   1 |  19 |    16 | 160 |          10 | 

The second group will have a key of `true` (numbers divisible by 5) and the
following sequnce as its run:

| Num | Min | Max | Count | Sum | Average | 
|----:|----:|----:|------:|----:|--------:| 
|   5 |   5 |  5  |     1 |   5 |       5 | 
|  10 |   5 | 10  |     2 |  15 |     7.5 | 
|  15 |   5 | 15  |     3 |  30 |      10 | 
|  20 |   5 | 20  |     4 |  50 |    12.5 | 


## Conflicts

If you statically import `Delegating.Delegate` members via
`using static Delegating.Delegate` then chances are that you will run into
some conflicts with `System.Linq`, which is a commonly imported namespace in
many source files and projects. The conflict specifically occurs between the
method `Enumerable` on `Delegating.Delegate` and the class
[`Enumerable`][Enumerable] from `System.Linq`. The workaround is to simply
import `System.Linq.Enumerable` explicitly. The following C# Interactive
session demonstrates the conflict and the workaround:

```
Microsoft (R) Visual C# Interactive Compiler version 2.2.0.61624
Copyright (C) Microsoft Corporation. All rights reserved.

Type "#help" for more information.
> #r "Delegating.dll"
> using static Delegating.Delegate;
> using System.Linq;
> Enumerable(() => { return _(); IEnumerator<double> _() { yield return Math.PI; } })
DelegatingEnumerable<double> { 3.1415926535897931 }
> // The following won't compile due to a conflict
> Enumerable.Range(1, 10)
(1,1): error CS0119: 'Delegate.Enumerable<T>(Func<IEnumerator<T>>)' is a method, which is not valid in the given context
> // The workaround is to explicitly import Enumerable from System.Linq
> using Enumerable = System.Linq.Enumerable;
> // Now the following compiles just fine
> Enumerable.Range(1, 10)
RangeIterator { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }
> // The Enumerable method from Delegating.Delegate continues to be available:
> Enumerable(() => { return _(); IEnumerator<double> _() { yield return Math.PI; } })
DelegatingEnumerable<double> { 3.1415926535897931 }
```

For more information and background, see [issue #2][#2].


## Building

Building the solution requires MSBuild 15. On a Windows host, run `build.cmd`
to build the solution and `pack.cmd` to build and pack.

On a non-Windows host with .NET Core SDK 2.1.400+ installed, use the
`dotnet` CLI for building and testing:

    $ dotnet build -c Debug -f netstandard2.0 src
    $ dotnet build -c Release -f netstandard2.0 src
    $ dotnet test tests


[build-badge]: https://img.shields.io/appveyor/ci/raboof/delegating.svg
[myget-badge]: https://img.shields.io/myget/raboof/v/Delegating.svg?label=myget
[edge-pkgs]: https://www.myget.org/feed/raboof/package/nuget/Delegating
[nuget-badge]: https://img.shields.io/nuget/v/Delegating.svg
[nuget-pkg]: https://www.nuget.org/packages/Delegating
[builds]: https://ci.appveyor.com/project/raboof/delegating
[IComparer]: https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.icomparer-1
[IDisposable]: https://docs.microsoft.com/en-us/dotnet/api/system.idisposable
[IEnumerable]: https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1
[IEqualityComparer]: https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.iequalitycomparer-1
[IObservable]: https://docs.microsoft.com/en-us/dotnet/api/system.iobservable-1
[IObserver]: https://docs.microsoft.com/en-us/dotnet/api/system.iobserver-1
[IProgress]: https://docs.microsoft.com/en-us/dotnet/api/system.iprogress-1
[IServiceProvider]: https://docs.microsoft.com/en-us/dotnet/api/system.iserviceprovider
[fsobjexpr]: https://docs.microsoft.com/en-us/dotnet/articles/fsharp/language-reference/object-expressions
[delegate]: https://docs.microsoft.com/en-us/dotnet/api/system.delegate
[cs-local-funcs]: https://docs.microsoft.com/en-us/dotnet/articles/csharp/whats-new/csharp-7#local-functions
[Enumerable]: https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable
[#2]: https://github.com/atifaziz/Delegating/issues/2
