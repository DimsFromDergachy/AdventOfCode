abstract class Solver
{
    protected Part _part;

    internal Solver(Part part = Part.A) => _part = part;

    internal abstract object Solve(string input);
}