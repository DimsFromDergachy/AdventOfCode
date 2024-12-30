static class StringExtensions
{
    internal static string[] Lines(
            this string source,
            StringSplitOptions options = StringSplitOptions.RemoveEmptyEntries) =>
        source.Split(['\r', '\n'], options);

    internal static string[] Words(this IEnumerable<char> source, params char[] separators) =>
        new string(source.ToArray()).Split(separators, StringSplitOptions.RemoveEmptyEntries);

    internal static IEnumerable<T> Parse<T>(this IEnumerable<string> words) =>
        typeof(T).Name switch
        {
            nameof(Int32) => words.Select( int.Parse).OfType<T>(),
            nameof(Int64) => words.Select(long.Parse).OfType<T>(),
            _ => throw new NotImplementedException(nameof(T)),
        };
}

public class StringExtensionsTest
{
    [Fact]
    internal void ParseTest()
    {
        Assert.Throws<NotImplementedException>(() => "".Words().Parse<string>());

        var input = "12 34 45 56";
        Assert.Equal([12, 34, 45, 56], input.Words().Parse< int>());
        Assert.Equal([12, 34, 45, 56], input.Words().Parse<long>());

        input = "1#2#3#4#5";
        Assert.Equal([1, 2, 3, 4, 5], input.Split('#').Parse< int>());
        Assert.Equal([1, 2, 3, 4, 5], input.Split('#').Parse<long>());

        input = @"
1
2
3";
        Assert.Equal([1, 2, 3], input.Lines().Parse< int>());
        Assert.Equal([1, 2, 3], input.Lines().Parse<long>());
    }
}
