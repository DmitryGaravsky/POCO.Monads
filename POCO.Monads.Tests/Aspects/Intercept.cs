namespace POCO.Monads.Aspects.Tests {
    using System;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using NUnit.Framework;

    #region Test Classes
    class Foo {
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void Greet() {
            Console.WriteLine("Hello! I'm " + GetName());
        }
        [MethodImpl(MethodImplOptions.NoInlining)]
        public string GetName() {
            return "foo";
        }
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static string GetClass() {
            return "Foo";
        }
        [MethodImpl(MethodImplOptions.NoInlining)]
        public virtual string GetFoo() {
            return "@Foo";
        }
    }
    class Bar {
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void Greet() {
            Console.WriteLine("Hello! I'm " + GetName());
        }
        [MethodImpl(MethodImplOptions.NoInlining)]
        public string GetName() {
            return "bar";
        }
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static string GetClass() {
            return "Bar";
        }
        [MethodImpl(MethodImplOptions.NoInlining)]
        public virtual string GetBar() {
            return "@Bar";
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
        [Test, Explicit]
        public void Test00_Instance_Virtual() {
            Foo foo = new Foo();
            Bar bar = new Bar();
            var method = typeof(Foo).GetMethod("GetFoo");
            var replacement = typeof(Bar).GetMethod("GetBar");
            Assert.AreEqual("@Foo", foo.GetFoo());
            Assert.AreEqual("@Bar", bar.GetBar());
            using(method.@Intercept(replacement)) {
                Assert.AreEqual("@Bar", foo.GetFoo());
                Assert.AreEqual("@Bar", bar.GetBar());
            }
            Assert.AreEqual("@Foo", foo.GetFoo());
            Assert.AreEqual("@Bar", bar.GetBar());
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
        [Test]
        public void Test02_Expression_VirtualToNonVirtual() {
            Foo foo = new Foo();
            Bar bar = new Bar();
            Expression<Func<Foo, string>> method = x => x.GetFoo();
            Expression<Func<Bar, string>> replacement = x => x.GetName();
            Assert.AreEqual("@Foo", foo.GetFoo());
            Assert.AreEqual("bar", bar.GetName());
            using(method.@Intercept(replacement)) {
                Assert.AreEqual("bar", foo.GetFoo());
                Assert.AreEqual("bar", bar.GetName());
            }
            Assert.AreEqual("@Foo", foo.GetFoo());
            Assert.AreEqual("bar", bar.GetName());
        }
        [Test]
        public void Test02_Expression_VirtualToStatic() {
            Foo foo = new Foo();
            Expression<Func<Foo, string>> method = x => x.GetFoo();
            Expression<Func<string>> replacement = () => Bar.GetClass();
            Assert.AreEqual("@Foo", foo.GetFoo());
            Assert.AreEqual("Bar", Bar.GetClass());
            using(method.@Intercept(replacement)) {
                Assert.AreEqual("Bar", foo.GetFoo());
                Assert.AreEqual("Bar", Bar.GetClass());
            }
            Assert.AreEqual("@Foo", foo.GetFoo());
            Assert.AreEqual("Bar", Bar.GetClass());
        }
        [Test]
        public void Test02_Expression_VirtualToVirtual() {
            Foo foo = new Foo();
            Bar bar = new Bar();
            Expression<Func<Foo, string>> method = x => x.GetFoo();
            Expression<Func<Bar, string>> replacement = x => x.GetBar();
            Assert.AreEqual("@Foo", foo.GetFoo());
            Assert.AreEqual("@Bar", bar.GetBar());
            using(method.@Intercept(replacement)) {
                Assert.AreEqual("@Bar", foo.GetFoo());
                Assert.AreEqual("@Bar", bar.GetBar());
            }
            Assert.AreEqual("@Foo", foo.GetFoo());
            Assert.AreEqual("@Bar", bar.GetBar());
        }
        [Test, Explicit]
        public void Test03_Static() {
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
        public void Test03_Static_Expression() {
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