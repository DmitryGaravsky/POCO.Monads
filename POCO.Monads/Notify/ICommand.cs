namespace POCO.Monads.Notify {
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq.Expressions;
    using Monads.Object;
    using Monads.Action;
    using Monads.Function;
    using System.Reflection;

    partial class @Notify {
        /// <summary>@Monad(x): POCO, Raise CanExecuteChanged</summary>
        [DebuggerStepThrough, DebuggerHidden]
        public static void RaiseCanExecuteChanged(this object @this, Expression<Action> expression) {
            RaiseCanExecuteChangedCore(@this, ((MethodCallExpression)expression.Body).Method);
        }
        /// <summary>@Monad(x): POCO, Raise CanExecuteChanged</summary>
        [DebuggerStepThrough, DebuggerHidden]
        public static void RaiseCanExecuteChanged<T>(this object @this, Expression<Action<T>> expression) {
            RaiseCanExecuteChangedCore(@this, ((MethodCallExpression)expression.Body).Method);
        }
        static void RaiseCanExecuteChangedCore(this object @this, MethodInfo methodInfo) {
            @this
                .@Get(x =>
                    GetGetPropertyValue(methodInfo, x.GetType()).@Result(x))
                .@Do(command =>
                    GetRaiseCanExecuteChanged(command.GetType()).@Execute(command));
        }
        readonly static IDictionary<string, Func<object, object>> getPropCache = new Dictionary<string, Func<object, object>>(StringComparer.Ordinal);
        [DebuggerStepThrough, DebuggerHidden]
        static Func<object, object> GetGetPropertyValue(MethodInfo methodInfo, Type type) {
            string key = type.FullName + "." + methodInfo.Name;
            Func<object, object> getter;
            if(!getPropCache.TryGetValue(key, out getter)) {
                var getMethod = Internal.MethodInfoHelper.GetMethodInfo(type, "get_" + methodInfo.Name + "Command");
                if(getMethod != null) {
                    var source = Expression.Parameter(typeof(object), "source");
                    getter = Expression.Lambda<Func<object, object>>(
                                Expression.Call(Expression.Convert(source, getMethod.DeclaringType), getMethod), source
                             ).Compile();
                    getPropCache.Add(key, getter);
                }
                else getPropCache.Add(key, null);
            }
            return getter;
        }
        readonly static IDictionary<Type, Action<object>> raiseCanExecuteCache = new Dictionary<Type, Action<object>>();
        [DebuggerStepThrough, DebuggerHidden]
        static Action<object> GetRaiseCanExecuteChanged(Type type) {
            Action<object> raise;
            if(!raiseCanExecuteCache.TryGetValue(type, out raise)) {
                var method = Internal.MethodInfoHelper.GetMethodInfo(type, "RaiseCanExecuteChanged");
                if(method != null) {
                    var source = Expression.Parameter(typeof(object), "source");
                    raise = Expression.Lambda<Action<object>>(
                                Expression.Call(Expression.Convert(source, method.DeclaringType), method), source
                            ).Compile();
                    raiseCanExecuteCache.Add(type, raise);
                }
                else raiseCanExecuteCache.Add(type, null);
            }
            return raise;
        }
    }
}