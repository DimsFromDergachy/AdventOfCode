using System.Text.RegularExpressions;

namespace Year2015.Day09;

[Solver(2015, 09, Part.A)]
class SalesSanta : Solver
{
    Regex regex = new Regex("(\\w+) to (\\w+) = (\\d+)");

    internal override object Solve(string input)
    {
        var graph = input.Lines()
                         .SelectMany(Parse)
                         .ToDictionary();

        var cities = graph.Keys.Select(pair => pair.Item1).Distinct();

        return cities.Permutations()
                     .Select(path => path.Zip(path.Skip(1))
                                         .Select(pair => graph[pair])
                                         .Sum())
                     .Min();
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
    public void Example()
    {
        var input = @"
London to Dublin = 464
London to Belfast = 518
Dublin to Belfast = 141";

        Assert.Equal(605, new SalesSanta().Solve(input));
    }
}