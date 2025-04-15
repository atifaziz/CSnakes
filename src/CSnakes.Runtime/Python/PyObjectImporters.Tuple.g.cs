// <auto-generated/>
//
// To re-generate this file, run the following command, replacing PROJECT with
// the C# project file name to which it belongs:
//
//     dotnet build -t:TransformTextTemplates PROJECT
//

#nullable enable // required for auto-generated sources (see below why)

// > Older code generation strategies may not be nullable aware. Setting the
// > project-level nullable context to "enable" could result in many
// > warnings that a user is unable to fix. To support this scenario any syntax
// > tree that is determined to be generated will have its nullable state
// > implicitly set to "disable", regardless of the overall project state.
//
// Source: https://github.com/dotnet/roslyn/blob/70e158ba6c2c99bd3c3fc0754af0dbf82a6d353d/docs/features/nullable-reference-types.md#generated-code

namespace CSnakes.Runtime.Python;
partial class PyObjectImporters
{
    public sealed class Tuple<T1, T2, TImporter1, TImporter2> :
        IPyObjectImporter<(T1, T2)>
        where TImporter1 : IPyObjectImporter<T1>
        where TImporter2 : IPyObjectImporter<T2>
    {
        static (T1, T2)
            IPyObjectImporter<(T1, T2)>.BareImport(PyObject obj)
        {
            GIL.Require();
            CheckTuple(obj);
            using var a = GetTupleItem(obj, 0);
            using var b = GetTupleItem(obj, 1);
            return (TImporter1.BareImport(a), TImporter2.BareImport(b));
        }
    }

    public sealed class Tuple<T1, T2, T3, TImporter1, TImporter2, TImporter3> :
        IPyObjectImporter<(T1, T2, T3)>
        where TImporter1 : IPyObjectImporter<T1>
        where TImporter2 : IPyObjectImporter<T2>
        where TImporter3 : IPyObjectImporter<T3>
    {
        static (T1, T2, T3)
            IPyObjectImporter<(T1, T2, T3)>.BareImport(PyObject obj)
        {
            GIL.Require();
            CheckTuple(obj);
            using var a = GetTupleItem(obj, 0);
            using var b = GetTupleItem(obj, 1);
            using var c = GetTupleItem(obj, 2);
            return (TImporter1.BareImport(a), TImporter2.BareImport(b), TImporter3.BareImport(c));
        }
    }

    public sealed class Tuple<T1, T2, T3, T4, TImporter1, TImporter2, TImporter3, TImporter4> :
        IPyObjectImporter<(T1, T2, T3, T4)>
        where TImporter1 : IPyObjectImporter<T1>
        where TImporter2 : IPyObjectImporter<T2>
        where TImporter3 : IPyObjectImporter<T3>
        where TImporter4 : IPyObjectImporter<T4>
    {
        static (T1, T2, T3, T4)
            IPyObjectImporter<(T1, T2, T3, T4)>.BareImport(PyObject obj)
        {
            GIL.Require();
            CheckTuple(obj);
            using var a = GetTupleItem(obj, 0);
            using var b = GetTupleItem(obj, 1);
            using var c = GetTupleItem(obj, 2);
            using var d = GetTupleItem(obj, 3);
            return (TImporter1.BareImport(a), TImporter2.BareImport(b), TImporter3.BareImport(c), TImporter4.BareImport(d));
        }
    }

    public sealed class Tuple<T1, T2, T3, T4, T5, TImporter1, TImporter2, TImporter3, TImporter4, TImporter5> :
        IPyObjectImporter<(T1, T2, T3, T4, T5)>
        where TImporter1 : IPyObjectImporter<T1>
        where TImporter2 : IPyObjectImporter<T2>
        where TImporter3 : IPyObjectImporter<T3>
        where TImporter4 : IPyObjectImporter<T4>
        where TImporter5 : IPyObjectImporter<T5>
    {
        static (T1, T2, T3, T4, T5)
            IPyObjectImporter<(T1, T2, T3, T4, T5)>.BareImport(PyObject obj)
        {
            GIL.Require();
            CheckTuple(obj);
            using var a = GetTupleItem(obj, 0);
            using var b = GetTupleItem(obj, 1);
            using var c = GetTupleItem(obj, 2);
            using var d = GetTupleItem(obj, 3);
            using var e = GetTupleItem(obj, 4);
            return (TImporter1.BareImport(a), TImporter2.BareImport(b), TImporter3.BareImport(c), TImporter4.BareImport(d), TImporter5.BareImport(e));
        }
    }

    public sealed class Tuple<T1, T2, T3, T4, T5, T6, TImporter1, TImporter2, TImporter3, TImporter4, TImporter5, TImporter6> :
        IPyObjectImporter<(T1, T2, T3, T4, T5, T6)>
        where TImporter1 : IPyObjectImporter<T1>
        where TImporter2 : IPyObjectImporter<T2>
        where TImporter3 : IPyObjectImporter<T3>
        where TImporter4 : IPyObjectImporter<T4>
        where TImporter5 : IPyObjectImporter<T5>
        where TImporter6 : IPyObjectImporter<T6>
    {
        static (T1, T2, T3, T4, T5, T6)
            IPyObjectImporter<(T1, T2, T3, T4, T5, T6)>.BareImport(PyObject obj)
        {
            GIL.Require();
            CheckTuple(obj);
            using var a = GetTupleItem(obj, 0);
            using var b = GetTupleItem(obj, 1);
            using var c = GetTupleItem(obj, 2);
            using var d = GetTupleItem(obj, 3);
            using var e = GetTupleItem(obj, 4);
            using var f = GetTupleItem(obj, 5);
            return (TImporter1.BareImport(a), TImporter2.BareImport(b), TImporter3.BareImport(c), TImporter4.BareImport(d), TImporter5.BareImport(e), TImporter6.BareImport(f));
        }
    }

    public sealed class Tuple<T1, T2, T3, T4, T5, T6, T7, TImporter1, TImporter2, TImporter3, TImporter4, TImporter5, TImporter6, TImporter7> :
        IPyObjectImporter<(T1, T2, T3, T4, T5, T6, T7)>
        where TImporter1 : IPyObjectImporter<T1>
        where TImporter2 : IPyObjectImporter<T2>
        where TImporter3 : IPyObjectImporter<T3>
        where TImporter4 : IPyObjectImporter<T4>
        where TImporter5 : IPyObjectImporter<T5>
        where TImporter6 : IPyObjectImporter<T6>
        where TImporter7 : IPyObjectImporter<T7>
    {
        static (T1, T2, T3, T4, T5, T6, T7)
            IPyObjectImporter<(T1, T2, T3, T4, T5, T6, T7)>.BareImport(PyObject obj)
        {
            GIL.Require();
            CheckTuple(obj);
            using var a = GetTupleItem(obj, 0);
            using var b = GetTupleItem(obj, 1);
            using var c = GetTupleItem(obj, 2);
            using var d = GetTupleItem(obj, 3);
            using var e = GetTupleItem(obj, 4);
            using var f = GetTupleItem(obj, 5);
            using var g = GetTupleItem(obj, 6);
            return (TImporter1.BareImport(a), TImporter2.BareImport(b), TImporter3.BareImport(c), TImporter4.BareImport(d), TImporter5.BareImport(e), TImporter6.BareImport(f), TImporter7.BareImport(g));
        }
    }

    public sealed class Tuple<T1, T2, T3, T4, T5, T6, T7, T8, TImporter1, TImporter2, TImporter3, TImporter4, TImporter5, TImporter6, TImporter7, TImporter8> :
        IPyObjectImporter<(T1, T2, T3, T4, T5, T6, T7, T8)>
        where TImporter1 : IPyObjectImporter<T1>
        where TImporter2 : IPyObjectImporter<T2>
        where TImporter3 : IPyObjectImporter<T3>
        where TImporter4 : IPyObjectImporter<T4>
        where TImporter5 : IPyObjectImporter<T5>
        where TImporter6 : IPyObjectImporter<T6>
        where TImporter7 : IPyObjectImporter<T7>
        where TImporter8 : IPyObjectImporter<T8>
    {
        static (T1, T2, T3, T4, T5, T6, T7, T8)
            IPyObjectImporter<(T1, T2, T3, T4, T5, T6, T7, T8)>.BareImport(PyObject obj)
        {
            GIL.Require();
            CheckTuple(obj);
            using var a = GetTupleItem(obj, 0);
            using var b = GetTupleItem(obj, 1);
            using var c = GetTupleItem(obj, 2);
            using var d = GetTupleItem(obj, 3);
            using var e = GetTupleItem(obj, 4);
            using var f = GetTupleItem(obj, 5);
            using var g = GetTupleItem(obj, 6);
            using var h = GetTupleItem(obj, 7);
            return (TImporter1.BareImport(a), TImporter2.BareImport(b), TImporter3.BareImport(c), TImporter4.BareImport(d), TImporter5.BareImport(e), TImporter6.BareImport(f), TImporter7.BareImport(g), TImporter8.BareImport(h));
        }
    }

    public sealed class Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, TImporter1, TImporter2, TImporter3, TImporter4, TImporter5, TImporter6, TImporter7, TImporter8, TImporter9> :
        IPyObjectImporter<(T1, T2, T3, T4, T5, T6, T7, T8, T9)>
        where TImporter1 : IPyObjectImporter<T1>
        where TImporter2 : IPyObjectImporter<T2>
        where TImporter3 : IPyObjectImporter<T3>
        where TImporter4 : IPyObjectImporter<T4>
        where TImporter5 : IPyObjectImporter<T5>
        where TImporter6 : IPyObjectImporter<T6>
        where TImporter7 : IPyObjectImporter<T7>
        where TImporter8 : IPyObjectImporter<T8>
        where TImporter9 : IPyObjectImporter<T9>
    {
        static (T1, T2, T3, T4, T5, T6, T7, T8, T9)
            IPyObjectImporter<(T1, T2, T3, T4, T5, T6, T7, T8, T9)>.BareImport(PyObject obj)
        {
            GIL.Require();
            CheckTuple(obj);
            using var a = GetTupleItem(obj, 0);
            using var b = GetTupleItem(obj, 1);
            using var c = GetTupleItem(obj, 2);
            using var d = GetTupleItem(obj, 3);
            using var e = GetTupleItem(obj, 4);
            using var f = GetTupleItem(obj, 5);
            using var g = GetTupleItem(obj, 6);
            using var h = GetTupleItem(obj, 7);
            using var i = GetTupleItem(obj, 8);
            return (TImporter1.BareImport(a), TImporter2.BareImport(b), TImporter3.BareImport(c), TImporter4.BareImport(d), TImporter5.BareImport(e), TImporter6.BareImport(f), TImporter7.BareImport(g), TImporter8.BareImport(h), TImporter9.BareImport(i));
        }
    }

    public sealed class Tuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TImporter1, TImporter2, TImporter3, TImporter4, TImporter5, TImporter6, TImporter7, TImporter8, TImporter9, TImporter10> :
        IPyObjectImporter<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)>
        where TImporter1 : IPyObjectImporter<T1>
        where TImporter2 : IPyObjectImporter<T2>
        where TImporter3 : IPyObjectImporter<T3>
        where TImporter4 : IPyObjectImporter<T4>
        where TImporter5 : IPyObjectImporter<T5>
        where TImporter6 : IPyObjectImporter<T6>
        where TImporter7 : IPyObjectImporter<T7>
        where TImporter8 : IPyObjectImporter<T8>
        where TImporter9 : IPyObjectImporter<T9>
        where TImporter10 : IPyObjectImporter<T10>
    {
        static (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)
            IPyObjectImporter<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)>.BareImport(PyObject obj)
        {
            GIL.Require();
            CheckTuple(obj);
            using var a = GetTupleItem(obj, 0);
            using var b = GetTupleItem(obj, 1);
            using var c = GetTupleItem(obj, 2);
            using var d = GetTupleItem(obj, 3);
            using var e = GetTupleItem(obj, 4);
            using var f = GetTupleItem(obj, 5);
            using var g = GetTupleItem(obj, 6);
            using var h = GetTupleItem(obj, 7);
            using var i = GetTupleItem(obj, 8);
            using var j = GetTupleItem(obj, 9);
            return (TImporter1.BareImport(a), TImporter2.BareImport(b), TImporter3.BareImport(c), TImporter4.BareImport(d), TImporter5.BareImport(e), TImporter6.BareImport(f), TImporter7.BareImport(g), TImporter8.BareImport(h), TImporter9.BareImport(i), TImporter10.BareImport(j));
        }
    }
}
