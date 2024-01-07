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

        // Recursive Best-First Search
        var rbfsSolver = new RecursiveBestFirstSolver(maze, point => HeuristicH3(point, maze.End));
        var rbfsResult = rbfsSolver.Solve(printSteps: true);
        Console.WriteLine("Recursive Best-First Search Result:");
        PrintResult(rbfsResult);
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
