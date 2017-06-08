# Common monadic extensions for POCO

```cs
public class Foo {
    public virtual Name { 
        get;
        set; 
    }
    public void Greet() {
        Console.WriteLine("Hello, I'm " + Name);
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


## @Intercept

```cs
class Bar {
    public void Greet() {
        Console.WriteLine("Hello, I'm Bar!!!");
    }
}

var method = typeof(Foo).GetMethod("GetName");
var replacement = typeof(Bar).GetMethod("GetName");

Foo foo = new Foo();
foo.Greet(); // "Hello, I'm Foo
using(method.@Intercept(replacement)) {
    foo.Greet(); // "Hello, I'm Bar!!!   
}

```
