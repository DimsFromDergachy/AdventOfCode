static class TupleExtensions
{
    internal static (TB, TA) Swap<TA, TB>(this (TA a, TB b) _) => (_.b, _.a);
}