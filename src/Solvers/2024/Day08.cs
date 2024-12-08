namespace Year2024.Day08;

[Solver(2024, 08, Part.A)]
class Resonant : Solver
{
    internal override object Solve(string input) 
    {
        var map = input.Lines()
                       .ToArray();

        return map.ToEnumerable()
                    .Where(kvp => kvp.Value != '.')
                    .GroupBy(kvp => kvp.Value)
                    // .Select(group => group.Select(kvp => kvp.Index))
                    .Select(kvp => (kvp.Key, Value: kvp.SelectMany(p1 => kvp.Select(p2 => Interfere(p1.Index, p2.Index)))))
                    .Select(kvp => (kvp.Key, Value: kvp.Value.Distinct()))
                    .Select(kvp => kvp.Value.Where(p => IsAntinode(map, kvp.Key, p)))
                    .SelectMany(ps => ps)
                    .Distinct()
                    .Count();
    }

    bool IsAntinode(char[,] map, char node, (int x, int y) point)
    {
        try
        {
            return map[point.x, point.y] != node;
        }
        catch (IndexOutOfRangeException)
        {
            return false;
        };
    }

    (int i, int j) Interfere((int x, int y) p1, (int x, int y) p2) =>
        (2 * p1.x - p2.x, 2 * p1.y - p2.y);
}

public class ResonantTest
{
    [Fact]
    internal void Example()
    {
        var input = @"
............
........0...
.....0......
.......0....
....0.......
......A.....
............
............
........A...
.........A..
............
............
";

        Assert.Equal(14, new Resonant().Solve(input));
    }

    [Fact]
    internal void Example2()
    {
        var input = @"
..........
..........
..........
....a.....
........a.
.....a....
..........
......A...
..........
..........
";

        Assert.Equal(4, new Resonant().Solve(input));
    }
}
