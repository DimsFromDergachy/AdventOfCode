var year = int.Parse(args[0]);
var day = int.Parse(args[1]);

Solver.Solver solver =  (year, day) switch
{
    (2015, 01) => new Solver.Solver<Y2015, D01, PartA>(),
    _ => throw new NotImplementedException($"{day}-th of {year} not found"),
};

Console.Out.Write(solver.Solve(Console.In.ReadToEnd()));
