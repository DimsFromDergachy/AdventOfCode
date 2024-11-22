var year = int.Parse(args[0]);
var day = int.Parse(args[1]);
var part = Enum.Parse<Part>(args.Skip(2).FirstOrDefault() ?? "A");

Solver solver =  (year, day, part) switch
{
    (2015, 01, Part.A) => new Solver2015_Day01_PartA(),
    (2015, 01, Part.B) => new Solver2015_Day01_PartB(),
    _ => throw new NotImplementedException($"Solver not found: {day}-th of {year} part {part}"),
};

Console.Out.Write(solver.Solve(Console.In.ReadToEnd()));
