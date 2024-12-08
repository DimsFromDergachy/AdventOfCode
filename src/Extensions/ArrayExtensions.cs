static class ArrayExtensions
{
    internal static T[,] ToArray<T>(this IEnumerable<IEnumerable<T>> source)
    {
        var n = source.Count();
        var m = source.First().Count();
        var array = new T[n, m];

        foreach (var (xs, i) in source.Zip(Enumerable.Range(0, int.MaxValue)))
            foreach (var (x, j) in xs.Zip(Enumerable.Range(0, int.MaxValue)))
                array[i, j] = x;

        return array;
    }

    internal static IEnumerable<((int i, int j) Index, T Value)> ToEnumerable<T>(this T[,] array) =>
        array.GetIndexes()
             .Zip(array.GetValues());

    internal static IEnumerable<(int i, int j)> GetIndexes<T>(this T[,] array)
    {
        for (int i = array.GetLowerBound(0); i <= array.GetUpperBound(0); i++)
        for (int j = array.GetLowerBound(1); j <= array.GetUpperBound(1); j++)
            yield return (i, j);
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
        var list = new List<List<int>> {
            new List<int> { 1, 2, 3 },
            new List<int> { 4, 5, 6 },
        };

        var array = list.ToArray<int>();
        Assert.Equal(2, array.GetLength(0));
        Assert.Equal(3, array.GetLength(1));
        Assert.Equal(1, array[0, 0]);
        Assert.Equal(2, array[0, 1]);
        Assert.Equal(3, array[0, 2]);
        Assert.Equal(4, array[1, 0]);
        Assert.Equal(5, array[1, 1]);
        Assert.Equal(6, array[1, 2]);
    }

    [Fact]
    public void ToEnumerable()
    {
        var array = new int[2,3] {{1,2,3}, {4,5,6}};
        Assert.Equal([((0, 0), 1), ((0, 1), 2), ((0, 2), 3),
                      ((1, 0), 4), ((1, 1), 5), ((1, 2), 6)],
            array.ToEnumerable());
    }
}