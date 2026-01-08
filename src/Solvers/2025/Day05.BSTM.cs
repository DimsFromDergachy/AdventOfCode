namespace Year2025.Day05.BSTM;

// BST + Merge ranges
// Node :: Left x Range x Right
// Invariant: Maximum of left subtree ranges is less than left bound of the range
//            (vice versa for the right as well)
// Merge ranges in case of intersection
//             (12, 15)
//            /        \
//      (3, 5)          (23, 31)
//             \
//              (8, 10)

record Range
{
    internal required long Left;
    internal required long Right;
    internal long Size => Right - Left + 1;

    public static bool operator <(long value, Range range) => value < range.Left;
    public static bool operator >(long value, Range range) => value > range.Right;

    public static bool operator <(Range range, long value) => range.Right < value;
    public static bool operator >(Range range, long value) => range.Left > value;

    public static implicit operator Range((long Left, long Right) value)
        => new Range { Left = value.Left, Right = value.Right };
}

static class RangeExtensions
{
    internal static bool In(this long value, Range range) => range.Left <= value && value <= range.Right;
    internal static bool Has(this Range range, long value) => value.In(range);
}

record Node
{
    internal Node? LeftNode;
    internal required Range Range;
    internal Node? RightNode;

    internal Node Merge(Range range) => Merge(range.Left, range.Right);

    internal Node Merge(long left, long right)
    {
        // go to left subtree
        if (right < this.Range)
        {
            return this with
            {
                LeftNode = LeftNode is null
                    ? new Node { Range = (left, right) }
                    : LeftNode.Merge(left, right)
            };
        }

        // go to right subtree
        if (left > this.Range)
        {
            return this with
            {
                RightNode = RightNode is null
                    ? new Node { Range = (left, right) }
                    : RightNode.Merge(left, right)
            };
        }

        (LeftNode, Range.Left) = MergeToLeft(this, left);
        (RightNode, Range.Right) = MergeToRight(this, right);

        return this;
    }

    private (Node? updated, long updatedLeft) MergeToLeft(Node? current, long left)
    {
        if (current is null)
            return (null, left);

        if (left < current.Range)
            return MergeToLeft(current.LeftNode, left);

        if (left.In(current.Range))
            return (current.LeftNode, current.Range.Left);

        if (left > current.Range)
        {
            (current.RightNode, var updatedLeft) = MergeToLeft(current.RightNode, left);
            return (current, updatedLeft);
        }

        throw new NotImplementedException();
    }

    private (Node? updated, long updatedRight) MergeToRight(Node? current, long right)
    {
        if (current is null)
            return (null, right);

        if (current.Range < right)
            return MergeToRight(current.RightNode, right);

        if (current.Range.Has(right))
            return (current.RightNode, current.Range.Right);

        if (current.Range > right)
        {
            (current.LeftNode, var updatedRight) = MergeToRight(current.LeftNode, right);
            return (current, updatedRight);
        }

        throw new NotImplementedException();
    }
}

static class BSTM
{
    internal static Node? Build(IEnumerable<Range> ranges)
        => ranges.Aggregate<Range, Node?>(null,
            (node, range) => node is null
                ? new Node { Range = range }
                : node.Merge(range));

    internal static bool In(this Node? node, long value)
    {
        if (node is null)
            return false;

        if (value < node.Range)
            return node.LeftNode.In(value);

        if (value.In(node.Range))
            return true;

        if (value > node.Range)
            return node.RightNode.In(value);

        throw new NotImplementedException();
    }

    internal static TResult Fold<TResult>(
        this Node? node, TResult seed, Func<TResult, Range, TResult> func)
    {
        if (node is null)
            return seed;

        var left = node.LeftNode.Fold(seed, func);
        var curr = func(left, node.Range);
        var right = node.RightNode.Fold(curr, func);
        return right;
    }
}

public class Test
{
    Node Input => new Node
    {
        LeftNode = new Node
        {
            Range = (3, 5),
            RightNode = new Node { Range = (8, 10) }
        },
        Range = (12, 15),
        RightNode = new Node { Range = (23, 31) },
    };
    Node? result;

    [Fact]
    public void MergeAround_3_5()
    {
        result = Input.Merge(0, 1);
        Assert.Equal((3, 5), result!.LeftNode!.Range);
        Assert.Equal((0, 1), result!.LeftNode!.LeftNode!.Range);
        Assert.Equal((8, 10), result!.LeftNode!.RightNode!.Range);

        result = Input.Merge(0, 4);
        Assert.Equal((0, 5), result.LeftNode!.Range);
        Assert.Equal((8, 10), result!.LeftNode!.RightNode!.Range);

        result = Input.Merge(4, 4);
        Assert.Equal((3, 5), result.LeftNode!.Range);
        Assert.Equal((8, 10), result!.LeftNode!.RightNode!.Range);

        result = Input.Merge(4, 7);
        Assert.Equal((3, 7), result.LeftNode!.Range);
        Assert.Equal((8, 10), result!.LeftNode!.RightNode!.Range);

        result = Input.Merge(6, 7);
        Assert.Equal((3, 5), result.LeftNode!.Range);
        Assert.Equal((8, 10), result.LeftNode!.RightNode!.Range);
        Assert.Equal((6, 7), result.LeftNode!.RightNode!.LeftNode!.Range);

        result = Input.Merge(0, 7);
        Assert.Equal((0, 7), result.LeftNode!.Range);
        Assert.Equal((8, 10), result!.LeftNode!.RightNode!.Range);
    }

    [Fact]
    public void MergeAround_3_5_And_8_10()
    {
        result = Input.Merge(0, 9);
        Assert.Equal((0, 10), result.LeftNode!.Range);
        Assert.Null(result.LeftNode!.LeftNode);
        Assert.Null(result.LeftNode!.RightNode);

        result = Input.Merge(0, 11);
        Assert.Equal((0, 11), result.LeftNode!.Range);
        Assert.Null(result.LeftNode!.LeftNode);
        Assert.Null(result.LeftNode!.RightNode);

        result = Input.Merge(4, 8);
        Assert.Equal((3, 10), result.LeftNode!.Range);
        Assert.Null(result.LeftNode!.LeftNode);
        Assert.Null(result.LeftNode!.RightNode);

        result = Input.Merge(11, 11);
        Assert.Equal((8, 10), result.LeftNode!.RightNode!.Range);
        Assert.Equal((11, 11), result.LeftNode!.RightNode!.RightNode!.Range);

        result = Input.Merge(4, 11);
        Assert.Equal((3, 11), result.LeftNode!.Range);
        Assert.Null(result.LeftNode!.LeftNode);
        Assert.Null(result.LeftNode!.RightNode);
    }

    [Fact]
    public void MergeAround_8_10()
    {
        result = Input.Merge(6, 7);
        Assert.Equal((8, 10), result.LeftNode!.RightNode!.Range);
        Assert.Equal((6, 7), result.LeftNode!.RightNode!.LeftNode!.Range);

        result = Input.Merge(7, 10);
        Assert.Equal((7, 10), result.LeftNode!.RightNode!.Range);
        Assert.Null(result.LeftNode!.RightNode!.LeftNode);
        Assert.Null(result.LeftNode!.RightNode!.RightNode);

        result = Input.Merge(8, 9);
        Assert.Equal((8, 10), result.LeftNode!.RightNode!.Range);
        Assert.Null(result.LeftNode!.RightNode!.LeftNode);
        Assert.Null(result.LeftNode!.RightNode!.RightNode);

        result = Input.Merge(10, 11);
        Assert.Equal((8, 11), result.LeftNode!.RightNode!.Range);
        Assert.Null(result.LeftNode!.RightNode!.LeftNode);
        Assert.Null(result.LeftNode!.RightNode!.RightNode);

        result = Input.Merge(11, 11);
        Assert.Equal((11, 11), result.LeftNode!.RightNode!.RightNode!.Range);
        Assert.Equal((8, 10), result.LeftNode!.RightNode!.Range);
        Assert.Null(result.LeftNode!.RightNode!.LeftNode);

        result = Input.Merge(7, 11);
        Assert.Equal((7, 11), result.LeftNode!.RightNode!.Range);
        Assert.Null(result.LeftNode!.RightNode!.LeftNode);
        Assert.Null(result.LeftNode!.RightNode!.RightNode);
    }

    [Fact]
    public void MergeAround_8_10_And_12_15()
    {
        result = Input.Merge(7, 13);
        Assert.Equal((7, 15), result.Range);
        Assert.Equal((3, 5), result.LeftNode!.Range);
        Assert.Null(result.LeftNode!.RightNode);

        result = Input.Merge(9, 12);
        Assert.Equal((8, 15), result.Range);
        Assert.Equal((3, 5), result.LeftNode!.Range);
        Assert.Null(result.LeftNode!.RightNode);

        result = Input.Merge(10, 18);
        Assert.Equal((8, 18), result.Range);
        Assert.Equal((3, 5), result.LeftNode!.Range);
        Assert.Null(result.LeftNode!.RightNode);
        Assert.Equal((23, 31), result.RightNode!.Range);

        result = Input.Merge(7, 22);
        Assert.Equal((7, 22), result.Range);
        Assert.Equal((3, 5), result.LeftNode!.Range);
        Assert.Null(result.LeftNode!.RightNode);
        Assert.Equal((23, 31), result.RightNode!.Range);
    }

    [Fact]
    public void MergeAround_12_15()
    {
        result = Input.Merge(11, 11);
        Assert.Equal((12, 15), result.Range);
        Assert.Equal((11, 11), result.LeftNode!.RightNode!.RightNode!.Range);

        result = Input.Merge(11, 12);
        Assert.Equal((11, 15), result.Range);
        Assert.Equal((3, 5), result.LeftNode!.Range);
        Assert.Equal((8, 10), result.LeftNode!.RightNode!.Range);
        Assert.Equal((23, 31), result.RightNode!.Range);

        result = Input.Merge(12, 15);
        Assert.Equal((12, 15), result.Range);
        Assert.Equal((3, 5), result.LeftNode!.Range);
        Assert.Equal((8, 10), result.LeftNode!.RightNode!.Range);
        Assert.Equal((23, 31), result.RightNode!.Range);

        result = Input.Merge(14, 22);
        Assert.Equal((12, 22), result.Range);
        Assert.Equal((3, 5), result.LeftNode!.Range);
        Assert.Equal((8, 10), result.LeftNode!.RightNode!.Range);
        Assert.Equal((23, 31), result.RightNode!.Range);

        result = Input.Merge(11, 16);
        Assert.Equal((11, 16), result.Range);
        Assert.Equal((3, 5), result.LeftNode!.Range);
        Assert.Equal((8, 10), result.LeftNode!.RightNode!.Range);
        Assert.Equal((23, 31), result.RightNode!.Range);
    }

    [Fact]
    public void MergeAround_23_31()
    {
        result = Input.Merge(20, 21);
        Assert.Equal((20, 21), result.RightNode!.LeftNode!.Range);
        Assert.Equal((12, 15), result.Range);
        Assert.Equal((23, 31), result.RightNode!.Range);
        Assert.Null(result.RightNode!.RightNode);

        result = Input.Merge(20, 23);
        Assert.Equal((20, 31), result.RightNode!.Range);
        Assert.Null(result.RightNode!.LeftNode);
        Assert.Null(result.RightNode!.RightNode);

        result = Input.Merge(26, 27);
        Assert.Equal((23, 31), result.RightNode!.Range);
        Assert.Null(result.RightNode!.LeftNode);
        Assert.Null(result.RightNode!.RightNode);

        result = Input.Merge(25, 35);
        Assert.Equal((23, 35), result.RightNode!.Range);
        Assert.Null(result.RightNode!.LeftNode);
        Assert.Null(result.RightNode!.RightNode);

        result = Input.Merge(35, 35);
        Assert.Equal((35, 35), result.RightNode!.RightNode!.Range);
        Assert.Equal((23, 31), result.RightNode!.Range);
        Assert.Null(result.RightNode!.LeftNode);
        Assert.Null(result.RightNode!.RightNode!.LeftNode);
        Assert.Null(result.RightNode!.RightNode!.RightNode);
    }

    [Fact]
    public void MergeAll()
    {
        result = Input.Merge(0, 25);
        Assert.Equal((0, 31), result.Range);
        Assert.Null(result.LeftNode);
        Assert.Null(result.RightNode);

        result = Input.Merge(4, 25);
        Assert.Equal((3, 31), result.Range);
        Assert.Null(result.LeftNode);
        Assert.Null(result.RightNode);

        result = Input.Merge(4, 35);
        Assert.Equal((3, 35), result.Range);
        Assert.Null(result.LeftNode);
        Assert.Null(result.RightNode);

        result = Input.Merge(0, 35);
        Assert.Equal((0, 35), result.Range);
        Assert.Null(result.LeftNode);
        Assert.Null(result.RightNode);
    }

    [Fact]
    public void Build()
    {
//             (12, 15)
//            /        \
//      (3, 5)          (23, 31)
//             \
//              (8, 10)

        var ranges = new Range[] { (12, 15), (3, 5), (23, 31), (8, 10)};
        Node result = BSTM.Build(ranges)!;

        Assert.Equal((12, 15), result.Range);
        Assert.Equal((3, 5), result.LeftNode!.Range);
        Assert.Null(result.LeftNode!.LeftNode);
        Assert.Equal((8, 10), result.LeftNode!.RightNode!.Range);
        Assert.Null(result.LeftNode!.RightNode!.LeftNode);
        Assert.Null(result.LeftNode!.RightNode!.RightNode);
        Assert.Equal((23, 31), result.RightNode!.Range);
        Assert.Null(result.RightNode!.LeftNode);
        Assert.Null(result.RightNode!.RightNode);
    }

    [Theory]
    [InlineData( 0, false)]
    [InlineData( 2, false)]
    [InlineData( 3,  true)]
    [InlineData( 4,  true)]
    [InlineData( 5,  true)]
    [InlineData( 6, false)]
    [InlineData( 7, false)]
    [InlineData( 8,  true)]
    [InlineData( 9,  true)]
    [InlineData(10,  true)]
    [InlineData(11, false)]
    [InlineData(12,  true)]
    [InlineData(15,  true)]
    [InlineData(16, false)]
    [InlineData(20, false)]
    [InlineData(22, false)]
    [InlineData(23,  true)]
    [InlineData(27,  true)]
    [InlineData(31,  true)]
    [InlineData(32, false)]
    [InlineData(35, false)]
    public void In(long value, bool expected)
        => Assert.Equal(expected, Input.In(value));
}