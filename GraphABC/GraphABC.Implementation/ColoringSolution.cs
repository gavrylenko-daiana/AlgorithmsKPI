using System.Diagnostics.CodeAnalysis;

namespace GraphABC.Implementation;

class ColoringSolution : IEqualityComparer<ColoringSolution>
{
    public static int[][] ConnectivityMatrix { get; set; }
    
    public static int TotalConnections { get; set; }

    private int[] ColorDistribution { get; set; }


    public ColoringSolution(int[] solution)
    {
        ColorDistribution = solution;
    }
    
    public double Quality
    {
        get {
            var conflicts = 0;
            for (var i = 0; i < ConnectivityMatrix.Length; i++)
            {
                for (var j = 0; j < ConnectivityMatrix[i].Length; j++)
                {
                    if (ColorDistribution[i] == ColorDistribution[ConnectivityMatrix[i][j]])
                        conflicts++;
                }
            }
            
            return (double)conflicts / TotalConnections;
        }
    }
    
    public override string ToString()
    {
        var output = string.Empty;
        for (var i = 0; i < ColorDistribution.Length; i++)
        {
            output += $"Node #{i} painted with: {ColorDistribution[i]}\n";
        }
        
        return output;
    }
    
    public bool Equals(ColoringSolution? x, ColoringSolution? y)
    {
        if (x == y)
        {
            return true;
        }

        if (x == null ^ y == null)
        {
            return false;
        }
        
        return x!.ColorDistribution.Equals(y!.ColorDistribution);
    }

    public int GetHashCode([DisallowNull] ColoringSolution obj)
    {
        return ColorDistribution.GetHashCode();
    }
}
