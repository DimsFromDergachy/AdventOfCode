namespace Year2024.Day08;

[Solver(2024, 08, Part.A)]
[Solver(2024, 08, Part.B)]
class Resonant : Solver
{
    internal Resonant(Part part) : base(part) {}

    internal override object Solve(string input) 
    {
        var map = input.Lines()
                       .ToArray();

        #pragma warning disable CS8524
        return Part switch
        {
            Part.A => map.ToEnumerable()
                         .Where(kvp => kvp.Value != '.')
                         .GroupBy(kvp => kvp.Value)
                         .Select(kvp => (kvp.Key, Value: kvp.SelectMany(p1 => kvp.Select(p2 => Interfere(p1.Index, p2.Index)))))
                         .Select(kvp => (kvp.Key, Value: kvp.Value.Distinct()))
                         .Select(kvp => kvp.Value.Where(p => IsAntinode(map, kvp.Key, p)))
                         .SelectMany(ps => ps)
                         .Distinct()
                         .Count(),
            Part.B => map.ToEnumerable()
                         .Where(kvp => kvp.Value != '.')
                         .GroupBy(kvp => kvp.Value)
                         .Select(group => group.Select(kvp => kvp.Index))
                         .SelectMany(ps => ps.SelectMany(p1 => ps.SelectMany(p2 => Interferes(p1, p2))))
                         .Where(p => OnMap(map, p))
                         .Distinct()
                         .Count(),
        };
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

    bool OnMap(char[,] map, (int x, int y) point)
    {
        try
        {
            _ = map[point.x, point.y];
            return true;
        }
        catch (IndexOutOfRangeException)
        {
            return false;
        };
    }

    (int i, int j) Interfere((int x, int y) p1, (int x, int y) p2) =>
        (2 * p1.x - p2.x, 2 * p1.y - p2.y);

    IEnumerable<(int i, int j)> Interferes((int x, int y) p1, (int x, int y) p2) =>
        Enumerable.Range(0, 50)
                  .Select(d => (p1.x + d * (p1.x - p2.x), p1.y + d * (p1.y - p2.y)));
}

public class ResonantTest
{
    // [Fact]
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

        Assert.Equal(14, new Resonant(Part.A).Solve(input));
    }

    // [Fact]
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

        Assert.Equal(4, new Resonant(Part.A).Solve(input));
    }

    // [Fact]
    internal void ExamplePartB()
    {
        var input = @"
T.........
...T......
.T........
..........
..........
..........
..........
..........
..........
..........
";

        Assert.Equal(9, new Resonant(Part.B).Solve(input));
    }

    // [Fact]
    internal void ExamplePartB2()
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

        Assert.Equal(34, new Resonant(Part.B).Solve(input));
    }

}
