namespace AdventOfCode.Solver;

public abstract class Puzzle<TYear, TDay, TInput, TResult>
{
    public abstract TResult SolveA(TInput input);
    public abstract TResult SolveB(TInput input);
}