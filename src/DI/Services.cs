static class Services
{
    internal static Solver GetSolver(int year, int day, Part part)
    {
        foreach (var type in typeof(Program).Assembly!.GetTypes())
        {
            if (type.BaseType != typeof(Solver))
                continue;

            var attributes = type.CustomAttributes
                .Where(a => a.AttributeType == typeof(SolverAttribute));

            foreach (var attribute in attributes)
            {
                var args = attribute.ConstructorArguments;

                if (year == int.Parse(args[0].Value!.ToString()!)
                    &&
                    day  == int.Parse(args[1].Value!.ToString()!)
                    &&
                    part == Enum.Parse<Part>(args[2].Value!.ToString()!))
                {
                    return (Solver) Activator.CreateInstance(type, part)!;
                }
            }
        }

        throw new NotImplementedException($"Solver not found: {day}-th of {year} part {part}");
    }
}