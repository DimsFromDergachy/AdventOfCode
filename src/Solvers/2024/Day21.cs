namespace Year2024.Day21;

[Solver(2024, 21, Part.A)]
[Solver(2024, 21, Part.B)]
internal class RemoteController : Solver
{
    int ChainLength = 2;
    internal RemoteController(Part part) : base(part)
    {
        #pragma warning disable CS8524
        ChainLength = Part switch
        {
            Part.A => 2, Part.B => 25,
        };
    }
    internal RemoteController(Part part, int chainLength) : this(part)
    {
        ChainLength = chainLength;
    }
    internal override object Solve(string input)
    {
        Remote[] robots = Enumerable.Range(0, ChainLength)
                                    .Select(_ => new RemoteDirPad())
                                    .OfType<Remote>()
                                    .Prepend(new RemoteNumPad())
                                    .ToArray();

        return input.Lines()
                    // .Sum(line => SolveSlow(robots, line));
                    .Sum(SolveFast);
    }

    long SolveSlow(Remote[] robots, string line)
    {
        var input = long.Parse(line.SkipLast(1).ToArray());
        var moves = robots.Aggregate(line.Skip(0),
                                    (l, r) => r.Moves(l));
        return input * moves.LongCount();
    }

    long SolveFast(string input)
    {
        var line = new RemoteNumPad().Moves(input).ToArray();
        var dyno = new MultiRemoteDirPad(ChainLength).GetDyno();

        return long.Parse(input.SkipLast(1)
                               .ToArray())
                    *
               line.Prepend('A')
                   .Zip(line)
                   .Sum(pair => dyno[(pair.First, pair.Second, ChainLength)]);
    }

    internal abstract class Remote
    {
        protected char pos = 'A';

        #pragma warning disable CS8618
        protected string[,] moves;
        internal IEnumerable<char> Move(char moveTo)
        {
            (pos, var prevs) = (moveTo, pos);
            return moves[this[prevs], this[moveTo]].Trim().Append('A');
        }

        internal IEnumerable<char> Moves(IEnumerable<char> chs) => chs.SelectMany(Move);

        internal int this[char ch] => @this(ch);
        internal abstract int @this(char ch);
    }

    internal class RemoteNumPad : Remote
    {
        // +---+---+---+
        // | 7 | 8 | 9 |
        // +---+---+---+
        // | 4 | 5 | 6 |
        // +---+---+---+
        // | 1 | 2 | 3 |
        // +---+---+---+
        //     | 0 | A |
        //     +---+---+

        internal RemoteNumPad()
        {
            moves = new string[11, 11] {
//------+++--------++-------++-------++-------++-------++-------++-------++-------++-------++-------++-------+++-----++
//   #  |||    0   ||   1   ||   2   ||   3   ||   4   ||   5   ||   6   ||   7   ||   8   ||   9   ||   A   |||     ||
//------+++--------++-------++-------++-------++-------++-------++-------++-------++-------++-------++-------+++-----++
/*   0  */{ "     ", "  ^< ", "  ^  ", "  ^> ", " ^^< ", " ^^  ", " ^^> ", "^^^< ", "^^^  ", "^^^> ", " >   ", }, //||
/*   1  */{ "  >v ", "     ", "   > ", "   >>", "  ^  ", "  <^ ", "  ^>>", " ^^  ", " ^^> ", " ^^>>", ">>v  ", }, //||
/*   2  */{ "  v  ", "   < ", "     ", "   > ", "  <^ ", "  ^  ", "  ^> ", " <^^ ", " ^^  ", " ^^> ", " v>  ", }, //||
/*   3  */{ "  <v ", "   <<", "   < ", "     ", "  <<^", "  <^ ", "  ^  ", " <<^^", " <^^ ", " ^^  ", "  v  ", }, //||
/*   4  */{ " >vv ", "  v  ", "  v> ", "  v>>", "     ", "   > ", "   >>", "  ^  ", "  ^> ", "  ^>>", ">>vv ", }, //||
/*   5  */{ " vv  ", "  <v ", "  v  ", "  v> ", "   < ", "     ", "   > ", "  <^ ", "  ^  ", "  ^> ", " vv> ", }, //||
/*   6  */{ " <vv ", "  <<v", "  <v ", "  v  ", "   <<", "   < ", "     ", "  <<^", "  <^ ", "  ^  ", "  vv ", }, //||
/*   7  */{ ">vvv ", " vv  ", " vv> ", " vv>>", "  v  ", "  v> ", "  v>>", "     ", "   > ", "   >>", ">>vvv", }, //||
/*   8  */{ "vvv  ", " <vv ", " vv  ", " vv> ", "  <v ", "  v  ", "  v> ", "   < ", "     ", "   > ", " vvv>", }, //||
/*   9  */{ "<vvv ", " <<vv", " <vv ", " vv  ", "  <<v", "  <v ", "  v  ", "   <<", "   < ", "     ", "  vvv", }, //||
/*   A  */{ "   < ", "  ^<<", " <^  ", "  ^  ", " ^^<<", " <^^ ", " ^^  ", "^^^<<", "<^^^ ", "^^^  ", "     ", }, //||
//------+++--------++-------++-------++-------++-------++-------++-------++-------++-------++-------++-------++++----++
            };
        }

        // int[,] moves = new int[11, 11] {
        //     //        0  1  2  3  4  5  6  7  8  9  A
        //     /* 0 */ { 1, 3, 2, 3, 4, 3, 4, 5, 4, 5, 2 },
        //     /* 1 */ { 3, 1, 2, 3, 2, 3, 4, 3, 4, 5, 4 },
        //     /* 2 */ { 2, 2, 1, 2, 3, 2, 3, 4, 3, 4, 3 },
        //     /* 3 */ { 3, 3, 2, 1, 4, 3, 2, 5, 4, 3, 2 },
        //     /* 4 */ { 4, 2, 3, 4, 1, 2, 3, 2, 3, 4, 5 },
        //     /* 5 */ { 3, 3, 2, 3, 2, 1, 2, 3, 2, 3, 4 },
        //     /* 6 */ { 4, 4, 3, 2, 3, 2, 1, 4, 3, 2, 3 },
        //     /* 7 */ { 5, 3, 4, 5, 2, 3, 4, 1, 2, 3, 6 },
        //     /* 8 */ { 4, 4, 3, 4, 3, 2, 3, 2, 1, 2, 5 },
        //     /* 9 */ { 5, 5, 4, 3, 4, 3, 2, 3, 2, 1, 4 },
        //     /* A */ { 2, 4, 3, 2, 5, 4, 3, 6, 5, 4, 1 },
        // };

        internal override int @this(char ch) => ch switch
        {
            'A' => 10,
             _  => ch - '0',
        };
    }

    internal class RemoteDirPad : Remote
    {
        //     +---+---+
        //     | ^ | A |
        // +---+---+---+
        // | < | v | > |
        // +---+---+---+

        internal RemoteDirPad(char pos = 'A')
        {
            base.pos = pos;
            moves = new string[5, 5] {
//-----++++-----++-----++-----++-----++-----++-----++
//  #  ||||  ^  ||  A  ||  <  ||  v  ||  >  ||     ||
//-----++++-----++-----++-----++-----++-----++-----++
/*  ^  */{ "   ", " > ", "v< ", " v ", " v>", }, //||
/*  A  */{ "  <", "   ", "v<<", "<v ", "  v", }, //||
/*  <  */{ ">^ ", ">>^", "   ", ">  ", ">> ", }, //||
/*  v  */{ " ^ ", " ^>", " < ", "   ", " > ", }, //||
/*  >  */{ "<^ ", "  ^", " <<", "  <", "   ", }, //||
//-----++++-----++-----++-----++-----++-----++-----++
            };
        }

        #pragma warning disable CS8509
        internal override int @this(char ch) => ch switch
        {
            '^' => 0,
            'A' => 1,
            '<' => 2,
            'v' => 3,
            '>' => 4,
        };
    }

    internal class MultiRemoteDirPad
    {
        int StackSize = 0;
        public MultiRemoteDirPad(int stackSize) { StackSize = stackSize; }
        Dictionary<(char a, char b, int level), long> dyno = new();

        internal Dictionary<(char a, char b, int level), long> GetDyno()
        {
            for (int level = 0; level <= StackSize; level++)
            {
                foreach (char a in "^A<v>")
                foreach (char b in "^A<v>")
                {
                    if (level == 0)
                    {
                        dyno.Add((a, b, level), 1);
                        continue;
                    }

                    if (level == 1)
                    {
                        var robot1 = new RemoteDirPad(a);
                        var moves1 = robot1.Moves([b]).ToArray();

                        dyno.Add((a, b, level), moves1.Count());
                        continue;
                    }

                    var robot = new RemoteDirPad(a);
                    var moves = robot.Moves([b]).Prepend('A').ToArray();
                    var pairs = moves.Zip(moves.Skip(1));

                    dyno.Add((a, b, level), pairs.Sum(pair =>
                        dyno[(pair.First, pair.Second, level - 1)]));

                }
            }

            return dyno;
        }
    }
}

public class RemoteControllerTest
{
    [Theory]
    [InlineData(68 *  29, "029A")]
    [InlineData(60 * 980, "980A")]
    [InlineData(68 * 179, "179A")]
    [InlineData(64 * 456, "456A")]
    [InlineData(64 * 379, "379A")]
    internal void Example(long complexity, string input)
    {
        Assert.Equal(complexity, new RemoteController(Part.A).Solve(input));
    }

    [Theory]
    [InlineData("029A", "<A^A^^>AvvvA")]
    [InlineData("379A", "^A<<^^A>>AvvvA")]
    [InlineData(  "A4", "A^^<<A")]
    [InlineData(  "37", "^A<<^^A")]
    internal void RemoteNumPad(string input, string expected)
    {
        var rnp = new RemoteController.RemoteNumPad();
        var result = input.SelectMany(rnp.Move).ToArray();
        Assert.Equal(expected, new string(result));
    }

    [Theory]
    [InlineData("^>", "<Av>A")]
    [InlineData("^>AvvvA", "<Av>A^A<vAAA^>A")]
    [InlineData("<A^A^^>AvvvA", "v<<A>>^A<A>A<AAv>A^A<vAAA^>A")]

    [InlineData("^A<<^^A", "<A>Av<<AA>^AA>A")] // 37
    [InlineData("^A^^<<A", "<A>A<AAv<AA>>^A")] // 37

    [InlineData("<A>Av<<AA>^AA>A", "v<<A>>^AvA^A<vA<AA>>^AAvA<^A>AAvA^A")] // 37
    [InlineData("<A>A<AAv<AA>>^A", "v<<A>>^AvA^Av<<A>>^AA<vA<A>>^AAvAA<^A>A")] // 37

    [InlineData("<<^^A", "v<<AA>^AA>A")] // 37
    [InlineData("^^<<A", "<AAv<AA>>^A")] // 37
    [InlineData("v<<AA>^AA>A", "<vA<AA>>^AAvA<^A>AAvA^A")] // 37
    [InlineData("<AAv<AA>>^A", "v<<A>>^AA<vA<A>>^AAvAA<^A>A")] // 37
    internal void RemoteDirPad(string input, string expected)
    {
        var rnp = new RemoteController.RemoteDirPad();
        var result = input.SelectMany(rnp.Move).ToArray();
        Assert.Equal(expected, new string(result));
    }

    [Fact]
    internal void SubOptimalNumPad()
    {
        RemoteController.Remote[] robots = [
            new RemoteController.RemoteNumPad(),
            new RemoteController.RemoteDirPad(),
            new RemoteController.RemoteDirPad(),
            new RemoteController.RemoteDirPad(),
            new RemoteController.RemoteDirPad(),
        ];

        foreach (char a in "0123456789A")
        foreach (char b in "0123456789A")
        {
            var _  = robots[0].Move(a);
            var b1 = robots[0].Move(b);

            var currentResult = b1.SelectMany(robots[1].Move)
                                  .SelectMany(robots[2].Move)
                                  .SelectMany(robots[3].Move)
                                  .SelectMany(robots[4].Move)
                                  .ToArray();

            foreach (var b1_ in b1.SkipLast(1).Permutations())
            {
                // the restrict area
                switch (a, b)
                {
                    case ('0', '1'):
                    case ('0', '4'):
                    case ('0', '7'):
                    {
                        if (b1_.TakeWhile(c => c == '<').Count() >= 1)
                            continue;
                        break;
                    }
                    case ('1', '0'):
                    case ('1', 'A'):
                    {
                        if (b1_.TakeWhile(c => c == 'v').Count() >= 1)
                            continue;
                        break;
                    }
                    case ('4', '0'):
                    case ('4', 'A'):
                    {
                        if (b1_.TakeWhile(c => c == 'v').Count() >= 2)
                            continue;
                        break;
                    }
                    case ('7', '0'):
                    case ('7', 'A'):
                    {
                        if (b1_.TakeWhile(c => c == 'v').Count() >= 3)
                            continue;
                        break;
                    }
                    case ('A', '1'):
                    case ('A', '4'):
                    case ('A', '7'):
                    {
                        if (b1_.TakeWhile(c => c == '<').Count() >= 2)
                            continue;
                        break;
                    }

                }

                var result = b1_.Append('A')
                                .SelectMany(robots[1].Move)
                                .SelectMany(robots[2].Move)
                                .SelectMany(robots[3].Move)
                                .SelectMany(robots[4].Move)
                                .ToArray();

                if (result.Count() < currentResult.Count())
                {
                    Assert.Fail($"AB: '{a}{b}':"
                        + $"\n\tcurr: {new string(b1.ToArray())}"
                        + $"\n\tperm: {new string(b1_.Append('A').ToArray())}"
                        + $"\n\tgood: {new string(result)}"
                        + $"\n\tbad:  {new string(currentResult)}");
                }
            }
        }
    }

    [Fact]
    internal void SubOptimalDirPad()
    {
        RemoteController.Remote[] robots = [
            new RemoteController.RemoteDirPad(),
            new RemoteController.RemoteDirPad(),
            new RemoteController.RemoteDirPad(),
            new RemoteController.RemoteDirPad(),
            new RemoteController.RemoteDirPad(),
            new RemoteController.RemoteDirPad(),
        ];

        foreach (char a in "^A<v>")
        foreach (char b in "^A<v>")
        {
            var _  = robots[0].Move(a);
            var b1 = robots[0].Move(b);

            var currentResult = b1.SelectMany(robots[1].Move)
                                  .SelectMany(robots[2].Move)
                                  .SelectMany(robots[3].Move)
                                  .SelectMany(robots[4].Move)
                                  .SelectMany(robots[5].Move)
                                  .ToArray();

            foreach (var b1_ in b1.SkipLast(1).Permutations())
            {
                // the restrict area
                switch (a, b)
                {
                    case ('^', '<'):
                    {
                        if (b1_.TakeWhile(c => c == '<').Count() >= 1)
                            continue;
                        break;
                    }
                    case ('A', '<'):
                    {
                        if (b1_.TakeWhile(c => c == '<').Count() >= 2)
                            continue;
                        break;
                    }
                    case ('<', '^'):
                    case ('<', 'A'):
                    {
                        if (b1_.TakeWhile(c => c == '^').Count() >= 1)
                            continue;
                        break;
                    }
                }

                var result = b1_.Append('A')
                                .SelectMany(robots[1].Move)
                                .SelectMany(robots[2].Move)
                                .SelectMany(robots[3].Move)
                                .SelectMany(robots[4].Move)
                                .SelectMany(robots[5].Move)
                                .ToArray();

                if (result.Count() < currentResult.Count())
                {
                    Assert.Fail($"AB: '{a}{b}':"
                        + $"\n\tcurr: {new string(b1.ToArray())}"
                        + $"\n\tperm: {new string(b1_.Append('A').ToArray())}"
                        + $"\n\tgood: {new string(result)}"
                        + $"\n\tbad:  {new string(currentResult)}");
                }
            }
        }
    }

    [Theory]
    [InlineData(  "7A",  0,         12L *  7L)]
    [InlineData("029A",  0,         12L * 29L)]
    [InlineData("029A",  1,         28L * 29L)]
    [InlineData("029A",  2,         68L * 29L)]
    [InlineData("029A",  3,        164L * 29L)]
    [InlineData("029A",  4,        404L * 29L)]
    [InlineData("029A",  5,        998L * 29L)]
    [InlineData("029A", 15,    9041286L * 29L)]

    [InlineData("980A", 15,    7801248840)]
    [InlineData("179A", 15,    1602626380)]
    [InlineData("456A", 15,    4059230832)]
    [InlineData("379A", 15,    3256906938)]

    [InlineData("965A", 15,    8584680530)]
    [InlineData("143A", 15,    1414098686)]
    [InlineData("528A", 15,    5031315168)]
    [InlineData("670A", 15,    6219986540)]
    [InlineData("973A", 15,    8655850812)]

    internal void PartB(string input, int chain, long expected)
    {
        Assert.Equal(expected, (long) new RemoteController(Part.B, chain).Solve(input));
    }

    [Fact]
    internal void PatternOfPartB()
    {
        var dyno = new RemoteController.MultiRemoteDirPad(25).GetDyno();
        Assert.Equal(          2L, dyno[('A', '^',  1)]);
        Assert.Equal(          8L, dyno[('A', '^',  2)]);
        Assert.Equal(      10420L, dyno[('A', '^', 10)]);
        Assert.Equal( 9009012838L, dyno[('A', '^', 25)]);

        // 567878317421856 - too high
        // 567878317421856
        // 567878317421856
        // 308324721117162 - too high
        // 271397390297138
    }
}
