# Common monadic extensions for POCO

```cs
public class Foo {
    public virtual Name { 
        get;
        set; 
    }
    public void Greet() {
        Console.WriteLine(Name);
    }
}
```

## @MayBe

```cs
Foo fooMayBeNull = service.GetFoo();
//...
string name = fooMayBeNull.@Get(x => x.Name);
fooMayBeNull.@Do(x => x.Greet());
```


## @Notify

```cs
class Foo {
    // ...
    void UpdateDependencies() {
        this.RaisePropertyChanged(() => Name);
        this.RaiseCanExecuteChanged(() => Greet());
    }
}
```
