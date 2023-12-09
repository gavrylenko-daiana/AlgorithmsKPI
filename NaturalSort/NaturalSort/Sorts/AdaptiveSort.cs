using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Concurrent;

public class AdaptiveSort
{
    private int _count = 1;
    private readonly object _lock = new();

    public async Task SortAsync(string sourceFile)
    {
        await RecSortAsync(sourceFile);
    }

    private async Task RecSortAsync(string sourceFile)
    {
        lock (_lock)
        {
            Console.WriteLine($"Stage {_count++}");
        }

        await SplitBySeriesAsync(sourceFile, "fileB.txt", "fileC.txt");
        bool isFinal = await MergePartsAsync("fileB.txt", "fileC.txt", sourceFile);

        if (isFinal)
        {
            Console.WriteLine("Sorted");
        }
        else
        {
            await RecSortAsync(sourceFile);
        }
    }

    private async Task SplitBySeriesAsync(string sourceFile, string fileB, string fileC)
    {
        int prev = int.MaxValue;
        bool isOddGroup = true;

        using (var sourceReader = new StreamReader(sourceFile))
        {
            using (var bWriter = new StreamWriter(fileB))
            using (var cWriter = new StreamWriter(fileC))
            {
                while (sourceReader.Peek() >= 0)
                {
                    int current = await ReadIntAsync(sourceReader);
                    if (current == int.MaxValue)
                        break;

                    if (current < prev)
                    {
                        isOddGroup = !isOddGroup;
                    }

                    if (isOddGroup)
                    {
                        await bWriter.WriteLineAsync(current.ToString());
                    }
                    else
                    {
                        await cWriter.WriteLineAsync(current.ToString());
                    }

                    prev = current;
                }

                await bWriter.FlushAsync();
                await cWriter.FlushAsync();
            }
        }
    }

    private async Task<bool> MergePartsAsync(string fileB, string fileC, string sourceFile)
    {
        using var bReader = new StreamReader(fileB);
        using var cReader = new StreamReader(fileC);
        using var sourceWriter = new StreamWriter(sourceFile);

        int topFromB = await ReadIntAsync(bReader);
        int topFromC = await ReadIntAsync(cReader);

        bool canMoveB = true;
        bool canMoveC = true;

        int bFileGroupsCount = 0;
        int cFileGroupsCount = 0;

        int prevB = 0, prevC = 0;

        while (topFromB != int.MaxValue || topFromC != int.MaxValue)
        {
            if (canMoveB && (topFromB < topFromC || !canMoveC))
            {
                await sourceWriter.WriteLineAsync(topFromB.ToString());
                prevB = topFromB;
                topFromB = await ReadIntAsync(bReader);

                canMoveB = prevB <= topFromB && topFromB != int.MaxValue;
                if (!canMoveB)
                {
                    bFileGroupsCount++;
                }
            }
            else if (canMoveC && (topFromC <= topFromB || !canMoveB))
            {
                await sourceWriter.WriteLineAsync(topFromC.ToString());
                prevC = topFromC;
                topFromC = await ReadIntAsync(cReader);

                canMoveC = prevC <= topFromC && topFromC != int.MaxValue;
                if (!canMoveC)
                {
                    cFileGroupsCount++;
                }
            }

            AdjustMove(ref canMoveB, ref canMoveC, topFromB, topFromC);
        }
        
        return bFileGroupsCount == 1 && cFileGroupsCount == 1;
    }

    private void AdjustMove(ref bool canMoveB, ref bool canMoveC, int topFromB, int topFromC)
    {
        if (canMoveB == canMoveC)
        {
            canMoveB = canMoveC = true;

            if (topFromB == int.MaxValue) canMoveB = false;
            if (topFromC == int.MaxValue) canMoveC = false;
        }
    }

    private async Task<int> ReadIntAsync(StreamReader reader)
    {
        var line = await reader.ReadLineAsync();
        return int.TryParse(line, out var result) ? result : int.MaxValue;
    }
}