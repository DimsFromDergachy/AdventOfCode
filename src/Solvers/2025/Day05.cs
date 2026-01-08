using Year2025.Day05.BSTM;

namespace Year2025.Day05;

[Solver(2025, 5, Part.A)]
[Solver(2025, 5, Part.B)]
class IDs : Solver
{
    internal IDs(Part part) : base(part) {}

    internal override object Solve(string input)
    {
        var lines = input.Lines(StringSplitOptions.None);

        var ranges = lines.TakeWhile(line => !line.Equals(string.Empty))
                          .SelectMany(line => line.Split('-'))
                          .Parse<long>()
                          .ChunkWith((a, b) => new BSTM.Range{ Left = a, Right = b});

        var ids = lines.SkipWhile(line => !line.Equals(string.Empty))
                       .Skip(1)
                       .Parse<long>();

        var bstm = BSTM.BSTM.Build(ranges)!;

        #pragma warning disable CS8524
        return Part switch
        {
            Part.A => ids.Count(bstm.In),
            Part.B => bstm.Fold<long>(0, (acc, range) => acc + range.Size),
        };
    }
}

public class Test
{
    [Fact]
    public void Example()
    {
        var input =
@"3-5
10-14
16-20
12-18

1
5
8
11
17
32";

        Assert.Equal((long)  3, new IDs(Part.A).Solve(input));
        Assert.Equal((long) 14, new IDs(Part.B).Solve(input));
    }
}