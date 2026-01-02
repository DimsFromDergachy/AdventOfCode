using System.Security.Cryptography;

namespace Year2025.Day04;

[Solver(2025, 4, Part.A)]
[Solver(2025, 4, Part.B)]
class Forklift : Solver
{
    internal Forklift(Part part) : base(part) {}

    internal override object Solve(string input)
    {
        var array = input.Lines()
                         .ToArray()
                         .AddBorders(Empty);

        if (Part == Part.A)
            return Clean(array).Removed;

        return (Array: array, Removed: 0).Unfold(pair => Clean(pair.Array))
                                         .Skip(1)
                                         .Select(x => x.Removed)
                                         .TakeWhile(x => x > 0)
                                         .Sum();
    }

    bool IsRemovable(char[,] array, (int x, int y) index)
        => DS.Count(d => array[index.x + d.dx, index.y + d.dy] != Empty) < 4;

    (char[,] Array, int Removed) Clean(char[,] array)
    {
        var removes = array.ToEnumerable()
                           .Where(pair => pair.Value != Empty)
                           .Where(pair => IsRemovable(array, pair.Index))
                           .ToArray();

        foreach (var pair in removes)
            array[pair.Index.x, pair.Index.y] = Empty;

        return (array, removes.Length);
    }

    char Empty => '.';
    (int dx, int dy)[] DS = [(-1, -1), (0, -1), (+1, -1),
                             (-1,  0),          (+1,  0),
                             (-1, +1), (0, +1), (+1, +1)];
}

public class Test
{
    [Fact]
    public void Example()
    {
        var input = @"
..@@.@@@@.
@@@.@.@.@@
@@@@@.@.@@
@.@@@@..@.
@@.@@@@.@@
.@@@@@@@.@
.@.@.@.@@@
@.@@@.@@@@
.@@@@@@@@.
@.@.@@@.@.
";

        Assert.Equal(13, new Forklift(Part.A).Solve(input));
        Assert.Equal(43, new Forklift(Part.B).Solve(input));
    }
}