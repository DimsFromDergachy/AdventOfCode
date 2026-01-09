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
        var wires = ParseWires(input);
        var gates = ParseGates(input);

        if (Part == Part.A)
            return Solver(wires, gates).Where(wire => wire.Key.StartsWith('z'))
                                       .OrderBy(wire => wire.Key)
                                       .Reverse()
                                       .Aggregate(0L, (res, wire) => 2 * res + (wire.Value ? 1 : 0));

        return 42;
    }

    List<Wire> ParseWires(string input)
        => input.Lines()
                .Where(line => wire.IsMatch(line))
                .Select(line => wire.Match(line).Groups)
                .Select(gs => new Wire
                {
                    wire = gs[1].Value,
                    bit = int.Parse(gs[2].Value) > 0,
                })
                .ToList();

    internal List<Gate> ParseGates(string input)
        => input.Lines()
                .Where(line => gate.IsMatch(line))
                .Select(line => gate.Match(line).Groups)
                .Select(gs => new Gate
                {
                    wire = gs[4].Value,
                    op   = gs[2].Value,
                    arg1 = gs[1].Value,
                    arg2 = gs[3].Value,
                })
                .ToList();

    internal IEnumerable<(int i, int x, int y, int z, int z2)> FailBit(
        Gate[] gates,
        int bit)
    {
        var wires_ = new Wire[90];

        for (int i = 0; i <= 44; i++)
        {
            wires_[ 0 + i] = new Wire { wire = $"x{i:D2}", bit = false, };
            wires_[45 + i] = new Wire { wire = $"y{i:D2}", bit = false, };
        }

        foreach (var (x, y, z1, z2) in new (int, int, int, int)[]
            {(0, 0, 0, 0), (0, 1, 1, 0), (1, 0, 1, 0), (1, 1, 0, 1)})
        {
            wires_[ 0 + bit].bit = x > 0;
            wires_[45 + bit].bit = y > 0;

            var result = Solver(wires_, gates);
            var e1 = result[$"z{bit+0:D2}"] ? 1 : 0;
            var e2 = result[$"z{bit+1:D2}"] ? 1 : 0;

            if (e1 != z1 || e2 != z2)
            {
                yield return (bit, x, y, e1, e2);
            }
        }
    }

    internal IEnumerable<(string v, int d)> SubTree(Gate[] gates, int bit)
    {
        var res = new List<(string w, int d)>();
        var queue = new Queue<(string w, int d)>();
        queue.Enqueue(($"z{bit:D2}", 0));

        while (queue.Any())
        {
            var (v, d) = queue.Dequeue();
            yield return (v, d);

            if (gates.Any(g => g.wire == v))
            {
                var gate = gates.Single(g => g.wire == v);

                queue.Enqueue((gate.arg1, d + 1));
                queue.Enqueue((gate.arg2, d + 1));
            }
        }
    }

    internal IEnumerable<Gate> Find(Gate[] gates, int bit)
    {
        {
            foreach (var g1 in gates)
            // foreach (var g2 in gates)
            // foreach (var g3 in gates)
            {
                var wire1 = g1.wire;
                var wire2 = "z44";
                var wire3 = "z45";
                if (wire1.StartsWith("x") || wire1.StartsWith("y"))
                    continue;

                if (wire2.StartsWith("x") || wire2.StartsWith("y"))
                    continue;

                if (wire3.StartsWith("x") || wire3.StartsWith("y"))
                    continue;

                var gate1 = gates.First(g => g.wire == wire1);
                var gate2 = gates.First(g => g.wire == wire2);
                var gate3 = gates.First(g => g.wire == wire3);
                var idx1 = Array.IndexOf(gates, gate1);
                var idx2 = Array.IndexOf(gates, gate2);
                var idx3 = Array.IndexOf(gates, gate3);
                var origOp1 = gate1.op;
                var origOp2 = gate2.op;
                var origOp3 = gate3.op;

                foreach (var op1 in new string[] {"AND", "OR", "XOR"})
                foreach (var op2 in new string[] {"AND", "OR", "XOR"})
                foreach (var op3 in new string[] {"AND", "OR", "XOR"})
                {
                    gates[idx1].op = op1;
                    gates[idx2].op = op2;
                    gates[idx3].op = op3;
                    if (!FailBit(gates, bit).Any())
                    {
                        yield return gate1;
                        yield return gate2;
                        yield return gate3;
                    }
                }

                gates[idx1].op = origOp1;
                gates[idx2].op = origOp2;
                gates[idx3].op = origOp3;
            }
        }
    }

    private IDictionary<string, bool> Solver(
        IList<Wire> wires,
        IList<Gate> gates)
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

struct Wire
{
    internal string wire;
    internal bool bit;
}

struct Gate
{
    internal string wire;
    internal string op;
    internal string arg1;
    internal string arg2;
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
        Assert.Equal(4L, new WiresMess(Part.A).Solve(input));
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

        Assert.Equal(2024L, new WiresMess(Part.A).Solve(input));
    }

    [Fact]
    internal void Input()
    {
        var input = File.ReadAllText("./Solvers/2024/Data/Day24");

        Assert.Equal(57632654722854L, new WiresMess(Part.A).Solve(input));
        // Assert.Equal(            42L, new WiresMess(Part.B).Solve(input));
    }

    [Fact]
    internal void FailBits()
    {
        var input = File.ReadAllText("./Solvers/2024/Data/Day24");

        // Fail bits: 6,14,15,22,23,38,39

        var solver = new WiresMess(Part.B);
        var gates = solver.ParseGates(input).ToArray();
        var result = Enumerable.Range(0, 44)
                               .SelectMany(bit => solver.FailBit(gates, bit))
                               .Select(f => f.i)
                               .Distinct()
                               .ToArray();

        Assert.Equal([6,14,15,22,23,38,39], result);
    }

    // [Theory]
    // [InlineData(06)]
    // [InlineData(14)]
    // [InlineData(15)]
    // [InlineData(22)]
    // [InlineData(23)]
    // [InlineData(38)]
    // [InlineData(39)]
    internal void Wanted(int bit)
    {
        var input = File.ReadAllText("./Solvers/2024/Data/Day24");

        var solver = new WiresMess(Part.B);
        var gates = solver.ParseGates(input).ToArray();

        Assert.Equal((object) bit, solver.Find(gates, bit).ToArray());
        Assert.Equal((object) bit, solver.SubTree(gates, bit).ToArray());
    }
}