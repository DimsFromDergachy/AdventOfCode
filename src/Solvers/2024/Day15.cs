namespace Year2024.Day15;

[Solver(2024, 15, Part.A)]
class Boxer : Solver
{
    internal Boxer(Part part) : base(part) {}

    internal override object Solve(string input)
    {
        var lines = input.Lines(StringSplitOptions.None)
                         .SkipWhile(string.IsNullOrEmpty);

        var map = lines.TakeWhile(line => !string.IsNullOrEmpty(line))
                       .ToArray();

        var moves = lines.SkipWhile(line => !string.IsNullOrEmpty(line))
                         .SelectMany(line => line)
                         .ToList();

        var robot = map.ToEnumerable()
                       .Single(pair => pair.Value == '@')
                       .Index;

        return moves.Aggregate((robot, Map: map), Move)
                    .Map
                    .ToEnumerable()
                    // .Where(pair => pair.Value == 'O' || pair.Value == '@')
                    // .ToArray();
                    .Where(pair => pair.Value == 'O')
                    .Select(pair => pair.Index)
                    .Sum(index => 100 * index.x + index.y);
    }

    #pragma warning disable CS8509
    (int dx, int dy) this[char ch] => ch switch
    {
        '>' => (+1,  0),
        '^' => ( 0, -1),
        '<' => (-1,  0),
        'v' => ( 0, +1),
    };

    ((int x, int y), char[,]) Move(((int x, int y) robot, char[,] map) _, char move)
    {
        var (dx, dy) = this[move];
        var moveTo = Enumerable.Range(1, int.MaxValue)
                               .Select(k => (x: _.robot.x + k * dx, y: _.robot.y + k * dy))
                               .SkipWhile(ps => _.map[ps.x, ps.y] == 'O')
                               .First();

        if (_.map[moveTo.x, moveTo.y] == '#')
            return (_.robot, _.map);

        _.map[moveTo.x, moveTo.y] = 'O';
        _.map[_.robot.x + dx, _.robot.y + dy] = '@';
        _.map[_.robot.x, _.robot.y] = '.';

        return ((_.robot.x + dx, _.robot.y + dy), _.map);
    }
}

public class Test()
{
    [Fact]
    internal void SmallExample()
    {
        var input = @"
########
#..O.O.#
##@.O..#
#...O..#
#.#.O..#
#...O..#
#......#
########

<^^>>>vv<v>>v<<
";

        Assert.Equal(2028, new Boxer(Part.A).Solve(input));
    }

    [Fact]
    internal void BigExample()
    {
        var input = @"
##########
#..O..O.O#
#......O.#
#.OO..O.O#
#..O@..O.#
#O#..O...#
#O..O..O.#
#.OO.O.OO#
#....O...#
##########

<vv>^<v^>v>^vv^v>v<>v^v<v<^vv<<<^><<><>>v<vvv<>^v^>^<<<><<v<<<v^vv^v>^
vvv<<^>^v^^><<>>><>^<<><^vv^^<>vvv<>><^^v>^>vv<>v<<<<v<^v>^<^^>>>^<v<v
><>vv>v^v^<>><>>>><^^>vv>v<^^^>>v^v^<^^>v^^>v^<^v>v<>>v^v^<v>v^^<^^vv<
<<v<^>>^^^^>>>v^<>vvv^><v<<<>^^^vv^<vvv>^>v<^^^^v<>^>vvvv><>>v^<<^^^^^
^><^><>>><>^^<<^^v>>><^<v>^<vv>>v>>>^v><>^v><<<<v>>v<v<v>vvv>^<><<>^><
^>><>^v<><^vvv<^^<><v<<<<<><^v<<<><<<^^<v<^^^><^>>^<v^><<<^>>^v<v^v<v^
>^>>^v>vv>^<<^v<>><<><<v<<v><>v<^vv<<<>^^v^>^^>>><<^v>>v^v><^^>>^<>vv^
<><^^>^^^<><vvvvv^v<v<<>^v<v>v<<^><<><<><<<^^<<<^<<>><<><^^^>^^<>^>v<>
^^>vv<^v^v<vv>^<><v<^v>^^^>>>^^vvv^>vvv<>>>^<^>>>>>^<<^v>^vvv<>^<><<v>
v^^>>><<^^<>>^v^<v^vv<>v^<<>^<^v^v><^<<<><<^<v><v<>vv>>v><v^<vv<>v^<<^
";

        Assert.Equal(10092, new Boxer(Part.A).Solve(input));
    }
}