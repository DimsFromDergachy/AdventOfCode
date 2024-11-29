using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Year2015.Day07;

[Solver(2015, 07, Part.A)]
class SolverA : Solver
{
    private readonly string Wire = "a";
    internal SolverA(string wire) => Wire = wire;
    public SolverA() {}

    private readonly Regex regex
        = new Regex("^((\\w+)|(\\w+)? ?(NOT|AND|OR|LSHIFT|RSHIFT) (\\w+)) -> (\\w+)$");

    internal override object Solve(string input) =>
        input.Lines()
             .Select(ParseInstruction)
             .ToDictionary(instruction => instruction.wire)
             .Calculate(Wire);

    Instruction ParseInstruction(string instruction)
    {
        var groups = regex.Match(instruction).Groups;

        return new Instruction
        {
            signal = groups[1].Value,
            left   = groups[3].Value,
            op     = groups[4].Value,
            right  = groups[5].Value,
            wire   = groups[6].Value,
        };
    }
}

struct Instruction
{
    internal required string signal;
    internal required string left;
    internal required string op;
    internal required string right;
    internal required string wire;
}

static class DictionaryExtensions
{
    #pragma warning disable CS8509
    internal static ushort Calculate(
        this Dictionary<string, Instruction> dictionary,
        string start)
    {
        var memo = new Dictionary<string, ushort>();
        var stack = new Stack<string>();

        stack.Push(start);

        while (stack.Any())
        {
            var wire = stack.Peek();

            if (memo.ContainsKey(wire))
            {
                stack.Pop();
                continue; // already calculated
            }

            if (ushort.TryParse(wire, out var value))
            {
                memo[wire] = value;
                stack.Pop();
                continue;
            }

            var instruction = dictionary[wire];

            switch (instruction.op)
            {
                case "":
                {
                    if (memo.ContainsKey(instruction.signal))
                    {
                        memo[wire] = memo[instruction.signal];
                        stack.Pop();
                        continue;
                    }
                    stack.Push(instruction.signal);
                    break;
                }
                case "NOT":
                {
                    if (memo.ContainsKey(instruction.right))
                    {
                        memo[wire] = (ushort) ~memo[instruction.right];
                        stack.Pop();
                        continue;
                    }
                    stack.Push(instruction.right);
                    break;
                }
                case "LSHIFT":
                case "RSHIFT":
                {
                    if (memo.ContainsKey(instruction.left))
                    {
                        memo[wire] = instruction.op switch
                            {
                                "LSHIFT" => (ushort) (memo[instruction.left] << int.Parse(instruction.right)),
                                "RSHIFT" => (ushort) (memo[instruction.left] >> int.Parse(instruction.right)),
                            };
                        stack.Pop();
                        continue;
                    }
                    stack.Push(instruction.left);
                    break;
                }
                case "AND":
                case "OR":
                {
                    if (memo.ContainsKey(instruction.left)
                        &&
                        memo.ContainsKey(instruction.right))
                    {
                        memo[wire] = instruction.op switch
                            {
                                "AND" => (ushort) (memo[instruction.left] & memo[instruction.right]),
                                "OR"  => (ushort) (memo[instruction.left] | memo[instruction.right]),
                            };
                        stack.Pop();
                        continue;
                    }
                    stack.Push(instruction.left);
                    stack.Push(instruction.right);
                    break;
                }
            };
        }

        return memo[start];
    }

    internal static LambdaExpression ToLambda(this Expression expression) =>
        Expression.Lambda(expression);
}

public class Test
{
    [Fact]
    public void Simple()
    {
        var input = "42 -> a";
        Assert.Equal((ushort) 42, (new SolverA("a")).Solve(input));
    }

    [Fact]
    public void Not()
    {
        var input = "0 -> b\nNOT b -> a";
        Assert.Equal(ushort.MaxValue, (new SolverA("a")).Solve(input));
    }

    [Fact]
    public void Example()
    {
        var input = @"
123 -> x
456 -> y
x AND y -> d
x OR y -> e
x LSHIFT 2 -> f
y RSHIFT 2 -> g
NOT x -> h
NOT y -> i";

        Assert.Equal((ushort)    72, new SolverA("d").Solve(input));
        Assert.Equal((ushort)   507, new SolverA("e").Solve(input));
        Assert.Equal((ushort)   492, new SolverA("f").Solve(input));
        Assert.Equal((ushort)   114, new SolverA("g").Solve(input));
        Assert.Equal((ushort) 65412, new SolverA("h").Solve(input));
        Assert.Equal((ushort) 65079, new SolverA("i").Solve(input));
        Assert.Equal((ushort)   123, new SolverA("x").Solve(input));
        Assert.Equal((ushort)   456, new SolverA("y").Solve(input));
    }

    [Fact]
    public void DirectWireConnection()
    {
        var input = @"
42 -> x
x -> y
y -> z
z -> a";

        Assert.Equal((ushort) 42, new SolverA().Solve(input));
    }

    [Fact]
    public void NumbersAsInput()
    {
        var input = @"
1 AND gd -> ge
5 -> gd";

        Assert.Equal((ushort) 1, new SolverA("ge").Solve(input));
    }

    [Fact(Timeout = 10000)]
    public void StackOverflow()
    {
        var input = @"
42 -> a
a AND a -> b
b AND b -> c
c AND c -> d
d AND d -> e
e AND e -> f
f AND f -> g
g AND g -> h
h AND h -> i
i AND i -> j
j AND j -> k
k AND k -> l
l AND l -> m
m AND m -> n
n AND n -> o
o AND o -> p
p AND p -> q
q AND q -> r
r AND r -> s
s AND s -> t
t AND t -> u
u AND u -> v
v AND v -> w
w AND w -> x
x AND x -> y
y AND y -> z
";

        Assert.Equal((ushort) 42, new SolverA("z").Solve(input));
    }
}