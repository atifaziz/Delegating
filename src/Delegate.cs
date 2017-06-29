#region Copyright (C) 2017 Atif Aziz. All rights reserved.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to
// deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
// sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
// IN THE SOFTWARE.
//
#endregion

namespace Delegating
{
    using System;
    using System.Collections.Generic;

    static partial class Delegate
    {
        public static IDisposable Disposable(Action delegatee) =>
            new DelegatingDisposable(delegatee);

        public static IServiceProvider ServiceProvider(Func<Type, object> delegatee) =>
            new DelegatingServiceProvider(delegatee);

        public static IComparer<T> Comparer<T>(Func<T, T, int> comparer) =>
            new DelegatingComparer<T>(comparer);

        public static IEqualityComparer<T> EqualityComparer<T>(Func<T, T, bool> equals, Func<T, int> getHashCode) =>
            new DelegatingEqualityComparer<T>(equals, getHashCode);

#if PROGRESS
        public static IProgress<T> Progress<T>(Action<T> delegatee) =>
            new DelegatingProgress<T>(delegatee);
#endif

        public static IEnumerable<T> Enumerable<T>(Func<IEnumerator<T>> delegatee) =>
            new DelegatingEnumerable<T>(delegatee);

#if OBSERVABLE

        public static IObservable<T> Observable<T>(Func<IObserver<T>, IDisposable> delegatee) =>
            new DelegatingObservable<T>(delegatee);

        public static IObserver<T> Observer<T>(Action<T> onNext,
                                               Action<Exception> onError = null,
                                               Action onCompleted = null) =>
            new DelegatingObserver<T>(onNext, onError, onCompleted);

#endif
    }
}