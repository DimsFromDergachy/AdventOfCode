using Year2015.Day02;

namespace Year2024.Day16;

[Solver(2024, 16, Part.A)]
class Mazer : Solver
{
    internal Mazer(Part part) : base(part) {}

    internal override object Solve(string input)
    {
        var maze = input.Lines()
                        .ToArray();

        return SolverA(maze);
    }

    int SolverA(char[,] maze)
    {
        var S = maze.ToEnumerable()
                    .Single(pair => pair.Value == 'S')
                    .Index;

        var stack = new Stack<(int z, int x, int y)>();
        var dyno = new int[4, maze.GetLength(0), maze.GetLength(1)];

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

            var forward = (current.z, x: current.x + this[current.z].dx, y: current.y + this[current.z].dy);
            var turnRight = ((current.z - 1 + 4) % 4, current.x, current.y);
            var turnLeft  = ((current.z + 1)     % 4, current.x, current.y);

            if (score + 1 < dyno[forward.z, forward.x, forward.y])
            {
                dyno[forward.z, forward.x, forward.y] = score + 1;
                stack.Push(forward);
            }
            
            foreach ((int z, int x, int y) next in new []{turnLeft, turnRight})
            {
                if (score + 1000 < dyno[next.z, next.x, next.y])
                {
                    dyno[next.z, next.x, next.y] = score + 1000;
                    stack.Push(next);
                }
            }
        }

        var E = maze.ToEnumerable()
            .Single(pair => pair.Value == 'E')
            .Index;

        return Enumerable.Range(0, 4)
                         .Select(z => dyno[z, E.x, E.y])
                         .Min();
    }

    #pragma warning disable CS8509
    (int dx, int dy) this[int z] => z switch
    {
        0 => (+1,  0),
        1 => ( 0, +1),
        2 => (-1,  0),
        3 => ( 0, -1),
    };
}

public class MazerTest
{
    [Fact]
    internal void Simple()
    {
        var input = @"
######
#S  E#
######
";
        Assert.Equal(3, new Mazer(Part.A).Solve(input));
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
    }
}
