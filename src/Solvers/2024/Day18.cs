namespace Year2024.Day18;

[Solver(2024, 18, Part.A)]
[Solver(2024, 18, Part.B)]
class RamRunner : Solver
{
    (int X, int Y) Size = (70, 70);
    int Count = 1024;
    List<(int dx, int dy)> dirs = new List<(int, int)> { (1, 0), (0, 1), (-1, 0), (0, -1) };

    internal RamRunner(Part part) : base(part) {}
    internal RamRunner(Part part, (int x, int y) size, int count) : base(part)
    {
        Size = size;
        Count = count;
    }

    internal override object Solve(string input)
    {
        var inputs = input.Lines()
                          .SelectMany(line => line.Split(',').Parse<int>())
                          .ChunkWith((a, b) => (x: a, y: b))
                          .ToArray();

        if (Part == Part.A)
        {
            var map = inputs.Take(Count)
                            .Aggregate(new bool[Size.X + 1, Size.Y + 1],
                                   (map, pair) =>
                                   {
                                        map[pair.x, pair.y] = true;
                                        return map;
                                   });
            return BFS(map, (0, 0), Size);
        }

        var (left, right) = (0, inputs.Count());

        while (left + 1 < right)
        {
            var middle = (left + right) / 2;

            var map = inputs.Take(middle)
                            .Aggregate(new bool[Size.X + 1, Size.Y + 1],
                                   (map, pair) =>
                                   {
                                        map[pair.x, pair.y] = true;
                                        return map;
                                   });

            if (BFS(map, (0, 0), Size) == -1)
                right = middle;
            else
                left = middle;
        }

        return inputs[left];
    }

    int BFS(bool[,] map, (int x, int y) start, (int x, int y) finish)
    {
        var dist = new int[Size.X + 1, Size.Y + 1];
        Array.ForEach(dist.GetIndexes().ToArray(),
                      (pair) => dist[pair.x, pair.y] = int.MaxValue);

        var queue = new Queue<(int x, int y)>();
        queue.Enqueue(start);
        dist[start.x, start.y] = 0;

        while (queue.Any())
        {
            var curr = queue.Dequeue();

            if (curr == finish)
                return dist[finish.x, finish.y];

            foreach (var dir in dirs)
            {
                try
                {
                    var next = (x: curr.x + dir.dx, y: curr.y + dir.dy);
                    if (map[next.x, next.y] == false
                        &&
                        dist[curr.x, curr.y] + 1 < dist[next.x, next.y])
                    {
                        dist[next.x, next.y] = dist[curr.x, curr.y] + 1;
                        queue.Enqueue(next);
                    }
                }
                catch (IndexOutOfRangeException) {}
            }
        }

        return -1;
    }
}

public class RamRunnerTest
{
    [Fact]
    public void Example()
    {
        var input = @"
5,4
4,2
4,5
3,0
2,1
6,3
2,4
1,5
0,6
3,3
2,6
5,1
1,2
5,5
2,5
6,5
1,4
0,4
6,4
1,1
6,1
1,0
0,5
1,6
2,0
";

        Assert.Equal(   22,  new RamRunner(Part.A, (6, 6), 12).Solve(input));
        Assert.Equal((6, 1), new RamRunner(Part.B, (6, 6), 12).Solve(input));
    }
}
