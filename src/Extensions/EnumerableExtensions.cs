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

    internal static IEnumerable<IEnumerable<TItem>> Permutations<TItem>(
        this IEnumerable<TItem> source)
        {
            if (source.Any())
            {
                foreach (var item in source)
                {
                    var lists = source.Except(new List<TItem> {item})
                                      .Permutations();
                    foreach (var list in lists)
                    {
                        yield return list.Prepend(item);
                    }
                }
            }
            else
            {
                yield return Enumerable.Empty<TItem>();
            }
        }
}

public class EnumerableExtensionsTest
{
    [Fact]
    public void Scan()
    {
        var array = new int[] { 1, 2, 3, 4, 5 };
        Assert.Equal([0, 1, 3, 6, 10, 15], array.Scan(0, (x, y) => x + y));
    }

    [Fact]
    public void Permutations()
    {
        var array = new int[] { 1, 2, 3 };
        Assert.Equal(1 * 2 * 3, array.Permutations().Count());
        Assert.All(array.Permutations(), p =>
            {
                Assert.Equal([1, 2, 3], p.Order());
            });
    }
}