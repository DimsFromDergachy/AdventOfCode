namespace Year2024.Day15;

[Solver(2024, 15, Part.A)]
[Solver(2024, 15, Part.B)]
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

        if (Part == Part.B)
            map = map.Widen();

        var robot = map.ToEnumerable()
                       .Single(pair => pair.Value == '@')
                       .Index;

        if (Part == Part.A)
        {
            return moves.Aggregate((robot, Map: map), Move)
                        .Map
                        .ToEnumerable()
                        .Where(pair => pair.Value == 'O')
                        .Select(pair => pair.Index)
                        .Sum(index => index.x + 100 * index.y);
        }

        return moves.Aggregate((robot, Map: map.Print()), Move2)
                    .Map
                    .ToEnumerable()
                    .Where(pair => pair.Value == '[')
                    .Select(pair => pair.Index)
                    .Sum(index => index.x + 100 * index.y);
    }

    #pragma warning disable CS8509
    (int dx, int dy) this[char ch] => ch switch
    {
        '>' => (+1,  0),
        '^' => ( 0, -1),
        '<' => (-1,  0),
        'v' => ( 0, +1),
    };

    ((int, int), char[,]) Move(((int x, int y) robot, char[,] map) _, char move)
    {
        var (dx, dy) = this[move];
        var moveTo = Enumerable.Range(1, int.MaxValue)
                               .Select(k => (x: _.robot.x + k * dx, y: _.robot.y + k * dy))
                               .SkipWhile(ps => _.map[ps.x, ps.y] == 'O')
                               .First();

        if (_.map[moveTo.x, moveTo.y] == '#')
            return _;

        _.map[moveTo.x, moveTo.y] = 'O';
        _.map[_.robot.x + dx, _.robot.y + dy] = '@';
        _.map[_.robot.x, _.robot.y] = '.';

        return ((_.robot.x + dx, _.robot.y + dy), _.map);
    }

    ((int, int), char[,]) Move2(((int x, int y) robot, char[,] map) _, char move)
    {
        #region debug
        // _.map[_.robot.x, _.robot.y] = move;
        // _.map.Print();
        // _.map[_.robot.x, _.robot.y] = '@';
        #endregion

        var moveTo = (x: _.robot.x + this[move].dx, y: _.robot.y + this[move].dy);

        if (_.map[moveTo.x, moveTo.y] == '#')
            return _;

        var boxes = new  List<(int x, int y)>();
        var stack = new Stack<(int x, int y)>();

        if (_.map[moveTo.x, moveTo.y] == '[')
            stack.Push(moveTo);

        if (_.map[moveTo.x, moveTo.y] == ']')
            stack.Push((moveTo.x - 1, moveTo.y));

        while (stack.Any())
        {
            var box = stack.Pop();
            boxes.Insert(0, box);
            var  leftMoveTo = (x: 0 + box.x + this[move].dx, y: box.y + this[move].dy);
            var rightMoveTo = (x: 1 + box.x + this[move].dx, y: box.y + this[move].dy);

            // stuck => don't move
            if (_.map[ leftMoveTo.x,  leftMoveTo.y] == '#'
                    ||
                _.map[rightMoveTo.x, rightMoveTo.y] == '#')
                return _;

            if (_.map[leftMoveTo.x, leftMoveTo.y] == '[')
                stack.Push(leftMoveTo);

            if (_.map[leftMoveTo.x, leftMoveTo.y] == ']' && move != '>')
                stack.Push((leftMoveTo.x - 1, leftMoveTo.y));

            if (_.map[rightMoveTo.x, rightMoveTo.y] == '[' && move != '<')
                stack.Push(rightMoveTo);

            if (_.map[rightMoveTo.x, rightMoveTo.y] == ']')
                stack.Push((rightMoveTo.x - 1, rightMoveTo.y));
        }

        switch (move)
        {
            case '^': { boxes = boxes.OrderBy(box => box.y).ToList(); break; }
            case 'v': { boxes = boxes.OrderBy(box => box.y).Reverse().ToList(); break; }
        }

        foreach (var box in boxes)
        {
            var  leftMoveTo = (x: 0 + box.x + this[move].dx, y: box.y + this[move].dy);
            var rightMoveTo = (x: 1 + box.x + this[move].dx, y: box.y + this[move].dy);

            _.map[0 + box.x, box.y] = '.';
            _.map[1 + box.x, box.y] = '.';
            _.map[ leftMoveTo.x,  leftMoveTo.y] = '[';
            _.map[rightMoveTo.x, rightMoveTo.y] = ']';
        }

        _.map[moveTo.x, moveTo.y] = '@';
        _.map[_.robot.x, _.robot.y] = '.';

        return (moveTo, _.map);
    }
}

static class MapExtension
{
    internal static char[,] Widen(this char[,] map)
    {
        var wmap = new char[2 * map.GetLength(0), map.GetLength(1)];

        foreach (var (index, ch) in map.ToEnumerable())
        {
            wmap[2 * index.x, index.y] = ch switch
            {
                '#' => '#',
                'O' => '[',
                '.' => '.',
                '@' => '@',
            };
            wmap[2 * index.x + 1, index.y] = ch switch
            {
                '#' => '#',
                'O' => ']',
                '.' => '.',
                '@' => '.',
            };
        }

        return wmap;
    }

    internal static char[,] Print(this char[,] map)
    {
        Console.WriteLine("============================================");

        foreach (var chs in map.GetValues().Chunk(map.GetLength(0)))
        {
            Console.WriteLine(new string(chs));
        }

        return map;
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
    internal void Example()
    {
        var input = @"
#######
#...#.#
#.....#
#..OO@#
#..O..#
#.....#
#######

<vv<<^^<<^^
";

        Assert.Equal(618, new Boxer(Part.B).Solve(input));
    }

    [Fact]
    // ##############
    // ##..[]..##..##
    // ##.[.[].....##
    // ##..[]......##
    // ##...>......##
    // ##..........##
    // ##############
    internal void CorruptedMoveUp()
    {
        var input = @"
#######
#...#.#
#.O...#
#.OO@.#
#.O...#
#.....#
#######

<vv<<^>>^^<
";

        Assert.Equal(814, new Boxer(Part.B).Solve(input));
    }

    [Fact]
    internal void CorruptedMoveDown()
    {
        var input = @"
#######
#...#.#
#.O...#
#.OO@.#
#.O...#
#.....#
#######

<^^<<v>>vv<
";

        Assert.Equal(1614, new Boxer(Part.B).Solve(input));
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
        Assert.Equal( 9021, new Boxer(Part.B).Solve(input));
    }
}