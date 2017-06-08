namespace POCO.Monads.Aspects.Tests {
    using System;
    using System.Linq.Expressions;
    using System.Reflection;
    using NUnit.Framework;

    #region Test Classes
    class Foo {
        public void Greet() {
            Console.WriteLine("Hello! I'm " + GetName());
        }
        public string GetName() {
            return "foo";
        }
        public static string GetClass() {
            return "Foo";
        }
    }
    class Bar {
        public void Greet() {
            Console.WriteLine("Hello! I'm " + GetName());
        }
        public string GetName() {
            return "bar";
        }
        public static string GetClass() {
            return "Bar";
        }
    }
    #endregion Test Classes
    [TestFixture, SingleThreaded]
    public class MethodTests_Method {
        [Test]
        public void Test00_Smoke() {
            var method = typeof(Foo).GetMethod("GetName");
            Assert.IsNull(((MethodBase)null).@Intercept(null));
            Assert.IsNull(method.@Intercept(null));
        }
        [Test]
        public void Test00_Smoke_Expression() {
            Expression<Func<Foo, string>> methodExpr1 = x => x.GetName();
            Assert.IsNull(((Expression<Func<Foo, string>>)null).@Intercept(null));
            Assert.IsNull(methodExpr1.@Intercept(null));

            Expression<Action<Foo>> methodExpr2 = x => x.Greet();
            Assert.IsNull(((Expression<Action<Foo>>)null).@Intercept(null));
            Assert.IsNull(methodExpr2.@Intercept(null));
        }
        [Test, Explicit]
        public void Test00_Instance() {
            Foo foo = new Foo();
            Bar bar = new Bar();
            var method = typeof(Foo).GetMethod("GetName");
            var replacement = typeof(Bar).GetMethod("GetName");
            Assert.AreEqual("foo", foo.GetName());
            Assert.AreEqual("bar", bar.GetName());
            using(method.@Intercept(replacement)) {
                Assert.AreEqual("bar", foo.GetName());
                Assert.AreEqual("bar", bar.GetName());
            }
            Assert.AreEqual("foo", foo.GetName());
            Assert.AreEqual("bar", bar.GetName());
        }
        [Test]
        public void Test01_Instance_Expression() {
            Foo foo = new Foo();
            Bar bar = new Bar();
            Expression<Func<Foo, string>> method = x => x.GetName();
            Expression<Func<Bar, string>> replacement = x => x.GetName();
            Assert.AreEqual("foo", foo.GetName());
            Assert.AreEqual("bar", bar.GetName());
            using(method.@Intercept(replacement)) {
                Assert.AreEqual("bar", foo.GetName());
                Assert.AreEqual("bar", bar.GetName());
            }
            Assert.AreEqual("foo", foo.GetName());
            Assert.AreEqual("bar", bar.GetName());
        }
        [Test, Explicit]
        public void Test02_Static() {
            var method = typeof(Foo).GetMethod("GetClass");
            var replacement = typeof(Bar).GetMethod("GetClass");
            Assert.AreEqual("Foo", Foo.GetClass());
            Assert.AreEqual("Bar", Bar.GetClass());
            using(method.@Intercept(replacement)) {
                Assert.AreEqual("Bar", Foo.GetClass());
                Assert.AreEqual("Bar", Bar.GetClass());
            }
            Assert.AreEqual("Foo", Foo.GetClass());
            Assert.AreEqual("Bar", Bar.GetClass());
        }
        [Test]
        public void Test02_Static_Expression() {
            Expression<Func<string>> method = () => Foo.GetClass();
            Expression<Func<string>> replacement = () => Bar.GetClass();
            Assert.AreEqual("Foo", Foo.GetClass());
            Assert.AreEqual("Bar", Bar.GetClass());
            using(method.@Intercept(replacement)) {
                Assert.AreEqual("Bar", Foo.GetClass());
                Assert.AreEqual("Bar", Bar.GetClass());
            }
            Assert.AreEqual("Foo", Foo.GetClass());
            Assert.AreEqual("Bar", Bar.GetClass());
        }
    }
}