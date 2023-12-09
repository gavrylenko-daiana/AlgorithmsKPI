namespace GraphABC.Implementation;

public class Network : ICloneable
{
    public List<Node> Nodes { get; init; } = new();
    
    public int PaintVariety => Nodes.Select(n => n.Paint).Distinct().Count();
    
    public bool IsStructuredProperly => Nodes.All(n => n.IsColoredProperly);
    

    public object Clone()
    {
        var connectivityMatrix = new int[Nodes.Count][];
        var colorings = Nodes.Select(node => node.Paint).ToArray();
        foreach (var node in Nodes)
        {
            connectivityMatrix[node.Identifier] = new int[node.Connectivity];
            var i = 0;
            foreach (var adjacent in node.AdjacentNodes)
            {
                connectivityMatrix[node.Identifier][i++] = adjacent.Identifier;
            }
        }

        var net = FromMatrix(connectivityMatrix);

        net.Nodes.ForEach(node => node.Paint = colorings[net.Nodes.IndexOf(node)]);

        return net;
    }

    public override string ToString()
    {
        return string.Join("\\n", Nodes);
    }

    public static Network ConstructNetwork(int totalNodes, int maxConnections)
    {
        var rng = new Random();
        var net = new Network();

        for (var i = 0; i < totalNodes; i++)
        {
            net.InsertNode(new Node(i));
        }

        var randomOrderNodes = net.Nodes.OrderBy(_ => rng.Next(totalNodes)).ToList();
        for (var nodeIndex = 0; nodeIndex < randomOrderNodes.Count; nodeIndex++)
        {
            var first = randomOrderNodes[nodeIndex];

            var nextIndex = nodeIndex != randomOrderNodes.Count - 1 ? nodeIndex + 1 : 0;
            var second = randomOrderNodes[nextIndex];

            net.ConnectNodes(first, second);
        }

        while (net.Nodes.All(node => node.Connectivity != maxConnections))
        {
            randomOrderNodes = net.Nodes.OrderBy(_ => rng.Next(totalNodes)).ToList();
            var first = randomOrderNodes.First();
            var second = randomOrderNodes.Last();

            net.ConnectNodes(first, second);
        }
        return net;
    }

    private static Network FromMatrix(int[][] matrix)
    {
        var net = new Network();

        for (var i = 0; i < matrix.Length; i++)
        {
            net.InsertNode(new Node(i));
        }

        for (var i = 0; i < matrix.Length; i++)
        {
            for (var j = 0; j < matrix[i].Length; j++)
            {
                net.Nodes[i].AddNeighbor(net.Nodes[matrix[i][j]]);
            }
        }

        return net;
    }
    
    private Node InsertNode(Node node)
    {
        if (Nodes.Any(n => n.Identifier == node.Identifier))
            throw new InvalidOperationException(nameof(node));

        Nodes.Add(node);
        return Nodes.Find(n => n.Equals(node))!;
    }

    private void ConnectNodes(Node firstNode, Node secondNode)
    {
        var first = Nodes.First(n => n.Identifier == firstNode.Identifier);
        var second = Nodes.First(n => n.Identifier == secondNode.Identifier);

        if (first == null || second == null)
            throw new ArgumentOutOfRangeException(nameof(firstNode), "One of the parameters is out of range");

        first.AddNeighbor(second);
    }
}
