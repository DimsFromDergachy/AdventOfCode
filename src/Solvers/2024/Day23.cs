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
                         .SelectMany(pair => new (string a, string b)[]
                                {(pair.a, pair.b), (pair.b, pair.a)})
                         .ToHashSet();

        var vs = edges.SelectMany(p => new string[] {p.a, p.b})
                      .Distinct()
                      .ToArray<string>();

        if (Part == Part.A)
        {
            var count = 0;
            foreach (var a in vs)
                foreach (var b in vs)
                    if (edges.Contains((a, b)) && a.CompareTo(b) > 0)
                        foreach (var c in vs)
                            if (edges.Contains((a, c)) && a.CompareTo(c) > 0
                                &&
                                edges.Contains((b, c)) && b.CompareTo(c) > 0)
                                if (new string[] {a, b, c}.Any(s => s.StartsWith('t')))
                                    count++;
            return count;
        }

        return "ko-ko";
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
    }
}
