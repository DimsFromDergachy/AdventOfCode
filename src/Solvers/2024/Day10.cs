namespace Year2024.Day10;

[Solver(2024, 10, Part.A)]
[Solver(2024, 10, Part.B)]
class Hiker : Solver
{
    public Hiker() {}
    internal Hiker(Part part) { Part = part; }

    List<(int dx, int dy)> dirs = new List<(int dx, int dy)>
        { (0, 1), (1, 0), (0, -1), (-1, 0) };

    internal override object Solve(string input)
    {
        var map = input.Lines()
                       .ToArray();

        #pragma warning disable CS8524
        return Part switch
        {
            Part.A => map.ToEnumerable()
                         .Where(pair => pair.Value == '0')
                         .Select(pair => pair.Index)
                         .Select(index => GetTrailTails(map, index))
                         .Select(tails => tails.DistinctBy(tail => tail.pos)
                                               .Count())
                         .Sum(),
            Part.B => map.ToEnumerable()
                         .Where(pair => pair.Value == '0')
                         .Select(pair => pair.Index)
                         .Select(index => GetTrailTails(map, index))
                         .SelectMany(tails => tails.GroupBy(tail => tail.pos)
                                                   .Select(gr => gr.MaxBy(trail => trail.score)))
                                                   .Select(tail => tail.score)
                         .Sum(),
        };
    }

    IEnumerable<((int x, int y) pos, int score)> GetTrailTails(char[,] map, (int x, int y) start)
    {
        int[,] scores = new int[map.GetLength(0), map.GetLength(1)];

        scores[start.x, start.y] = 1;
        var stack = new Stack<(int x, int y)>();
        stack.Push(start);

        while (stack.Any())
        {
            var current = stack.Pop();
            scores[current.x, current.y]++;
            var height = map[current.x, current.y];

            if (height == '9')
            {
                yield return (current, scores[current.x, current.y]);
                continue;
            }

            foreach (var (dx, dy) in dirs)
            {
                try
                {
                    (int x, int y) next = (current.x + dx, current.y + dy);
                    if (map[next.x, next.y] == height + 1)
                    {
                        stack.Push(next);
                    }
                } catch (IndexOutOfRangeException) {}
            }
        }
    }
}

public class HikerTest
{
    [Fact]
    internal void Test()
    {
        var input = @"
89010123
78121874
87430965
96549874
45678903
32019012
01329801
10456732";

        Assert.Equal(36, new Hiker(Part.A).Solve(input));
        Assert.Equal(81, new Hiker(Part.B).Solve(input));
    }

    [Fact]
    internal void Simple()
    {
        var input = @"
.....0.
..4321.
..5..2.
..6543.
..7..4.
..8765.
..9....";

        Assert.Equal(1, new Hiker(Part.A).Solve(input));
        Assert.Equal(3, new Hiker(Part.B).Solve(input));
    }
}