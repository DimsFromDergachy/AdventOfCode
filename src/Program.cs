var year = int.Parse(args[0]);
var day = int.Parse(args[1]);
var part = Enum.Parse<Part>(args.Skip(2).FirstOrDefault() ?? "A");

var solver = Services.GetSolver(year, day, part);

Console.Out.Write(solver.Solve(Console.In.ReadToEnd()));