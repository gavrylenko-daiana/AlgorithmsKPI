namespace RedBlackTreeVisualizer.Models
{
    public static class DataValidator
    {
        public static bool IsIdValid(string? line)
        {
            if (line is null)
            {
                return false;
            }

            return int.TryParse(line, out _);
        }
    }
}
