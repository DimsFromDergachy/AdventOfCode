namespace Year2024.Day23;

[Solver(2024, 23, Part.A)]
[Solver(2024, 23, Part.B)]
class LanParty : Solver
{
    internal LanParty(Part part) : base(part) {}

    internal override object Solve(string input)
    {
        var edges = input.Lines()
                         .Select(line => (a: line.Substring(0, 2),
                                          b: line.Substring(3, 2)))
                         .ToList();

        var graph = new Graph<string>(edges);

        if (Part == Part.A)
        {
            return graph.Completes()
                        .Where(c => c.Count() == 3)
                        .Where(c => c.Any(s => s.StartsWith('t')))
                        .Select(c => string.Join("", c.Order()))
                        .Count();
        }

        return string.Join(',', graph.Completes()
                                     .MaxBy(c => c.Count())!
                                     .Order());
    }
}

class Graph<TVertex> where TVertex : notnull
{
    bool[,] _adjacency;
    Dictionary<TVertex, int> _vertices;

    int this[TVertex v] => _vertices[v];
    bool this[TVertex a, TVertex b] =>
        _adjacency[this[a], this[b]];

    internal Graph(IList<(TVertex a, TVertex b)> edges)
    {
        _vertices = edges.SelectMany(p => new TVertex[] {p.a, p.b})
                         .Distinct()
                         .Zip(Enumerable.Range(0, int.MaxValue))
                         .ToDictionary(pair => pair.First, pair => pair.Second);

        _adjacency = new bool[_vertices.Count(), _vertices.Count()];
        foreach (var edge in edges)
        {
            _adjacency[this[edge.a], this[edge.b]] = true;
            _adjacency[this[edge.b], this[edge.a]] = true;
        }
    }

    internal IEnumerable<IList<TVertex>> Completes() =>
        Completes(Enumerable.Empty<TVertex>().ToList(), _vertices.Keys.ToList());

    private IEnumerable<IList<TVertex>> Completes(IList<TVertex> complete, IList<TVertex> vertices)
    {
        yield return complete;

        while (vertices.Any())
        {
            var v = vertices.First();
            vertices = vertices.Skip(1).ToList();

            if (complete.All(w => this[v, w]))
                foreach (var c in Completes(complete.Append(v).ToList(), vertices))
                    yield return c;
        }
    }
}

public class LanPartyTest
{
    [Fact]
    internal void Example()
    {
        var input = @"
kh-tc
qp-kh
de-cg
ka-co
yn-aq
qp-ub
cg-tb
vc-aq
tb-ka
wh-tc
yn-cg
kh-ub
ta-co
de-co
tc-td
tb-wq
wh-td
ta-ka
td-qp
aq-cg
wq-ub
ub-vc
de-ta
wq-aq
wq-vc
wh-yn
ka-de
kh-ta
co-tc
wh-qp
tb-vc
td-yn
";

        Assert.Equal(            7, new LanParty(Part.A).Solve(input));
        Assert.Equal("co,de,ka,ta", new LanParty(Part.B).Solve(input));

        // dk,dl,hi,lz,pq,rj,rk,tk,tx,yh,zb,zt - WA
    }
}
