# Storm.NET
Simple Topologically Ordered Reactive Model

[![appveyor](https://ci.appveyor.com/api/projects/status/github/StormDotNet/Storm.NET?svg=true)](https://ci.appveyor.com/project/StormDotNetAdmin/storm-net)
[![codecov](https://codecov.io/gh/StormDotNet/Storm.NET/branch/master/graph/badge.svg)](https://codecov.io/gh/StormDotNet/Storm.NET)

## Introduction

Storm.NET is a simple, topologically ordered, reactive model.

### What *reactive model* means?

Take a look on this code:

```C#
  var a = 1;
  var b = 2;
  var c = a + b;
  a = 2;
```

`c` is evaluated once, its value is `3`.
After affecting `2` to `a`, `c` didn't reacted and its value is still `3`.

**Let's make it reactive:**

```C#
var a = Storm.Input.Create(1);
var b = Storm.Input.Create(2);
var c = Storm.Func.Create(a, b, (aValue, bValue) => aValue + bValue);
a.SetValue(2);
```

This time, `c` reacted and its value is now `4`.

That's a reactive model.

### What *topologically ordered* means?

When a reactive model is built, the notion of dependency graph appear quickly. Each item of the model is a node on the graph, each item dependency is an arrow in the graph.

Here is the dependency graph of the previous exemple:

![a<-c, b<-c](https://user-images.githubusercontent.com/58727950/71514740-6013a500-28a0-11ea-825a-d8c800393c28.png)

More complex graphs can appear quickly, consider this code:

```C#
var input = Storm.Input.Create<int>();

// Let's divide input value by 2
var a = Storm.Func.Create(input, aValue => aValue / 2); // a store the quotient
var b = Storm.Func.Create(input, bValue => bValue % 2); // b store the remainder

// Let's build back the input value
var output = Storm.Func.Create(a, b, (aValue, bValue) => aValue * 2 + bValue); // output store the same value as input
```

The dependency graph of this code is called a diamond graph:

![input<-a, input-b, a<-output, b<-output](https://user-images.githubusercontent.com/9695349/71476317-bae7c680-27e4-11ea-863e-e909b8ce3765.png)

In this exemple:
  - `output` must be evaluated after `a` and `b`
  - `a` and `b` must be evaluated after `input`

One can evaluate the graph in this order: `input`, `a`, `b` and `output`.

That is called a topological sort of the graph.

*Storm.NET* ensure a topologically ordered evaluation.

## Available features

### Fluent Factories

*Storm.NET* nodes are created via the `Storm` factory. This allow fluent parameterization of the created objects:

```C#
var a = Storm.Input.WithoutCompare.Create<int>(); // 'a' will not compare old and new values. It raise 'changed' at each update.
var f = Storm.Func.FromStates.Create(a, b, (aState, bState) => ...); // the evaluation of f will be done with extended informations
```

**`WithCompare`/`WithoutCompare`**

When a *node* is created using the `WithCompare` flavour: on update, it's new value is compared to the old value using the provided `IEqualityComparer` (or the `DefaultEqualityComparer`) to define the state `Changed`/`Unchanged`.

When a *node* is created using the `WithoutCompare` flavour: on update, the state is always `Changed`. That's can be usefull to create repeatable actions.

The default flavour is `WithCompare`.

### Immutable

`Immutable` nodes are roots of the dependency graph. That's it, they don't have dependencies. They don't trigger any graph update since their content is immutable.

`Storm.Immutable` factory provide three methods:
 - `CreateError<T>(StormError error)` create an immutable node that contains the given error.
 - `CreateError<T>(string message)` create an immutable node that contains an error with the given message.
 - `CreateValue<T>(T value)` create an immutable node that contains the given value.

### Input

`Input` nodes are roots of the dependency graph. That's it, they don't have dependencies. They are the entry point of a graph update via the `SetValue` and `SetError` method.

`Storm.Input` factory came with two flavours `WithCompare`/`WithoutCompare`.

The default content is `Storm.Error.EmptyContent`.

### Function

`Func` nodes are readonly, their values depends only on their dependencies. They are the core of the graph.

`Storm.Func` factory came with four flavours mixed from `WithCompare`/`WithoutCompare` and `FromValues`/`FromStates`.

When a `StormFunc` node evaluation fail (throw an exception), it's content is a `Storm.Error`. The inner exception contain the original exception.

When a `StormFunc` node is created using the `FromValues` flavour, it's value is evaluated from the values of the dependencies. If one or more dependencies are in an `Error` state, the *Storm function* content is a `Storm.Error` with an inner `AggregateException` contening dependencies errors.

Example:
```C#
var a = Storm.Input.Create<int>();
var b = Storm.Input.Create<int>();

// c content is a + b when a and b are not in error.
var c = Storm.Func.Create(a, b, (aValue, bValue) => aValue + bValue);
```

When a `StormFunc` node is created using the `FromStates` flavour, it's value is evaluated from the states informations of the dependencies.
For each dependency, the state information contains:
 - `VisitState` either:
   - `NotVisited`: The dependency was not impacted by the current update.
   - `VisitedWithChange`: The dependency was impacted by the current update and its content changed.
   - `VisitedWithoutChange`: The dependency was impacted by the current update but its content was not changed.
 - `Content` the content of the dependency.
 
 Example:
 ```C#
var a = Storm.Input.Create<int>();
var b = Storm.Input.Create<int>();

// c will contain a value only if it just changed to a valid value
// otherwise, it will contain the value of b or default if b is in error.
var c = Storm.Func.FromStates.Create(a, b, (aState, bState) =>
{
    if (aState.VisitState == EStormFuncInputState.VisitedWithChange && !aState.Content.TryGetError(out _))
        return aState.Content.GetValueOrThrow();

    return bState.Content.GetValueOr(default);
});
```

### Socket

Creating a complex model with hundreds of inter-dependent `Input` and `Func` nodes force the developer to declare the nodes in a topological order. That it, you can't create a `Func` before its dependency. This is a tough work, and it's hard to maintain.

`Socket` nodes can be created before their dependencies.

This allow the user to create the nodes, then to interconnect them with the `Connect` method:

```C#
public class GlobalModel
{
    public GlobalModel()
    {
        // instantiate nodes.
        FooSubModel = new FooSubModel();
        BarSubModel = new BarSubModel();

        // create dependency
        FooSubModel.Configure(this);
        BarSubModel.Configure(this);
    }

    public FooSubModel FooSubModel { get; }
    public BarSubModel BarSubModel { get; }
}

public class FooSubModel
{
    private readonly IStormSocket<string> _fooString = Storm.Socket.Create<string>();

    public void Configure(GlobalModel globalModel)
    {
        _fooString.Connect(Storm.Func.Create(globalModel.BarSubModel.BarInt, barInt => $"{barInt:00}"));
    }

    public IStormInput<int> FooInt { get; } = Storm.Input.Create(0);

    public IStorm<string> FooString => _fooString;
}

public class BarSubModel
{
    private readonly IStormSocket<string> _barString = Storm.Socket.Create<string>();

    public void Configure(GlobalModel globalModel)
    {
        _barString.Connect(Storm.Func.Create(globalModel.FooSubModel.FooInt, fooInt => $"{fooInt:00}"));
    }

    public IStormInput<int> BarInt { get; } = Storm.Input.Create(0);

    public IStorm<string> BarString => _barString;
}
```

The dependency graph show how `FooSubModel` and `BarSubModel` are interdependent. They could not have been created without the use of sockets:

![FooInt<-BarString, BarInt<-FooString](https://user-images.githubusercontent.com/9695349/71519701-efc54d80-28b8-11ea-9e58-e3dc0a368a18.png)

### Switch

> Q: I don't know how, I end up with a `IStorm<IStorm<T>>`.
> A: Can't you use a `Func`, put all known `IStorm<T>` possible values as input and made a switch on it?
> Q: Erh.. Nope.
> A: In this case, just made a `Switch` on it.

`Switch` transform an `IStorm<IStorm<T>>` into a `IStorm<T>`.

**Remark**

If you know all possible target, a `Func` may be preferable:

Example:
```C#
var selector = Storm.Input.Create(0);
var evenTarget = Storm.Input.Create("even");
var oddTarget = Storm.Input.Create("odd");

var output = Storm.Switch.Create(selector, i => i % 2 == 0 ? evenTarget : oddTarget);
```

Can be transformed to:
```C#
var selector = Storm.Input.Create(0);
var evenTarget = Storm.Input.Create("even");
var oddTarget = Storm.Input.Create("odd");

var output = Storm.Func.Create(selector, evenTarget, oddTarget, (i, evenValue, oddValue) => i % 2 == 0 ? evenValue : oddValue);
```

But in some cases you can't:
```C#
var selectedIndex = Storm.Input.Create(0);
var models = new List<FooSubModel>();
var selectedFooString = Storm.Switch.Create(selectedIndex, i => models[i].FooString);
```
