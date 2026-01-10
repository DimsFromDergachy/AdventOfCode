namespace Year2025.Day06;

[Solver(2025, 06, Part.A)]
[Solver(2025, 06, Part.B)]
class MathHomework : Solver
{
    internal MathHomework(Part part) : base(part) {}

    internal override object Solve(string input)
    {
        var lines = input.Lines();

        var matrix = lines.SkipLast(1)
                          .Select(line => line.Words().Parse<long>())
                          .ToArray();

        var ops = lines.Last()
                       .Words()
                       .Select(word => word.First())
                       .Select<char, Monoid<long>>(
                                #pragma warning disable CS8509
                                op => op switch
                                {
                                    '+' => new Sum<long>(),
                                    '*' => new Product<long>(),
                                })
                       .ToArray();

        return matrix.FoldColumns(ops)
                     .Sum();
    }
}

public class Test
{
    [Fact]
    public void Example()
    {
        var input = @"
123  328   51  64 
 45  64   387  23 
  6  98   215  314
*    +    *    +  
";

        Assert.Equal(4277556L, new MathHomework(Part.A).Solve(input));
        Assert.Equal(3263827L, new MathHomework(Part.B).Solve(input));
    }
}