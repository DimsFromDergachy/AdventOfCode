namespace Year2015.Day05;

[Solver(2015, 05, Part.A)]
[Solver(2015, 05, Part.B)]
class Nicer : Solver
{
    internal Nicer(Part part) : base(part) {}

    internal override object Solve(string input) =>
        input.Lines()
             .Count(IsNice);

    #pragma warning disable CS8524
    bool IsNice(string line) => Part switch
    {
        Part.A => line.HasVowels(atLeast: 3) &&
                  line.HasDouble()           &&
                 !line.Contains("ab")        &&
                 !line.Contains("cd")        &&
                 !line.Contains("pq")        &&
                 !line.Contains("xy"),
        Part.B => line.HasDoublePair() &&
                  line.HasXOX(),

    };
}

static class StringExtensions
{
    internal static bool HasVowels(this string source, int atLeast) =>
        source.Count("aeiou".Contains) >= atLeast;

    internal static bool HasDouble(this string source) =>
        source.Zip(source.Skip(1))
              .Any(pair => pair.First == pair.Second);

    internal static bool HasXOX(this string source) =>
        source.Zip(source.Skip(2))
              .Any(pair => pair.First == pair.Second);

    internal static bool HasDoublePair(this string source) =>
        Enumerable.Range(0, int.MaxValue)
                  // zipping result: "abcd" -> [(0, a, b), (1, b, c), (2, c, d)]
                  .Zip(source, source.Skip(1))
                  .GroupBy(triple => (triple.Second, triple.Third))
                  .Any(group => group.Max(x => x.First) - group.Min(x => x.First) > 1);
}

public class NicerTest
{
    [Theory]
    [InlineData(true, "ugknbfddgicrmopn")]
    [InlineData(true, "aaa")]
    [InlineData(false, "jchzalrnumimnmhp")]
    [InlineData(false, "haegwjzuvuyypxyu")]
    [InlineData(false, "dvszwmarrgswjxmb")]
    internal void ExampleA(bool isNice, string input)
    {
        Assert.Equal(isNice ? 1 : 0, new Nicer(Part.A).Solve(input));
    }

    [Theory]
    [InlineData(true, "qjhvhtzxzqqjkmpb")]
    [InlineData(true, "xxyxx")]
    [InlineData(false, "uurcxstgmygtbstg")]
    [InlineData(false, "ieodomkazucvgmuy")]
    internal void ExampleB(bool isNice, string input)
    {
        Assert.Equal(isNice ? 1 : 0, new Nicer(Part.B).Solve(input));
    }
}
