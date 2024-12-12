namespace Year2024.Day11;

[Solver(2024, 11, Part.A)]
[Solver(2024, 11, Part.B)]
class Blinker : Solver
{
    int Blink = 75;

    internal Blinker(int blink) { Blink = blink; }
    internal Blinker(Part part) : base(part)
    {
        #pragma warning disable CS8524
        Blink = part switch {
            Part.A => 25,
            Part.B => 75,
        };
    }

    internal override object Solve(string input) =>
        input.Parse<int>()
             .Select(stone => Dyno(stone, Blink))
             .Sum();


    Dictionary<(long stone, long blink), long> memo = new Dictionary<(long, long), long>();

    long Dyno(long stone, long blink)
    {
        if (blink == 0)
            return 1;

        if (memo.ContainsKey((stone, blink)))
        {
            return memo[(stone, blink)];
        }

        var split = (stone, stone.ToString()) switch
        {
            (0, _) => new long[] {1},
            (long n, string s) when s.Count() % 2 == 1 => [n * 2024],
            (long n, string s) => [long.Parse(s.Substring(0, s.Count() / 2)),
                                   long.Parse(s.Substring(s.Count() / 2))],
        };

        return memo[(stone, blink)] = split.Select(stone => Dyno(stone, blink - 1))
                                           .Sum();

    }
}

public class BlinkerTest
{
    [Theory]
    [InlineData(0, 2)]
    [InlineData(1, 3)]
    [InlineData(2, 4)]
    [InlineData(3, 5)]
    [InlineData(4, 9)]
    [InlineData(5, 13)]
    [InlineData(6, 22)]
    [InlineData(25, 55312)]
    internal void Example(int blink, int expected)
    {
        var input = @"125 17";
        Assert.Equal((long)expected, new Blinker(blink).Solve(input));
    }

    [Fact]
    internal void Puzzle()
    {
        var input = @"28591 78 0 3159881 4254 524155 598 1";
        Assert.Equal(         220722L, new Blinker(Part.A).Solve(input));
        Assert.Equal(261952051690787L, new Blinker(Part.B).Solve(input));
    }
}
