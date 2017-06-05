namespace POCO.Monads.Notify.Tests {
    using System;
    using System.Linq.Expressions;
    using NUnit.Framework;

    [TestFixture]
    public class NotifyTests_INPC {
        [OneTimeTearDown]
        public void FixtureTearDown() {
            @Notify.Reset(typeof(NPC_Obj));
        }
        [Test]
        public void Test_00_NPC_InnerChange() {
            NPC_Obj obj = new NPC_Obj();
            Assert.IsNull(obj.changedProperty);
            obj.OnNameChanged();
            Assert.AreEqual("Name", obj.changedProperty);
        }
        [Test]
        public void Test_01_NPC_Name() {
            NPC_Obj obj = new NPC_Obj();
            Assert.IsNull(obj.changedProperty);
            obj.RaisePropertyChanged("Test");
            Assert.AreEqual("Test", obj.changedProperty);
        }
        [Test]
        public void Test_01_NPC_Lambda() {
            NPC_Obj obj = new NPC_Obj();
            Assert.IsNull(obj.changedProperty);
            Expression<Func<string>> e = () => obj.Name;
            obj.RaisePropertyChanged((LambdaExpression)e);
            Assert.AreEqual("Name", obj.changedProperty);
        }
    }
}