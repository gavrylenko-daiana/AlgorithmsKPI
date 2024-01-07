using Maze.Helpers;
using Maze.Models;

namespace Maze.Search;

public class RecursiveBestFirstSolver
{
    private int _iterationCount = 0;
    private readonly MazeModel _maze;
    private readonly Func<(int, int), int> _heuristic;
    private static int SomeMargin = 1;

    public RecursiveBestFirstSolver(MazeModel maze, Func<(int, int), int> heuristic)
    {
        _maze = maze;
        _heuristic = heuristic;
    }

    public PathFindingResult Solve(bool printSteps = false)
    {
        _iterationCount = 0;
        var path = new Stack<(int, int)>();
        var visited = new HashSet<(int, int)>();
        
        return RecursiveSearch(_maze.Start, int.MaxValue, visited, printSteps, path);
    }

    private PathFindingResult RecursiveSearch((int, int) current, int limit, HashSet<(int, int)> visited, bool printSteps, Stack<(int, int)> path)
    {
        _iterationCount++;
        
        if (printSteps)
        {
            Console.WriteLine($"Iteration {_iterationCount}: Visiting node at {current}");
        }

        if (current == _maze.End)
        {
            path.Push(current);
            
            return new PathFindingResult(new List<(int, int)>(path.Reverse()), path.Count);
        }

        if (!visited.Contains(current))
        {
            visited.Add(current);
            path.Push(current);
            var children = MazeHelper.GetChildren(_maze, current).ToList();
            
            if (children.Count == 0)
            {
                path.Pop();
                
                return new PathFindingResult(null, int.MaxValue);
            }

            children.Sort((a, b) => _heuristic(a).CompareTo(_heuristic(b)));

            foreach (var child in children)
            {
                int newLimit = Math.Min(limit, _heuristic(child) + SomeMargin);
                var result = RecursiveSearch(child, newLimit, visited, printSteps, path);
                
                if (result.FoundPath != null)
                {
                    return result;
                }
            }

            path.Pop();
        }

        return new PathFindingResult(null, int.MaxValue);
    }
}
