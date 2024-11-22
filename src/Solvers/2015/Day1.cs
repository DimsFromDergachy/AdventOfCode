#pragma warning disable CS8509

namespace Solver;

public class Solver<Y2015, D01, PartA> : SolverBase<Y2015, D01, PartA>
{
    public override object Solve(string input) =>
        input.Aggregate(0, (res, ch) => ch switch
        {
            '(' => res + 1,
            ')' => res - 1,
        });
}

public class Solver<Y2015, D01, PartB> : SolverBase<Y2015, D01, PartB>
{
    public override object Solve(string input)
    {
        throw new NotImplementedException();
    }
}