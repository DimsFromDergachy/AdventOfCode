#pragma warning disable CS8509

class Solver2015_Day01_PartA : Solver
{
    internal override object Solve(string input) =>
        input.Aggregate(0, (res, ch) => ch switch
        {
            '(' => res + 1,
            ')' => res - 1,
        });
}

class Solver2015_Day01_PartB : Solver
{
    internal override object Solve(string input)
    {
        throw new NotImplementedException();
    }
}