#pragma warning disable CS8509

namespace Solver;

public class Solver<Y2015, Day01, PartA> : Solver
{
    public override object Solve(string input) =>
        input.Aggregate(0, (res, ch) => ch switch
        {
            '(' => res + 1,
            ')' => res - 1,
        });
}
