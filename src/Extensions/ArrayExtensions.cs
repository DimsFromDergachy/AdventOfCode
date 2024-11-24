namespace Year2015.Day06;

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