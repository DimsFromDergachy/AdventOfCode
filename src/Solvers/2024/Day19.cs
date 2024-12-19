namespace Year2024.Day19;

[Solver(2024, 19, Part.A)]
[Solver(2024, 19, Part.B)]
class Onsen : Solver
{
    internal Onsen(Part part) : base(part) {}

    internal override object Solve(string input)
    {
        var towels = input.Lines()
                          .First()
                          .Words([',', ' '])
                          .ToList()
                          .ToArray();

        if (Part == Part.A)
            return input.Lines()
                        .Skip(1)
                        .Count(line => IsPattern(towels, line) > 0);

        return input.Lines()
                    .Skip(1)
                    .Sum(line => IsPattern(towels, line));
    }

    long IsPattern(string[] towels, string line)
    {
        int maxk = towels.Select(towel => towel.Count()).Max();
        var dyno = new long[line.Count() + 1, maxk + 1];
        dyno[0, 0] = 1;

        for (int n = 0; n < line.Count(); n++)
        {
            var sub = line.Substring(n);
            foreach (var towel in towels)
            {
                if (sub.StartsWith(towel))
                {
                    for (int k = 0; k <= maxk; k++)
                    {
                        dyno[n + towel.Count(), towel.Count()] += dyno[n, k];
                    }
                }
            }
        }

        return Enumerable.Range(1, maxk)
                         .Sum(k => dyno[line.Count(), k]);
    }
}

public class Test
{
    [Fact]
    public void Example()
    {
        var input = @"
r, wr, b, g, bwu, rb, gb, br

brwrr
bggr
gbbr
rrbgbr
ubwu
bwurrg
brgr
bbrgwb
";
        Assert.Equal( 6, new Onsen(Part.A).Solve(input));
        Assert.Equal(16L, new Onsen(Part.B).Solve(input));
    }
}
