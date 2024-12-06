namespace Year2024.Day06;

[Solver(2024, 06, Part.A)]
class Guard : Solver
{
    new List<(int dx, int dy)> dirs = new List<(int, int)>
        {(-1, 0), (0, 1), (0, +1), (+1, 0)};

    internal override object Solve(string input)
    {
        var map = input.Lines()
                       .ToArray();

        var start = map.GetIndexes()
                       .Zip(map.ToEnumerable())
                       .First(pair => pair.Second == '^')
                       .First;

        return GuardPath(map, start).Distinct().Count();
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

        Assert.Equal(41, new Guard().Solve(input));
    }
}