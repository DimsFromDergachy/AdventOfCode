namespace Year2024.Day02;

[Solver(2024, 02, Part.A)]
[Solver(2024, 02, Part.B)]
class RedNosedChecker : Solver
{
    internal override object Solve(string input) =>
        input.Lines()
             .Select(line => line.Words().Select(int.Parse))
             .Count(Part == Part.A ? IsSave : IsSaver);

    bool IsSave(IEnumerable<int> report) =>
        report.Zip(report.Skip(1), (a, b) => a - b)
              .All(x => 1 <= x && x <= 3)
            ||
        report.Zip(report.Skip(1), (a, b) => b - a)
              .All(x => 1 <= x && x <= 3);

    bool IsSaver(IEnumerable<int> report) =>
        report.Extract()
              .Select(pair => pair.Item2)
              .Any(IsSave);
}