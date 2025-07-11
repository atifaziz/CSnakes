using Superpower.Display;

namespace CSnakes.Parser;

public enum PythonToken
{
    [Token(Example = "|")]
    Pipe,

    [Token(Example = "(")]
    OpenParenthesis,

    [Token(Example = ")")]
    CloseParenthesis,

    [Token(Example = "[")]
    OpenBracket,

    [Token(Example = "]")]
    CloseBracket,

    [Token(Example = ":")]
    Colon,

    [Token(Example = ",")]
    Comma,

    [Token(Example = "*")]
    Asterisk,

    [Token(Example = "**")]
    DoubleAsterisk,

    Identifier,

    [Token(Category = "qualified identifier")]
    QualifiedIdentifier,

    [Token(Example = "->")]
    Arrow,

    [Token(Example = "/")]
    Slash,

    [Token(Example = "=")]
    Equal,

    [Token(Example = "def")]
    Def,

    [Token(Example = "async")]
    Async,

    Integer,
    Decimal,
    HexadecimalInteger,
    BinaryInteger,
    OctalInteger,
    DoubleQuotedString,
    SingleQuotedString,
    DoubleQuotedByteString,
    SingleQuotedByteString,
    True,
    False,
    None,

    [Token(Example = "...")]
    Ellipsis,

    [Token(Example = ", /")]
    CommaSlash,

    [Token(Example = ", *")]
    CommaStar,

    [Token(Example = ", **")]
    CommaStarStar,

    [Token(Example = ", )")]
    CommaCloseParenthesis,
}
