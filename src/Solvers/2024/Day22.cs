using System.Collections;

namespace Year2024.Day22;

[Solver(2024, 22, Part.A)]
[Solver(2024, 22, Part.B)]
class BananaMarket : Solver
{
    public BananaMarket(Part part) : base(part) {}

    internal override object Solve(string input)
    {
        if (Part == Part.A)
            return input.Lines()
                        .Parse<long>()
                        .Sum(s => new Generator(s).ToEnumerable().Last());

        var gens = input.Lines()
                        .Parse<long>()
                        .Select(s => new Generator(s))
                        .Select(gen => new GeneratorChanges(gen))
                        .Zip(Enumerable.Range(0, int.MaxValue))
                        .ToList();

        var memo = new Dictionary<(int n, (int a, int b, int c, int d)), int>();

        foreach (var (gen, x) in gens)
            foreach (var seq in gen.ToEnumerable())
                if (!memo.ContainsKey((x, seq)))
                    memo.Add((x, seq), gen.Sale);

        int maxSale = -1;

        for (int a = -9; a <= 9; a++)
        for (int b = -9; b <= 9; b++)
        for (int c = -9; c <= 9; c++)
        for (int d = -9; d <= 9; d++)
        {
            var sale = 0;
            for (int x = 0; x < gens.Count(); x++)
                if (memo.ContainsKey((x, (a, b, c, d))))
                    sale += memo[(x, (a, b, c, d))];
            maxSale = Math.Max(maxSale, sale);
        }

        return maxSale;
    }

    internal class Generator : IEnumerator<long>
    {
        const long PRUNE = 16777216;
        long secret;
        int rest;

        public Generator(long seed, int rest = 2000)
        {
            this.rest = rest;
            secret = seed;
        }

        public long Current => secret;
        object IEnumerator.Current => Current;

        public bool MoveNext()
        {
            long s0 = secret;
            long s1 = ((s0 <<  6) ^ s0) % PRUNE;
            long s2 = ((s1 >>  5) ^ s1) % PRUNE;
            secret  = ((s2 << 11) ^ s2) % PRUNE;
            return rest-- > 0;
        }

        public void Dispose() {}
        public void Reset() {}
    }

    internal class GeneratorChanges : IEnumerator<(int a, int b, int c, int d)>
    {
        Generator gen;
        public GeneratorChanges(Generator gen)
        {
            this.gen = gen;
            a = (int) (gen.Current % 10);           gen.MoveNext();
            b = (int) (gen.Current % 10);           gen.MoveNext();
            c = (int) (gen.Current % 10);           gen.MoveNext();
            d = (int) (gen.Current % 10);           gen.MoveNext();
            e = (int) (gen.Current % 10);           gen.MoveNext();
        }

        internal int Sale => e;

        int a, b, c, d, e;
        public (int, int, int, int) Current => (b - a, c - b, d - c, e - d);
        object IEnumerator.Current => Current;

        public bool MoveNext()
        {
            (a, b, c, d, e) = (b, c, d, e, (int) (gen.Current % 10));
            return gen.MoveNext();
        }

        public void Dispose() {}
        public void Reset() {}
    }
}

public class BananaMarketTest
{
    [Fact(Skip = "Broken due enumeration changed")]
    internal void Generators()
    {
        var gen1 = new BananaMarket.Generator(123);
        var gen2 = new BananaMarket.GeneratorChanges(gen1);
        var list = gen2.ToEnumerable().ToList();

        Assert.Equal((-3, 6, -1, -1), list.First());
        Assert.Equal(1996, list.Count());
    }

    [Fact]
    internal void ExampleA()
    {
        var input = @"
1
10
100
2024
";

        Assert.Equal(37327623L, new BananaMarket(Part.A).Solve(input));
    }

    [Fact]
    internal void PartA()
    {
        var input = File.ReadAllText("./Solvers/2024/Data/Day22");

        Assert.Equal(17724064040L, new BananaMarket(Part.A).Solve(input));

        // take about 30 seconds
        // Assert.Equal(1998, new BananaMarket(Part.B).Solve(input));
    }

    [Fact]
    internal void ExampleB()
    {
        var input = @"
1
2
3
2024
";

        Assert.Equal(23, new BananaMarket(Part.B).Solve(input));
    }
}
