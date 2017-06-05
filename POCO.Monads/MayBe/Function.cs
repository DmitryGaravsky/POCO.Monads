namespace POCO.Monads.Function {
    using System;
    using System.Diagnostics;

    public static class @MayBe {
        /// <summary>@Monad(Func<TResult>): Result</summary>
        [DebuggerStepThrough, DebuggerHidden]
        public static TResult @Result<TResult>(this Func<TResult> func, TResult defaultResult = default(TResult)) {
            return (func != null) ?
                func() : defaultResult;
        }
        /// <summary>@Monad(Func<T,TResult>): Result</summary>
        [DebuggerStepThrough, DebuggerHidden]
        public static TResult @Result<T, TResult>(this Func<T, TResult> func, T arg, TResult defaultResult = default(TResult)) {
            return (func != null) ?
                func(arg) : defaultResult;
        }
        /// <summary>@Monad(Func<T1,T2,TResult>): Result</summary>
        [DebuggerStepThrough, DebuggerHidden]
        public static TResult @Result<T1, T2, TResult>(this Func<T1, T2, TResult> func, T1 arg1, T2 arg2, TResult defaultResult = default(TResult)) {
            return (func != null) ?
                func(arg1, arg2) : defaultResult;
        }
    }
}