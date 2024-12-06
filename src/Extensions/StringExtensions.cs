static class StringExtensions
{
    internal static string[] Lines(
            this string source,
            StringSplitOptions options = StringSplitOptions.RemoveEmptyEntries) =>
        source.Split(['\r', '\n'], options);

    internal static string[] Words(this string source) =>
        source.Split([' '], StringSplitOptions.RemoveEmptyEntries);

    internal static bool HasVowels(this string source, int atLeast) =>
        source.Count("aeiou".Contains) >= atLeast;

    internal static bool HasDouble(this string source) =>
        source.Zip(source.Skip(1))
              .Any(pair => pair.First == pair.Second);
}
