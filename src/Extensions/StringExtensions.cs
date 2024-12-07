static class StringExtensions
{
    internal static string[] Lines(
            this string source,
            StringSplitOptions options = StringSplitOptions.RemoveEmptyEntries) =>
        source.Split(['\r', '\n'], options);

    internal static string[] Words(this string source, char separator = ' ') =>
        source.Split(separator, StringSplitOptions.RemoveEmptyEntries);

    internal static bool HasVowels(this string source, int atLeast) =>
        source.Count("aeiou".Contains) >= atLeast;

    internal static bool HasDouble(this string source) =>
        source.Zip(source.Skip(1))
              .Any(pair => pair.First == pair.Second);

    internal static IEnumerable<T> Parse<T>(this string line, char separator = ' ') =>
        typeof(T).Name switch
        {
            nameof(Int32) => line.Words(separator).Select( int.Parse).OfType<T>(),
            nameof(Int64) => line.Words(separator).Select(long.Parse).OfType<T>(),
            _ => throw new NotImplementedException(nameof(Int32)),
        };
}

public class StringExtensionsTest
{
    [Fact]
    internal void ParseTest()
    {
        Assert.Throws<NotImplementedException>(() => "".Parse<string>());

        var input = "12 34 45 56";
        Assert.Equal([12, 34, 45, 56], input.Parse<int>());
        Assert.Equal([12, 34, 45, 56], input.Parse<long>());

        input = "1#2#3#4#5";
        Assert.Equal([1,2,3,4,5], input.Parse<int>('#'));
        Assert.Equal([1,2,3,4,5], input.Parse<long>('#'));
    }
}
