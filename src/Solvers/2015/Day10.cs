using System.Text;

namespace Year2015.Day10;

[Solver(2015, 10, Part.A)]
[Solver(2015, 10, Part.B)]
class LookAndSayGamer : Solver
{
    public LookAndSayGamer() {}
    internal LookAndSayGamer(Part part) { Part = part; }

    int Count
    {
        #pragma warning disable CS8524
        get => Part switch
        {
            Part.A => 40,
            Part.B => 50,
        };
    }

    internal override object Solve(string input) =>
        Enumerable.Range(0, Count)
                  .Aggregate(input, (s, _) => Step(s))
                  .Count();

    internal string Step(string input) =>
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
    internal void Step(string input, string expected)
    {
        var solver = new LookAndSayGamer();
        Assert.Equal(expected, solver.Step(input));
    }

    [Theory]
    [InlineData(Part.A, "3113322113", 329356)]
    [InlineData(Part.B, "3113322113", 4666278)]
    internal void Example(Part part, string input, int expected)
    {
        Assert.Equal(expected, new LookAndSayGamer(part).Solve(input));
    }
}