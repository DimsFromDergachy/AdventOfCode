namespace Year2024.Day04;

[Solver(2024, 04, Part.A)]
[Solver(2024, 04, Part.B)]
class XMAS : Solver
{
    record Coord { internal int X; internal int Y; }

    List<(int, int)> ds = new List<(int, int)>
        { (1, 0), (1, 1), (0, 1), (-1, 1), (-1, 0), (-1, -1), (0, -1), (1, -1) };

    List<List<(Coord, char)>> masks = new List<List<(Coord, char)>>
    {
        new List<(Coord, char)>
        {
            (new Coord { X = -1, Y = -1}, 'M'),
            (new Coord { X =  0, Y =  0}, 'A'),
            (new Coord { X = +1, Y = +1}, 'S'),
            (new Coord { X = +1, Y = -1}, 'M'),
            (new Coord { X =  0, Y =  0}, 'A'),
            (new Coord { X = -1, Y = +1}, 'S'),
        },
        new List<(Coord, char)>
        {
            (new Coord { X = -1, Y = -1}, 'M'),
            (new Coord { X =  0, Y =  0}, 'A'),
            (new Coord { X = +1, Y = +1}, 'S'),
            (new Coord { X = -1, Y = +1}, 'M'),
            (new Coord { X =  0, Y =  0}, 'A'),
            (new Coord { X = +1, Y = -1}, 'S'),
        },
        new List<(Coord, char)>
        {
            (new Coord { X = +1, Y = +1}, 'M'),
            (new Coord { X =  0, Y =  0}, 'A'),
            (new Coord { X = -1, Y = -1}, 'S'),
            (new Coord { X = +1, Y = -1}, 'M'),
            (new Coord { X =  0, Y =  0}, 'A'),
            (new Coord { X = -1, Y = +1}, 'S'),
        },
        new List<(Coord, char)>
        {
            (new Coord { X = +1, Y = +1}, 'M'),
            (new Coord { X =  0, Y =  0}, 'A'),
            (new Coord { X = -1, Y = -1}, 'S'),
            (new Coord { X = -1, Y = +1}, 'M'),
            (new Coord { X =  0, Y =  0}, 'A'),
            (new Coord { X = +1, Y = -1}, 'S'),
        },
    };


    internal override object Solve(string input)
    {
        var lines = input.Lines();
        var n = lines.Count();
        var m = lines.First().Count();

        var array = new char[n, m];

        var x = 0;
        foreach (var line in lines)
        {
            var y = 0;
            foreach (var ch in line)
            {
                array[x, y] = ch;
                y++;
            }
            x++;
        }

        int res = 0;
        for (int i = 0; i < n; i++)
            for (int j = 0; j < m; j++)
                if (Part == Part.A)
                    foreach (var (dx, dy) in ds)
                        try
                        {
                            if ('X' == array[i + 0 * dx, j + 0 * dy]
                                    &&
                                'M' == array[i + 1 * dx, j + 1 * dy]
                                    &&
                                'A' == array[i + 2 * dx, j + 2 * dy]
                                    &&
                                'S' == array[i + 3 * dx, j + 3 * dy])
                            {
                                res++;
                            }
                        }
                        catch (IndexOutOfRangeException) {}
                else
                    foreach (var mask in masks)
                        try
                        {
                            var good = true;
                            foreach (var (coord, ch) in mask)
                                if (array[i + coord.X, j + coord.Y] != ch)
                                    good = false;
                            if (good) res++;
                        } catch (IndexOutOfRangeException) {}


        return res;
    }
}

public class XmasTest
{
    [Fact]
    public void Simple()
    {
        Assert.Equal(1, new XMAS().Solve("XMAS"));

        var input = @"
M.S
.A.
M.S
";

        var solver = new XMAS();
        solver.Part = Part.B;
        Assert.Equal(1, solver.Solve(input));
    }

    [Fact]
    public void ExampleA()
    {
        var input1 = @"
MMMSXXMASM
MSAMXMSMSA
AMXSXMAAMM
MSAMASMSMX
XMASAMXAMM
XXAMMXXAMA
SMSMSASXSS
SAXAMASAAA
MAMMMXMMMM
MXMXAXMASX";

        var input2 = @"
....XXMAS.
.SAMXMS...
...S..A...
..A.A.MS.X
XMASAMX.MM
X.....XA.A
S.S.S.S.SS
.A.A.A.A.A
..M.M.M.MM
.X.X.XMASX";

        Assert.Equal(18, new XMAS().Solve(input1));
        Assert.Equal(18, new XMAS().Solve(input2));
    }

    [Fact]
    public void ExampleB()
    {
        var input = @"
.M.S......
..A..MSMS.
.M.S.MAA..
..A.ASMSM.
.M.S.M....
..........
S.S.S.S.S.
.A.A.A.A..
M.M.M.M.M.
..........
";

        var solver = new XMAS();
        solver.Part = Part.B;
        Assert.Equal(9, solver.Solve(input));
    }
}