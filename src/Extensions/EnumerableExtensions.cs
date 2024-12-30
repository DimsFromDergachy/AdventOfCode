static class EnumerableExtensions
{
    // Scan [1,2,3,4,5] 0 (+) => [0,1,3,6,10,15]
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

    // Unzip [(1, 'a'), (2, 'b'), (3, 'c')] => ([1,2,3], ['a', 'b', 'c'])
    internal static (IEnumerable<T1>, IEnumerable<T2>) Unzip<T1, T2>(
        this IEnumerable<(T1, T2)> source) =>
            (source.Select(item => item.Item1), source.Select(item => item.Item2));

    // Extract [1,2,3] => [(1, [2,3]), (2, [1,3]), (3, [1,2])]
    internal static IEnumerable<(TItem, IEnumerable<TItem>)> Extract<TItem>(
        this IEnumerable<TItem> source)
    {
        if (!source.Any())
            yield break;

        // Invariant: source === head ++ [elem] ++ tail
        var head = Enumerable.Empty<TItem>();
        var elem = source.First();
        var tail = source.Skip(1);

        yield return (elem, tail);
        while (tail.Any())
        {
            head = head.Append(elem);
            elem = tail.First();
            tail = tail.Skip(1);

            yield return (elem, head.Concat(tail));
        }
    }

    // Permutations [1,2,3] => [[1,2,3],[1,3,2],[2,1,3],[2,3,1],[3,1,2],[3,2,1]]
    internal static IEnumerable<IEnumerable<TItem>> Permutations<TItem>(
        this IEnumerable<TItem> source)
    {
        if (!source.Any())
        {
            yield return Enumerable.Empty<TItem>();
            yield break;
        }

        foreach (var (item, rest) in source.Extract())
        {
            foreach (var perm in rest.Permutations())
            {
                yield return perm.Prepend(item);
            }
        }
    }

    // Group   [1,1,2,1] => [[1,1], [2], [1]]
    // GroupBy [1,1,2,1] => [[1,1,1], [2]]
    public static IEnumerable<List<TSource>> Group<TSource>(this IEnumerable<TSource> source)
    {
        var list = new List<TSource>();

        foreach (var elem in source)
        {
            if (list.Any() && !list.First()!.Equals(elem))
            {
                yield return list;
                list = new List<TSource>();
            }
            list.Add(elem);
        }

        if (list.Any())
        {
            yield return list;
        }
    }

    // Cycle [1,2,3,4] => [1,2,3,4,1,2,3,4,1,2,3,4, ... ]
    internal static IEnumerable<TSource> Cycle<TSource>(this IEnumerable<TSource> source)
    {
        if (!source.Any())
            throw new ArgumentException("empty collection");

        while (true)
        {
            foreach (var item in source)
                yield return item;
        }
    }

    internal static IEnumerable<TSource> ToEnumerable<TSource>(this IEnumerator<TSource> enumerator)
    {
        do
            yield return enumerator.Current;
        while (enumerator.MoveNext());
    }
}

public class EnumerableExtensionsTest
{
    [Fact]
    public void Scan()
    {
        Assert.Equal([42], Enumerable.Empty<int>().Scan(42, null!));

        var array = new int[] { 1, 2, 3, 4, 5 };
        Assert.Equal([0, 1, 3, 6, 10, 15], array.Scan(0, (x, y) => x + y));
    }

    [Fact]
    public void Extract()
    {
        Assert.Empty(Enumerable.Empty<int>().Extract());

        var array = new int[] { 1, 2, 3, 5, 5 };
        Assert.Equal(array, array.Extract().Select(pair => pair.Item1));
        Assert.Collection(array.Extract(),
            p1 =>
            {
                // Why does this fail?
                // Assert.Equal((1, new int [] {2, 3, 5, 5}), (p1.Item1, p1.Item2.ToArray()));
                Assert.Equal(1, p1.Item1);
                Assert.Equal([2, 3, 5, 5], p1.Item2.ToArray());
            },
            p2 => Assert.Equal([1, 3, 5, 5], p2.Item2.ToArray()),
            p3 => Assert.Equal([1, 2, 5, 5], p3.Item2.ToArray()),
            p4 => Assert.Equal([1, 2, 3, 5], p4.Item2.ToArray()),
            p5 => Assert.Equal([1, 2, 3, 5], p5.Item2.ToArray())
        );
    }

    [Fact]
    public void Permutations()
    {
        Assert.Equal([Enumerable.Empty<int>()],
            Enumerable.Empty<int>().Permutations());

        var array = new int[] { 1, 2, 3 };
        Assert.Collection(array.Permutations(),
            p1 => Assert.Equal([1, 2, 3], p1),
            p2 => Assert.Equal([1, 3, 2], p2),
            p3 => Assert.Equal([2, 1, 3], p3),
            p4 => Assert.Equal([2, 3, 1], p4),
            p5 => Assert.Equal([3, 1, 2], p5),
            p6 => Assert.Equal([3, 2, 1], p6)
        );
    }

    [Fact]
    public void Group()
    {
        Assert.Empty(Enumerable.Empty<int>().Group());

        var array = new int[] { 1, 1, 2, 1 };
        Assert.Collection(array.Group(),
            p1 => Assert.Equal([1, 1], p1),
            p2 => Assert.Equal(   [2], p2),
            p3 => Assert.Equal(   [1], p3)
        );
    }

    [Fact]
    public void Cycle()
    {
        Assert.Throws<ArgumentException>(() => Enumerable.Empty<int>().Cycle().ToList());
    }
}