using Maze.Generators;
using Maze.Models;
using Maze.Search;

class Program
{
    static void Main(string[] args)
    {
        var mazeGenerator = new MazeGenerator();
        var maze = mazeGenerator.GenerateMaze();

        // Iterative Deepening Search
        var idsSolver = new IterativeDeepeningSolver(maze);
        var idsResult = idsSolver.Solve(printSteps: true);
        Console.WriteLine("Iterative Deepening Search Result:");
        PrintResult(idsResult);
        Console.WriteLine($"Total Dead Ends: {idsSolver.DeadEndCount}");
        Console.WriteLine($"Total States: {idsSolver.TotalStates}");
        Console.WriteLine($"Max States in Memory: {idsSolver.MaxStatesInMemory}\n");

        // Recursive Best-First Search
        var rbfsSolver = new RecursiveBestFirstSolver(maze, point => HeuristicH3(point, maze.End));
        var rbfsResult = rbfsSolver.Solve(printSteps: true);
        Console.WriteLine("\nRecursive Best-First Search Result:");
        PrintResult(rbfsResult);
        Console.WriteLine($"Total Dead Ends: {rbfsSolver.DeadEndCount}");
        Console.WriteLine($"Total States: {rbfsSolver.TotalStates}");
        Console.WriteLine($"Max States in Memory: {rbfsSolver.MaxStatesInMemory}");

    }

    static int HeuristicH3((int row, int col) point, (int row, int col) goal)
    {
        return (int)Math.Sqrt(Math.Pow(point.row - goal.row, 2) + Math.Pow(point.col - goal.col, 2));
    }

    static void PrintResult(PathFindingResult result)
    {
        if (result.FoundPath == null || result.FoundPath.Count == 0)
        {
            Console.WriteLine("No path found.");
        }
        else
        {
            Console.WriteLine($"Path found with depth {result.DepthReached}:");
            
            foreach (var step in result.FoundPath)
            {
                Console.WriteLine($"({step.Item1}, {step.Item2})");
            }
        }
    }
}
