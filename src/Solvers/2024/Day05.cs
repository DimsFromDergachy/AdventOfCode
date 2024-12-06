namespace Year2024.Day05;

[Solver(2024, 05, Part.A)]
class PrintHelper : Solver
{
    internal override object Solve(string input)
    {
        var lines = input.Lines(StringSplitOptions.None);

        var rules = lines.TakeWhile(line => !line.Equals(string.Empty))
                         .Select(line => line.Split('|')
                                             .Select(word => int.Parse(word)))
                         .Select(rule => (rule.First(), rule.Skip(1).First()));
        var updates = lines.SkipWhile(line => !line.Equals(string.Empty))
                           .Skip(1)
                           .TakeWhile(line => !line.Equals(string.Empty))
                           .Select(line => line.Split(',')
                                               .Select(word => int.Parse(word)));

        Console.WriteLine($"r: {rules.Count()}");
        Console.WriteLine($"u: {updates.Count()}");

        return updates.Where(update => CheckUpdate(rules, update))
                      .Select(update => update.Skip(update.Count() / 2).First())
                      .Sum();
    }

    bool CheckUpdate(IEnumerable<(int, int)> rules, IEnumerable<int> update)
    {
        return rules.All(rule => !FailRule(rule, update));
    }

    bool FailRule((int a, int b) rule, IEnumerable<int> update)
    {
        return update.SkipWhile(x => x != rule.b).Any(x => x == rule.a);
    }
}

public class PrintHelperTest
{
    [Fact]
    internal void Example()
    {
        var input =
@"47|53
97|13
97|61
97|47
75|29
61|13
75|53
29|13
97|29
53|29
61|53
97|53
61|29
47|13
75|47
97|75
47|61
75|61
47|29
75|13
53|13

75,47,61,53,29
97,61,53,29,13
75,29,13
75,97,47,61,53
61,13,29
97,13,75,29,47";

        Assert.Equal(143, new PrintHelper().Solve(input));
    }

    [Fact]
    internal void Input()
    {
        var input = File.ReadAllText("./Solvers/2024/Input/Day05");

        Assert.Equal(7074, new PrintHelper().Solve(input));
    }
}
