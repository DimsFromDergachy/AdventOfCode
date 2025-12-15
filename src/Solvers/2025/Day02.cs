using System.Collections;

namespace Year2025.Day02;

[Solver(2025, 02, Part.A)]
[Solver(2025, 02, Part.B)]
class IdValidator : Solver
{
    internal IdValidator(Part part) : base(part) { }

    internal override object Solve(string input)
        => input.Words(',', '-')
                .Parse<long>()
                .Chunk(2)
                .Select(xs => new InvalidGenerator(Part, xs[0], xs[1]))
                .SelectMany(g => g.ToEnumerable())
                .Sum();

    internal class InvalidGenerator : IEnumerator<long>
    {
        private long current;
        private long start;
        private long finish;

        private readonly int[] gens = [2, 3, 4, 5, 6, 7, 8, 9, 10, 11];

        public InvalidGenerator(Part part, long start, long finish)
        {
            this.start = start;
            this.finish = finish;
            current = 0;

            if (part == Part.A)
                gens = [2];
        }

        public long Current => current;

        object IEnumerator.Current => Current;
        public void Dispose() { }

        public bool MoveNext()
        {
            current = gens.Min(g => Next(g, current == 0 ? start : ++current));
            return current <= finish;
        }

        public void Reset() => throw new NotImplementedException();
    }

    // next G N => XX...X (G times) and XX...X >= N
    internal static long Next(int G, long N)
    {
        var S = N.ToString();
        var size = S.Length / G;
        long x = S.Length % G == 0
            ? long.Parse(S.Substring(0, size))
            : (long)Math.Pow(10, size);

        if (Replicate(x) < N)
            x++;

        return Replicate(x);

        long Replicate(long x)
            => long.Parse(Enumerable.Repeat(x.ToString(), G)
                    .SelectMany(x => x)
                    .ToArray());
    }
}

public class SolverTest
{
    [Theory]
    [InlineData(Part.A, 33, "11-22")]
    [InlineData(Part.B, 33, "11-22")]
    [InlineData(Part.A, 99, "95-115")]
    [InlineData(Part.B, 99+111, "95-115")]
    [InlineData(Part.A, 1010, "998-1012")]
    [InlineData(Part.B, 1010+999, "998-1012")]
    [InlineData(Part.A, 3333, "1010-1212")]
    [InlineData(Part.B, 3333, "1010-1212")]
    [InlineData(Part.A, 1188511885, "1188511880-1188511890")]
    [InlineData(Part.B, 1188511885, "1188511880-1188511890")]
    [InlineData(Part.A, 222222, "222220-222224")]
    [InlineData(Part.B, 222222, "222220-222224")]
    [InlineData(Part.A, 0, "1698522-1698528")]
    [InlineData(Part.B, 0, "1698522-1698528")]
    [InlineData(Part.A, 446446, "446443-446449")]
    [InlineData(Part.B, 446446, "446443-446449")]
    [InlineData(Part.A, 38593859, "38593856-38593862")]
    [InlineData(Part.B, 38593859, "38593856-38593862")]
    [InlineData(Part.A, 0, "565653-565659")]
    [InlineData(Part.B, 565656, "565653-565659")]
    [InlineData(Part.A, 0, "824824821-824824827")]
    [InlineData(Part.B, 824824824, "824824821-824824827")]
    [InlineData(Part.A, 0, "2121212118-2121212124")]
    [InlineData(Part.B, 2121212121, "2121212118-2121212124")]
    internal void ExampleA(Part part, long expected, string input)
    {
        Assert.Equal(expected, new IdValidator(part).Solve(input));
    }

    [Theory]
    [InlineData(Part.A, 1227775554)]
    [InlineData(Part.B, 4174379265)]
    internal void Example(Part part, long expected)
    {
        var input = @"11-22,95-115,998-1012,1188511880-1188511890,222220-222224,
1698522-1698528,446443-446449,38593856-38593862,565653-565659,
824824821-824824827,2121212118-2121212124";

        Assert.Equal(expected, new IdValidator(part).Solve(input));
    }
}
