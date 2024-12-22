using System.Collections;

namespace Year2024.Day22;

[Solver(2024, 22, Part.A)]
class PseudoRandomGenerator : Solver
{
    internal override object Solve(string input)
    {
        return input.Parse<long>('\r', '\n')
                    .Sum(s => Enumerable.Range(0, 2000)
                                        .Aggregate(
                                            new Generator(s),
                                            (gen, _) => {
                                                gen.MoveNext(); return gen;
                                            })
                                        .Current);
    }

    internal class Generator : IEnumerator<long>
    {
        const long PRUNE = 16777216;
        long _secret;

        public Generator(long seed)
        {
            _secret = seed;
        }

        public long Current => _secret;
        object IEnumerator.Current => Current;

        public bool MoveNext()
        {
            long s0 = _secret;
            long s1 = ((s0 <<  6) ^ s0) % PRUNE;
            long s2 = ((s1 >>  5) ^ s1) % PRUNE;
            _secret = ((s2 << 11) ^ s2) % PRUNE;
            return true;
        }

        public void Dispose() {}
        public void Reset() {}
    }
}

public class PseudoRandomGeneratorTest
{
    [Fact]
    internal void Generator123()
    {
        var gen = new PseudoRandomGenerator.Generator(123);     _ = gen.MoveNext();

        Assert.Equal(15887950, gen.Current);                    _ = gen.MoveNext();
        Assert.Equal(16495136, gen.Current);                    _ = gen.MoveNext();
        Assert.Equal(  527345, gen.Current);                    _ = gen.MoveNext();
        Assert.Equal(  704524, gen.Current);                    _ = gen.MoveNext();
        Assert.Equal( 1553684, gen.Current);                    _ = gen.MoveNext();
        Assert.Equal(12683156, gen.Current);                    _ = gen.MoveNext();
        Assert.Equal(11100544, gen.Current);                    _ = gen.MoveNext();
        Assert.Equal(12249484, gen.Current);                    _ = gen.MoveNext();
        Assert.Equal( 7753432, gen.Current);                    _ = gen.MoveNext();
        Assert.Equal( 5908254, gen.Current);                    _ = gen.MoveNext();
    }
}
