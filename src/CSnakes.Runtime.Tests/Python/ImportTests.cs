using CSnakes.Runtime.Python;

namespace CSnakes.Runtime.Tests.Python;
public class ImportTests : RuntimeTestBase
{
    [Fact]
    public void TestImportModule()
    {
        using PyObject sys = Import.ImportModule("sys");
        Assert.NotNull(sys);
        Assert.Equal("<module 'sys' (built-in)>", sys.ToString());
    }

    [Fact]
    public void TestReloadModule()
    {
        PyObject sys = Import.ImportModule("sys");
        Assert.NotNull(sys);
        Import.ReloadModule(ref sys);
        Assert.Equal("<module 'sys' (built-in)>", sys.ToString());
    }

    [Fact]
    public void TestReloadModuleThatIsntAModule()
    {
        PyObject sys = PyObject.From(42); // definitely not a module
        Assert.NotNull(sys);
        Assert.Throws<PythonInvocationException>(() => Import.ReloadModule(ref sys));
    }

    [Fact]
    public void TestImportFromString()
    {
        string source = "print('hello world')"; // Python code string that prints "hello world"
        string path = Environment.CurrentDirectory;
        using PyObject module = Import.ImportModule("test_module", source, path);
        Assert.NotNull(module);
        Assert.StartsWith("<module 'test_module' ", module.ToString());
    }

    [Fact]
    public void TestImportFromStringWithInvalidCode()
    {
        string source = "print('hello world'"; // Invalid Python code
        string path = Environment.CurrentDirectory;
        Assert.Throws<PythonInvocationException>(() => Import.ImportModule("test_module", source, path));
    }

    [Fact]
    public void TestImportFromStringWithInvalidPath()
    {
        string source = "print('hello world')"; // Valid Python code
        Assert.Throws<ArgumentException>(() => Import.ImportModule("", source, ""));
    }

    [Fact]
    public void TestImportFromStringWithNullValues()
    {
        Assert.Throws<ArgumentNullException>(() => Import.ImportModule("test_module", (string)null!, null!));
    }

    [Fact]
    public void TestImportFromStringWithEmptyValues()
    {
        string source = ""; // Invalid Python code
        string path = ""; // Invalid path
        Assert.Throws<ArgumentException>(() => Import.ImportModule("test_module", source, path));
    }
}
