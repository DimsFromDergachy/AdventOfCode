namespace Year2015.Day08;

[Solver(2015, 08, Part.A)]
class SolverA : Solver
{
    internal override object Solve(string input) =>
        input.Lines()
             .Sum(line => line.Count() - line.Unescape().Count());
}

[Solver(2015, 08, Part.B)]
class SolverB : Solver
{
    internal override object Solve(string input) =>
        input.Lines()
             .Sum(line => line.Escape().Count() - line.Count());
}

static class StringExtensions
{
    internal static IEnumerable<char> Unescape(this string source)
    {
        var context = string.Empty;
        foreach (char ch in source.Skip(1).SkipLast(1))
        {
            switch (ch, context)
            {
                case ('\\', ""):
                {
                    context += ch;
                    break;
                }
                case (_, ""):
                {
                    yield return ch;
                    break;
                }
                case ('x', "\\"):
                {
                    context += ch;
                    break;
                }
                case (_, "\\"):
                {
                    yield return ch;
                    context = string.Empty;
                    break;
                }
                default:
                {
                    if (context.Count() == 3)
                    {
                        context += ch;
                        yield return '\''; // TODO: Doesn't matter context
                        context = string.Empty;
                    }
                    else
                        context += ch;
                    break;
                }
            }
        }
    }

    internal static IEnumerable<char> Escape(this string source)
    {
        yield return '\"';
        foreach (var ch in source)
        {
            switch (ch)
            {
                case '\\':
                {
                    yield return '\\';
                    yield return '\\';
                    break;
                }
                case '\"':
                {
                    yield return '\\';
                    yield return '\"';
                    break;
                }
                default:
                {
                    yield return ch;
                    break;
                }
            }
        }
        yield return '\"';
    }
}

public class Test
{
    [Theory]
    [InlineData(@"""""", "")]                   //  ""          ->
    [InlineData(@"""a""", "a")]                 //  "a"         ->  a
    [InlineData(@"""abc""", "abc")]             //  "abc"       ->  abc
    [InlineData(@"""aaa\""aaa""", "aaa\"aaa")]  //  "aaa\"aaa"  ->  aaa"aaa
    [InlineData(@"""\x27""", "'")]              //  "\x27"      ->  '
    [InlineData(@"""\\""", "\\")]               //  "\\"        ->  \
    [InlineData(@"""\\\\""", "\\\\")]           //  "\\\\"      ->  \\
    [InlineData(@"""\\x""", "\\x")]             //  "\\x"       ->  \x
    public void Unescape(string input, string expected)
    {
        Assert.StartsWith("\"", input);
        Assert.EndsWith("\"", input);
        Assert.Equal(expected, new string(input.Unescape().ToArray()));
    }

    [Theory]
    [InlineData(@"""""", @"""\""\""""")]                        //  ""          ->  "\"\""
    [InlineData(@"""abc""", @"""\""abc\""""")]                  //  "abc"       ->  "\"abc\""
    [InlineData(@"""aaa\""aaa""", @"""\""aaa\\\""aaa\""""")]    //  "aaa\"aaa"  ->  "\"aaa\\\"aaa\""
    [InlineData(@"""\x27""", @"""\""\\x27\""""")]               //  "\x27"      ->  "\"\\x27\""
    public void Escape(string input, string expected)
    {
        Assert.StartsWith("\"", input);
        Assert.EndsWith("\"", input);
        Assert.Equal(expected, new string(input.Escape().ToArray()));
    }
}