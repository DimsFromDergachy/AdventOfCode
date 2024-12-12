namespace Year2015.Day03;

struct Position { internal int X; internal int Y; }

[Solver(2015, 03, Part.A)]
[Solver(2015, 03, Part.B)]
class Visitor : Solver
{
    internal Visitor(Part part) : base(part) {}

    Position[] Start => [
        new Position { X = 0, Y = 0, },
        new Position { X = 0, Y = 0, }
    ];

    #pragma warning disable CS8524
    internal override object Solve(string input) =>
        Part switch
        {
            Part.A => input.Scan(new Position { X = 0, Y = 0 }, Move)
                           .Distinct()
                           .Count(),
            Part.B => input.Chunk(2)
                           .Scan(Start, Moves)
                           .SelectMany(ps => ps)
                           .Distinct()
                           .Count(),
        };

    Position[] Moves(Position[] ps, char[] moves) =>
        ps.Zip(moves, Move)
          .ToArray();

    #pragma warning disable CS8509
    Position Move(Position pos, char move) => move switch
    {
        '>' => pos with { X = ++pos.X },
        '<' => pos with { X = --pos.X },
        '^' => pos with { Y = ++pos.Y },
        'v' => pos with { Y = --pos.Y },
    };
}

public class VisitorTest
{
    [Theory]
    [InlineData(Part.A,  2, ">")]
    [InlineData(Part.A,  2, "^v")]
    [InlineData(Part.B,  3, "^v")]
    [InlineData(Part.A,  4, "^>v<")]
    [InlineData(Part.B,  3, "^>v<")]
    [InlineData(Part.A,  2, "^v^v^v^v^v")]
    [InlineData(Part.B, 11, "^v^v^v^v^v")]
    internal void Example(Part part, int expected, string input)
    {
        Assert.Equal(expected, new Visitor(part).Solve(input));
    }
}