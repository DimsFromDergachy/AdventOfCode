namespace Year2025.Day01;

[Solver(2025, 01, Part.A)]
class Day01 : Solver
{
    internal Day01(Part part) : base(part) { }

    internal override object Solve(string input)
    {
        throw new NotImplementedException("Coming soon ...");
    }
}


public class Test
{
    [Fact]
    public void Example()
    {
        var input = @"
some input 1
some input 2
";

        Assert.Fail("DO NOT TEST ME (!!!)");

        Assert.Equal(11, new Day01(Part.A).Solve(input));
        Assert.Equal(31, new Day01(Part.B).Solve(input));
    }
}