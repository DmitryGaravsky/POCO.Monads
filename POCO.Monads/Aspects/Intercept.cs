namespace POCO.Monads.Aspects {
    using System;
    using System.Diagnostics;
    using System.Linq.Expressions;
    using System.Reflection;

    public static class @Aspect {
        /// <summary>@Monad(Delegate): Replace method</summary>
        [DebuggerStepThrough, DebuggerHidden]
        public static IDisposable @Intercept<T>(this Expression<Action<T>> method, Expression<Action<T>> replacement) {
            return @Intercept((LambdaExpression)method, (LambdaExpression)replacement);
        }
        /// <summary>@Monad(Delegate): Replace method</summary>
        [DebuggerStepThrough, DebuggerHidden]
        public static IDisposable @Intercept<T, TResult>(this Expression<Func<T, TResult>> method, Expression<Func<T, TResult>> replacement) {
            return @Intercept((LambdaExpression)method, (LambdaExpression)replacement);
        }
        /// <summary>@Monad(Delegate): Replace method</summary>
        [DebuggerStepThrough, DebuggerHidden]
        public static IDisposable @Intercept(this LambdaExpression method, LambdaExpression replacement) {
            if(method == null || replacement == null) 
                return null;
            return @Intercept(Internal.ExpressionHelper.GetMethod(method), Internal.ExpressionHelper.GetMethod(replacement));
        }
        /// <summary>@Monad(MethodBase): Replace method</summary>
        [DebuggerStepThrough, DebuggerHidden]
        public static IDisposable @Intercept(this MethodBase method, MethodBase replacement) {
            if(method == null || replacement == null)
                return null;
            return new InterceptorToken(method, replacement);
        }
        #region Token
        sealed class InterceptorToken : IDisposable {
            IntPtr? originalValue;
            readonly MethodBase method;
            public InterceptorToken(MethodBase method, MethodBase replacement) {
                this.method = method;
                this.originalValue = Internal.RuntimeMethodHelper.Replace(replacement, method);
            }
            void IDisposable.Dispose() {
                if(originalValue.HasValue)
                    Internal.RuntimeMethodHelper.Restore(method, originalValue.Value);
                originalValue = null;
            }
        }
        #endregion Token
    }
}