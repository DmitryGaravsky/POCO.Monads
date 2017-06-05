namespace POCO.Monads.Notify.Tests {
    using NUnit.Framework;

    [TestFixture]
    public partial class NotifyTests_Command {
        [Test]
        public void Test_01_Command() {
            Obj_WithCommand obj = new Obj_WithCommand();
            Assert.AreEqual(0, ((Command)obj.HelloCommand).counter);
            obj.OnHelloChanged();
            Assert.AreEqual(1, ((Command)obj.HelloCommand).counter);
            Assert.AreEqual(0, ((Command)obj.SayCommand).counter);
            obj.OnSayChanged();
            Assert.AreEqual(1, ((Command)obj.SayCommand).counter);
            Assert.AreEqual(1, ((Command)obj.HelloCommand).counter);
            @Notify.Reset(obj);
            @Notify.Reset();
        }
    }
}