namespace Year2024.Day09;

[Solver(2024, 09, Part.A)]
[Solver(2024, 09, Part.B)]
class Defragmenter : Solver
{
    public Defragmenter() {}
    internal Defragmenter(Part part) { Part = part; }

    internal override object Solve(string input) =>
        input.Chunk(2)
             .Select(pair => (int.Parse(pair.First().ToString()),
                              int.Parse(pair.Skip(1).First().ToString())))
             .ToArray()
             .Defragment()
             .SelectMany(pair => Enumerable.Repeat(pair.Item1, pair.Item2))
             .Zip(Enumerable.Range(0, int.MaxValue))
             .Select(pair => (long) (pair.First * pair.Second))
             .Sum();
}

static class Extensions
{
    internal static IEnumerable<(int, int)> Defragment(this (int fill, int empty)[] array)
    {
        (int id, int index) left  = (0, 0);
        (int id, int count) right = (array.Count() - 1, array.Last().fill);

        while (true)
        {
            if (left.id > right.id)
                yield break;

            if (left.id == right.id)
            {
                yield return (right.id, right.count);
                yield break;
            }

            if (left.index == 0)
            {
                yield return (left.id, array[left.id].fill);
                left.index = array[left.id].fill;
            }
            if (left.index > 0)
            {
                int empty = array[left.id].empty - (left.index - array[left.id].fill);
                if (empty < right.count)
                {
                    yield return (right.id, empty);
                    left = (left.id + 1, left.index = 0);
                    right.count -= empty;
                }
                else if (empty == right.count)
                {
                    yield return (right.id, right.count);
                    left = (left.id + 1, left.index = 0);
                    right = (right.id - 1, array[right.id - 1].fill);
                }
                else if (empty > right.count)
                {
                    yield return (right.id, right.count);
                    left.index += right.count;
                    right = (right.id - 1, array[right.id - 1].fill);
                }
            }
        }
    }
}

public class ATest
{
    [Fact]
    internal void Example()
    {
        var input = @"2333133121414131402" + "0";

        Assert.Equal((long)1928, new Defragmenter(Part.A).Solve(input));
    }
}
