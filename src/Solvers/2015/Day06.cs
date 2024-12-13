using System.Text.RegularExpressions;

namespace Year2015.Day06;

[Solver(2015, 06, Part.A)]
class LightControllerA : Solver
{
    private const int Size = 1000;
    private readonly Regex regex
        = new Regex("(turn on|toggle|turn off) (\\d+),(\\d+) through (\\d+),(\\d+)");
    private bool[,] Start = new bool[Size, Size];

    internal override object Solve(string input) =>
        input.Lines()
             .Aggregate(Start, DoInstruction)
             .GetValues()
             .Count(light => light);

    bool[,] DoInstruction(bool[,] lights, string instruction)
    {
        var groups = regex.Match(instruction).Groups;

        var command = groups[1].Value;
        var x1 = int.Parse(groups[2].Value);
        var y1 = int.Parse(groups[3].Value);
        var x2 = int.Parse(groups[4].Value);
        var y2 = int.Parse(groups[5].Value);

        for (int x = Math.Min(x1, x2); x <= Math.Max(x1, x2); x++)
            for (int y = Math.Min(y1, y2); y <= Math.Max(y1, y2); y++)
            {
                #pragma warning disable CS8509
                lights[x, y] = command switch
                {
                    "turn on"  => true,
                    "toggle"   => !lights[x, y],
                    "turn off" => false,
                };
            }

        return lights;
    }
}

[Solver(2015, 06, Part.B)]
class LightControllerB : Solver
{
    private const int Size = 1000;
    private readonly Regex regex
        = new Regex("(turn on|toggle|turn off) (\\d+),(\\d+) through (\\d+),(\\d+)");
    private int[,] Start = new int[Size, Size];

    internal override object Solve(string input) =>
        input.Lines()
             .Aggregate(Start, DoInstruction)
             .GetValues()
             .Sum();

    int[,] DoInstruction(int[,] lights, string instruction)
    {
        var groups = regex.Match(instruction).Groups;

        var command = groups[1].Value;
        var x1 = int.Parse(groups[2].Value);
        var y1 = int.Parse(groups[3].Value);
        var x2 = int.Parse(groups[4].Value);
        var y2 = int.Parse(groups[5].Value);

        for (int x = Math.Min(x1, x2); x <= Math.Max(x1, x2); x++)
            for (int y = Math.Min(y1, y2); y <= Math.Max(y1, y2); y++)
            {
                #pragma warning disable CS8509
                lights[x, y] = command switch
                {
                    "turn on"  => lights[x, y] + 1,
                    "toggle"   => lights[x, y] + 2,
                    "turn off" => Math.Max(0, lights[x, y] - 1),
                };
            }

        return lights;
    }
}

public class LightControllerTest
{
    [Fact]
    internal void ExampleA()
    {
        var input = @"
turn on 0,0 through 999,999
toggle 0,0 through 999,0
turn off 499,499 through 500,500
";

        Assert.Equal(1000 * 1000 - 1000 - 4, new LightControllerA().Solve(input));
    }

    [Fact]
    internal void ExampleB()
    {
        var input = @"
turn on 0,0 through 0,0
toggle 0,0 through 999,999
";

        Assert.Equal(1 + 2000000, new LightControllerB().Solve(input));
    }
}