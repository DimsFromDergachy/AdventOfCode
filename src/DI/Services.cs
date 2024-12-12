using System.Reflection;

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
                    var types = new [] { typeof(Part) };
                    var ctor = type.GetConstructor(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic, null, types, null);
                    if (ctor != null)
                    {
                        return (Solver) ctor.Invoke([part]);
                    }

                    var solver = (Solver) Activator.CreateInstance(type)!;
                    solver.Part = part;
                    return solver;
                }
            }
        }

        throw new NotImplementedException($"Solver not found: {day}-th of {year} part {part}");
    }
}

public class ServicesTest
{
    [Theory]
    [InlineData(2015, 01, Part.A)]
    [InlineData(2015, 01, Part.B)]
    [InlineData(2024, 10, Part.A)]
    [InlineData(2024, 10, Part.B)]
    [InlineData(2024, 12, Part.A)]
    [InlineData(2024, 12, Part.B)]
    internal void OK(int year, int day, Part part)
    {
        Assert.Equal(part, Services.GetSolver(year, day, part).Part);
    }

    [Theory]
    [InlineData(2014, 01, Part.A)]
    [InlineData(2014, 02, Part.B)]
    [InlineData(2015, 26, Part.A)]
    [InlineData(2015, 26, Part.B)]
    [InlineData(3024, 01, Part.A)]
    [InlineData(3024, 02, Part.B)]
    internal void Fail(int year, int day, Part part)
    {
        Assert.Throws<NotImplementedException>(() => Services.GetSolver(year, day, part));
    }

    [Fact]
    internal void PrivateCtor()
    {
        Assert.Equal(Part.A, Services.GetSolver(0000, 00, Part.A).Part);
        Assert.Equal(Part.B, Services.GetSolver(0000, 00, Part.B).Part);
    }

    [Solver(0000, 00, Part.A)]
    [Solver(0000, 00, Part.B)]
    internal class TestSolver : Solver
    {
        private TestSolver() {}
        internal TestSolver(Part part) : base(part) {}
        internal override object Solve(string input) => throw new NotImplementedException();
    }
}