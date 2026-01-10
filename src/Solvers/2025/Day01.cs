namespace Year2025.Day01;

[Solver(2025, 01, Part.A)]
[Solver(2025, 01, Part.B)]
class RotaryLock : Solver
{
    internal RotaryLock(Part part) : base(part) { }

    internal override object Solve(string input)
    {
        var inputs = input.Replace('L', '-')
                          .Replace('R', '+')
                          .Words()
                          .Parse<int>();

        var rotary = new Rotary();

        #pragma warning disable CS8524
        return Part switch
        {
            Part.A => inputs.Scan(50, MoveA)
                            .Count(x => x % 100 == 0),
            Part.B => inputs.Aggregate(new Rotary(), (r, x) => r.Rotate(x))
                            .Clicks,
        };
    }

    int MoveA(int acc, int rotate) => acc + rotate;

    class Rotary
    {
        internal int Position { get; private set; } = 50;
        internal int Clicks { get; private set; }

        internal Rotary Rotate(int rotate)
        {
            while (rotate != 0)
                rotate = RotateToClick(rotate);
            return this;
        }

        private int RotateToClick(int rotate)
        {
            if (Position == 0 && rotate < 0)
                Position = 100;

            if (Position + rotate >= 100)
            {
                (Position, rotate) = (Position + (100 - Position), rotate - (100 - Position));
                Position = 0;
                Clicks++;
                return rotate;
            }

            if (Position + rotate <= 0)
            {
                (Position, rotate) = (Position - Position, rotate + Position);
                Clicks++;
                return rotate;
            }

            Position += rotate;
            Assert.True(Position >   0);
            Assert.True(Position < 100);
            return 0;
        }
    }
}

public class Test
{
    [Fact]
    public void Example()
    {
        var input = @"
L68
L30
R48
L5
R60
L55
L1
L99
R14
L82";
        Assert.Equal(3, new RotaryLock(Part.A).Solve(input));

        Assert.Equal(1, new RotaryLock(Part.B).Solve(string.Join(" ",
            input.Lines().Take(1))));
        Assert.Equal(1, new RotaryLock(Part.B).Solve(string.Join(" ",
            input.Lines().Take(2))));
        Assert.Equal(2, new RotaryLock(Part.B).Solve(string.Join(" ",
            input.Lines().Take(3))));
        Assert.Equal(3, new RotaryLock(Part.B).Solve(string.Join(" ",
            input.Lines().Take(5))));
        Assert.Equal(4, new RotaryLock(Part.B).Solve(string.Join(" ",
            input.Lines().Take(6))));

        Assert.Equal(6, new RotaryLock(Part.B).Solve(input));
    }

    [Theory]
    [InlineData("R50", 1)]
    [InlineData("L50", 1)]
    [InlineData("R1", 0)]
    [InlineData("L1", 0)]
    [InlineData("R99", 0)]
    [InlineData("L99", 0)]
    public void OneStep(string input, int expected)
    {
        Assert.Equal(expected, new RotaryLock(Part.A).Solve(input));
    }
}