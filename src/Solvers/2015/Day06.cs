using System.Text.RegularExpressions;

namespace Year2015.Day06;

[Solver(2015, 06, Part.A)]
class LightController : Solver
{
    private const int Size = 10;
    private readonly Regex regex
        = new Regex("(turn on|toogle|turn off) (\\d+),(\\d+) through (\\d),(\\d)");
    private bool[,] Start = new bool[Size, Size];

    internal override object Solve(string input) =>
        input.Lines()
             .Aggregate(Start, DoInstruction)
             .ToEnumerable()
             .Count(light => light);

    bool[,] DoInstruction(bool[,] lights, string instruction)
    {
        var groups = regex.Match(instruction).Groups;

        var command = groups[0].Value;
        var x1 = int.Parse(groups[1].Value);
        var y1 = int.Parse(groups[2].Value);
        var x2 = int.Parse(groups[3].Value);
        var y2 = int.Parse(groups[4].Value);

        for (int x = Math.Min(x1, x2); x <= Math.Max(x1, x2); x++)
            for (int y = Math.Min(y1, y2); y <= Math.Max(y1, y2); y++)
            {
                #pragma warning disable CS8509
                lights[x, y] = command switch
                {
                    "turn on"  => true,
                    "toogle"   => !lights[x, y],
                    "turn off" => false,
                };
            }

        return lights;
    }
}

static class ArrayExtensions
{
    internal static IEnumerable<T> ToEnumerable<T>(this T[,] array)
    {
        var enumerator = array.GetEnumerator();
        do
        {
            yield return (T) enumerator.Current;
        } while (enumerator.MoveNext());
    }
}