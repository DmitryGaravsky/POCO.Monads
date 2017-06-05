namespace POCO.Monads.Expressions.Tests {
    using System;
    using System.Linq.Expressions;
    using NUnit.Framework;

    #region Test Classes
    class Foo {
        public string Name { get; set; }
    }
    #endregion Test Classes

    [TestFixture]
    public partial class NameTests {
        [Test]
        public void Test_00_ValidExpresssion() {
            Expression<Func<Foo, string>> expr = x => x.Name;
            Assert.AreEqual("Name", expr.@Property());
            Assert.AreEqual("Name", ((LambdaExpression)expr).@Property());
        }
        [Test]
        public void Test_00_ValidExpresssion_Unary() {
            Expression<Func<object, string>> expr = (x) => (string)(((Foo)x).Name);
            Assert.AreEqual("Name", expr.@Property());
            Assert.AreEqual("Name", ((LambdaExpression)expr).@Property());
        }
        [Test]
        public void Test_01_Throws_On_Null() {
            Expression<Action> someExpr = null;
            Assert.Throws(typeof(ArgumentNullException), () => someExpr.@Property());
            Assert.Throws(typeof(ArgumentNullException), () => someExpr.@Property());
        }
        [Test]
        public void Test_01_Throws_On_InvalidExpression() {
            Expression<Action> expr = () => new Foo();
            Assert.Throws(typeof(ArgumentException), () => expr.@Property());
            Assert.Throws(typeof(ArgumentException), () => expr.@Property());
        }
        [Test]
        public void Test_01_Throws_On_InvalidExpression_Property() {
            Expression<Func<Foo, int>> expr = x => x.Name.Length;
            Assert.Throws(typeof(ArgumentException), () => expr.@Property());
            Assert.Throws(typeof(ArgumentException), () => expr.@Property());
        }
    }
}