namespace Year2024.Day12;

[Solver(2024, 12, Part.A)]
class Gardener : Solver
{
    List<(int dx, int dy)> dirs = new List<(int dx, int dy)>
        { (0, 1), (1, 0), (-1, 0), (0, -1) };

    internal override object Solve(string input)
    {
        var map = input.Lines()
                       .ToArray();

        return Solve(map).Select(pair => pair.area * pair.perimeter).Sum();
    }

    IEnumerable<(int area, int perimeter)> Solve(char[,] map)
    {
        bool[,] visited = new bool[map.GetLength(0), map.GetLength(1)];

        foreach (var start in map.ToEnumerable())
        {
            if (visited[start.Index.i, start.Index.j])
                continue;

            var plant = start.Value;
            var (area, perimeter) = (0, 0);
            var stack = new Stack<(int x, int y)>();
            stack.Push(start.Index);

            while (stack.Any())
            {
                var current = stack.Pop();
                if (visited[current.x, current.y])
                    continue;

                visited[current.x, current.y] = true;
                area++;

                foreach (var dir in dirs)
                {
                    try
                    {
                        (int x, int y) next = (current.x + dir.dx, current.y + dir.dy);

                        if (map[next.x, next.y] == plant)
                        {
                            perimeter--; // Neighbor eats a perimeter

                            if (visited[next.x, next.y])
                                continue;

                            stack.Push(next);
                        }
                    }
                    catch (IndexOutOfRangeException) {}
                }
            }

            yield return (area, 4 * area + perimeter);
        }
    }
}

public class GardenerTest
{
    [Fact]
    internal void Example()
    {
        var input = @"
OOOOO
OXOXO
OOOOO
OXOXO
OOOOO
";

        Assert.Equal(772, new Gardener().Solve(input));
    }
}