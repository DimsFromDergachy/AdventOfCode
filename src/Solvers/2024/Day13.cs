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

    long[] Parse(string[] line)
    {
        var a = A.Match(line[0]).Groups;
        var b = B.Match(line[1]).Groups;
        var p = P.Match(line[2]).Groups;

        return new string[] {
                a[1].Value, a[2].Value,
                b[1].Value, b[2].Value,
                p[1].Value, p[2].Value,
            }
            .Select(x => long.Parse(x))
            .ToArray();
    }

    IEnumerable<(long A, long B)> Solve(long[] pz)
    {
        pz[4] += 10000000000000;
        pz[5] += 10000000000000;
        long A, B;
        if ((pz[0] * pz[3] - pz[1] * pz[2]) == 0)
        {
            if (pz[0] > 3 * pz[2])
            {
                A = pz[4] / pz[0];
                B = pz[4] % pz[0] / pz[2];
            }
            else
            {
                A = pz[4] % pz[2] / pz[0];
                B = pz[4] / pz[2];
            }
        }
        else
        {
            A = (pz[4] * pz[3] - pz[5] * pz[2]) / (pz[0] * pz[3] - pz[1] * pz[2]);
            B = (pz[4] * pz[1] - pz[5] * pz[0]) / (pz[1] * pz[2] - pz[0] * pz[3]);
        }

        if (A >= 0 && B >= 0
            &&
            A * pz[0] + B * pz[2] == pz[4]
            &&
            A * pz[1] + B * pz[3] == pz[5])
            yield return (A, B);
    }

}

public class WinnerTest
{
    [Fact]
    internal void ExampleB()
    {
        var input = @"
Button A: X+94, Y+34
Button B: X+22, Y+67
Prize: X=10000000008400, Y=10000000005400

Button A: X+26, Y+66
Button B: X+67, Y+21
Prize: X=10000000012748, Y=10000000012176

Button A: X+17, Y+86
Button B: X+84, Y+37
Prize: X=10000000007870, Y=10000000006450

Button A: X+69, Y+23
Button B: X+27, Y+71
Prize: X=10000000018641, Y=10000000010279
";
        //Assert.Equal(480L, new Winner(Part.B).Solve(input));
    }

    [Fact]
    internal void ExampleA()
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
        Assert.Equal(480L, new Winner(Part.A).Solve(input));
    }

    [Fact]
    public void SpecialCase()
    {
        var input = @"
Button A: X+1, Y+2
Button B: X+2, Y+4
Prize: X=0, Y=0";
        Assert.Equal(0L, new Winner(Part.A).Solve(input));

        input = @"
Button A: X+1, Y+2
Button B: X+2, Y+4
Prize: X=1, Y=1";
        Assert.Equal(0L, new Winner(Part.A).Solve(input));

        input = @"
Button A: X+1, Y+2
Button B: X+2, Y+4
Prize: X=1, Y=2";
        Assert.Equal(3L, new Winner(Part.A).Solve(input));

        input = @"
Button A: X+1, Y+2
Button B: X+2, Y+4
Prize: X=2, Y=4";
        Assert.Equal(1L, new Winner(Part.A).Solve(input));

        input = @"
Button A: X+1, Y+2
Button B: X+2, Y+4
Prize: X=15, Y=30";
        Assert.Equal(3L + 7L, new Winner(Part.A).Solve(input));
    }
}
