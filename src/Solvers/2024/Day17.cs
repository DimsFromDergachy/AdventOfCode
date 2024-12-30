using System.Collections;

namespace Year2024.Day17;

[Solver(2024, 17, Part.A)]
[Solver(2024, 17, Part.B)]
class Computer : Solver
{
    internal Computer(Part part) : base(part) {}

    internal override object Solve(string input)
    {
        var lines = input.Lines().ToList().ToArray();
        var state = new State
        {
            A = long.Parse(lines[0].Words().Last()),
            B = long.Parse(lines[1].Words().Last()),
            C = long.Parse(lines[2].Words().Last()),
        };

        var program = lines.Last()
                           .Substring("Program: ".Count())
                           .Split(',')
                           .Parse<int>()
                           .ToArray();

        if (Part == Part.A)
            return string.Join(",", Execute(state, program).Select(p => p.output));

        long A = 0; // this is the program halt condition
        for (int p = program.Count() - 1; p >= 0; p--)
        {
            long B = p > 0 ? program[p] : 0; // the previous step has been printed from B register
            var tail = program.Skip(p);
            for (A = A * 8; true; A++)
            {
                state.A = A;
                state.B = B;

                var output = Execute(state, program).Select(p => p.output);
                if (output.Zip(tail).All(p => p.First == p.Second))
                {
                    break;
                }
            }
        }

        return A;
    }

    IEnumerable<(State state, long output)> Execute(State state, int[] program)
    {
        var iterator = new StateEnumerator(state, program);

        while (iterator.MoveNext())
        {
            if (iterator.Current.output != null)
                yield return (iterator.Current, iterator.Current.output.Value);
        }
    }

    struct State
    {
        internal long A;
        internal long B;
        internal long C;
        internal int Pointer;
        internal long Combo(long operand) => operand switch
        {
            4 => A,
            5 => B,
            6 => C,
            7 => throw new NotImplementedException($"UB: {this}"),
            _ => operand,
        };
        internal long? output;
    }

    class StateEnumerator : IEnumerator<State>
    {
        int[] _program;
        State _init;
        State _state;

        public StateEnumerator(State init, int[] program)
        {
            _program = program;
            _state = _init = init;
        }

        public State Current => _state;
        object IEnumerator.Current => _state;

        public void Dispose() {}

        public bool MoveNext()
        {
            if (_state.Pointer >= _program.Count())
                return false;

            var instruction = _program[_state.Pointer++];
            var operand     = _program[_state.Pointer++];

            _state.output = null;

            switch (instruction)
            {
                case 0: // adv
                {
                    _state.A = _state.A / (1 << (int)_state.Combo(operand));
                    break;
                }
                case 1: // bxl
                {
                    _state.B = _state.B ^ operand;
                    break;
                }
                case 2: // bst
                {
                    _state.B = _state.Combo(operand) % 8;
                    break;
                }
                case 3: // jnz
                {
                    _state.Pointer = _state.A == 0 ? _state.Pointer : operand;
                    break;
                }
                case 4: // bxc
                {
                    _state.B = _state.B ^ _state.C;
                    break;
                }
                case 5: // out
                {
                    _state.output = _state.Combo(operand) % 8;
                    break;
                }
                case 6: // bdv
                {
                    _state.B = _state.A / (1 << (int)_state.Combo(operand));
                    break;
                }
                case 7: // cdv
                {
                    _state.C = _state.A / (1 << (int)_state.Combo(operand));
                    break;
                }
                default: return false;
            }

            return true;
        }

        public void Reset() { _state = _init;}
    }
}

public class ComputerTest
{
    [Fact]
    internal void ExampleA()
    {
        var input = @"
Register A: 729
Register B: 0
Register C: 0

Program: 0,1,5,4,3,0
";

        Assert.Equal("4,6,3,5,6,3,5,2,1,0", new Computer(Part.A).Solve(input));
    }

    [Fact]
    internal void ExampleB()
    {
        var input = @"
Register A: 117440
Register B: 0
Register C: 0

Program: 0,3,5,4,3,0
";

        Assert.Equal("0,3,5,4,3,0", new Computer(Part.A).Solve(input));
        Assert.Equal(117440L, new Computer(Part.B).Solve(input));
    }

    [Fact]
    internal void Quine()
    {
        var input = @"
Register A: 236539226447469
Register B: 0
Register C: 0

Program: 2,4,1,3,7,5,0,3,1,5,4,4,5,5,3,0
";

        Assert.Equal("2,4,1,3,7,5,0,3,1,5,4,4,5,5,3,0", new Computer(Part.A).Solve(input));
        Assert.Equal(236539226447469L, new Computer(Part.B).Solve(input));
    }
}