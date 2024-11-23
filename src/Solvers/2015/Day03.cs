namespace Year2015.Day03;

[Solver(2015, 03, Part.A)]
class SolverA : Solver
{
    internal override object Solve(string input) =>
        input.Scan(new Position { X = 0, Y = 0 }, Move)
             .Distinct()
             .Count();

    #pragma warning disable CS8509
    Position Move(Position pos, char move) => move switch
    {
        '>' => pos with { X = ++pos.X },
        '<' => pos with { X = --pos.X },
        '^' => pos with { Y = ++pos.Y },
        'v' => pos with { Y = --pos.Y },
    };
}

struct Position { internal int X; internal int Y; }

[Solver(2015, 03, Part.B)]
class SolverB : Solver
{
    Position[] Start => [
        new Position { X = 0, Y = 0, },
        new Position { X = 0, Y = 0, }
    ];

    internal override object Solve(string input) =>
        input.Chunk(2)
             .Scan(Start, Moves)
             .SelectMany(ps => ps)
             .Distinct()
             .Count();

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