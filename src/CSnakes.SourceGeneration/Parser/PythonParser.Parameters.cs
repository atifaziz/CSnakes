using CSnakes.Parser.Types;
using Superpower;
using Superpower.Parsers;

namespace CSnakes.Parser;
public static partial class PythonParser
{
    public static TokenListParser<PythonToken, (string Name, PythonTypeSpec? Type)>
        PythonParameterParser { get; } =
            (from arg in PythonNormalArgParser
             from type in Token.EqualTo(PythonToken.Colon)
                               .IgnoreThen(PythonTypeDefinitionParser.AssumeNotNull())
                               .OptionalOrDefault()
             select (arg.Name, type))
           .Named("Parameter");

    public static TokenListParser<PythonToken, (string Name, PythonTypeSpec? TypeSpec, PythonConstant? DefaultValue)>
        OptionalPythonParameterParser { get; } =
            (from param in PythonParameterParser
             from defaultValue in Token.EqualTo(PythonToken.Equal)
                                       .IgnoreThen(ConstantValueTokenizer.AssumeNotNull())
                                       .OptionalOrDefault()
             select (param.Name, param.Type, defaultValue))
            .Named("Parameter");

    public static TokenListParser<PythonToken, PythonFunctionParameterList> PythonParameterListParser { get; } =
        CreatePythonParameterListParser();

    public static TokenListParser<PythonToken, PythonFunctionParameterList> CreatePythonParameterListParser()
    {
        /*
         parameter_list            ::=  defparameter ("," defparameter)* "," "/" ["," [parameter_list_no_posonly]]
                                     |  parameter_list_no_posonly
         parameter_list_no_posonly ::=  defparameter ("," defparameter)* ["," [parameter_list_starargs]]
                                     |  parameter_list_starargs
         parameter_list_starargs   ::=  "*" [star_parameter] ("," defparameter)* ["," ["**" parameter [","]]]
                                     |  "**" parameter [","]
         */

        var comma = Token.EqualTo(PythonToken.Comma);

        var commaParameters =
            comma.IgnoreThen(OptionalPythonParameterParser.Select(p => new PythonFunctionParameter.Keyword(p.Name, p.TypeSpec, p.DefaultValue))
                                                          .AtLeastOnceDelimitedBy(comma));

        var optionalKwargParameterParser =
            Token.EqualTo(PythonToken.CommaStarStar)
                 .IgnoreThen(PythonParameterParser.Select(p => new PythonFunctionParameter.VariadicKeyword(p.Name, p.Type))
                                                  .AsNullable())
                 .OptionalOrDefault();

        var starParameters =
            Parse.OneOf(from vp in PythonParameterParser
                        from ks in commaParameters.OptionalOrDefault([])
                        select new PythonFunctionParameterList(varpos: (true, new PythonFunctionParameter.VariadicPositional(vp.Name, vp.Type)),
                                                               keyword: [..ks]),
                        from ks in commaParameters
                        select new PythonFunctionParameterList(keyword: [..ks]));

        var namedArgParameterParser =
            from namedArgParameters in
                Token.EqualTo(PythonToken.CommaStar)
                     .IgnoreThen(starParameters)
                     .OptionalOrDefault(PythonFunctionParameterList.Empty)
            from kwargParameter in optionalKwargParameterParser
            select kwargParameter is { } some ? namedArgParameters.WithVariadicKeyword((true, some)) : namedArgParameters;

        var b =
            from rps in OptionalPythonParameterParser.Select(p => new PythonFunctionParameter.Normal(p.Name, p.TypeSpec, p.DefaultValue))
                                                     .ManyDelimitedBy(comma)
            from ps in namedArgParameterParser
            select ps.WithRegular([.. rps]);

        // ( arg "," )+ ", /" ( ( "," arg )* ( ", *" ( "," arg )+ )? )?
        var a =
            from pps in OptionalPythonParameterParser.Select(p => new PythonFunctionParameter.Positional(p.Name, p.TypeSpec, p.DefaultValue))
                                                     .AtLeastOnceDelimitedBy(comma)
                                                     .ThenIgnore(Token.EqualTo(PythonToken.CommaSlash))
            from ps in Parse.OneOf(comma.IgnoreThen(b), namedArgParameterParser)
            select ps.WithPositional([.. pps]);

        return Parse.OneOf(// "**" ...
                           from vkp in Token.EqualTo(PythonToken.DoubleAsterisk)
                                            .IgnoreThen(PythonParameterParser)
                           select new PythonFunctionParameterList(varkw: (true, new PythonFunctionParameter.VariadicKeyword(vkp.Name, vkp.Type))),
                           // "*" ...
                           from kps in Token.EqualTo(PythonToken.Asterisk)
                                            .IgnoreThen(starParameters)
                           from vkp in optionalKwargParameterParser
                           select vkp is { } some ? kps.WithVariadicKeyword((true, some)) : kps,
                           // ( arg "," )+ "/" ...
                           a.Try(),
                           b)
                    .Between(Token.EqualTo(PythonToken.OpenParenthesis),
                             Token.EqualTo(PythonToken.CloseParenthesis).Or(Token.EqualTo(PythonToken.CommaCloseParenthesis)))
                    .Named("Parameter List");
    }
}
