namespace Maze.Models;

public class PathFindingResult
{
    public List<(int, int)> FoundPath { get; } 
    public int DepthReached { get; set; }

    public PathFindingResult(List<(int, int)> foundPath, int depthReached)
    {
        FoundPath = foundPath;
        DepthReached = depthReached;
    }
}
