namespace Delegating.Tests
{
    using System;

    static class NotImplementedFunc
    {
        public static Func<T, TResult> Of<T, TResult>() =>
            t => throw new NotImplementedException();

        public static Func<T1, T2, TResult> Of<T1, T2, TResult>() =>
            (t1, t2) => throw new NotImplementedException();

        public static Func<T1, T2, T3, TResult> Of<T1, T2, T3, TResult>() =>
            (t1, t2, t3) => throw new NotImplementedException();

        public static Func<T1, T2, T3, T4, TResult> Of<T1, T2, T3, T4, TResult>() =>
            (t1, t2, t3, t4) => throw new NotImplementedException();
    }
}