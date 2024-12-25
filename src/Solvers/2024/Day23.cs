namespace Year2024.Day23;

[Solver(2024, 23, Part.A)]
[Solver(2024, 23, Part.B)]
class LanParty : Solver
{
    internal LanParty(Part part) : base(part) {}

    internal override object Solve(string input)
    {
        var graph = input.Lines()
                         .Select(line => (a: line.Substring(0, 2),
                                          b: line.Substring(3, 2)))
                         .SelectMany(edge => new (string, string)[] {edge, edge.Swap()})
                         .ToList()
                         .ToGraph();

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
