static class EnumerableExtensions
{
    internal static IEnumerable<TAccumulate> Scan<TSource, TAccumulate>(
        this IEnumerable<TSource> source,
        TAccumulate seed,
        Func<TAccumulate, TSource, TAccumulate> func)
        {
            yield return seed;
            foreach (var item in source)
            {
                seed = func(seed, item);
                yield return seed;
            }
        }

    internal static (IEnumerable<T1>, IEnumerable<T2>) Unzip<T1, T2>(
        this IEnumerable<(T1, T2)> source) =>
            (source.Select(item => item.Item1), source.Select(item => item.Item2));
}

public class EnumerableExtensionsTest
{
    [Fact]
    public void ScanTest()
    {
        var array = new int[] { 1, 2, 3, 4, 5 };
        Assert.Equal([0, 1, 3, 6, 10, 15], array.Scan(0, (x, y) => x + y));
    }
}