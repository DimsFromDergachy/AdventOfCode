namespace Year2024.Day06;

[Solver(2024, 06, Part.A)]
[Solver(2024, 06, Part.B)]
class Guard : Solver
{
    public Guard() {}
    internal Guard(Part part) { Part = part; }

    internal override object Solve(string input)
    {
        var map = input.Lines()
                       .ToArray();

        var start = map.GetIndexes()
                       .Zip(map.ToEnumerable())
                       .First(pair => pair.Second == '^')
                       .First;

        #pragma warning disable CS8524
        return Part switch
        {
            Part.A => GuardPath(map, start).Distinct().Count(),
            Part.B => GuardObstacles(map, start).Count(),
            //Part.B => string.Join(",", GuardObstacles(map, start)),
        };
    }

    IEnumerable<(int, int)> GuardPath(char[,] map, (int x, int y) start)
    {
        yield return start;

        var dirs = new List<(int dx, int dy)>
            {(-1, 0), (0, 1), (1, 0), (0, -1)}.Cycle();

        while (true)
        {
            var dir = dirs.First();
            var next = (start.x + dir.dx, start.y + dir.dy);

            try
            {
                if (map[next.Item1, next.Item2] == '#')
                {
                    dirs = dirs.Skip(1);
                    continue;
                }
                start = next;
            } catch(IndexOutOfRangeException)
            {
                yield break;
            }

            yield return start;
        }
    }

    bool CheckLoop(char[,] map, (int x, int y) start)
    {
        var size = map.GetLength(0) * map.GetLength(1);
        return GuardPath(map, start).Skip(size).Any();
    }

    IEnumerable<(int x, int y)> GuardObstacles(char[,] map, (int x, int y) start)
    {
        foreach (var (x, y) in map.GetIndexes())
        {
            if (map[x, y] != '.')
                continue;
            map[x, y] = '#';
            if (CheckLoop(map, start))
                yield return (x, y);
            map[x, y] = '.';
        }
    }
}

public class GuardTest
{
    [Fact]
    internal void Simple()
    {
        var input = @"
#..
^.#
.#.";
        Assert.Equal(2, new Guard().Solve(input));
    }

    [Fact]
    internal void Example()
    {
        var input = @"
....#.....
.........#
..........
..#.......
.......#..
..........
.#..^.....
........#.
#.........
......#...";

        Assert.Equal(41, new Guard(Part.A).Solve(input));
        Assert.Equal( 6, new Guard(Part.B).Solve(input));
    }
}