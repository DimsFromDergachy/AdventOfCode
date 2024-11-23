namespace Year2015.Day05;

[Solver(2015, 05, Part.A)]
class Nicer : Solver
{
    internal override object Solve(string input) => 
        input.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries)
             .Where(IsNice)
             .Count();

    bool IsNice(string line) =>
         line.HasVowels(atLeast: 3) &&
         line.HasDouble()           &&
        !line.Contains("ab")        &&
        !line.Contains("cd")        &&
        !line.Contains("pq")        &&
        !line.Contains("xy");
}
