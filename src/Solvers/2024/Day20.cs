namespace Year2024.Day20;

[Solver(2024, 20, Part.A)]
[Solver(2024, 20, Part.B)]
class Cheater : Solver
{
    internal Cheater(Part part) : base(part) {}

    List<(int dx, int dy)> dirs = new List<(int dx, int dy)>
        { (0, 1), (1, 0), (0, -1), (-1, 0) };


    internal override object Solve(string input)
    {
        var maze = input.Lines()
                        .ToArray();

        return Cheats(maze).Count(p => p.cost >= 100);
    }

    IEnumerable<((int x, int y) cheat, int cost)> Cheats(char[,] maze)
    {
        var S = maze.ToEnumerable()
                    .Single(pair => pair.Value == 'S')
                    .Index;

        var dyno = new int[maze.GetLength(1), maze.GetLength(0)];
        Array.ForEach(dyno.GetIndexes().ToArray(), pair => dyno[pair.x, pair.y] = int.MaxValue);
        dyno[S.x, S.y] = 0;

        var stack = new Stack<(int x, int y)>();
        stack.Push(S);

        while (stack.Any())
        {
            var (x, y) = stack.Pop();

            foreach (var (dx, dy) in dirs)
            {
                var next = (x: x + dx, y: y + dy);

                if (maze[next.x, next.y] != '#'
                    &&
                    dyno[next.x, next.y] > dyno[x, y] + 1)
                {
                    stack.Push(next);
                    dyno[next.x, next.y] = dyno[x, y] + 1;
                }

                var cheat = (x: x + 2 * dx, y: y + 2 * dy);
                var free  = 0;

                try
                {
                    if (dyno[x, y] - 2 > dyno[cheat.x, cheat.y])
                        free = dyno[x, y] - 2 - dyno[cheat.x, cheat.y];
                }
                catch (IndexOutOfRangeException) {}

                if (free > 0)
                    yield return (cheat, free);
            }
        }

        // yield return ((1, 3), dyno[1, 3]);
        // yield return ((1, 2), dyno[1, 2]);
        // yield return ((5, 7), dyno[5, 7]);
    }
}

public class CheaterTest
{
    [Fact]
    internal void Example()
    {
        var input = @"
###############
#...#...#.....#
#.#.#.#.#.###.#
#S#...#.#.#...#
#######.#.#.###
#######.#.#...#
#######.#.###.#
###..E#...#...#
###.#######.###
#...###...#...#
#.#####.#.###.#
#.#...#.#.#...#
#.#.#.#.#.#.###
#...#...#...###
###############
";

        Assert.Equal(0, new Cheater(Part.A).Solve(input));

        // 1501 - too low
    }
}
