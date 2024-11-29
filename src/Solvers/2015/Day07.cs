using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Year2015.Day07;

[Solver(2015, 07, Part.A)]
class SolverA : Solver
{
    private readonly string Wire = "a";
    internal SolverA(string wire) => Wire = wire;
    public SolverA() {}

    internal static readonly IDictionary<string, Expression> Memo = new Dictionary<string, Expression>();

    private readonly Regex regex
        = new Regex("^((\\w+)|(\\w+)? ?(NOT|AND|OR|LSHIFT|RSHIFT) (\\w+)) -> (\\w+)$");

    internal override object Solve(string input) =>
        input.Lines()
             .Select(ParseInstruction)
             .ToDictionary(instruction => instruction.wire)
             .BuildExpression(Wire)
             .ToLambda()
             .Compile()
             .DynamicInvoke()!;

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
    internal static Expression BuildExpression(
        this Dictionary<string, Instruction> dictionary,
        string wire)
    {
        if (SolverA.Memo.ContainsKey(wire))
            return SolverA.Memo[wire];

        if (ushort.TryParse(wire, out var wire_))
            return SolverA.Memo[wire] = Expression.Constant(wire_);

        var instruction = dictionary[wire];

        #pragma warning disable CS8509
        return SolverA.Memo[wire] = instruction.op switch
        {
            ""       => dictionary.BuildExpression(instruction.signal),
            "NOT"    => Expression.MakeUnary(
                            ExpressionType.Not,
                            dictionary.BuildExpression(instruction.right),
                            typeof(ushort)),
            "AND"    => Expression.MakeBinary(
                            ExpressionType.And,
                            left:  dictionary.BuildExpression(instruction.left),
                            right: dictionary.BuildExpression(instruction.right)),
            "OR"    => Expression.MakeBinary(
                            ExpressionType.Or,
                            left:  dictionary.BuildExpression(instruction.left),
                            right: dictionary.BuildExpression(instruction.right)),
            "LSHIFT" => Expression.MakeBinary(
                            ExpressionType.LeftShift,
                            left:  dictionary.BuildExpression(instruction.left),
                            Expression.Constant(int.Parse(instruction.right))),
            "RSHIFT" => Expression.MakeBinary(
                            ExpressionType.RightShift,
                            left:  dictionary.BuildExpression(instruction.left),
                            Expression.Constant(int.Parse(instruction.right))),
        };
    }

    internal static LambdaExpression ToLambda(this Expression expression) =>
        Expression.Lambda(expression);
}

public class Test
{
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

        Assert.Equal((ushort)    72, new SolverA("d").Solve(input)); SolverA.Memo.Clear();
        Assert.Equal((ushort)   507, new SolverA("e").Solve(input)); SolverA.Memo.Clear();
        Assert.Equal((ushort)   492, new SolverA("f").Solve(input)); SolverA.Memo.Clear();
        Assert.Equal((ushort)   114, new SolverA("g").Solve(input)); SolverA.Memo.Clear();
        Assert.Equal((ushort) 65412, new SolverA("h").Solve(input)); SolverA.Memo.Clear();
        Assert.Equal((ushort) 65079, new SolverA("i").Solve(input)); SolverA.Memo.Clear();
        Assert.Equal((ushort)   123, new SolverA("x").Solve(input)); SolverA.Memo.Clear();
        Assert.Equal((ushort)   456, new SolverA("y").Solve(input)); SolverA.Memo.Clear();
    }

    [Fact]
    public void DirectWireConnection()
    {
        var input = @"
42 -> x
x -> y
y -> z
z -> a";

        Assert.Equal((ushort) 42, new SolverA().Solve(input)); SolverA.Memo.Clear();
    }

    [Fact]
    public void NumbersAsInput()
    {
        var input = @"
1 AND gd -> ge
5 -> gd";

        Assert.Equal((ushort) 1, new SolverA("ge").Solve(input)); SolverA.Memo.Clear();
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
y AND y -> y
z AND z -> z
";

        Assert.Equal((ushort) 42, new SolverA("z").Solve(input)); SolverA.Memo.Clear();
    }
}