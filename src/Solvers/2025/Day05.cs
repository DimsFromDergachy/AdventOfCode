namespace Year2025.Day05;

[Solver(2025, 5, Part.A)]
[Solver(2025, 5, Part.B)]
class IDs : Solver
{
    internal IDs(Part part) : base(part) {}

    internal override object Solve(string input) => 42;
}

public class Test
{
    [Fact]
    public void Example()
    {
        var input = @"
3-5
10-14
16-20
12-18

1
5
8
11
17
32
";

        Assert.Equal(3, new IDs(Part.A).Solve(input));
    }
}