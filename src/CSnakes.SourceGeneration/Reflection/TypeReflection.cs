using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using CSnakes.Parser.Types;
using System.ComponentModel;

namespace CSnakes.Reflection;

public static class TypeReflection
{
    public enum ConversionDirection
    {
        ToPython,
        FromPython
    }

    public static TypeSyntax AsPredefinedType(PythonTypeSpec pythonType, ConversionDirection direction)
    {
        // If type is an alias, e.g. "list[int]", "list[float]", etc.
        if (pythonType.HasArguments())
        {
            // Get last occurrence of ] in pythonType
            return pythonType.Name.Replace("typing.", "") switch
            {
                "list" => CreateListType(pythonType.Arguments[0], direction),
                "List" => CreateListType(pythonType.Arguments[0], direction),
                "Tuple" => CreateTupleType(pythonType.Arguments, direction),
                "tuple" => CreateTupleType(pythonType.Arguments, direction),
                "dict" => CreateDictionaryType(pythonType.Arguments[0], pythonType.Arguments[1], direction),
                "Dict" => CreateDictionaryType(pythonType.Arguments[0], pythonType.Arguments[1], direction),
                "Mapping" => CreateDictionaryType(pythonType.Arguments[0], pythonType.Arguments[1], direction),
                "Sequence" => CreateListType(pythonType.Arguments[0], direction),
                "Optional" => AsPredefinedType(pythonType.Arguments[0], direction),
                "Generator" => CreateGeneratorType(pythonType.Arguments[0], pythonType.Arguments[1], pythonType.Arguments[2], direction),
                
                // Todo more types... see https://docs.python.org/3/library/stdtypes.html#standard-generic-classes
                _ => SyntaxFactory.ParseTypeName("PyObject"),
            };
        }
        return (pythonType.Name, direction) switch
        {
            ("int", _) => SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.LongKeyword)),
            ("str", _) => SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.StringKeyword)),
            ("float", _) => SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.DoubleKeyword)),
            ("bool", _) => SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.BoolKeyword)),
            ("bytes", _) => SyntaxFactory.ParseTypeName("byte[]"),
            ("Buffer", ConversionDirection.FromPython) => SyntaxFactory.ParseTypeName("IPyBuffer"),
            ("typing.Buffer", ConversionDirection.FromPython) => SyntaxFactory.ParseTypeName("IPyBuffer"),
            ("collections.abc.Buffer", ConversionDirection.FromPython) => SyntaxFactory.ParseTypeName("IPyBuffer"),
            _ => SyntaxFactory.ParseTypeName("PyObject"),
        };
    }

    private static TypeSyntax CreateDictionaryType(PythonTypeSpec keyType, PythonTypeSpec valueType, ConversionDirection direction) => CreateGenericType("IReadOnlyDictionary", [
            AsPredefinedType(keyType, direction),
            AsPredefinedType(valueType, direction)
            ]);

    private static TypeSyntax CreateGeneratorType(PythonTypeSpec yieldType, PythonTypeSpec sendType, PythonTypeSpec returnType, ConversionDirection direction) => CreateGenericType("IGeneratorIterator", [
            AsPredefinedType(yieldType, direction),
            AsPredefinedType(sendType, direction),
            AsPredefinedType(returnType, direction)
            ]);

    private static TypeSyntax CreateTupleType(PythonTypeSpec[] tupleTypes, ConversionDirection direction)
    {
        if (tupleTypes.Length == 1)
        {
            return CreateGenericType("ValueTuple", tupleTypes.Select(t => AsPredefinedType(t, direction)));
        }

        IEnumerable<TupleElementSyntax> tupleTypeSyntaxGroups = tupleTypes.Select((x, i) => new { Index = i, Value = x })
            .GroupBy(x => x.Index / 7)
            .Select(x => x.Select(v => v.Value))
            .Select(typeSpecs => typeSpecs.Select(t => AsPredefinedType(t, direction)))
            .SelectMany(item => item.Select(SyntaxFactory.TupleElement));

        return SyntaxFactory.TupleType(
            SyntaxFactory.Token(SyntaxKind.OpenParenToken),
            SyntaxFactory.SeparatedList(tupleTypeSyntaxGroups),
            SyntaxFactory.Token(SyntaxKind.CloseParenToken));
    }

    private static TypeSyntax CreateListType(PythonTypeSpec genericOf, ConversionDirection direction) => CreateGenericType("IReadOnlyList", [AsPredefinedType(genericOf, direction)]);

    internal static TypeSyntax CreateGenericType(string typeName, IEnumerable<TypeSyntax> genericArguments) =>
        SyntaxFactory.GenericName(
            SyntaxFactory.Identifier(typeName))
            .WithTypeArgumentList(
                SyntaxFactory.TypeArgumentList(
                    SyntaxFactory.SeparatedList(
                        genericArguments)));
}