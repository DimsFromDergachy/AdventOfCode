namespace Year2024.Day01;

[Solver(2024, 01, Part.A)]
[Solver(2024, 01, Part.B)]
class Historian : Solver
{
    internal Historian(Part part) : base(part) {}

    internal override object Solve(string input)
    {
        var (list1, list2) = input.Lines()
             .SelectMany(line => line.Parse<int>())
             .Chunk(2)
             .Select(nums => (nums[0], nums[1]))
             .Unzip();

        #pragma warning disable CS8524
        return Part switch
        {
            Part.A => SolveA(list1, list2),
            Part.B => SolveB(list1, list2),
        };
    }

    int SolveA(IEnumerable<int> list1, IEnumerable<int> list2) =>
        list1.Order()
             .Zip(list2.Order(), (a, b) => Math.Abs(a - b))
             .Sum();

    int SolveB(IEnumerable<int> list1, IEnumerable<int> list2)
    {
        var group = list2.GroupBy(x => x)
                         .ToDictionary(group => group.Key);

        return list1.Select(x => group.ContainsKey(x) ? x * group[x].Count() : 0)
                    .Sum();
    }
}

public class Test
{
    [Fact]
    public void Example()
    {
        var input = @"
3   4
4   3
2   5
1   3
3   9
3   3";

        Assert.Equal(11, new Historian(Part.A).Solve(input));
        Assert.Equal(31, new Historian(Part.B).Solve(input));
    }
}
