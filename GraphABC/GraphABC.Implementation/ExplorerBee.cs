namespace GraphABC.Implementation;

public class ExplorerBee
{
    public ExplorerBee(Network net)
    {
        Network = net;
        AlreadyChosen = new HashSet<Node>(net.Nodes.Count);
        ChosenNodeId = -1;
    }

    public Network Network { get; }
    public int ChosenNodeId { get; set; }
    public HashSet<Node> AlreadyChosen { get; }

    public Node PickNode()
    {
        var node = Network.Nodes
            .Where(n => !AlreadyChosen.Contains(n))
            .OrderByDescending(n => n.Connectivity)
            .FirstOrDefault()!;

        ChosenNodeId = node.Identifier;
        AlreadyChosen.Add(node);
        
        return node;
    }

    public double EvaluateNodeValue(Node node, IEnumerable<Node> chosenNodes, int observers)
    {
        var totalConnectivity = chosenNodes.Sum(n => n.Connectivity);
        
        return observers * ((double)node.Connectivity / totalConnectivity);
    }

    public static IEnumerable<Node> SelectTopNodes(Network net, int number)
    {
        return net.Nodes
            .OrderByDescending(n => n.Connectivity)
            .Take(number);
    }
}
