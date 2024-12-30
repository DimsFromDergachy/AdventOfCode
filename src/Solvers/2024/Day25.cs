namespace Year2024.Day25;

[Solver(2024, 25, Part.A)]
class Locker : Solver
{
    public Locker(Part part) : base(part) {}

    internal override object Solve(string input)
    {
        var parsed = input.Lines()
                          .Chunk(7)
                          .Select(Parse)
                          .ToList();

        var locks = parsed.OfType<Lock>().ToArray();
        var keys  = parsed.OfType<Key >().ToArray();

        return locks.SelectMany(@lock => keys.Select(key => (@lock, key)))
                    .Count(pair => IsMatch(pair.@lock, pair.key));
    }

    bool IsMatch(Lock @lock, Key key) =>
        @lock.Heights
             .Zip(key.Heights)
             .All(pair => pair.First + pair.Second <= 5);

    object Parse(string[] lines)
    {
        if (lines.Count() != 7)
            throw new NotImplementedException();

        var Heights = Enumerable.Range(0, 5)
                                .Select(x => lines.Skip(1)
                                                  .SkipLast(1)
                                                  .Count(line => line[x] == '#'));

        if (lines.First().All(c => c == '#'))
            return new Lock { Heights = Heights.ToArray(), };

        if (lines.First().All(c => c == '.'))
            return new Key  { Heights = Heights.ToArray(), };

        throw new NotImplementedException(lines.First());
    }
}

record Key  { internal required int[] Heights; }
record Lock { internal required int[] Heights; }

public class LockerTest
{
    [Fact]
    internal void Example()
    {
        var input = @"
#####
.####
.####
.####
.#.#.
.#...
.....

#####
##.##
.#.##
...##
...#.
...#.
.....

.....
#....
#....
#...#
#.#.#
#.###
#####

.....
.....
#.#..
###..
###.#
###.#
#####

.....
.....
.....
#....
#.#..
#.#.#
#####
";

        Assert.Equal(3, new Locker(Part.A).Solve(input));
    }
}
