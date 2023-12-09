using NaturalSort.Generators;

using System.Diagnostics;

new TxtGenerator().GenerateSize(1, "file.txt");
File.Delete("baseUnsorted.txt");
File.Copy("file.txt", "baseUnsorted.txt");
Stopwatch sw = Stopwatch.StartNew();
await new AdaptiveSort().SortAsync("file.txt");
sw.Stop();
Console.WriteLine(sw.Elapsed.TotalSeconds);
Console.ReadLine();