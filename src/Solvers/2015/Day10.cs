using System.Text;

namespace Year2015.Day10;

[Solver(2015, 10, Part.A)]
[Solver(2015, 10, Part.B)]
class LookAndSayGamer : Solver
{
    internal LookAndSayGamer(Part part) : base(part) {}

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
        Assert.Equal(expected, new LookAndSayGamer(Part.A).Step(input));
    }

    [Fact]
    internal void Example()
    {
        var input = "3113322113";
        Assert.Equal(329356, new LookAndSayGamer(Part.A).Solve(input));
        Assert.Equal(4666278, new LookAndSayGamer(Part.B).Solve(input));
    }
}