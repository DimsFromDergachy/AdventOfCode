class Graph<TVertex> where TVertex : notnull
{
    bool[,] _adjacency;
    Dictionary<TVertex, int> _vertices;

    int this[TVertex v] => _vertices[v];
    bool this[TVertex a, TVertex b] =>
        _adjacency[this[a], this[b]];

    internal Graph(IList<(TVertex a, TVertex b)> edges)
    {
        _vertices = edges.SelectMany(p => new TVertex[] {p.a, p.b})
                         .Distinct()
                         .Zip(Enumerable.Range(0, int.MaxValue))
                         .ToDictionary(pair => pair.First, pair => pair.Second);

        _adjacency = new bool[_vertices.Count(), _vertices.Count()];
        foreach (var edge in edges)
        {
            _adjacency[this[edge.a], this[edge.b]] = true;
        }
    }

    internal IEnumerable<IList<TVertex>> Completes() =>
        Completes(Enumerable.Empty<TVertex>().ToList(), _vertices.Keys.ToList());

    private IEnumerable<IList<TVertex>> Completes(IList<TVertex> complete, IList<TVertex> vertices)
    {
        yield return complete;

        while (vertices.Any())
        {
            var v = vertices.First();
            vertices = vertices.Skip(1).ToList();

            if (complete.All(w => this[v, w]))
                foreach (var c in Completes(complete.Append(v).ToList(), vertices))
                    yield return c;
        }
    }
}

static class GraphExtensions
{
    internal static Graph<TVertex> ToGraph<TVertex>(this IList<(TVertex a, TVertex b)> source)
        where TVertex : notnull
            => new Graph<TVertex>(source);
}

