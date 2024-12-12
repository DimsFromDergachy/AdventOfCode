namespace Year2015.Day01;

[Solver(2015, 01, Part.A)]
[Solver(2015, 01, Part.B)]
class Walker : Solver
{
    internal Walker(Part part) : base(part) {}

    #pragma warning disable CS8524
    internal override object Solve(string input) =>
        Part switch {
            Part.A => input.Aggregate(0, Move),
            Part.B => input.Scan(0, Move)
                           .TakeWhile(x => x != -1)
                           .Count(),
        };


    #pragma warning disable CS8509
    int Move(int res, char ch) => ch switch
    {
        '(' => res + 1,
        ')' => res - 1,
    };
}

public class SolverTest
{
    [Theory]
    [InlineData( 0, "(())")]
    [InlineData( 0, "()()")]
    [InlineData( 3, "(((")]
    [InlineData( 3, "(()(()(")]
    [InlineData( 3, "))(((((")]
    [InlineData(-1, "())")]
    [InlineData(-1, "))(")]
    [InlineData(-3, ")))")]
    [InlineData(-3, ")())())")]
    internal void ExampleA(int expected, string input)
    {
        Assert.Equal(expected, new Walker(Part.A).Solve(input));
    }

    [Theory]
    [InlineData(1, ")")]
    [InlineData(5, "()())")]
    internal void ExampleB(int expected, string input)
    {
        Assert.Equal(expected, new Walker(Part.B).Solve(input));
    }
}
