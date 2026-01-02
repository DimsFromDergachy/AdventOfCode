static class ArrayExtensions
{
    // internal static TElem[,] ToArray<TRank, TElem>(this IEnumerable<IEnumerable<TElem>> source)
    internal static TElem[,] ToArray<TElem>(this IEnumerable<IEnumerable<TElem>> source)
    {
        var n = source.First().Count();
        var m = source.Count();
        var array = new TElem[n, m];

        foreach (var (xs, y) in source.Zip(Enumerable.Range(0, m)))
            foreach (var (elem, x) in xs.Zip(Enumerable.Range(0, n)))
                array[x, y] = elem;

        return array;
    }

    internal static T[,] Print<T>(this T[,] array)
    {
        for (int j = 0; j < array.GetLength(1); j++)
        {
            for (int i = 0; i < array.GetLength(0); i++)
                Console.Write(string.Format("{0,5}", array[i, j]));
            Console.WriteLine();
        }

        return array;
    }


    internal static IEnumerable<((int x, int y) Index, T Value)> ToEnumerable<T>(this T[,] array) =>
        array.GetIndexes()
             .Select(index => (index, array[index.x, index.y]));

    internal static IEnumerable<(int x, int y)> GetIndexes<T>(this T[,] array)
    {
        for (int y = array.GetLowerBound(1); y <= array.GetUpperBound(1); y++)
        for (int x = array.GetLowerBound(0); x <= array.GetUpperBound(0); x++)
            yield return (x, y);
    }

    internal static IEnumerable<(int x, int y, int z)> GetIndexes<T>(this T[,,] array)
    {
        for (int z = array.GetLowerBound(0); z <= array.GetUpperBound(0); z++)
        for (int y = array.GetLowerBound(2); y <= array.GetUpperBound(2); y++)
        for (int x = array.GetLowerBound(1); x <= array.GetUpperBound(1); x++)
            yield return (x, y, z);
    }

    internal static IEnumerable<T> GetValues<T>(this T[,] array)
    {
        foreach (var index in array.GetIndexes())
        {
            yield return array[index.x, index.y];
        }
    }

    internal static TElem[,] AddBorders<TElem>(this TElem[,] array, TElem border, int size = 1)
    {
        var result = new TElem[array.GetLength(0) + 2 * size,
                               array.GetLength(1) + 2 * size];

        for (int y = 0; y < result.GetLength(1); y++)
        for (int x = 0; x < result.GetLength(0); x++)
            if (0 <= x - size && x - size < array.GetLength(0)
             && 0 <= y - size && y - size < array.GetLength(1))
                result[x, y] = array[x - size, y - size];
            else
                result[x, y] = border;

        return result;
    }
}

internal class _2D {}

public class ArrayExtensionsTest
{
    [Fact]
    public void ToArray()
    {
        var input = @"
ABCD
EFGH";

        // var array = input.Lines().ToArray<_2D, char>();
        var array = input.Lines().ToArray<char>();
        Assert.Equal(4, array.GetLength(0));
        Assert.Equal(2, array.GetLength(1));
        Assert.Equal('A', array[0, 0]);
        Assert.Equal('B', array[1, 0]);
        Assert.Equal('C', array[2, 0]);
        Assert.Equal('D', array[3, 0]);
        Assert.Equal('E', array[0, 1]);
        Assert.Equal('F', array[1, 1]);
        Assert.Equal('G', array[2, 1]);
        Assert.Equal('H', array[3, 1]);
    }

    [Fact]
    public void ToEnumerable()
    {
        var input = @"
1 2 3
4 5 6";

        var array = input.Lines()
                         .Select(line => line.Words().Parse<int>())
                         .ToArray();
        Assert.Equal([((0, 0), 1), ((1, 0), 2), ((2, 0), 3),
                      ((0, 1), 4), ((1, 1), 5), ((2, 1), 6)],
            array.ToEnumerable());
    }

    [Fact]
    public void WithBorders()
    {
        var input = @"
XOOXO
OXXOX
XOXXO
";

        var expected = @"
.......
.XOOXO.
.OXXOX.
.XOXXO.
.......
";

        Assert.Equal(expected.Lines().ToArray(),
                        input.Lines().ToArray().AddBorders('.'));
    }
}