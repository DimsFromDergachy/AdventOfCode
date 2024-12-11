namespace Year2024.Day11;

[Solver(2024, 11, Part.A)]
class Blinker : Solver
{
    int Blink = 25;

    public Blinker() {}
    internal Blinker(int blink) { Blink = blink; }

    internal override object Solve(string input) =>
        input.Parse<int>()
             .Select(stone => Dyno(stone, Blink))
             .Sum();


    Dictionary<(long stone, int blink), int> memo = new Dictionary<(long, int), int>();

    int Dyno(long stone, int blink)
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
            (long n, string s) => [int.Parse(s.Substring(0, s.Count() / 2)),
                                   int.Parse(s.Substring(s.Count() / 2))],
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
        Assert.Equal(expected, new Blinker(blink).Solve(input));
    }
}
