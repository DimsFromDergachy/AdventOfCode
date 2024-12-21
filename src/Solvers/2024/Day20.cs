namespace Year2024.Day20;

[Solver(2024, 20, Part.A)]
[Solver(2024, 20, Part.B)]
class Cheater : Solver
{
    #pragma warning disable CS8524

    int MaxCheatPath => Part switch { Part.A => 2, Part.B => 20, };
    int MinCost = 100;
    internal Cheater(Part part) : base(part) {}
    internal Cheater(Part part, int minCost) : base(part)
    {
        MinCost = minCost;
    }

    List<(int dx, int dy)> dirs = new List<(int dx, int dy)>
        { (0, 1), (1, 0), (0, -1), (-1, 0) };


    internal override object Solve(string input)
    {
        var maze = input.Lines()
                        .ToArray();

        return Cheats(maze).Count(p => p.cost >= MinCost);
    }

    IEnumerable<(int start, int end, int cost)> Cheats(char[,] maze)
    {
        var S = maze.ToEnumerable()
                    .Single(pair => pair.Value == 'S')
                    .Index;

        var dyno = new int[maze.GetLength(1), maze.GetLength(0)];
        Array.ForEach(dyno.GetIndexes().ToArray(), pair => dyno[pair.x, pair.y] = int.MaxValue);
        dyno[S.x, S.y] = 0;

        var stack = new Stack<(int x, int y)>();
        stack.Push(S);
        var track = new List<((int x, int y) point, int score)>();

        while (stack.Any())
        {
            var (x, y) = stack.Pop();
            track.Add(((x, y), dyno[x, y]));

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
            }
        }

        var cheats = new HashSet<((int x, int y) cheatStart, (int x, int y) cheatEnd, int cheatDiscount)>();

        foreach (var (cheatStart, scoreStart) in track)
        {
            var startScore = dyno[cheatStart.x, cheatStart.y];

            Array.ForEach(maze.ToEnumerable()
                              .Where(pair => pair.Value == '#')
                              .Select(pair => pair.Index)
                              .ToArray(),
                          index => dyno[index.x, index.y] = int.MaxValue );

            var queue = new Queue<(int x, int y)>();
            queue.Enqueue(cheatStart);

            while (queue.Any())
            {
                var (x, y) = queue.Dequeue();

                // cheat's over
                if (dyno[x, y] > startScore + MaxCheatPath)
                    continue;

                foreach (var (dx, dy) in dirs)
                {
                    try
                    {
                        var (nx, ny) = (x + dx, y + dy);
                        if (maze[nx, ny] == '#' && dyno[x, y] + 1 < dyno[nx, ny])
                        {
                            dyno[nx, ny] = dyno[x, y] + 1;
                            queue.Enqueue((nx, ny));
                        }
                    }
                    catch (IndexOutOfRangeException) {}
                }
            }

            foreach (var (cheatEnd, scoreEnd) in track)
            {
                if (scoreEnd <= scoreStart)
                    continue;

                var cheatDiscountMax = int.MinValue;

                foreach (var (dx, dy) in dirs)
                {
                    var (nx, ny) = (cheatEnd.x + dx, cheatEnd.y + dy);

                    if (maze[nx, ny] != '#')
                        continue;

                    var cheatLenght = dyno[nx, ny] + 1 - dyno[cheatStart.x, cheatStart.y];

                    if (cheatLenght > MaxCheatPath)
                        continue;

                    var cheatDiscount = dyno[cheatEnd.x, cheatEnd.y] - dyno[cheatStart.x, cheatStart.y] - cheatLenght;

                    if (cheatDiscount > 0 && cheatDiscount > cheatDiscountMax)
                    {
                        cheatDiscountMax = cheatDiscount;
                    }
                }
                cheats.Add((cheatStart, cheatEnd, cheatDiscountMax));
            }
        }

        foreach (var (cheatStart, cheatEnd, cheatDiscount) in cheats)
        {
            yield return (dyno[cheatStart.x, cheatStart.y], dyno[cheatEnd.x, cheatEnd.y], cheatDiscount);
        }
    }
}

public class CheaterTest
{
    [Fact]
    internal void ExampleA()
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

        var cheats = new (int gr, int save)[]
            {
            };

        // (1, 64) (1, 40) (1, 38) (1, 36)
        // (1, 20) (3, 12) (2, 10) (4,  8)
        // (2,  6) (14, 4) (14, 2)

        Assert.Equal( 0, new Cheater(Part.A, 65).Solve(input));
        Assert.Equal( 1, new Cheater(Part.A, 64).Solve(input));
        Assert.Equal( 2, new Cheater(Part.A, 40).Solve(input));
        Assert.Equal( 3, new Cheater(Part.A, 38).Solve(input));
        Assert.Equal( 4, new Cheater(Part.A, 36).Solve(input));
        Assert.Equal( 8, new Cheater(Part.A, 12).Solve(input));
        Assert.Equal(44, new Cheater(Part.A,  2).Solve(input));
        Assert.Equal(44, new Cheater(Part.A,  0).Solve(input));

        Assert.Equal( 3, new Cheater(Part.B, 76).Solve(input));
        Assert.Equal( 7, new Cheater(Part.B, 74).Solve(input));
        Assert.Equal(19, new Cheater(Part.B, 72).Solve(input));
    }
}
