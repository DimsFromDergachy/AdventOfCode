using System.Text.RegularExpressions;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Year2024.Day14;

[Solver(2024, 14, Part.A)]
[Solver(2024, 14, Part.B)]
class SwarmController : Solver
{ 
    (int X, int Y) Space { get; set; } = (101, 103);
    internal SwarmController(Part part): base(part) {}
    internal SwarmController(Part part, (int, int) space) : base(part) { Space = space; }

    Regex RobotRegex = new Regex(@"p=(-?\d+),(-?\d+) v=(-?\d+),(-?\d+)");

    internal override object Solve(string input)
    {
        var robots = input.Lines()
                          .Select(Parse)
                          .ToArray();

        if (Part == Part.A)
        {
            var robots2 = robots.Select(r => Move(r, 100));

            var q1 = robots2.Count(r => r.x < Space.X / 2 && r.y < Space.Y / 2);
            var q2 = robots2.Count(r => r.x > Space.X / 2 && r.y < Space.Y / 2);
            var q3 = robots2.Count(r => r.x < Space.X / 2 && r.y > Space.Y / 2);
            var q4 = robots2.Count(r => r.x > Space.X / 2 && r.y > Space.Y / 2);

            return q1 * q2 * q3 * q4;
            // return (q1, q2, q3, q4);
        }

        var time = 8159;
        robots = robots.Select(r => Move(r, time)).ToArray();
        Print(time, robots);
        return time;
    }

    void Print(int time, Drone[] robots)
    {
        Console.WriteLine($"======= TIME {time:D6} ========== ");

        var Rgba32 = new Rgba32(1.0F, 1.0F, 1.0F);

        using var image = new Image<Rgba32>(105, 105);
        foreach (var robot in robots)
            image[robot.x, robot.y] = Rgba32;

        image.SaveAsJpeg($".//Day14_{time:D6}.jpg");

        Thread.Sleep(100);
    }

    Drone Move(Drone robot, int time)
    {
        return robot with
        {
            x = ((robot.x + time * robot.vx) % Space.X + Space.X) % Space.X,
            y = ((robot.y + time * robot.vy) % Space.Y + Space.Y) % Space.Y,
        };
    }

    Drone Parse(string line)
    {
        var group = RobotRegex.Match(line).Groups;
        return new Drone
        {
             x = int.Parse(group[1].Value),
             y = int.Parse(group[2].Value),
            vx = int.Parse(group[3].Value),
            vy = int.Parse(group[4].Value),
        };
    }

    internal static IEnumerable<int> A1()
    {
        var a = 22;
        while (true)
        {
            yield return a;
            a += 103;
        }
    }

    internal static IEnumerable<int> A2()
    {
        var b = 79;
        while (true)
        {
            yield return b;
            b += 101;
        }
    }

    private record Drone
    {
        internal int x;
        internal int y;
        internal int vx;
        internal int vy;
    }
}

public class SwarmTest
{
    [Fact]
    internal void Example()
    {
        var input = @"
p=0,4 v=3,-3
p=6,3 v=-1,-3
p=10,3 v=-1,2
p=2,0 v=2,-1
p=0,0 v=1,3
p=3,0 v=-2,-2
p=7,6 v=-1,-3
p=3,0 v=-1,-2
p=9,3 v=2,3
p=7,3 v=-1,2
p=2,4 v=2,-3
p=9,5 v=-3,-3
";

        Assert.Equal(  12, new SwarmController(Part.A, (11, 7)).Solve(input));
        Assert.Equal(8159, new SwarmController(Part.B, (11, 7)).Solve(input));
    }

    [Fact]
    internal void Find()
    {
        var e1 = SwarmController.A1().GetEnumerator();
        var e2 = SwarmController.A2().GetEnumerator();

        e1.MoveNext();
        e2.MoveNext();

        while (e1.Current != e2.Current)
            if (e1.Current < e2.Current)
                e1.MoveNext();
            else
                e2.MoveNext();

        Assert.Equal(8159, e1.Current);
    }
}