namespace Year2024.Day05;

[Solver(2024, 05, Part.A)]
[Solver(2024, 05, Part.B)]
class PrintHelper : Solver
{
    internal PrintHelper(Part part) : base(part) {}

    internal override object Solve(string input)
    {
        var lines = input.Lines(StringSplitOptions.None);

        var rules = lines.TakeWhile(line => !line.Equals(string.Empty))
                         .SelectMany(line => line.Split('|').Parse<int>())
                         .ChunkTo<int, Tuple<int, int>>();
        var updates = lines.SkipWhile(line => !line.Equals(string.Empty))
                           .Skip(1)
                           .TakeWhile(line => !line.Equals(string.Empty))
                           .Select(line => line.Split(',').Parse<int>());

        #pragma warning disable CS8524
        return Part switch
        {
            Part.A => updates.Where(update => CheckUpdate(rules, update))
                             .Sum(update => update.Skip(update.Count() / 2).First()),
            Part.B => updates.Where(update => !CheckUpdate(rules, update))
                             .Select(update => update.ToArray())
                             .Select(update => FixUpdate(rules, update))
                             .Sum(update => update.Skip(update.Count() / 2).First()),
        };
    }

    bool CheckUpdate(IEnumerable<(int, int)> rules, IEnumerable<int> update)
    {
        return rules.All(rule => !FailRule(rule, update));
    }

    bool FailRule((int a, int b) rule, IEnumerable<int> update)
    {
        return update.SkipWhile(x => x != rule.b).Any(x => x == rule.a);
    }

    int[] FixUpdate(IEnumerable<(int a, int b)> rules, int[] update)
    {
        if (CheckUpdate(rules, update))
            return update;

        foreach (var rule in rules)
        {
            for (int i = 0; i < update.Length; i++)
            {
                if (update[i] != rule.b)
                    continue;
                for (int j = i + 1; j < update.Length; j++)
                    if (update[j] == rule.a)
                    {
                        (update[i], update[j]) = (update[j], update[i]);
                        break;
                    }
            }
        }
        return FixUpdate(rules, update);
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

        Assert.Equal(143, new PrintHelper(Part.A).Solve(input));
        Assert.Equal(123, new PrintHelper(Part.B).Solve(input));
    }

    [Fact]
    internal void Input()
    {
        var input = File.ReadAllText("./Solvers/2024/Data/Day05");

        Assert.Equal(7074, new PrintHelper(Part.A).Solve(input));
        Assert.Equal(4828, new PrintHelper(Part.B).Solve(input));
    }
}
