using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCommandLine
{
    public static class Util
    {

        public static IEnumerable<string> JoinQuotedStringSegments(IEnumerable<string> strEnumerable)
        {
            var output = new List<string>();
            var queue = new Queue<string>(strEnumerable);

            var currentString = new List<string>();

            while (queue.Count > 0)
            {

                var s = queue.Dequeue();
                var building = currentString.Count > 0;
                if (building)
                    if (s.EndsWith("\""))
                    {
                        currentString.Add(s.Substring(0, s.Length - 1));
                        output.Add(string.Join(" ", currentString));
                        currentString.Clear();
                    }
                    else
                        currentString.Add(s);
                else
                    if (s.StartsWith("\""))
                    {
                        if (s.EndsWith("\""))
                            output.Add(s.Substring(1, s.Length - 2));
                        else
                            currentString.Add(s.Substring(1));
                    }
                    else
                        output.Add(s);

            }

            if (currentString.Count > 0)
            {
                output.Add("\"" + string.Join(" ", currentString));
            }

            return output;
        }

        public static string BumpyCaseToHyphenate(string s)
        {
            var sb = new StringBuilder();
            var remainingLetters = new Queue<char>(s);

            while (remainingLetters.Count > 0)
            {

                var ucWord = remainingLetters.DequeueStringWhile(char.IsUpper).ToLower();

                if (ucWord.Length > 0)
                {
                    if (sb.Length > 0) //Ignore first char which is always capitalized by convention
                        sb.Append('-');

                    //ucWord can be ..
                    if (ucWord.Length == 1)
                    {
                        //.. a simple separator (the V in "TestValue")
                        sb.Append(ucWord);
                    }
                    else
                    {
                        //.. or an initialism and can either be..
                        if (remainingLetters.Count == 0) // .. terminal (the HTTP in "UseHTTP")
                            sb.Append(ucWord);
                        else
                        {
                            sb.Append(ucWord.Substring(0, ucWord.Length - 1))//.. non-terminal (the SSLA in "SSLActive")
                                .Append('-').Append(ucWord.Last()); // which needs an extra hyphen between the L and A
                        }
                    }
                }

                sb.Append(remainingLetters.DequeueStringWhile(char.IsLower));
            }

            return sb.ToString();
        }

        static string DequeueStringWhile(this Queue<char> q, Func<char, bool> func)
        {
            return string.Concat(q.DequeueWhile(func));
        }

        public static IEnumerable<T> DequeueWhile<T>(this Queue<T> q, Func<T, bool> pred)
        {
            return q.TakeWhile(pred).ToArray().Select(s => {    q.Dequeue();
                                                                return s;
            });
        }

    }
}
