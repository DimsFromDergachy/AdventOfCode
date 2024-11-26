static class ArrayExtensions
{
    internal static IEnumerable<T> ToEnumerable<T>(this T[,] array)
    {
        var enumerator = array.GetEnumerator();
        while (enumerator.MoveNext())
        {
            yield return (T) enumerator.Current;
        }
    }
}

public class ArrayExtensionsTest
{
    [Fact]
    public void ToEnumerable()
    {
        var array = new int[3,3] {{1,2,3}, {4,5,6}, {7,8,9}};
        Assert.Equal(45, array.ToEnumerable().Sum());
    }
}