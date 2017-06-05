namespace POCO.Monads.Object {
    using System;
    using System.Diagnostics;

    public static class @MayBe {
        /// <summary>@Monad(T): If not null</summary>
        [DebuggerStepThrough, DebuggerHidden]
        public static TResult @Get<T, TResult>(this T @this, Func<T, TResult> @get, TResult defaultValue = default(TResult)) {
            return (@this != null) ? @get(@this) : defaultValue;
        }
        /// <summary>@Monad(T): If not null</summary>
        [DebuggerStepThrough, DebuggerHidden]
        public static void @Do<T>(this T @this, Action<T> @do) {
            if(@this != null)
                @do(@this);
        }
    }
}