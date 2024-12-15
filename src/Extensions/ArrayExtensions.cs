static class ArrayExtensions
{
    internal static T[,] ToArray<T>(this IEnumerable<IEnumerable<T>> source)
    {
        var n = source.First().Count();
        var m = source.Count();
        var array = new T[n, m];

        foreach (var (xs, y) in source.Zip(Enumerable.Range(0, m)))
            foreach (var (elem, x) in xs.Zip(Enumerable.Range(0, n)))
                array[x, y] = elem;

        return array;
    }

    internal static IEnumerable<((int x, int y) Index, T Value)> ToEnumerable<T>(this T[,] array) =>
        array.GetIndexes()
             .Zip(array.GetValues());

    internal static IEnumerable<(int x, int y)> GetIndexes<T>(this T[,] array)
    {
        for (int y = array.GetLowerBound(0); y <= array.GetUpperBound(0); y++)
        for (int x = array.GetLowerBound(1); x <= array.GetUpperBound(1); x++)
            yield return (x, y);
    }

    internal static IEnumerable<T> GetValues<T>(this T[,] array)
    {
        var enumerator = array.GetEnumerator();
        while (enumerator.MoveNext())
        {
            yield return (T) enumerator.Current;
        }
    }
}

public class ArrayExtensionsTest
{
    [Fact]
    public void ToArray()
    {
        var input = @"
ABCD
EFGH";

        var array = input.Lines().ToArray();
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
        var array = new int[2,3] {{1,2,3}, {4,5,6}};
        Assert.Equal([((0, 0), 1), ((1, 0), 2), ((2, 0), 3),
                      ((0, 1), 4), ((1, 1), 5), ((2, 1), 6)],
            array.ToEnumerable());
    }
}