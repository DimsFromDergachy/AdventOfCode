using System.Text.RegularExpressions;

namespace Year2024.Day24;

[Solver(2024, 24, Part.A)]
class WiresMess : Solver
{
    internal WiresMess(Part part) : base(part) {}

    Regex wire = new Regex(@"^(.+): (\d+)$");
    Regex gate = new Regex(@"^(.+) (AND|OR|XOR) (.+) -> (.+)$");

    internal override object Solve(string input)
    {
        var wires = input.Lines()
                         .Where(line => wire.IsMatch(line))
                         .Select(line => wire.Match(line).Groups)
                         .Select(gs => (wire: gs[1].Value,
                                         bit: int.Parse(gs[2].Value) > 0))
                         .ToList();

        var gates = input.Lines()
                         .Where(line => gate.IsMatch(line))
                         .Select(line => gate.Match(line).Groups)
                         .Select(gs => (wire: gs[4].Value,
                                          op: gs[2].Value,
                                        arg1: gs[1].Value,
                                        arg2: gs[3].Value))
                         .ToList();

        return Solver(wires, gates).Where(wire => wire.Key.StartsWith('z'))
                                   .OrderBy(wire => wire.Key)
                                   .Reverse()
                                   .Aggregate(0L, (res, wire) => 2 * res + (wire.Value ? 1 : 0));
    }

    private IDictionary<string, bool> Solver(List<(string wire, bool bit)> wires,
                        List<(string wire, string op, string arg1, string arg2)> gates)
    {
        var memo = wires.ToDictionary(wire => wire.wire, wire => wire.bit);

        foreach (var zGate in gates.Where(g => g.wire.StartsWith('z')))
        {
            var stack = new Stack<string>();
            stack.Push(zGate.wire);

            while (stack.Any())
            {
                var wire = stack.Peek();

                if (memo.ContainsKey(wire))
                {
                    stack.Pop();
                    continue;
                }

                var gate = gates.Single(g => g.wire == wire);

                if (!memo.ContainsKey(gate.arg1))
                {
                    stack.Push(gate.arg1);
                    continue;
                }

                if (!memo.ContainsKey(gate.arg2))
                {
                    stack.Push(gate.arg2);
                    continue;
                }

                stack.Pop();

                #pragma warning disable CS8509
                memo.Add(wire, gate.op switch
                {
                    "AND" => memo[gate.arg1] && memo[gate.arg2],
                    "OR"  => memo[gate.arg1] || memo[gate.arg2],
                    "XOR" => memo[gate.arg1]  ^ memo[gate.arg2],
                });
            }
        }

        return memo;
    }
}

public class WiresMessTest
{
    [Fact]
    internal void Example()
    {
        var input = @"
x00: 1
x01: 1
x02: 1
y00: 0
y01: 1
y02: 0

x00 AND y00 -> z00
x01 XOR y01 -> z01
x02 OR y02 -> z02
";
        Assert.Equal(4, new WiresMess(Part.A).Solve(input));
    }

    [Fact]
    internal void Example2()
    {
        var input = @"
x00: 1
x01: 0
x02: 1
x03: 1
x04: 0
y00: 1
y01: 1
y02: 1
y03: 1
y04: 1

ntg XOR fgs -> mjb
y02 OR x01 -> tnw
kwq OR kpj -> z05
x00 OR x03 -> fst
tgd XOR rvg -> z01
vdt OR tnw -> bfw
bfw AND frj -> z10
ffh OR nrd -> bqk
y00 AND y03 -> djm
y03 OR y00 -> psh
bqk OR frj -> z08
tnw OR fst -> frj
gnj AND tgd -> z11
bfw XOR mjb -> z00
x03 OR x00 -> vdt
gnj AND wpb -> z02
x04 AND y00 -> kjc
djm OR pbm -> qhw
nrd AND vdt -> hwm
kjc AND fst -> rvg
y04 OR y02 -> fgs
y01 AND x02 -> pbm
ntg OR kjc -> kwq
psh XOR fgs -> tgd
qhw XOR tgd -> z09
pbm OR djm -> kpj
x03 XOR y03 -> ffh
x00 XOR y04 -> ntg
bfw OR bqk -> z06
nrd XOR fgs -> wpb
frj XOR qhw -> z04
bqk OR frj -> z07
y03 OR x01 -> nrd
hwm AND bqk -> z03
tgd XOR rvg -> z12
tnw OR pbm -> gnj
";

        Assert.Equal(2024, new WiresMess(Part.A).Solve(input));
    }
}