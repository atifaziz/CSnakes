using CSnakes.Runtime.Python;

namespace CSnakes.Runtime.Tests.Python;
public class PyObjectTests : RuntimeTestBase
{
    [Fact]
    public void TestToString()
    {
        using PyObject? pyObj = PyObject.From("Hello, World!");
        Assert.NotNull(pyObj);
        Assert.Equal("Hello, World!", pyObj!.ToString());
    }

    [Fact]
    public void TestObjectType()
    {
        using PyObject? pyObj = PyObject.From("Hello, World!");
        Assert.NotNull(pyObj);
        Assert.Equal("<class 'str'>", pyObj!.GetPythonType().ToString());
    }

    [Fact]
    public void TestObjectGetAttr()
    {
        using PyObject? pyObj = PyObject.From("Hello, World!");
        Assert.NotNull(pyObj);
        Assert.True(pyObj!.HasAttr("__doc__"));
        using PyObject? pyObjDoc = pyObj!.GetAttr("__doc__");
        Assert.NotNull(pyObjDoc);
        Assert.Contains("Create a new string ", pyObjDoc!.ToString());
    }

    [Fact]
    public void TestObjectGetRepr()
    {
        using PyObject? pyObj = PyObject.From("hello");
        Assert.NotNull(pyObj);
        string pyObjDoc = pyObj!.GetRepr();
        Assert.False(string.IsNullOrEmpty(pyObjDoc));
        Assert.Contains("'hello'", pyObjDoc);
    }

    [Fact]
    public void CannotUnsafelyGetHandleFromDisposedPyObject()
    {
        using (GIL.Acquire())
        {
            using PyObject? pyObj = PyObject.From("hello");
            Assert.NotNull(pyObj);
            pyObj.Dispose();
            Assert.Throws<ObjectDisposedException>(() => pyObj!.ToString());
        }
    }

    [Fact]
    public void TestObjectIsNone()
    {
        var obj1 = PyObject.None;
        var obj2 = PyObject.None;
        Assert.True(obj2.Is(obj2));
    }

    [Fact]
    public void TestObjectIsSmallIntegers()
    {
        // Small numbers are the same object in Python, weird implementation detail.
        var obj1 = PyObject.From(42);
        var obj2 = PyObject.From(42);
        Assert.True(obj1!.Is(obj2!));
    }

    [InlineData(null, null)]
    [InlineData(42, 42)]
    [InlineData(42123434, 42123434)]
    [InlineData("Hello!", "Hello!")]
    [InlineData(3, 3.0)]
    [Theory]
    public void TestObjectEquals(object? o1, object? o2)
    {
        using var obj1 = PyObject.From(o1);
        using var obj2 = PyObject.From(o2);
        Assert.True(obj1!.Equals(obj2));
        Assert.True(obj1 == obj2);
    }

    [Fact]
    public void TestObjectEqualsCollection()
    {
        using var obj1 = PyObject.From<IEnumerable<string>>(new[] { "Hello!", "World!" });
        using var obj2 = PyObject.From<IEnumerable<string>>(new[] { "Hello!", "World!" });
        Assert.True(obj1!.Equals(obj2));
        Assert.True(obj1 == obj2);
    }

    [InlineData(null, true)]
    [InlineData(42, 44)]
    [InlineData(42123434, 421234)]
    [InlineData("Hello!", "Hello?")]
    [InlineData(3, 3.2)]
    [Theory]
    public void TestObjectNotEquals(object? o1, object? o2)
    {
        using var obj1 = PyObject.From(o1);
        using var obj2 = PyObject.From(o2);
        Assert.True(obj1!.NotEquals(obj2));
        Assert.True(obj1 != obj2);
    }

    [Fact]
    public void TestObjectNotEqualsCollection()
    {
        using var obj1 = PyObject.From<IEnumerable<string>>(new[] { "Hello!", "World!" });
        using var obj2 = PyObject.From<IEnumerable<string>>(new[] { "Hello?", "World?" });
        Assert.True(obj1!.NotEquals(obj2));
        Assert.True(obj1 != obj2);
    }

    [InlineData(null, null, false, false)]
    [InlineData(0, null, false, false)]
    [InlineData(null, 0, false, false)]
    [InlineData(long.MaxValue, 0, false, true)]
    [InlineData(long.MinValue, 0, true, false)]
    [InlineData(0, long.MaxValue, true, false)]
    [InlineData(0, long.MinValue, false, true)]
    [InlineData((long)-1, 1, true, false)]
    [InlineData(1, (long)-1, false, true)]
    [InlineData("a", "b", true, false)]
    [InlineData("b", "a", false, true)]
    [InlineData(3.0, 3.2, true, false)]
    [Theory]
    public void TestObjectStrictInequality(object? o1, object? o2, bool expectedLT, bool expectedGT)
    {
        using var obj1 = o1 is null ? null : PyObject.From(o1);
        using var obj2 = o2 is null ? null : PyObject.From(o2);
        Assert.Equal(expectedLT, obj1 < obj2);
        Assert.Equal(expectedGT, obj1 > obj2);
    }

    [InlineData(null, null, true, true)]
    [InlineData(0, null, false, false)]
    [InlineData(null, 0, false, false)]
    [InlineData(long.MaxValue, 0, false, true)]
    [InlineData(long.MinValue, 0, true, false)]
    [InlineData(0, long.MaxValue, true, false)]
    [InlineData(0, long.MinValue, false, true)]
    [InlineData((long)-1, 1, true, false)]
    [InlineData(1, (long)-1, false, true)]
    [InlineData("a", "b", true, false)]
    [InlineData("b", "a", false, true)]
    [InlineData(3.0, 3.2, true, false)]
    [InlineData("b", "b", true, true)]
    [InlineData(3.0, 3.0, true, true)]
    [Theory]
    public void TestObjectNotStrictInequality(object? o1, object? o2, bool expectedLT, bool expectedGT)
    {
        using var obj1 = o1 is null ? null : PyObject.From(o1);
        using var obj2 = o2 is null ? null : PyObject.From(o2);
        Assert.Equal(expectedLT, obj1 <= obj2);
        Assert.Equal(expectedGT, obj1 >= obj2);
    }

    public static IEnumerable<object[]> BooleanishTestCases(bool yes) => new TheoryData<bool, object?>
    {
        { yes,  /* True      */ true },
        { !yes, /* False     */ false },
        { !yes, /* None      */ null },
        { yes,  /* -42       */ -42 },
        { !yes, /* 0         */ 0 },
        { yes,  /* 42        */ 42 },
        { !yes, /* ""        */ "" },
        { yes,  /* "foobar"  */ "foobar" },
        { !yes, /* []        */ Array.Empty<int>() },
        { yes,  /* [42]      */ new[] { 42 } },
        { !yes, /* {}        */ new Dictionary<int, string>() },
        { yes,  /* { 0: "" } */ new Dictionary<int, string> { [0] = "" } },
        { !yes, /* ()        */ new ValueTuple() },
        { yes,  /* (0,)      */ new ValueTuple<int>(0) }
    };

    /// <summary>
    /// Exercises <see cref="PyObject.op_True"/>.
    /// </summary>
    [Theory]
    [MemberData(nameof(BooleanishTestCases), true)]
    public void TestTruthy(bool expected, object input)
    {
        using var obj = PyObject.From(input);
        if (obj)
            Assert.True(expected);
        else
            Assert.False(expected);
    }

    /// <summary>
    /// Exercises <see cref="PyObject.op_LogicalNot"/>.
    /// </summary>
    [Theory]
    [MemberData(nameof(BooleanishTestCases), false)]
    public void TestFalsy(bool expected, object input)
    {
        using var obj = PyObject.From(input);
        var result = !obj;
        Assert.Equal(expected, result);
    }

    /// <summary>
    /// Tests for short-circuiting behavior of the logical AND operator (&).
    /// If left operand is falsy, right operand should not be evaluated.
    /// </summary>
    [Theory]
    [MemberData(nameof(BooleanishTestCases), true)] // Get all test cases (both truthy and falsy)
    public void TestLogicalAndShortCircuit(bool isTruthy, object input)
    {
        using var leftObj = PyObject.From(input);
        using var rightObj = PyObject.From("right_operand");

        // Perform the logical AND operation
        using var result = leftObj & rightObj;

        // Check short-circuiting based on truthiness
        if (!isTruthy)
        {
            // For falsy left operand, result should be the left operand (short-circuit)
            Assert.True(result.Is(leftObj), "Short-circuit AND failed: When left operand is falsy, result should be the left operand");
        }
        else
        {
            // For truthy left operand, result should be the right operand (no short-circuit)
            Assert.True(result.Is(rightObj), "Non-short-circuit AND failed: When left operand is truthy, result should be the right operand");
        }
    }

    /// <summary>
    /// Tests for short-circuiting behavior of the logical OR operator (|).
    /// If left operand is truthy, right operand should not be evaluated.
    /// </summary>
    [Theory]
    [MemberData(nameof(BooleanishTestCases), true)] // Get all test cases (both truthy and falsy)
    public void TestLogicalOrShortCircuit(bool isTruthy, object input)
    {
        using var leftObj = PyObject.From(input);
        using var rightObj = PyObject.From("right_operand");

        // Perform the logical OR operation
        using var result = leftObj | rightObj;

        // Check short-circuiting based on truthiness
        if (isTruthy)
        {
            // For truthy left operand, result should be the left operand (short-circuit)
            Assert.True(result.Is(leftObj), "Short-circuit OR failed: When left operand is truthy, result should be the left operand");
        }
        else
        {
            // For falsy left operand, result should be the right operand (no short-circuit)
            Assert.True(result.Is(rightObj), "Non-short-circuit OR failed: When left operand is falsy, result should be the right operand");
        }
    }

    /// <summary>
    /// Tests for short-circuiting behavior of the C# logical AND operator (&&).
    /// If left operand is falsy, right operand should not be evaluated.
    /// </summary>
    [Fact]
    public void TestShortCircuitingLogicalAnd()
    {
        bool evaluationOccurred = false;

        // Function that sets a flag when evaluated and returns a PyObject
        PyObject FlagFunction()
        {
            evaluationOccurred = true;
            return PyObject.From(true);
        }

        // Test with falsy left operand - should short-circuit
        using var falsy = PyObject.From(false);

        // Reset flag
        evaluationOccurred = false;

        // Perform logical AND operation using implicit bool conversion
        // The key is to get the bool value first, then use && with the function call
        bool leftResult = falsy ? true : false;
        bool result1 = leftResult && (FlagFunction() ? true : false);

        // Assert that right side wasn't evaluated (flag should still be false)
        Assert.False(evaluationOccurred, "Short-circuit failed: right operand was evaluated when left operand was falsy");
        Assert.False(result1);

        // Test with truthy left operand - should not short-circuit
        using var truthy = PyObject.From(true);

        // Reset flag
        evaluationOccurred = false;

        // Perform logical AND operation
        leftResult = truthy ? true : false;
        bool result2 = leftResult && (FlagFunction() ? true : false);

        // Assert that right side was evaluated (flag should be true)
        Assert.True(evaluationOccurred, "Short-circuit incorrectly applied: right operand was not evaluated when left operand was truthy");
        Assert.True(result2);
    }

    /// <summary>
    /// Tests for short-circuiting behavior of the C# logical OR operator (||).
    /// If left operand is truthy, right operand should not be evaluated.
    /// </summary>
    [Fact]
    public void TestShortCircuitingLogicalOr()
    {
        bool evaluationOccurred = false;

        // Function that sets a flag when evaluated and returns a PyObject
        PyObject FlagFunction()
        {
            evaluationOccurred = true;
            return PyObject.From(true);
        }

        // Test with truthy left operand - should short-circuit
        using var truthy = PyObject.From(true);

        // Reset flag
        evaluationOccurred = false;

        // Perform logical OR operation using implicit bool conversion
        bool leftResult = truthy ? true : false;
        bool result1 = leftResult || (FlagFunction() ? true : false);

        // Assert that right side wasn't evaluated (flag should still be false)
        Assert.False(evaluationOccurred, "Short-circuit failed: right operand was evaluated when left operand was truthy");
        Assert.True(result1);

        // Test with falsy left operand - should not short-circuit
        using var falsy = PyObject.From(false);

        // Reset flag
        evaluationOccurred = false;

        // Perform logical OR operation
        leftResult = falsy ? true : false;
        bool result2 = leftResult || (FlagFunction() ? true : false);

        // Assert that right side was evaluated (flag should be true)
        Assert.True(evaluationOccurred, "Short-circuit incorrectly applied: right operand was not evaluated when left operand was falsy");
        Assert.True(result2);
    }
}
