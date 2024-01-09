namespace GraphABC.Implementation;

class ObserverBee
{
    public void AssignColor(Node node, List<int> utilizedColors, Queue<int> freeColors)
    {
        var colorIndex = 0;
        
        while (!node.IsColoredProperly || node.Paint == -1)
        {
            if (colorIndex >= utilizedColors.Count - 1 || utilizedColors.Count == 0)
            {
                var newColor = freeColors.Dequeue();
                node.Paint = newColor;
                utilizedColors.Add(newColor);
                
                return;
            }

            node.Paint = utilizedColors[colorIndex++];
        }
    }
}
