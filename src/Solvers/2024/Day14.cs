using System.Text.RegularExpressions;

namespace Year2024.Day14;

record Robot
{
    internal int x;
    internal int y;
    internal int vx;
    internal int vy;
}

[Solver(2024, 14, Part.A)]
class A : Solver
{ 
    (int X, int Y) Space { get; set; } = (101, 103);
    internal A(Part part): base(part) {}
    internal A(Part part, (int, int) space) : base(part) { Space = space; }

    Regex RobotRegex = new Regex(@"p=(-?\d+),(-?\d+) v=(-?\d+),(-?\d)");

    internal override object Solve(string input)
    {
        var robots = input.Lines()
                          .Select(Parse);

        robots = robots.Select(Shift)
                       .Select(r => Move(r, 100));

        var q1 = robots.Count(r => r.x < 0 && r.y < 0);
        var q2 = robots.Count(r => r.x > 0 && r.y < 0);
        var q3 = robots.Count(r => r.x < 0 && r.y > 0);
        var q4 = robots.Count(r => r.x > 0 && r.y < 0);

        //return (long)q1 * (long)q2 * (long)q3 * (long)q4;
        return (q1, q2, q3, q4);
    }

    Robot Shift(Robot robot, int time)
    {
        return robot with
        {
            x = robot.x - Space.X / 2,
            y = robot.y - Space.Y / 2,
        };
    }



    Robot Move(Robot robot, int time)
    {
        return robot with
        {
            x = (robot.x + time * robot.vx) % (Space.X / 2),
            y = (robot.y + time * robot.vy) % (Space.Y / 2),
        };
    }

    Robot Parse(string line)
    {
        var group = RobotRegex.Match(line).Groups;
        return new Robot
        {
             x = int.Parse(group[1].Value),
             y = int.Parse(group[2].Value),
            vx = int.Parse(group[3].Value),
            vy = int.Parse(group[4].Value),
        };
    }
}

public class Test
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

        Assert.Equal(12, new A(Part.A, (11, 7)).Solve(input));

        // (120, 121, 115, 121)
    }
}