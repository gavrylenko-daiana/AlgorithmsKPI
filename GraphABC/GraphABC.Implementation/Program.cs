using GraphABC.Implementation;

var totalNodes = 250;
var maxConnectivity = 25;
var beePopulation = 35;
var explorers = 3;
var observers = beePopulation - explorers;

var network = Network.ConstructNetwork(totalNodes, maxConnectivity);
var bca = new BeeColonyAlgorithm(network, explorers, observers);
var solution = bca.Execute(displayIterations: true);

Console.WriteLine(solution.ToString());

Console.WriteLine("--------------------------------------");
Console.WriteLine("Final Outcome");
Console.WriteLine(solution.IsStructuredProperly ? $"Color Diversity: {solution.PaintVariety}" : "Solution is incorrect");
