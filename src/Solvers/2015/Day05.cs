namespace Year2015.Day05;

[Solver(2015, 05, Part.A)]
class Nicer : Solver
{
    internal override object Solve(string input) =>
        input.Lines()
             .Count(IsNice);

    bool IsNice(string line) =>
         line.HasVowels(atLeast: 3) &&
         line.HasDouble()           &&
        !line.Contains("ab")        &&
        !line.Contains("cd")        &&
        !line.Contains("pq")        &&
        !line.Contains("xy");
}

[Solver(2015, 05, Part.B)]
class Nicer2 : Solver
{
    internal override object Solve(string input) =>
        input.Lines()
             .Count(IsNice);

    bool IsNice(string line) =>
        line.HasDoublePair() &&
        line.HasXOX();
}

static class StringExtensions
{
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
