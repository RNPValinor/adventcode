using System.Linq;
using System.Text;

namespace _2018.Utils
{
    public class StringUtils
    {
        public static string UpperFirst(string s)
        {
            var builder = new StringBuilder(s.First().ToString().ToUpper());

            builder.Append(s.Substring(1));

            return builder.ToString();
        }
    }
}