namespace Year2024.Day09;

[Solver(2024, 09, Part.A)]
[Solver(2024, 09, Part.B)]
class Defragmenter : Solver
{
    internal Defragmenter(Part part) : base(part) {}

    internal override object Solve(string input) =>
        input.Chunk(2)
             .Select(pair => (int.Parse(pair.First().ToString()),
                              int.Parse(pair.Skip(1).First().ToString())))
             .ToArray()
             .Defragment(Part)
             .SelectMany(pair => Enumerable.Repeat(pair.Item1, pair.Item2))
             .Zip(Enumerable.Range(0, int.MaxValue))
             .Select(pair => (long) (pair.First * pair.Second))
             .Sum();
}

static class Extensions
{
    #pragma warning disable CS8524
    internal static IEnumerable<(int, int)> Defragment(this (int fill, int empty)[] array, Part part) =>
        part switch {
            Part.A => array.DefragmentA(),
            Part.B => array.DefragmentB(),
        };

    static IEnumerable<(int, int)> DefragmentA(this (int fill, int empty)[] array)
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

    static IEnumerable<(int, int)> DefragmentB(this (int fill, int empty)[] array)
    {
        List<(int idx, int idx_new)> memo = new List<(int, int)>();

        for (var idx = array.Count() - 1; idx > 0; idx--)
        {
            var new_idx = Array.FindIndex(array, elem => elem.empty >= array[idx].fill);

            if (new_idx > -1 && new_idx < idx)
            {
                memo.Add((idx, new_idx));
                array[new_idx].empty -= array[idx].fill;
            };
        }

        var memoSet = memo.Select(pair => pair.idx).ToHashSet();
        var groups = memo.GroupBy(elem => elem.idx_new).ToDictionary(gr => gr.Key);

        for (var idx = 0; idx < array.Count(); idx++)
        {
            if (memoSet.Contains(idx))
            {
                yield return (0, array[idx].fill);
            }
            else
            {
                yield return (idx, array[idx].fill);
            }

            if (groups.ContainsKey(idx))
                foreach (var item in groups[idx])
                {
                    yield return (item.idx, array[item.idx].fill);
                }

            yield return (0, array[idx].empty);
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
        Assert.Equal((long)2858, new Defragmenter(Part.B).Solve(input));
    }
}
