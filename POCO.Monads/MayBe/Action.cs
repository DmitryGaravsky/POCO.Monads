namespace POCO.Monads.Action {
    using System;
    using System.Diagnostics;

    public static class @MayBe {
        /// <summary>@Monad(Action): Invoke</summary>
        [DebuggerStepThrough, DebuggerHidden]
        public static void @Execute(this Action action) {
            if(action != null)
                action();
        }
        /// <summary>@Monad(Action<T>): Invoke</summary>
        [DebuggerStepThrough, DebuggerHidden]
        public static void @Execute<T>(this Action<T> action, T arg) {
            if(action != null)
                action(arg);
        }
        /// <summary>@Monad(Action<T1,T2>): Invoke</summary>
        [DebuggerStepThrough, DebuggerHidden]
        public static void @Execute<T1, T2>(this Action<T1, T2> action, T1 arg1, T2 arg2) {
            if(action != null)
                action(arg1, arg2);
        }
    }
}