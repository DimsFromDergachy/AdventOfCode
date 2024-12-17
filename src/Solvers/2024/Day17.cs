namespace Year2024.Day17;

[Solver(2024, 17, Part.A)]
class Computer : Solver
{
    internal Computer(Part part) : base(part) {}

    internal override object Solve(string input)
    {
        var lines = input.Lines().ToList().ToArray();
        var state = new State
        {
            A = int.Parse(lines[0].Words().Last()),
            B = int.Parse(lines[1].Words().Last()),
            C = int.Parse(lines[2].Words().Last()),
        };

        var program = lines.Last()
                           .Skip(9)
                           .Parse<int>(',')
                           .ToArray();

        return string.Join(",", Execute(state, program).Select(p => p.print));
    }

    IEnumerable<(State state, int print)> Execute(State state, int[] program)
    {
        while (true)
        {
            if (state.Pointer >= program.Count())
                yield break; // Halt

            var instruction = program[state.Pointer++];
            var operand     = program[state.Pointer++];

            switch (instruction)
            {
                case 0: // adv
                {
                    state.A = state.A / (1 << state.Combo(operand));
                    break;
                }
                case 1: // bxl
                {
                    state.B = state.B ^ operand;
                    break;
                }
                case 2: // bst
                {
                    state.B = state.Combo(operand) % 8;
                    break;
                }
                case 3: // jnz
                {
                    state.Pointer = state.A == 0 ? state.Pointer : operand;
                    break;
                }
                case 4: // bxc
                {
                    state.B = state.B ^ state.C;
                    break;
                }
                case 5: // out
                {
                    yield return (state, state.Combo(operand) % 8);
                    break;
                }
                case 6: // bdv
                {
                    state.B = state.A / (1 << state.Combo(operand));
                    break;
                }
                case 7: // cdv
                {
                    state.C = state.A / (1 << state.Combo(operand));
                    break;
                }
                default: throw new NotImplementedException($"UB: {state} {instruction} {operand}");
            }
        }
    }

    record State
    {
        internal int A;
        internal int B;
        internal int C;
        internal int Pointer;
        internal int Combo(int operand) => operand switch
        {
            4 => A,
            5 => B,
            6 => C,
            7 => throw new NotImplementedException($"UB: {this}"),
            _ => operand,
        };
    }
}

public class ComputerTest
{
    [Fact]
    internal void Example1()
    {
        var input = @"
Register A: 729
Register B: 0
Register C: 0

Program: 0,1,5,4,3,0
";

        Assert.Equal("4,6,3,5,6,3,5,2,1,0", new Computer(Part.A).Solve(input));
    }
}