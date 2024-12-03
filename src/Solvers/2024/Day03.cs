using System.Text.RegularExpressions;

namespace Year2024.Day03;

[Solver(2024, 03, Part.A)]
class Decorrupter : Solver
{
    Regex regex = new Regex("mul\\((\\d{1,3}),(\\d{1,3})\\)");

    internal override object Solve(string input) =>
        regex.Matches(input)
             .Select(match => match.Groups)
             .Select(group => int.Parse(group[1].Value) * int.Parse(group[2].Value))
             .Sum();
}

public class DecorrupterTest
{
    [Theory]
    [InlineData(0, "")]
    [InlineData(0, "mul(1234,123)")]
    [InlineData(0, "mul(123,1234)")]
    [InlineData(10000, "mul(100,100)")]
    [InlineData(161, "xmul(2,4)%&mul[3,7]!@^do_not_mul(5,5)+mul(32,64]then(mul(11,8)mul(8,5))")]
    public void Example(int expected, string input)
    {
        Assert.Equal(expected, new Decorrupter().Solve(input));
    }
}

