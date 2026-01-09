namespace Year2024.Day12;

[Solver(2024, 12, Part.A)]
[Solver(2024, 12, Part.B)]
class Gardener : Solver
{
    internal Gardener(Part part) : base(part) {}

    List<(int dx, int dy)> dirs = new List<(int dx, int dy)>
        { (0, 1), (1, 0), (-1, 0), (0, -1) };

    internal override object Solve(string input)
    {
        var map = input.Lines()
                       .ToArray();

        return Solve(map).Sum(pair => pair.area * pair.perimeter);
    }

    IEnumerable<(int area, int perimeter)> Solve(char[,] map)
    {
        bool[,] visited = new bool[map.GetLength(0), map.GetLength(1)];

        foreach (var start in map.ToEnumerable())
        {
            if (visited[start.Index.x, start.Index.y])
                continue;

            var plant = start.Value;
            var (area, perimeter) = (0, 0);
            var stack = new Stack<(int x, int y)>();
            stack.Push(start.Index);
            HashSet<((int x, int y) point, (int dx, int dy) dir)> fences = new();

            while (stack.Any())
            {
                var curr = stack.Pop();
                if (visited[curr.x, curr.y])
                    continue;

                visited[curr.x, curr.y] = true;
                area++;
                fences.Add(((curr.x + 0, curr.y + 0), (+1,  0)));
                fences.Add(((curr.x + 1, curr.y + 0), ( 0, +1)));
                fences.Add(((curr.x + 1, curr.y + 1), (-1,  0)));
                fences.Add(((curr.x + 0, curr.y + 1), ( 0, -1)));

                foreach (var dir in dirs)
                {
                    try
                    {
                        (int x, int y) next = (curr.x + dir.dx, curr.y + dir.dy);

                        if (map[next.x, next.y] == plant)
                        {
                            perimeter--; // Neighbor eats a perimeter

                            switch (dir)
                            {
                                case (1, 0):
                                {
                                    fences.Remove(((curr.x + 1, curr.y + 0), ( 0, +1)));
                                    fences.Remove(((next.x + 0, next.y + 1), ( 0, -1)));
                                    break;
                                }
                                case (0, 1):
                                {
                                    fences.Remove(((curr.x + 1, curr.y + 1), (-1,  0)));
                                    fences.Remove(((next.x + 0, next.y + 0), (+1,  0)));
                                    break;
                                }
                                case (-1, 0):
                                {
                                    fences.Remove(((curr.x + 0, curr.y + 1), ( 0, -1)));
                                    fences.Remove(((next.x + 1, next.y + 0), ( 0, +1)));
                                    break;
                                }
                                case (0, -1):
                                {
                                    fences.Remove(((curr.x + 0, curr.y + 0), (+1,  0)));
                                    fences.Remove(((next.x + 1, next.y + 1), (-1,  0)));
                                    break;
                                }
                            };

                            if (visited[next.x, next.y])
                                continue;

                            stack.Push(next);
                        }
                    }
                    catch (IndexOutOfRangeException) {}
                }
            }

            if (Part == Part.A)
                yield return (area, 4 * area + perimeter);

            if (Part == Part.B)
            {
                var hs1 = fences.GroupBy(fence => fence.dir)
                                .First(gr => gr.Key == (+1, 0))
                                .Select(fence => fence.point)
                                .GroupBy(fence => fence.y)
                                .Select(gr => gr.OrderBy(fence => fence.x))
                                .Select(gr => gr.Aggregate((-2, 0), (res, point) => {
                                    if (res.Item1 + 1 < point.x)
                                        return (point.x, res.Item2 + 1);
                                    else
                                        return (point.x, res.Item2);
                                    }))
                            .Sum(gr => gr.Item2);

                var hs2 = fences.GroupBy(fence => fence.dir)
                                .First(gr => gr.Key == (-1, 0))
                                .Select(fence => fence.point)
                                .GroupBy(fence => fence.y)
                                .Select(gr => gr.OrderBy(fence => fence.x))
                                .Select(gr => gr.Aggregate((-2, 0), (res, point) => {
                                    if (res.Item1 + 1 < point.x)
                                        return (point.x, res.Item2 + 1);
                                    else
                                        return (point.x, res.Item2);
                                    }))
                                .Sum(gr => gr.Item2);

                var vs1 = fences.GroupBy(fence => fence.dir)
                                .First(gr => gr.Key == (0, +1))
                                .Select(fence => fence.point)
                                .GroupBy(fence => fence.x)
                                .Select(gr => gr.OrderBy(fence => fence.y))
                                .Select(gr => gr.Aggregate((-2, 0), (res, point) => {
                                    if (res.Item1 + 1 < point.y)
                                        return (point.y, res.Item2 + 1);
                                    else
                                        return (point.y, res.Item2);
                                    }))
                                .Sum(gr => gr.Item2);

                var vs2 = fences.GroupBy(fence => fence.dir)
                                .First(gr => gr.Key == (0, -1))
                                .Select(fence => fence.point)
                                .GroupBy(fence => fence.x)
                                .Select(gr => gr.OrderBy(fence => fence.y))
                                .Select(gr => gr.Aggregate((-2, 0), (res, point) => {
                                    if (res.Item1 + 1 < point.y)
                                        return (point.y, res.Item2 + 1);
                                    else
                                        return (point.y, res.Item2);
                                    }))
                                .Sum(gr => gr.Item2);

                yield return (area, hs1 + vs1 + hs2 + vs2);
            }
        }
    }
}

public class GardenerTest
{
    [Fact]
    internal void ExampleA()
    {
        var input = @"
OOOOO
OXOXO
OOOOO
OXOXO
OOOOO
";

        Assert.Equal(772, new Gardener(Part.A).Solve(input));
        Assert.Equal(436, new Gardener(Part.B).Solve(input));
    }

    [Fact]
    internal void ExampleB()
    {
        var input = @"
AAAAAA
AAABBA
AAABBA
ABBAAA
ABBAAA
AAAAAA
";

        Assert.Equal(1184, new Gardener(Part.A).Solve(input));
        Assert.Equal( 368, new Gardener(Part.B).Solve(input));
    }

    [Fact]
    internal void ExampleC()
    {
        var input = @"
RRRRIICCFF
RRRRIICCCF
VVRRRCCFFF
VVRCCCJFFF
VVVVCJJCFE
VVIVCCJJEE
VVIIICJJEE
MIIIIIJJEE
MIIISIJEEE
MMMISSJEEE
";

        Assert.Equal(1930, new Gardener(Part.A).Solve(input));
        Assert.Equal(1206, new Gardener(Part.B).Solve(input));
    }
}