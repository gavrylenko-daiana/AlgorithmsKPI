using GraphABC.Implementation;

namespace GraphABC.UnitTests;

public class AlgorithmUnitTests
{
    [Fact]
    public void Execute_ReturnsValidNetwork_WithValidInput()
    {
        var network = Network.ConstructNetwork(10, 5);
        var algorithm = new BeeColonyAlgorithm(network, 3, 5);

        var result = algorithm.Execute();

        Assert.NotNull(result);
        Assert.True(result.IsStructuredProperly);
    }

    [Fact]
    public void Execute_ThrowsException_WithInvalidInput()
    {
        var network = new Network();

        Assert.Throws<ArgumentOutOfRangeException>(() => new BeeColonyAlgorithm(network, -1, -1));
    }

    [Fact]
    public void Execute_CompletesExecution_WithValidInput()
    {
        var network = Network.ConstructNetwork(10, 5);
        var algorithm = new BeeColonyAlgorithm(network, 3, 5);

        var result = algorithm.Execute();

        Assert.NotNull(result);
    }

    [Fact]
    public void Execute_DoesNotExceedMaxPaintVariety()
    {
        var network = Network.ConstructNetwork(10, 5);
        var maxPaintVariety = 10;
        var algorithm = new BeeColonyAlgorithm(network, 3, 5);

        var result = algorithm.Execute();

        Assert.True(result.PaintVariety <= maxPaintVariety);
    }
}
