namespace Year2025.Day03;

[Solver(2025, 03, Part.A)]
[Solver(2025, 03, Part.B)]
class Battery : Solver
{
    internal Battery(Part part) : base(part) { }

    internal override object Solve(string input)
        => input.Lines()
                .Select(Parse)
                .Sum(line => Solve(line, Part == Part.A ? 2 : 12));

    private long[] Parse(string line)
        => line.ToArray()
               .Select(ch => long.Parse(ch.ToString()))
               .Prepend(0)
               .ToArray();

    private long Solve(long[] o, int steps)
    {
        var a = new long[o.Length];

        for (int i = 0; i < steps; i++)
            (o, a) = DoStep(o, a);

        return a.Last();
    }

    //  0   8   1   8   1   8   1   9   1   1   1   1   2   1   1   1
    //      8   8   8   8   8   8   9   9   9   9   9   9   9   9   9
    //     --  81  88  88  88  88  89  91  91  91  91  92  92  92  92
    //    --- --- 818 881 888 888 889 891 911 911 911 912 921 921 921

    //  0   2   3   4   2   3   4   2   3   4   2   3   4   2   7   8
    //      2   3   4   4   4   4   4   4   4   4   4   4   4   7   8
    //     --  23  34  42  43  44  44  44  44  44  44  44  44  47  78
    //    --- --- 234 342 423 434 442 443 444 444 444 444 444 447 478

    //  b[i] = max(b[i-1], 10*a[i-1] + o[i])
    (long[], long[]) DoStep(long[] o, long[] a)
    {
        var b = new long[o.Length];
        for (int i = 1; i < o.Length; i++)
            b[i] = Math.Max(b[i - 1], 10 * a[i - 1] + o[i]);

        return (o, b);
    }
}


public class Test
{
    [Fact]
    public void Example()
    {
        var input = @"
987654321111111
811111111111119
234234234234278
818181911112111
";

        Assert.Equal((long)98 + 89 + 78 + 92, new Battery(Part.A).Solve(input));
        Assert.Equal(3121910778619, new Battery(Part.B).Solve(input));
    }
}