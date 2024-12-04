namespace Year2024.Day04;

[Solver(2024, 04, Part.A)]
class XMAS : Solver
{
    IEnumerable<(int, int)> ds = new List<(int, int)>
        { (1, 0), (1, 1), (0, 1), (-1, 1), (-1, 0), (-1, -1), (0, -1), (1, -1) };

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
                foreach (var (dx, dy) in ds)
                {
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
                }

        return res;
    }
}

public class XmasTest
{
    [Fact]
    public void Simple()
    {
        Assert.Equal(1, new XMAS().Solve("XMAS"));
    }

    [Fact]
    public void Example()
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
}