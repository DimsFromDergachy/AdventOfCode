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



record Node
{
    internal Node? LeftNode;
    internal (long Left, long Right) Range;
    internal Node? RightNode;

    public static bool operator <(long value, Node node) => value < node.Range.Left;
    public static bool operator >(long value, Node node) => value > node.Range.Right;
    public static bool operator ==(long value, Node node) => !(value != node); // In
    public static bool operator !=(long value, Node node) => value < node || node < value;

    public static bool operator <(Node node, long value) => node.Range.Right < value;
    public static bool operator >(Node node, long value) => node.Range.Left > value;
    public static bool operator ==(Node node, long value) => !(node != value); // In
    public static bool operator !=(Node node, long value) => node < value || value < node;

    internal Node Merge(long left, long right)
    {
        // go to left subtree
        if (right < this)
        {
            return this with
            {
                LeftNode = LeftNode is null
                    ? new Node { Range = (left, right) }
                    : LeftNode.Merge(left, right)
            };
        }

        // go to right subtree
        if (this < left)
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

    private (Node? updated, long updatedLeft) MergeToLeft(/*this*/ Node? current, long left)
    {
        if (current is null)
            return (null, left);

        if (left < current)
            return MergeToLeft(current.LeftNode, left);

        if (current == left) // left in current
            return (current.LeftNode, current.Range.Left);

        if (current < left)
        {
            (current.RightNode, var updatedLeft) = MergeToLeft(current.RightNode, left);
            return (current, updatedLeft);
        }

        throw new NotImplementedException();
    }

    private (Node? updated, long updatedRight) MergeToRight(/*this*/Node? current, long right)
    {
        if (current is null)
            return (null, right);

        if (current < right)
            return MergeToRight(current.RightNode, right);

        if (current == right) // right in current
            return (current.RightNode, current.Range.Right);

        if (right < current)
        {
            (current.LeftNode, var updatedRight) = MergeToRight(current.LeftNode, right);
            return (current, updatedRight);
        }

        throw new NotImplementedException();
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
}