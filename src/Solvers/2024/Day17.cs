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
                           .Skip(9)
                           .Parse<int>(',')
                           .ToArray();

        if (Part == Part.A)
            return string.Join(",", Execute(state, program).Select(p => p.output));

        int maxP = 0;

        // for (long x = 4373700205; x < long.MaxValue; x++)
        // for (long x = 1243744394861L; x < long.MaxValue; x++)
        for (long x = 1; x < long.MaxValue; x++)
        {
            state.A = x;
            int p = 0;
            bool good = true;

            var iteratorA = new StateEnumerator(state, program);
            // var iteratorB = new StateEnumerator(state, program);

            // do
            {
                iteratorA.MoveNext();
                iteratorA.MoveNext();
                iteratorA.MoveNext();
                iteratorA.MoveNext();
                iteratorA.MoveNext();
                iteratorA.MoveNext();
                iteratorA.MoveNext();

                if (iteratorA.Current.output != null)
                {
                    if (p >= program.Count() || iteratorA.Current.output.Value != program[p++])
                    {
                        good = false;
                        break;
                    }
                }

                iteratorA.MoveNext();

                if (iteratorA.Current.A == 1)
                {
                    return x;
                }

                // if (iteratorA.MoveNext() == false)
                // {
                //     break;
                // }

                // if (iteratorB.MoveNext() && iteratorB.MoveNext())
                // {
                //     // Infinity loop check
                //     if (iteratorA.Current.Equals(iteratorB.Current))
                //     {
                //         Console.Out.WriteLine($"Infinity loop detected with {x}");
                //         good = false;
                //         break;
                //     }
                // }

            }
            // while (true);

            if (good && p == program.Count())
            {
                Console.WriteLine(" BINGO ");
                return x;
            }

            // if (x % 10000000 == 0)
            //     Console.WriteLine(x);

            if (p > maxP)
            {
                maxP = p;
                Console.WriteLine($"{p:D2} - {x:D20} - {Convert.ToString(x,2).PadLeft(64,'0')}");
            }
        }

        throw new NotImplementedException();
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

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var that = (State) obj;

            return A == that.A
                && B == that.B
                && C == that.C
                && Pointer == that.Pointer;
        }

        public override int GetHashCode()
        {
            return (int) (A ^ B ^ C ^ Pointer);
        }
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
Register A: 2024
Register B: 0
Register C: 0

Program: 0,3,5,4,3,0
";

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
    }
}