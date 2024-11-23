var year = int.Parse(args[0]);
var day = int.Parse(args[1]);
var part = Enum.Parse<Part>(args.Skip(2).FirstOrDefault() ?? "A");

var solver = GetSolver(year, day, part);

Console.Out.Write(solver.Solve(Console.In.ReadToEnd()));

Solver GetSolver(int year, int day, Part part)
{
    foreach (var type in typeof(Program).Assembly!.GetTypes())
    {
        if (type.BaseType != typeof(Solver))
            continue;

        var partAttribute = type.CustomAttributes.Where(a => a.AttributeType == typeof(SolverAttribute)).Single();
        var args2 = partAttribute.ConstructorArguments;

        if (year == int.Parse(args2[0].Value!.ToString()!)
            &&
            day  == int.Parse(args2[1].Value!.ToString()!)
            &&
            part == Enum.Parse<Part>(args2[2].Value!.ToString()!))
        {
            return (Solver) Activator.CreateInstance(type)!;
        }
    }

    throw new NotImplementedException($"Solver not found: {day}-th of {year} part {part}");
}