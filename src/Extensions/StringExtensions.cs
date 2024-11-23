namespace Year2015.Day05;

static class StringExtensions
{
    internal static bool HasVowels(this string source, int atLeast) =>
        source.Count("aeiou".Contains) >= atLeast;

    internal static bool HasDouble(this string source) =>
        source.Zip(source.Skip(1))
              .Any(pair => pair.First == pair.Second);
}