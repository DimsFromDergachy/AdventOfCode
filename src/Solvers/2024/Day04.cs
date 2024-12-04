namespace Year2024.Day04;

[Solver(2024, 04, Part.A)]
[Solver(2024, 04, Part.B)]
class XMAS : Solver
{
    public XMAS() { }
    internal XMAS(Part part) { Part = part; }

    List<(char ch, int dx, int dy)> maskA = new List<(char, int, int)>
    {
        ('X',  0,  0), ('M',  1,  0), ('A',  2,  0), ('S',  3,  0),
        ('X',  0,  0), ('M',  1,  1), ('A',  2,  2), ('S',  3,  3),
        ('X',  0,  0), ('M',  0,  1), ('A',  0,  2), ('S',  0,  3),
        ('X',  0,  0), ('M', -1,  1), ('A', -2,  2), ('S', -3,  3),
        ('X',  0,  0), ('M', -1,  0), ('A', -2,  0), ('S', -3,  0),
        ('X',  0,  0), ('M', -1, -1), ('A', -2, -2), ('S', -3, -3),
        ('X',  0,  0), ('M',  0, -1), ('A',  0, -2), ('S',  0, -3),
        ('X',  0,  0), ('M',  1, -1), ('A',  2, -2), ('S',  3, -3),
    };

    List<(char ch, int dx, int dy)> maskB = new List<(char, int, int)>
    {
        ('M', -1, -1), ('M', +1, -1), ('A', 0, 0), ('S', +1, +1), ('S', -1, +1),
        ('M', -1, -1), ('M', -1, +1), ('A', 0, 0), ('S', +1, +1), ('S', +1, -1),
        ('M', +1, +1), ('M', +1, -1), ('A', 0, 0), ('S', -1, -1), ('S', -1, +1),
        ('M', +1, +1), ('M', -1, +1), ('A', 0, 0), ('S', -1, -1), ('S', +1, -1),
    };

    internal override object Solve(string input)
    {
        var array = input.Lines().ToArray();

        #pragma warning disable CS8524
        var masks = Part switch
        {
            Part.A => maskA.Chunk(4),
            Part.B => maskB.Chunk(5),
        };

        return array.GetIndexes()
                    .SelectMany(idx => masks.Select(m => (idx.Item1, idx.Item2, m)))
                    .Count(idx => 
                    {
                        var (i, j, mask) = idx;
                        try
                        {
                            return mask.All(maskChars => maskChars.ch == array[i + maskChars.dx, j + maskChars.dy]);
                        } catch (IndexOutOfRangeException) {
                            return false;
                        }
                    });
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

        Assert.Equal(1, new XMAS(Part.B).Solve(input));
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

        Assert.Equal(9, new XMAS(Part.B).Solve(input));
    }
}