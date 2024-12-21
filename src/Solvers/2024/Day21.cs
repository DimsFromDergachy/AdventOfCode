namespace Year2024.Day21;

[Solver(2024, 21, Part.A)]
internal class RemoteController : Solver
{
    internal RemoteController(Part part) : base(part) {}

    internal override object Solve(string input)
    {
        Remote[] robots =
        [
            new RemoteNumPad(),
            new RemoteDirPad(),
            new RemoteDirPad(),
        ];

        return input.Lines()
                    .Select(line => int.Parse(line.SkipLast(1)
                                                  .ToArray())
                        * robots.Aggregate(line.Skip(0),
                                          (line, robot) => line.SelectMany(ch => robot.Move(ch)))
                                .Count())
                    .Sum();
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

        internal RemoteDirPad()
        {
            moves = new string[5, 5] {
//-----++++-----++-----++-----++-----++-----++-----++
//  #  ||||  ^  ||  A  ||  <  ||  v  ||  >  ||     ||
//-----++++-----++-----++-----++-----++-----++-----++
/*  ^  */{ "   ", " > ", "v< ", " v ", " >v", }, //||
/*  A  */{ "  <", "   ", "v<<", " v<", "  v", }, //||
/*  <  */{ ">^ ", ">>^", "   ", ">  ", ">> ", }, //||
/*  v  */{ " ^ ", " >^", " < ", "   ", " > ", }, //||
/*  >  */{ " ^<", "  ^", " <<", "  <", "   ", }, //||
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
}

public class RemoteControllerTest
{
    [Theory]
    [InlineData(68 *  29, "029A")]
    [InlineData(60 * 980, "980A")]
    [InlineData(68 * 179, "179A")]
    [InlineData(64 * 456, "456A")]
    [InlineData(64 * 379, "379A")]
    internal void Example(int complexity, string input)
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
    [InlineData("^>", "<A>vA")]
    [InlineData("^>AvvvA", "<A>vA^Av<AAA>^A")]
    [InlineData("<A^A^^>AvvvA", "v<<A>>^A<A>A<AA>vA^Av<AAA>^A")]
    [InlineData("^A^^<<A>>AvvvA", "<A>A<AAv<AA>>^AvAA^Av<AAA>^A")] // 379A -> 1
    [InlineData("<A>A<AAv<AA>>^AvAA^Av<AAA>^A", "v<<A>>^AvA^Av<<A>>^AAv<A<A>>^AAvAA^<A>Av<A>^AA<A>Av<A<A>>^AAAvA^<A>A")] // 379A -> 2
    [InlineData("^A<<^^A>>AvvvA", "<A>Av<<AA>^AA>AvAA^Av<AAA>^A")] // 379A -> 1
    [InlineData("<A>Av<<AA>^AA>AvAA^Av<AAA>^A", "v<<A>>^AvA^Av<A<AA>>^AAvA^<A>AAvA^Av<A>^AA<A>Av<A<A>>^AAAvA^<A>A")] // 379A -> 2

    [InlineData("^A<<^^A", "<A>Av<<AA>^AA>A")] // 37
    [InlineData("^A^^<<A", "<A>A<AAv<AA>>^A")] // 37

    [InlineData("<A>Av<<AA>^AA>A", "v<<A>>^AvA^Av<A<AA>>^AAvA^<A>AAvA^A")] // 37
    [InlineData("<A>A<AAv<AA>>^A", "v<<A>>^AvA^Av<<A>>^AAv<A<A>>^AAvAA^<A>A")] // 37

    [InlineData("<<^^A", "v<<AA>^AA>A")] // 37
    [InlineData("^^<<A", "<AAv<AA>>^A")] // 37
    [InlineData("v<<AA>^AA>A", "v<A<AA>>^AAvA^<A>AAvA^A")] // 37
    [InlineData("<AAv<AA>>^A", "v<<A>>^AAv<A<A>>^AAvAA^<A>A")] // 37

    internal void RemoteDirPad(string input, string expected)
    {
        var rnp = new RemoteController.RemoteDirPad();
        var result = input.SelectMany(rnp.Move).ToArray();
        Assert.Equal(expected, new string(result));
    }

    [Fact]
    internal void SubOptimals()
    {
        RemoteController.Remote[] robots = [
            new RemoteController.RemoteNumPad(),
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
                                  .ToArray();


            foreach (var b1_ in b1.SkipLast(1).Permutations())
            {
                switch (a, b)
                {
                    // not allowed
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
}
