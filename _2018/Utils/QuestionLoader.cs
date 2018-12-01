using System.IO;

namespace _2018.Utils
{
    public static class QuestionLoader
    {
        public static string Load(int day, bool partTwo = false)
        {
            var fileStream = new FileStream($"Days/Inputs/Day{day}{(partTwo ? "Part2" : "")}.txt", FileMode.Open);

            using (var reader = new StreamReader(fileStream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}