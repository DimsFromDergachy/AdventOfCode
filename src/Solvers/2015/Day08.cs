namespace Year2015.Day08;

[Solver(2015, 08, Part.A)]
class SolverA : Solver
{
    internal override object Solve(string input) =>
        input.Lines()
             .Select(line => 2 + line.Count() - line.Unescape().Count())
             .Sum();
}

internal static class StringExtensions
{
    #pragma warning disable CS8509
    internal static IEnumerable<char> Unescape(this string source)
    {
        var context = string.Empty;
        foreach (char ch in source)
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
                        yield return '#'; // TODO: Doesn't matter context
                        context = string.Empty;
                    }
                    else
                        context += ch;
                    break;
                }
            }
        }
    }
}

public class Test
{
    [Theory]
    [InlineData(@"""""", 2)]            //  ""          ->
    [InlineData(@"""a""", 2)]           //  "a"         ->  a
    [InlineData(@"""abc""", 2)]         //  "abc"       ->  abc
    [InlineData(@"""aaa\""aaa""", 3)]   //  "aaa\"aaa"  ->  aaa"aaa
    [InlineData(@"""\x27""", 5)]        //  "\x27"      ->  '
    [InlineData(@"""\\""", 3)]          //  "\\"        ->  \
    [InlineData(@"""\\\\""", 4)]        //  "\\\\"      ->  \\
    [InlineData(@"""\\x""", 3)]         //  "\\x"       ->  \x
    public void Example(string input, int expected)
    {
        Assert.StartsWith("\"", input);
        Assert.EndsWith("\"", input);
        Assert.Equal(expected, new SolverA().Solve(input));
    }
}