namespace POCO.Monads.Internal {
    using System.Linq.Expressions;
    using System.Reflection;

    static class ExpressionHelper {
        internal static MethodInfo GetMethod(LambdaExpression expression) {
            return ((MethodCallExpression)expression.Body).Method;
        }
    }
}