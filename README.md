# Delegating

[![Build Status][build-badge]][builds]
[![NuGet][nuget-badge]][nuget-pkg]
[![MyGet][myget-badge]][edge-pkgs]

Delegating is a .NET Standard Library that provides delegating implementations
of common interfaces, typically those with a single method only. The actual
implementation is delegated to an `Action` or a function (`Func<>`, `Func<,>`
and so on depending on the arity of the original method).


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
- [`IProgress`][IProgress]
- [`IServiceProvider`][IServiceProvider]


[build-badge]: https://img.shields.io/appveyor/ci/raboof/delegating.svg
[myget-badge]: https://img.shields.io/myget/raboof/v/Delegating.svg?label=myget
[edge-pkgs]: https://www.myget.org/feed/raboof/package/nuget/Delegating
[nuget-badge]: https://img.shields.io/nuget/v/Delegating.svg
[nuget-pkg]: https://www.nuget.org/packages/Delegating
[builds]: https://ci.appveyor.com/project/raboof/delegating
[IComparer]: https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.icomparer-1
[IDisposable]: https://docs.microsoft.com/en-us/dotnet/api/system.idisposable
[IEqualityComparer]: https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.iequalitycomparer-1
[IProgress]: https://docs.microsoft.com/en-us/dotnet/api/system.iprogress-1
[IServiceProvider]: https://docs.microsoft.com/en-us/dotnet/api/system.iserviceprovider
[fsobjexpr]: https://docs.microsoft.com/en-us/dotnet/articles/fsharp/language-reference/object-expressions
[delegate]: https://docs.microsoft.com/en-us/dotnet/api/system.delegate
