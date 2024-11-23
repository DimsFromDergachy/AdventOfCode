abstract class Solver
{
    internal Part Part { get; set; } = Part.A;

    internal abstract object Solve(string input);
}