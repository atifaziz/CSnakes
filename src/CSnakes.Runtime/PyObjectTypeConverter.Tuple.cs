using CSnakes.Runtime.CPython;
using CSnakes.Runtime.Python;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace CSnakes.Runtime;
internal partial class PyObjectTypeConverter
{
    private PyObject ConvertFromTuple(ITypeDescriptorContext? context, CultureInfo? culture, ITuple t)
    {
        List<PyObject> pyObjects = [];

        for (var i = 0; i < t.Length; i++)
        {
            var currentValue = t[i];
            if (currentValue is null)
            {
                // TODO: handle null values
            }
            pyObjects.Add(ToPython(currentValue, context, culture));
        }

        return PyTuple.CreateTuple(pyObjects);
    }

    private object? ConvertToTuple(ITypeDescriptorContext? context, CultureInfo? culture, PyObject pyObj, Type destinationType)
    {
        // We have to convert the Python values to CLR values, as if we just tried As<object>() it would
        // not parse the Python type to a CLR type, only to a new Python type.
        Type[] types = destinationType.GetGenericArguments();
        object?[] clrValues;

        var tupleValues = new List<PyObject>();
        for (nint i = 0; i < CPythonAPI.PyTuple_Size(pyObj); i++)
        {
            PyObject value = PyObject.Create(CPythonAPI.PyTuple_GetItem(pyObj, i));
            tupleValues.Add(value);
        }

        // If we have more than 8 python values, we are going to have a nested tuple, which we have to unpack.
        if (tupleValues.Count > 8)
        {
            // We are hitting nested tuples here, which will be treated in a different way.
            object?[] firstSeven = tupleValues.Take(7).Select((p, i) => ConvertTo(context, culture, p, types[i])).ToArray();

            // Get the rest of the values and convert them to a nested tuple.
            IEnumerable<PyObject> rest = tupleValues.Skip(7);

            // Back to a Python tuple.
            using PyObject pyTuple = PyTuple.CreateTuple(rest);

            // Use the decoder pipeline to decode the nested tuple (and its values).
            // We do this because that means if we have nested nested tuples, they'll be decoded as well.
            object? nestedTuple = ConvertTo(context, culture, pyTuple, types[7]);

            // Append our nested tuple to the first seven values.
            clrValues = [.. firstSeven, nestedTuple];
        }
        else
        {
            clrValues = tupleValues.Select((p, i) => ConvertTo(context, culture, p, types[i])).ToArray();
        }

        // Dispose of all the Python values that we captured from the Tuple.
        foreach (var value in tupleValues)
        {
            value.Dispose();
        }

        if (!knownDynamicTypes.TryGetValue(destinationType, out DynamicTypeInfo? typeInfo))
        {
            ConstructorInfo ctor = destinationType.GetConstructors().First(c => c.GetParameters().Length == clrValues.Length);
            typeInfo = new(ctor);
            knownDynamicTypes[destinationType] = typeInfo;
        }

        return (ITuple)typeInfo.ReturnTypeConstructor.Invoke(clrValues);
    }
}