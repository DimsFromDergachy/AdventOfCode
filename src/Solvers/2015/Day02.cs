namespace Year2015.Day02;

[Solver(2015, 02, Part.A)]
class SolverA : Solver
{
    internal override object Solve(string input) =>
        input.Lines()
             .Select(line => line.Split('x').Parse<int>().ToArray())
             .Select(Wrapping)
             .Sum();

    int Wrapping(params int[] args)
    {
        var (a, b, c) = (args[0], args[1], args[2]);
        return 2 * (a * b + b * c + a * c)
                 + (a * b * c / args.Max());
    }
}

[Solver(2015, 02, Part.B)]
class SolverB : Solver
{
    internal override object Solve(string input) =>
        input.Lines()
             .Select(line => line.Words('x').Parse<int>().ToArray())
             .Select(Ribboning)
             .Sum();

    int Ribboning(params int[] args)
    {
        var (a, b, c) = (args[0], args[1], args[2]);
        return 2 * (a + b + c - args.Max())
                 + (a * b * c);
    }
}

public class SolverTest
{
    [Fact]
    internal void Example()
    {
        var input = @"
2x3x4
1x1x10";

        Assert.Equal(101, new SolverA().Solve(input));
        Assert.Equal( 48, new SolverB().Solve(input));
    }
}