using Maze.Helpers;

namespace Maze.Search;

using Models;
using System.Collections.Generic;
using System.Drawing;

public class IterativeDeepeningSolver
{
    public int DeadEndCount => _deadEndCount;
    public int TotalStates => _totalStates;
    public int MaxStatesInMemory => _maxStatesInMemory;

    private int _iterationCount = 0;
    private int _deadEndCount = 0;
    private int _totalStates = 0;
    private int _maxStatesInMemory = 0;
    private readonly MazeModel _maze;

    public IterativeDeepeningSolver(MazeModel maze)
    {
        _maze = maze;
    }

    public PathFindingResult Solve(bool printSteps = false)
    {
        _iterationCount = 0;
        int maxDepth = _maze.Layout.GetLength(0) + _maze.Layout.GetLength(1) - 1;
        var visited = new HashSet<(int, int)>();

        for (int depth = 1; depth <= maxDepth; depth++)
        {
            visited.Clear();
            var result = DeepFirstSearch(_maze.Start, depth, visited, printSteps, new Stack<(int, int)>());

            if (result.FoundPath != null && IsOptimalPath(result.FoundPath))
            {
                Console.WriteLine("Total Iterations: " + _iterationCount);

                return result;
            }
        }

        return new PathFindingResult(null, int.MaxValue);
    }

    private PathFindingResult DeepFirstSearch((int, int) current, int depth, HashSet<(int, int)> visited,
        bool printSteps, Stack<(int, int)> path)
    {
        _totalStates++;

        if (depth == 0)
        {
            _deadEndCount++;
            return new PathFindingResult(null, int.MaxValue);
        }

        if (visited.Contains(current))
        {
            return new PathFindingResult(null, int.MaxValue);
        }

        _iterationCount++;

        if (printSteps)
        {
            Console.WriteLine($"Iteration {_iterationCount}: Visiting node at {current}, Depth: {depth}");
        }

        if (_maze.End == current && depth == 1)
        {
            path.Push(current);
            var foundPath = new List<(int, int)>(path.Reverse());
            return new PathFindingResult(foundPath, depth);
        }

        visited.Add(current);
        path.Push(current);

        foreach (var child in MazeHelper.GetChildren(_maze, current))
        {
            if (!visited.Contains(child))
            {
                var result = DeepFirstSearch(child, depth - 1, visited, printSteps, path);

                if (result.FoundPath != null)
                {
                    result.DepthReached = depth;
                    return result;
                }
            }
        }

        path.Pop();
        visited.Remove(current);

        _maxStatesInMemory = Math.Max(_maxStatesInMemory, visited.Count + path.Count);

        return new PathFindingResult(null, int.MaxValue);
    }

    private bool IsOptimalPath(List<(int, int)> path)
    {
        int manhattanDistance = CalculateManhattanDistance(_maze.Start, _maze.End);
        return path.Count <= manhattanDistance;
    }

    private int CalculateManhattanDistance((int, int) start, (int, int) end)
    {
        return Math.Abs(start.Item1 - end.Item1) + Math.Abs(start.Item2 - end.Item2) + 1;
    }
}