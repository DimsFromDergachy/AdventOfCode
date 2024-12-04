using System.Text;

namespace Year2015.Day10;

[Solver(2015, 10, Part.A)]
class LookAndSayGamer : Solver
{
    internal int Count {get; set; }= 40;

    internal override object Solve(string input) =>
        Enumerable.Range(0, Count)
                  .Aggregate(input, (s, _) => Step(s))
                  .Count();

    string Step(string input) =>
        input.Group()
             .SelectMany(gr => gr.Count().ToString() + gr.First())
             .Aggregate(new StringBuilder(), (b, ch) => b.Append(ch))
             .ToString();
}

public class LookAndSayGamerTest
{
    [Theory]
    [InlineData("1", "11")]
    [InlineData("11", "21")]
    [InlineData("21", "1211")]
    [InlineData("1211", "111221")]
    [InlineData("111221", "312211")]
    public void Example(string input, string expected)
    {
        var solver = new LookAndSayGamer();
        solver.Count = 1;
        Assert.Equal(expected, solver.Solve(input));
    }
}