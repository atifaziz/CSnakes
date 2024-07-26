# CSnakes - a tool for embedding Python code into .NET projects

<img src="docs/res/logo.jpeg" alt="drawing" width="200"/> 

CSnakes is a .NET Source Generator and Runtime that you can use to embed Python code and libraries into your .NET Solution without the need for REST, HTTP, or Microservices.

![image](https://github.com/tonybaloney/PythonCodeGen/assets/1532417/39ca2f2a-416b-447a-a237-59e9613a4990)

## Features

- .NET Standard 2.0 (.NET 6+)
- Supports up to Python 3.12
- Supports Virtual Environments and C-Extensions
- Supports Windows, macOS, and Linux
- Uses Python's C-API for fast invocation of Python code directly in the .NET process
- Uses Python type hinting to generate function signatures with .NET native types
- Supports nested sequence and mapping types (`tuple`, `dict`, `list`)
- Suports default values

## Examples

Given the following Python file called `example.py`

```python

def hello_world(name: str, age: int) -> str:
  return f"Hello {name}, you must be {age} years old!"
```

CSnakes will generate a static .NET class called `Example` with the function:

```csharp
public class Example {
  public static string HelloWorld(string name, long age) {
    ...
  }
}
```

When called, `HelloWorld()` will invoke the Python function from `example.py` using Python's C-API and return native .NET types.
