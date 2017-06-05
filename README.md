# Common monadic extensions for POCO

## @MayBe

```cs
string name = fooMayBeNull.@Get(x => x.Name);
//
fooMayBeNull.@Do(x => x.Greet());
```


## @Notify

```cs
class Foo {
    void UpdateDependencies() {
        this.RaisePropertyChanged(() => Name);
        this.RaiseCanExecuteChanged(() => Greet());
    }
}
```