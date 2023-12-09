namespace GraphABC.Implementation;

public class Node : IEquatable<Node>
{
    public int Identifier { get; init; }
    
    public int Paint { get; set; }
    
    public HashSet<Node> AdjacentNodes { get; init; }

    public int Connectivity => AdjacentNodes.Count;
    
    public bool IsColoredProperly => AdjacentNodes.All(n => n.Paint != Paint);

    public Node(int id, int paint = -1)
    {
        Identifier = id;
        Paint = paint;
        AdjacentNodes = new HashSet<Node>();
    }

    public void AddNeighbor(Node adjacent)
    {
        AdjacentNodes.Add(adjacent);
        adjacent.AdjacentNodes.Add(this);
    }

    public override string ToString()
    {
        return $"{nameof(Identifier)}: {Identifier}, {nameof(Paint)}: {Paint}, {nameof(Connectivity)}: {Connectivity}";
    }

    public bool Equals(Node? other)
    {
        return other?.Identifier == Identifier && other.Paint == Paint;
    }
    
    public override bool Equals(object? obj)
    {
        return Equals(obj as Node);
    }
    
    public override int GetHashCode()
    {
        return Identifier.GetHashCode();
    }
}
