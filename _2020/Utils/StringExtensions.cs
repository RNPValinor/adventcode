namespace _2020.Utils
{
    public static class StringExtensions
    {
        public static string ReplaceFirst(this string text, char search, char replace)
        {
            var idx = text.IndexOf(search);

            return idx == -1 ? text : text.Remove(idx, 1).Insert(idx, $"{replace}");
        }
    }
}