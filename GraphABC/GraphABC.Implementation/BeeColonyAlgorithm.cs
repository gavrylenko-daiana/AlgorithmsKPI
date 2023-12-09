namespace GraphABC.Implementation;

public class BeeColonyAlgorithm
{
    private readonly Network _baseNetwork;
    
    private readonly int _nodeCount;
    
    private readonly int _explorersCount;
    
    private readonly int _observersCount;
    
    private readonly List<ExplorerBee> _explorers;
    
    private readonly List<ObserverBee> _observers;
    
    private readonly Queue<int> _colorsQueue;
    
    private readonly List<int> _appliedColors;
    

    public BeeColonyAlgorithm(Network network, int explorersCount, int observersCount)
    {
        _baseNetwork = network;
        _nodeCount = network.Nodes.Count;
        _explorersCount = explorersCount;
        _observersCount = observersCount;
        _explorers = Enumerable.Range(0, explorersCount).Select(_ => new ExplorerBee((Network)network.Clone())).ToList();
        _observers = Enumerable.Range(0, observersCount).Select(_ => new ObserverBee()).ToList();
        _colorsQueue = new Queue<int>(Enumerable.Range(0, _nodeCount));
        _appliedColors = new List<int>(_nodeCount);
    }

    public Network Execute(bool displayIterations = false)
    {
        InitiateFirstNodes();
        var iteration = 0;
        while (_explorers.Any(e => e.AlreadyChosen.Count != _nodeCount))
        {
            Dictionary<ExplorerBee, Node> explorersNodes = _explorers.ToDictionary(e => e, e => e.PickNode());
            var nodesValues = explorersNodes.Select(pair => new {
                Node = pair.Value,
                Value = pair.Key.EvaluateNodeValue(pair.Value, explorersNodes.Values, _observersCount)
            });

            foreach (var nodeValue in nodesValues)
            {
                var observerIndex = 0;
                foreach (var adjacent in nodeValue.Node.AdjacentNodes)
                {
                    if (observerIndex >= nodeValue.Value - 1)
                        break;

                    _observers[observerIndex++].AssignColor(adjacent, _appliedColors, _colorsQueue);
                }

                _observers[++observerIndex].AssignColor(nodeValue.Node, _appliedColors, _colorsQueue);
            }
            iteration++;
            if (displayIterations && iteration % 20 == 0)
                DisplayIteration(iteration);
        }

        return _explorers.Select(e => e.Network).MinBy(n => n.PaintVariety)!;
    }

    private void InitiateFirstNodes()
    {
        var topNodes = ExplorerBee.SelectTopNodes(_baseNetwork, _explorersCount).ToList();
        for (var i = 0; i < _explorersCount; i++)
        {
            _explorers[i].ChosenNodeId = topNodes[i].Identifier;
            _explorers[i].AlreadyChosen.Add(topNodes[i]);
        }
    }

    private void DisplayIteration(int iteration)
    {
        Console.WriteLine("Iteration:" + iteration);
        
        for (var i = 0; i < _explorers.Count; i++)
        {
            var result = _explorers[i].Network.IsStructuredProperly
                ? $"PaintVariety: {_explorers[i].Network.PaintVariety}\n"
                : "Solution is incorrect\n";
            Console.WriteLine($"Explorer #{i} - {result}");
        }

        Console.WriteLine();
    }
}
