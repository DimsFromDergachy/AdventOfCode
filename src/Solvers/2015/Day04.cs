using System.Security.Cryptography;
using System.Text;

namespace Year2015.Day04;

[Solver(2015, 04, Part.A)]
class Miner : Solver
{
    internal override object Solve(string input) =>
        Enumerable.Range(0, int.MaxValue)
                  .Select(nonce => (nonce, Hash(input, nonce)))
                  .First(pair => pair.Item2.StartsWith("00000"));

    string Hash(string input, int nonce)
    {
        using var md5 = MD5.Create();
        var bytes = Encoding.ASCII.GetBytes(input + nonce);
        var hash = md5.ComputeHash(bytes);
        return Convert.ToHexString(hash);
    }
}