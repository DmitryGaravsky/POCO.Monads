namespace POCO.Monads.Expressions {
    using System;
    using System.Diagnostics;
    using System.Linq.Expressions;

    public static partial class @Name {
        /// <summary>@Monad(Expression<Func<T>>): Property Name</summary>
        [DebuggerStepThrough, DebuggerHidden]
        public static string @Property<T>(this Expression<Func<T>> expression) {
            return @Property((LambdaExpression)expression);
        }
        /// <summary>@Monad(LambdaExpression): Property Name</summary>
        [DebuggerStepThrough, DebuggerHidden]
        public static string @Property(this LambdaExpression expression) {
            MemberExpression memberExpression = GetMemberExpression(expression);
            if(IsPropertyExpression(memberExpression.Expression as MemberExpression))
                throw new ArgumentException("Not a property: " + expression.ToString());
            return memberExpression.Member.Name;
        }
        static bool IsPropertyExpression(MemberExpression expression) {
            return (expression != null) && (expression.Member.MemberType == System.Reflection.MemberTypes.Property);
        }
        static MemberExpression GetMemberExpression(LambdaExpression expression) {
            if(expression == null)
                throw new ArgumentNullException("expression");
            Expression body = expression.Body;
            if(body is UnaryExpression)
                body = ((UnaryExpression)body).Operand;
            MemberExpression memberExpression = body as MemberExpression;
            if(memberExpression == null)
                throw new ArgumentException("Not a member expression: " + expression.ToString());
            return memberExpression;
        }
    }
}