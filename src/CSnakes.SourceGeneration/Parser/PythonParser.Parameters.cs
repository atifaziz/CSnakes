using CSnakes.Parser.Types;
using Superpower;
using Superpower.Parsers;

namespace CSnakes.Parser;
public static partial class PythonParser
{
    public static TokenListParser<PythonToken, (string Name, PythonTypeSpec? Type)>
        BasePythonParameterParser { get; } =
            (from arg in PythonNormalArgParser
             from type in Token.EqualTo(PythonToken.Colon)
                               .IgnoreThen(PythonTypeDefinitionParser.AssumeNotNull())
                               .OptionalOrDefault()
             select (arg.Name, type))
           .Named("Parameter");

    public static TokenListParser<PythonToken, (string Name, PythonTypeSpec? Type, PythonConstant? Default)>
        PythonParameterParser { get; } =
            (from param in BasePythonParameterParser
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
            comma.IgnoreThen(PythonParameterParser.Select(p => new PythonFunctionParameter.Keyword(p.Name, p.Type, p.Default))
                                                  .AtLeastOnceDelimitedBy(comma));

        var optionalKwargParameterParser =
            Token.EqualTo(PythonToken.CommaStarStar)
                 .IgnoreThen(BasePythonParameterParser.Select(p => new PythonFunctionParameter.VariadicKeyword(p.Name, p.Type))
                                                      .AsNullable())
                 .OptionalOrDefault();

        var starParameters =
            Parse.OneOf(from vp in BasePythonParameterParser
                        from ks in commaParameters.OptionalOrDefault([])
                        select new PythonFunctionParameterList(varpos: new PythonFunctionParameter.VariadicPositional(vp.Name, vp.Type),
                                                               keyword: [..ks]),
                        from ks in commaParameters
                        select new PythonFunctionParameterList(keyword: [..ks]));

        var namedArgParameterParser =
            from namedArgParameters in
                Token.EqualTo(PythonToken.CommaStar)
                     .IgnoreThen(starParameters)
                     .OptionalOrDefault(PythonFunctionParameterList.Empty)
            from kwargParameter in optionalKwargParameterParser
            select namedArgParameters.WithVariadicKeyword(kwargParameter);

        var b =
            from rps in PythonParameterParser.Select(p => new PythonFunctionParameter.Normal(p.Name, p.Type, p.Default))
                                             .ManyDelimitedBy(comma)
            from ps in namedArgParameterParser
            select ps.WithRegular([.. rps]);

        // ( arg "," )+ ", /" ( ( "," arg )* ( ", *" ( "," arg )+ )? )?
        var a =
            from pps in PythonParameterParser.Select(p => new PythonFunctionParameter.Positional(p.Name, p.Type, p.Default))
                                             .AtLeastOnceDelimitedBy(comma)
                                             .ThenIgnore(Token.EqualTo(PythonToken.CommaSlash))
            from ps in Parse.OneOf(comma.IgnoreThen(b), namedArgParameterParser)
            select ps.WithPositional([.. pps]);

        return Parse.OneOf(// "**" ...
                           from vkp in Token.EqualTo(PythonToken.DoubleAsterisk)
                                            .IgnoreThen(BasePythonParameterParser)
                           select new PythonFunctionParameterList(varkw: new PythonFunctionParameter.VariadicKeyword(vkp.Name, vkp.Type)),
                           // "*" ...
                           from kps in Token.EqualTo(PythonToken.Asterisk)
                                            .IgnoreThen(starParameters)
                           from vkp in optionalKwargParameterParser
                           select kps.WithVariadicKeyword(vkp),
                           // ( arg "," )+ "/" ...
                           a.Try(),
                           b)
                    .Between(Token.EqualTo(PythonToken.OpenParenthesis),
                             Token.EqualTo(PythonToken.CloseParenthesis).Or(Token.EqualTo(PythonToken.CommaCloseParenthesis)))
                    .Named("Parameter List");
    }
}
