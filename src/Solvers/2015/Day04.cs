using System.Security.Cryptography;
using System.Text;

namespace Year2015.Day04;

[Solver(2015, 04, Part.A)]
[Solver(2015, 04, Part.B)]
class Miner : Solver
{
    internal Miner(Part part) : base(part) {}

    #pragma warning disable CS8524
    private string HashPrefix => Part switch
    {
        Part.A => "00000",
        Part.B => "000000",
    };

    internal override object Solve(string input) =>
        Enumerable.Range(0, int.MaxValue)
                  .Select(nonce => (nonce, Hash(input, nonce)))
                  .First(pair => pair.Item2.StartsWith(HashPrefix));

    string Hash(string input, int nonce)
    {
        using var md5 = MD5.Create();
        var bytes = Encoding.ASCII.GetBytes(input + nonce);
        var hash = md5.ComputeHash(bytes);
        return Convert.ToHexString(hash);
    }
}

public class MinerTest
{
    [Theory]
    [InlineData(Part.A,  609043, "abcdef")]
    [InlineData(Part.A, 1048970, "pqrstuv")]
    [InlineData(Part.A,  346386, "iwrupvqb")]
    //[InlineData(Part.B, 9958218, "iwrupvqb")] // +6 sec
    internal void Example(Part part, int expected, string input)
    {
        (int result, string _) = (ValueTuple<int, string>) new Miner(part).Solve(input);
        Assert.Equal(expected, result);
    }
}