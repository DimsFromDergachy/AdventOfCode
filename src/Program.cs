using AdventOfCode.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace AdventOfCode;

public class Program
{
    static void Main(string[] args)
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

        builder.Configuration.Sources.Clear();

        builder.Configuration.AddJsonFile("appsettings.json");

        PuzzleOptions options = new();
        builder.Configuration.GetSection(nameof(PuzzleOptions))
            .Bind(options);

        Console.WriteLine($"Options = {options.Year}, {options.Day}, {options.Part}");
    }
}