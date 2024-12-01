namespace Year2024.Day01;

[Solver(2024, 01, Part.A)]
class Historian : Solver
{
    internal override object Solve(string input)
    {
        var (list1, list2) = input.Lines()
             .SelectMany(line => line.Split(' ', StringSplitOptions.RemoveEmptyEntries))
             .Select(int.Parse)
             .Chunk(2)
             .Select(nums => (nums[0], nums[1]))
             .Unzip();

        return list1.Order()
                    .Zip(list2.Order(), (a, b) => Math.Abs(a - b))
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

        Assert.Equal(11, new SolverA().Solve(input));
    }
}
