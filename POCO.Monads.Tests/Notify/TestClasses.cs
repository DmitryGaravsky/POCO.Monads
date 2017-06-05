namespace POCO.Monads.Notify.Tests {

    class NPC_Obj {
        internal string changedProperty;
        public string Name {
            get;
            set;
        }
        protected void RaisePropertyChanged(string propertyName) {
            this.changedProperty = propertyName;
        }
        internal void OnNameChanged() {
            this.RaisePropertyChanged(() => Name);
        }
    }
    class Command {
        internal int counter = 0;
        protected void RaiseCanExecuteChanged() {
            counter++;
        }
    }
    class Obj_WithCommand {
        public Obj_WithCommand() {
            HelloCommand = new Command();
            SayCommand = new Command();
        }
        public object HelloCommand {
            get;
            private set;
        }
        public object SayCommand {
            get;
            private set;
        }
        public void Hello() { }
        public void Say(string text) { }
        //
        internal void OnHelloChanged() {
            this.RaiseCanExecuteChanged(() => Hello());
        }
        internal void OnSayChanged() {
            this.RaiseCanExecuteChanged((string msg) => Say(msg));
        }
    }
}