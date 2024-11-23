#pragma warning disable CS8509

[Solver(2015, 01, Part.A)]
class Solver2015_Day01_PartA : Solver
{
    internal override object Solve(string input) =>
        input.Aggregate(0, (res, ch) => ch switch
        {
            '(' => res + 1,
            ')' => res - 1,
        });
}

[Solver(2015, 01, Part.B)]
class Solver2015_Day01_PartB : Solver
{
    internal override object Solve(string input)
    {
        throw new NotImplementedException();
    }
}