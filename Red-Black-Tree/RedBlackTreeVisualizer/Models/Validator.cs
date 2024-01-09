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

            return uint.TryParse(line, out _);
        }

        public static bool IsCompensationValid(string? line)
        {
            if (line is null || line.Contains(',') || double.TryParse(line, out var compensation) == false ||
                compensation < 0)
            {
                return false;
            }

            return true;
        }
    }
}
