namespace Delegating.Tests
{
    static class Capture
    {
        public static TResult CaptureInvocation<T1, T2, TResult>(out (T1, T2) args, T1 a, T2 b, TResult result)
        {
            args = (a, b);
            return result;
        }
    }
}