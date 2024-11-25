using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Year2015.Day07;

class SolverA : Solver
{
    private readonly Regex regex
        = new Regex("^((\\d+)|(\\b+)? ?(NOT|AND|OR|LSHIFT|RSHIFT) (\\b+)) -> (\\b+)$");

    internal override object Solve(string input) =>
        input.Lines()
             .Select(ParseInstruction)
             .Select(x => x.ToExpression());
            //  .BuildTree()
            //  .Compile()
            //  .Run();

    Instruction ParseInstruction(string instruction)
    {
        var groups = regex.Match(instruction).Groups;

        return new Instruction
        {
            signal = groups[1].Value == ""
                ? (ushort) 0
                : ushort.Parse(groups[1].Value),
            left  = groups[3].Value,
            op    = groups[4].Value,
            right = groups[5].Value,
            wire  = groups[6].Value,
        };
    }
}

struct Instruction
{
    internal required ushort signal;
    internal required string left;
    internal required string op;
    internal required string right;
    internal required string wire;
}

static class InstructionExtensions
{
    #pragma warning disable CS8603
    internal static Expression ToExpression(this Instruction instruction)
    {
        switch (instruction.op)
        {
            case "":
            {
                
                return null;
            }
        };
    }
}