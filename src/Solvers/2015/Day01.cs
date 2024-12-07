namespace Year2015.Day01;

[Solver(2015, 01, Part.A)]
class SolverA : Solver
{
    internal override object Solve(string input) =>
        input.Aggregate(0, Move);

    #pragma warning disable CS8509
    int Move(int res, char ch) => ch switch
    {
        '(' => res + 1,
        ')' => res - 1,
    };
}

[Solver(2015, 01, Part.B)]
class SolverB : Solver
{
    internal override object Solve(string input) =>
        input.Scan(0, Move)
            .TakeWhile(x => x != -1)
            .Count();

    #pragma warning disable CS8509
    int Move(int res, char ch) => ch switch
    {
        '(' => res + 1,
        ')' => res - 1,
    };
}
