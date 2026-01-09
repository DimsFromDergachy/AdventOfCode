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

        // return Cheats(maze).Count(p => p.cost >= MinCost);
        return ManhattanTrick(maze).Count(p => p.cost >= MinCost);
    }

    internal IEnumerable<((int x, int y) point, int score)> Track(char[,] maze)
    {
        var curr = maze.ToEnumerable()
                          .Single(pair => pair.Value == 'S')
                          .Index;

        int score = 0;
        var prev = curr;

        yield return (curr, score++);

        do
        {
            foreach (var (dx, dy) in dirs)
            {
                var next = (x: curr.x + dx, y: curr.y + dy);

                if (maze[next.x, next.y] != '#' && next != prev)
                {
                    (prev, curr) = (curr, next);
                    break;
                }
            }
            yield return (curr, score++);
        }
        while (maze[curr.x, curr.y] != 'E');
    }

    IEnumerable<(int start, int end, int cost)> Cheats(char[,] maze)
    {
        var M = maze.GetLength(1);
        var N = maze.GetLength(0);
        var dist = new int[M * N, M * N];

        foreach (var (x, y) in dist.GetIndexes())
            dist[x, y] = int.MaxValue / 2;

        foreach (var (x, y) in maze.GetIndexes())
            if (maze[x, y] == '#')
                dist[x + N * y, x + N * y] = 0;

        foreach (var (sx, sy) in maze.GetIndexes())
        {
            if (maze[sx, sy] != '#')
                continue;

            #region Dijkstra
            var queue = new Queue<(int x, int y)>();
            queue.Enqueue((sx, sy));
            var visited = new bool[M, N];
            visited[sx, sy] = true;

            while (queue.Any())
            {
                var (x, y) = queue.Dequeue();
                foreach (var (dx, dy) in dirs)
                {
                    var (nx, ny) = (x + dx, y + dy);
                    try
                    {
                        _ = maze[nx, ny];

                        if (visited[nx, ny] || maze[nx, ny] != '#')
                            continue;

                        queue.Enqueue((nx, ny));
                        visited[nx, ny] = true;
                        dist[sx + N * sy, nx + N * ny] = dist[sx + N * sy, x + N * y] + 1;
                        dist[nx + N * ny, sx + N * sy] = dist[sx + N * sy, x + N * y] + 1;
                    } catch (IndexOutOfRangeException) {}
                }
            }
            #endregion
        }

        var track = Track(maze).ToList();

        foreach (var v in track)
        foreach (var w in track)
        {
            if (v.score >= w.score)
                continue;

            var maxCheat = 0;

            foreach (var dv in dirs)
            foreach (var dw in dirs)
            {
                var v_ = (x: v.point.x + dv.dx, y: v.point.y + dv.dy);
                var w_ = (x: w.point.x + dw.dx, y: w.point.y + dw.dy);

                var path = dist[v_.x + N * v_.y, w_.x + N * w_.y] + 2;
                var cheat = w.score - v.score - path;

                if (path <= MaxCheatPath && cheat > 0 && cheat > maxCheat)
                    maxCheat = cheat;
            }

            if (maxCheat > 0)
                yield return (v.score, w.score, maxCheat);
        }
    }

    IEnumerable<(int start, int end, int cost)> ManhattanTrick(char[,] maze)
    {
        var track = Track(maze).ToList();

        foreach (var v in track)
        foreach (var w in track)
        {
            if (v.score >= w.score)
                continue;

            var dist = Math.Abs(v.point.x - w.point.x) + Math.Abs(v.point.y - w.point.y);
            var cheat = w.score - v.score - dist;

            if (dist <= MaxCheatPath && cheat > 0)
                yield return (v.score, w.score, cheat);
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

        Assert.Equal( 0, new Cheater(Part.A, 65).Solve(input));
        Assert.Equal( 1, new Cheater(Part.A, 64).Solve(input));
        Assert.Equal( 2, new Cheater(Part.A, 40).Solve(input));
        Assert.Equal( 3, new Cheater(Part.A, 38).Solve(input));
        Assert.Equal( 4, new Cheater(Part.A, 36).Solve(input));
        Assert.Equal( 5, new Cheater(Part.A, 20).Solve(input));
        Assert.Equal( 8, new Cheater(Part.A, 12).Solve(input));
        Assert.Equal(10, new Cheater(Part.A, 10).Solve(input));
        Assert.Equal(14, new Cheater(Part.A,  8).Solve(input));
        Assert.Equal(16, new Cheater(Part.A,  6).Solve(input));
        Assert.Equal(30, new Cheater(Part.A,  4).Solve(input));
        Assert.Equal(44, new Cheater(Part.A,  2).Solve(input));
        Assert.Equal(44, new Cheater(Part.A,  0).Solve(input));

        Assert.Equal( 3, new Cheater(Part.B, 76).Solve(input));
        Assert.Equal( 3 + 4, new Cheater(Part.B, 74).Solve(input));
        Assert.Equal( 3 + 4 + 22, new Cheater(Part.B, 72).Solve(input));
    }

    [Fact]
    internal void GetTrack()
    {
        var input = @"
######
#E##S#
#....#
######";

        var maze = input.Lines().ToArray();

        var solver = new Cheater(Part.A);
        var track = solver.Track(maze).ToList();
        Assert.Equal(6, track.Count());
        Assert.Equal(((4, 1), 0), track.First());
        Assert.Equal(((1, 1), 5), track.Last());
    }

    [Fact]
    internal void PartA()
    {
        var input = File.ReadAllText("./Solvers/2024/Data/Day20");

        Assert.Equal(   1530, new Cheater(Part.A).Solve(input)); 
        Assert.Equal(1033983, new Cheater(Part.B).Solve(input));
    }
}
