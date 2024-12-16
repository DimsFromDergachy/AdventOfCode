namespace Year2024.Day16;

[Solver(2024, 16, Part.A)]
[Solver(2024, 16, Part.B)]
class Mazer : Solver
{
    internal Mazer(Part part) : base(part) {}

    internal override object Solve(string input)
    {
        var maze = input.Lines()
                        .ToArray();

        return Solve(maze);
    }

    int Solve(char[,] maze)
    {
        var S = maze.ToEnumerable()
                    .Single(pair => pair.Value == 'S')
                    .Index;

        var stack = new Stack<(int z, int x, int y)>();
        var dyno = new int[4, maze.GetLength(0), maze.GetLength(1)];
        var prevs = new Dictionary<(int z, int x, int y), List<(int z, int x, int y)>>();
        prevs[(0, S.x, S.y)] = new List<(int z, int x, int y)>();

        foreach (var index in dyno.GetIndexes())
            dyno[index.z, index.x, index.y] = int.MaxValue;
        dyno[0, S.x, S.y] = 0;

        stack.Push((0, S.x, S.y));

        while (stack.Any())
        {
            var current = stack.Pop();
            if (maze[current.x, current.y] == '#')
                continue;

            var score = dyno[current.z, current.x, current.y];

            var forward = (
                score: score + 1,
                next: (current.z, x: current.x + this[current.z].dx, y: current.y + this[current.z].dy)
            );
            var turnRight = (
                score: score + 1000,
                next: ((current.z - 1 + 4) % 4, current.x, current.y)
            );
            var turnLeft = (
                score: score + 1000,
                next: ((current.z + 1) % 4, current.x, current.y)
            );

            foreach ((int score, (int z, int x, int y) next) _ in new []{ forward, turnLeft, turnRight })
            {
                if (_.score == dyno[_.next.z, _.next.x, _.next.y])
                {
                    prevs[_.next].Add(current);
                }

                if (_.score < dyno[_.next.z, _.next.x, _.next.y])
                {
                    dyno[_.next.z, _.next.x, _.next.y] = _.score;
                    prevs[_.next] = new List<(int, int, int)> { current };
                    stack.Push(_.next);
                }
            }
        }

        var E = maze.ToEnumerable()
                    .Single(pair => pair.Value == 'E')
                    .Index;

        var min = Enumerable.Range(0, 4)
                            .Select(z => dyno[z, E.x, E.y])
                            .Min();

        if (Part == Part.A)
            return min;

        var path = new List<(int z, int x, int y)>();

        stack.Clear();
        Array.ForEach(Enumerable.Range(0, 4)
                                .Where(z => dyno[z, E.x, E.y] == min)
                                .ToArray(),
                      z => stack.Push((z, E.x, E.y)));

        while (stack.Any())
        {
            var current = stack.Pop();
            path.Add(current);

            foreach (var prev in prevs[current])
                stack.Push(prev);
        }

        return path.Select(pair => (pair.x, pair.y)).Distinct().Count();
    }

    #pragma warning disable CS8509
    (int dx, int dy) this[int z] => z switch
    {
        0 => (+1,  0),
        1 => ( 0, -1),
        2 => (-1,  0),
        3 => ( 0, +1),
    };
}

public class MazerTest
{
    [Fact]
    internal void Simple()
    {
        var input = @"
###########
#         #
#S ## ## E#
#         #
###########
";
        Assert.Equal(3010, new Mazer(Part.A).Solve(input));
        Assert.Equal(  21, new Mazer(Part.B).Solve(input));
    }

    [Fact]
    internal void Example1()
    {
        var input = @"
###############
#.......#....E#
#.#.###.#.###.#
#.....#.#...#.#
#.###.#####.#.#
#.#.#.......#.#
#.#.#####.###.#
#...........#.#
###.#.#####.#.#
#...#.....#.#.#
#.#.#.###.#.#.#
#.....#...#.#.#
#.###.#.#.#.#.#
#S..#.....#...#
###############
";
        Assert.Equal(7036, new Mazer(Part.A).Solve(input));
        Assert.Equal(  45, new Mazer(Part.B).Solve(input));
    }

    [Fact]
    internal void Example2()
    {
        var input = @"
#################
#...#...#...#..E#
#.#.#.#.#.#.#.#.#
#.#.#.#...#...#.#
#.#.#.#.###.#.#.#
#...#.#.#.....#.#
#.#.#.#.#.#####.#
#.#...#.#.#.....#
#.#.#####.#.###.#
#.#.#.......#...#
#.#.###.#####.###
#.#.#...#.....#.#
#.#.#.#####.###.#
#.#.#.........#.#
#.#.#.#########.#
#S#.............#
#################
";
        Assert.Equal(11048, new Mazer(Part.A).Solve(input));
        Assert.Equal(   64, new Mazer(Part.B).Solve(input));
    }
}
