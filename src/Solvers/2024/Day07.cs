namespace Year2024.Day07;

[Solver(2024, 07, Part.A)]
[Solver(2024, 07, Part.B)]
class Repairer : Solver
{
    public Repairer() {}
    internal Repairer(Part part) { Part = part; }

    internal override object Solve(string input) =>
        input.Lines()
             .Select(Parse)
             .Where(eq => Check(eq.result, eq.args))
             .Sum(eq => eq.result);

    (long result, long[] args) Parse(string line) =>
            (long.Parse(line.Substring(0, line.IndexOf(':'))),
             line.Substring(2 + line.IndexOf(':'))
                 .Words()
                 .Select(long.Parse)
                 .ToArray());

    bool Check(long exp, long[] args) => Results(exp, 0, args).Any(r => r == exp);

    IEnumerable<long> Results(long exp, long sum, IEnumerable<long> args)
    {
        if (sum > exp)
            yield break;

        if (args.Any())
        {
            foreach (var r in Results(exp, sum + args.First(), args.Skip(1)))
                yield return r;
            foreach (var r in Results(exp, sum * args.First(), args.Skip(1)))
                yield return r;
            if (Part == Part.B)
                foreach (var r in Results(exp, long.Parse(sum.ToString() + args.First().ToString()), args.Skip(1)))
                    yield return r;
        } else {
            yield return sum;
        }
    }
}

public class RepairerTest
{
    [Fact]
    internal void ExampleA()
    {
        var input = @"
190: 10 19
3267: 81 40 27
83: 17 5
156: 15 6
7290: 6 8 6 15
161011: 16 10 13
192: 17 8 14
21037: 9 7 18 13
292: 11 6 16 20
";

        Assert.Equal((long)3749, new Repairer(Part.A).Solve(input));
    }

    [Fact]
    internal void ExampleB()
    {
        var input = @"
190: 10 19
3267: 81 40 27
292: 11 6 16 20
156: 15 6
7290: 6 8 6 15
192: 17 8 14";

        Assert.Equal((long)11387, new Repairer(Part.B).Solve(input));
    }
}
