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
                       .Zip(map.GetValues())
                       .First(pair => pair.Second == '^')
                       .First;

        #pragma warning disable CS8524
        return Part switch
        {
            Part.A => GuardPath(map, start).Distinct().Count(),
            Part.B => GuardObstacles(map, start).Distinct().Count(),
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
        var start_ = start;
        List<(int x, int y)> path = new List<(int, int)> { start };

        var dirs = new List<(int dx, int dy)>
            {(-1, 0), (0, 1), (1, 0), (0, -1)}.Cycle();

        (int x, int y)? obstacle = null;

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

                if (path.Contains(start)
                    ||
                    start.y == start_.y && start.x >= start_.x)
                {
                    map[next.Item1, next.Item2] = '#';
                    if (CheckLoop(map, start_))
                        obstacle = next;
                    map[next.Item1, next.Item2] = '.';
                }

                start = next;

            } catch(IndexOutOfRangeException)
            {
                yield break;
            }

            if (obstacle != null)
            {
                yield return obstacle.Value;
                obstacle = null;
            }

            path.Add(start);
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
        Assert.Equal(2, new Guard(Part.A).Solve(input));
        Assert.Equal(0, new Guard(Part.B).Solve(input));
    }

    [Fact]
    internal void Simple2()
    {
        var input = @"
..#.
#...
...#
^...
....
.#..
..#.
";
        Assert.Equal(9, new Guard(Part.A).Solve(input));
        Assert.Equal(2, new Guard(Part.B).Solve(input));
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