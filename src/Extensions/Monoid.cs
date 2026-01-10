using System.Numerics;

interface Monoid<T>
{
    T Empty { get; }        //  mempty
    T Append(T a, T b);     // mappend

    T Concat(IEnumerable<T> ts) => ts.Aggregate(Empty, Append);
}

struct Sum<T> : Monoid<T> where T : INumber<T>
{
    public T Empty => T.Zero;
    public T Append(T a, T b) => a + b;
}

struct Product<T> : Monoid<T> where T : INumber<T>
{
    public T Empty => T.One;
    public T Append(T a, T b) => a * b;
}