# Advanced Usage

## Handling very large integers

Python's `int` type is closer in structure to C#.NET's `System.Numerics.BigInteger` than to `System.Int64`. This means that when you are working with very large integers in Python, you may need to use the `BigInteger` type in C# to handle the results.

You can use this using the TypeConverter class to convert between `BigInteger` and `PyObject` instances. Here's an example of how you can call a Python function that returns a very large integer:

```csharp
using CSnakes.Runtime.Python;
using System.Numerics;

const string number = "12345678987654345678764345678987654345678765";
// Something that is too big for a long (I8)
BigInteger bignumber = BigInteger.Parse(number);

using (GIL.Acquire())
{
    using PyObject? pyObj = PyObject.From(bignumber);

    // Do stuff with the integer object
    // e.g. call a function with this as an argument

    // Convert a Python int back into a BigInteger like this..
    BigInteger integer = pyObj.As<BigInteger>();
}
```

## Free-Threading Mode

Python 3.13 introduced a new feature called "free-threading mode" which allows the Python interpreter to run in a multi-threaded environment without the Global Interpreter Lock (GIL). This is a significant change to the Python runtime and can have a big impact on the performance of Python code running in a multi-threaded environment.

CSnakes supports free-threading mode, but it is disabled by default.

To use free-threading you can use the `RedistributableLocator` from version Python 3.13 and request `freeThreaded` builds:

```csharp
var builder = Host.CreateApplicationBuilder();
var pb = builder.Services.WithPython()
  .WithHome(Environment.CurrentDirectory) // Path to your Python modules.
  .FromRedistributable("3.13", freeThreaded: true);
var app = builder.Build();

env = app.Services.GetRequiredService<IPythonEnvironment>();
```

Whilst free-threading mode is **supported** at a high-level from CSnakes, it is still an experimental feature in Python 3.13 and may not be suitable for all use-cases. Also, most Python libraries, especially those written in C, are not yet compatible with free-threading mode, so you may need to test your code carefully.

## Calling Python without the Source Generator

The Source Generator library is a useful tool for creating the boilerplate code to invoke a Python function from a `PythonEnvironment` instance and convert the types based on the type annotations in the Python function.

It is still possible to call Python code without the Source Generator, but you will need to write the boilerplate code yourself. Here's an example of how you can call a Python function without the Source Generator to call a Python function in a module called `test_basic`:

```python
def test_int_float(a: int, b: float) -> float:
    return a + b
```

The C# code to call this function needs to:

1. Convert the .NET types to `PyObject` instances and back.
1. Use the `GIL.Acquire()` method to acquire the Global Interpreter Lock for all conversions and calls to Python.
1. Use the `Import.ImportModule` method to import the module and store a reference once so that it can be used multiple times.
1. Dispose the module when it is no longer needed.

```csharp
using CSnakes.Runtime;
using CSnakes.Runtime.Python;

using System;
using System.Collections.Generic;
using System.Diagnostics;

using Microsoft.Extensions.Logging;

namespace CSnakes.Runtime;

public sealed class ExampleDirectIntegration : IDisposable
{
    private readonly PyObject module;

    private readonly ILogger<IPythonEnvironment> logger;

    internal TestBasicInternal(IPythonEnvironment env)
    {
        this.logger = env.Logger;
        using (GIL.Acquire())
        {
            logger.LogInformation("Importing module {ModuleName}", "test_basic");
            module = Import.ImportModule("test_basic");
        }
    }

    public void Dispose()
    {
        logger.LogInformation("Disposing module");
        module.Dispose();
    }

    public double TestIntFloat(long a, double b)
    {
        using (GIL.Acquire())
        {
            logger.LogInformation("Invoking Python function: {FunctionName}", "test_int_float");
            using var __underlyingPythonFunc = this.module.GetAttr("test_int_float");
            using PyObject a_pyObject = PyObject.From(a);
            using PyObject b_pyObject = PyObject.From(b);
            using var __result_pyObject = __underlyingPythonFunc.Call(a_pyObject, b_pyObject);
            return __result_pyObject.As<double>();
        }
    }
}
```

### Disabling the Source Generator

The Source Generator will only activate on files that are marked as `AdditionalFiles` in the project file. If you want to disable the Source Generator, you can remove the `AdditionalFiles` entry from the project file.

Alternatively , you can disable the Source Generator by setting the `DisableCSnakesRuntimeSourceGenerator` property in the project file:

```xml
<DisableCSnakesRuntimeSourceGenerator>true</DisableCSnakesRuntimeSourceGenerator>
```

## Hot Reload

CSnakes supports [hot reload](https://learn.microsoft.com/visualstudio/debugger/hot-reload?view=vs-2022) of Python code in Visual Studio and supported IDEs. 
This means that you can make changes to your Python code within the function body and see the changes reflected in your C# code without restarting the application.

This feature is enabled in the generated classes in CSnakes. When you make changes to the Python code, the modules are reloaded in the .NET runtime and subsequent calls to the Python code will use the new code.

To enable Hot Reload, see the [VS 2022](https://learn.microsoft.com/visualstudio/debugger/hot-reload?view=vs-2022) documentation.

### Limitations

Beyond the C# [limitations](https://learn.microsoft.com/visualstudio/debugger/supported-code-changes-csharp?view=vs-2022), Hot Reload would not support changes to the Python code which require additional changes to the C# such as :

- Removing functions
- Changing function signatures
- Changing the return type of a function
- Changing the type of a function argument
- Changing the name of a function
- Changing the name of a module

The Hot Reload feature is useful for iterating on the __body__ of a Python function, without having to restart the debugger or application.

## Disabling Signal Handlers in Python

By default, Python will install signal handlers for certain signals, such as `SIGINT` (Ctrl+C) and `SIGTERM`. This can interfere with the normal operation of your application, especially if you are using a framework that has its own signal handlers.
This means that signal handlers on C# code will not be called when the signal is received, and the Python code will handle the signal instead.

You can disable this behavior by using the `.DisableSignalHandlers()` method on the `IPythonEnvironment` instance:

```csharp
var builder = Host.CreateApplicationBuilder();
var pb = builder.Services.WithPython()
  .WithHome(Environment.CurrentDirectory)
  .FromRedistributable()
  .DisableSignalHandlers(); // Disable Python signal handlers
var app = builder.Build();

env = app.Services.GetRequiredService<IPythonEnvironment>();
```
