using System.Text.RegularExpressions;

namespace Year2015.Day09;

[Solver(2015, 09, Part.A)]
[Solver(2015, 09, Part.B)]
class SalesSanta : Solver
{
    internal SalesSanta(Part part) : base(part) {}

    Regex regex = new Regex("(\\w+) to (\\w+) = (\\d+)");

    internal override object Solve(string input)
    {
        var graph = input.Lines()
                         .SelectMany(Parse)
                         .ToDictionary();

        var distances = graph.Keys
                             .Select(pair => pair.Item1)
                             .Distinct()
                             .Permutations()
                             .Select(path => path.Zip(path.Skip(1))
                                                 .Select(pair => graph[pair])
                                                 .Sum());

        #pragma warning disable CS8524
        return Part switch
        {
            Part.A => distances.Min(),
            Part.B => distances.Max(),
        };
    }

    IEnumerable<((string, string), int)> Parse(string line)
    {
        var group = regex.Match(line).Groups;

        var city1 =           group[1].Value;
        var city2 =           group[2].Value;
        var dist  = int.Parse(group[3].Value);

        yield return ((city1, city2), dist);
        yield return ((city2, city1), dist);
    }
}

public class SalesSantaTest
{
    [Fact]
    internal void Example()
    {
        var input = @"
London to Dublin = 464
London to Belfast = 518
Dublin to Belfast = 141";

        Assert.Equal(605, new SalesSanta(Part.A).Solve(input));
        Assert.Equal(982, new SalesSanta(Part.B).Solve(input));
    }
}