namespace Year2015.Day02;

[Solver(2015, 02, Part.A)]
class SolverA : Solver
{
    internal override object Solve(string input) =>
        input.Split(['x', '\r', '\n'], StringSplitOptions.RemoveEmptyEntries)
             .Select(int.Parse)
             .Chunk(3)
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
        input.Split(['x', '\r', '\n'], StringSplitOptions.RemoveEmptyEntries)
             .Select(int.Parse)
             .Chunk(3)
             .Select(Ribboning)
             .Sum();

    int Ribboning(params int[] args)
    {
        var (a, b, c) = (args[0], args[1], args[2]);
        return 2 * (a + b + c - args.Max())
                 + (a * b * c);
    }
}