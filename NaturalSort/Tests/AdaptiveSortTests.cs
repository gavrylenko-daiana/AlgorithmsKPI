using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NaturalSort.Generators;
using Xunit;

namespace Tests
{
    public class AdaptiveSortTests : IDisposable
    {
        private const string TestFolderPath = "TestFiles";

        public AdaptiveSortTests()
        {
            Cleanup();
            Directory.CreateDirectory(TestFolderPath);
        }

        private string CreateTestFile(string fileName, params string[] contents)
        {
            var filePath = Path.Combine(TestFolderPath, fileName);
            File.WriteAllLines(filePath, contents);
            return filePath;
        }

        private void Cleanup()
        {
            if (Directory.Exists(TestFolderPath))
            {
                var dirInfo = new DirectoryInfo(TestFolderPath);

                foreach (var file in dirInfo.GetFiles())
                {
                    file.Attributes = FileAttributes.Normal;
                    file.Delete();
                }
                
                dirInfo.Delete(true);
            }
        }

        #region Numeric Sorting Tests

        [Fact]
        public async Task TestSortAsyncWithPositiveNumbers()
        {
            var testFilePath = CreateTestFile("positiveNumbers.txt", new[] { "5", "12", "7", "9", "1" });
            var adaptiveSort = new AdaptiveSort();

            await adaptiveSort.SortAsync(testFilePath);
            var sortedLines = File.ReadAllLines(testFilePath).Select(int.Parse).ToArray();

            Assert.Equal(new[] { 1, 5, 7, 9, 12 }, sortedLines);
        }

        [Fact]
        public async Task TestSortAsyncWithNegativeNumbers()
        {
            var testFilePath = CreateTestFile("negativeNumbers.txt", new[] { "-5", "-12", "-7", "-9", "-1" });
            var adaptiveSort = new AdaptiveSort();

            await adaptiveSort.SortAsync(testFilePath);
            var sortedLines = File.ReadAllLines(testFilePath).Select(int.Parse).ToArray();

            Assert.Equal(new[] { -12, -9, -7, -5, -1 }, sortedLines);
        }

        #endregion

        #region Edge Cases

        [Fact]
        public async Task TestSortAsyncWithNullPath_ShouldThrowException()
        {
            var adaptiveSort = new AdaptiveSort();
            await Assert.ThrowsAsync<ArgumentNullException>(() => adaptiveSort.SortAsync(null));
        }

        [Fact]
        public async Task TestSortAsyncWithNonExistentFile_ShouldThrowException()
        {
            var adaptiveSort = new AdaptiveSort();
            await Assert.ThrowsAsync<FileNotFoundException>(() => adaptiveSort.SortAsync("nonExistentFile.txt"));
        }

        [Fact]
        public async Task TestSortAsyncWithReadOnlyFile_ShouldThrowException()
        {
            var testFilePath = CreateTestFile("readOnlyFile.txt", new[] { "5", "3", "8", "4", "2" });
            File.SetAttributes(testFilePath, FileAttributes.ReadOnly);
            var adaptiveSort = new AdaptiveSort();

            Exception exception = null;

            try
            {
                await adaptiveSort.SortAsync(testFilePath);
            }
            catch (UnauthorizedAccessException ex)
            {
                exception = ex;
            }
            finally
            {
                File.SetAttributes(testFilePath, FileAttributes.Normal);
            }

            Assert.NotNull(exception);
        }

        #endregion

        public void Dispose()
        {
            Cleanup();
        }
    }
}