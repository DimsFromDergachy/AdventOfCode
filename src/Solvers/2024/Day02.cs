namespace Year2024.Day02;

[Solver(2024, 02, Part.A)]
[Solver(2024, 02, Part.B)]
class RedNosedChecker : Solver
{
    internal RedNosedChecker(Part part) : base(part) {}

    internal override object Solve(string input) =>
        input.Lines()
             .Select(line => line.Parse<int>())
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

public class RedNosedTest
{
    [Fact]
    internal void Example()
    {
        var input = @"
7 6 4 2 1
1 2 7 8 9
9 7 6 2 1
1 3 2 4 5
8 6 4 4 1
1 3 6 7 9
";

        Assert.Equal(2, new RedNosedChecker(Part.A).Solve(input));
        Assert.Equal(4, new RedNosedChecker(Part.B).Solve(input));
    }
}