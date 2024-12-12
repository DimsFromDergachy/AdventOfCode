abstract class Solver
{
    internal Part Part { get; set; } = Part.A;

    public Solver() {}
    public Solver(Part part) { Part = part; }

    internal abstract object Solve(string input);
}