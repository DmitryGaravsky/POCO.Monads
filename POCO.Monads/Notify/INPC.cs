namespace POCO.Monads.Notify {
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq.Expressions;
    using Monads.Action;
    using Monads.Expressions;
    using Monads.Object;

    public static partial class @Notify {
        /// <summary>@Monad(x): POCO, Raise INPC</summary>
        [DebuggerStepThrough, DebuggerHidden]
        public static void RaisePropertyChanged<T>(this object @this, Expression<Func<T>> expression) {
            RaisePropertyChangedCore(@this, () => expression.@Property());
        }
        /// <summary>@Monad(x): POCO, Raise INPC</summary>
        [DebuggerStepThrough, DebuggerHidden]
        public static void RaisePropertyChanged(this object @this, LambdaExpression selectorExpression) {
            RaisePropertyChangedCore(@this, () => selectorExpression.@Property());
        }
        /// <summary>@Monad(x): POCO, Raise INPC</summary>
        [DebuggerStepThrough, DebuggerHidden]
        public static void RaisePropertyChanged(this object @this, string propertyName) {
            RaisePropertyChangedCore(@this, () => propertyName);
        }
        static void RaisePropertyChangedCore(object @this, Func<string> getPropertyName) {
            @this.@Do(x =>
                    GetRaisePropertyChanged(x.GetType()).@Execute(x, getPropertyName()));
        }
        readonly static IDictionary<Type, Action<object, string>> raisePropertyCache = new Dictionary<Type, Action<object, string>>();
        [DebuggerStepThrough, DebuggerHidden]
        static Action<object, string> GetRaisePropertyChanged(Type type) {
            Action<object, string> raise;
            if(!raisePropertyCache.TryGetValue(type, out raise)) {
                var method = Internal.MethodInfoHelper.GetMethodInfo(type, "RaisePropertyChanged", new Type[] { typeof(string) });
                if(method != null) {
                    var source = Expression.Parameter(typeof(object), "source");
                    var parameter = Expression.Parameter(typeof(string), "parameter");
                    raise = Expression.Lambda<Action<object, string>>(
                                Expression.Call(Expression.Convert(source, method.DeclaringType), method, parameter),
                                    source, parameter
                            ).Compile();
                    raisePropertyCache.Add(type, raise);
                }
                else raisePropertyCache.Add(type, null);
            }
            return raise;
        }
    }
}