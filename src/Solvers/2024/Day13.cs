using System.Text.RegularExpressions;

namespace Year2024.Day13;

[Solver(2024, 13, Part.A)]
class Winner : Solver
{
    Regex A = new Regex(@"Button A: X\+(\d+), Y\+(\d+)");
    Regex B = new Regex(@"Button B: X\+(\d+), Y\+(\d+)");
    Regex P = new Regex(@"Prize: X=(\d+), Y=(\d+)");

    internal Winner(Part part) : base(part) {}

    internal override object Solve(string input)
    {
        return input.Lines()
                    .Chunk(3)
                    .Select(Parse)
                    .Select(pzl => Solve(pzl).FirstOrDefault())
                    .Sum(pair => 3 * pair.A + pair.B);
    }

    int[] Parse(string[] line)
    {
        var a = A.Match(line[0]).Groups;
        var b = B.Match(line[1]).Groups;
        var p = P.Match(line[2]).Groups;

        return new string[] {
                a[1].Value, a[2].Value,
                b[1].Value, b[2].Value,
                p[1].Value, p[2].Value,
            }
            .Select(x => int.Parse(x))
            .ToArray();
    }

    IEnumerable<(int A, int B)> Solve(int[] puzzle) =>
        Enumerable.Range(0, 101)
                  .Select(A => (A, B: (puzzle[4] - A * puzzle[0]) / puzzle[2]))
                  .Where(pair => pair.A * puzzle[0] + pair.B * puzzle[2] == puzzle[4]
                                    &&
                                 pair.A * puzzle[1] + pair.B * puzzle[3] == puzzle[5]);
}

public class WinnerTest
{
    [Fact]
    internal void Example()
    {
        var input = @"
Button A: X+94, Y+34
Button B: X+22, Y+67
Prize: X=8400, Y=5400

Button A: X+26, Y+66
Button B: X+67, Y+21
Prize: X=12748, Y=12176

Button A: X+17, Y+86
Button B: X+84, Y+37
Prize: X=7870, Y=6450

Button A: X+69, Y+23
Button B: X+27, Y+71
Prize: X=18641, Y=10279
";
        Assert.Equal(480, new Winner(Part.A).Solve(input));
    }
}
