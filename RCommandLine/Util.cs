using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RCommandLine
{
    using System.Text.RegularExpressions;

    public static class Util
    {

        const string Quote = "\"";

        public static IEnumerable<string> JoinQuotedStringSegments(IEnumerable<string> strEnumerable)
        {
            Queue<bool> stringQuoteInfo;
            return JoinQuotedStringSegments(strEnumerable, out stringQuoteInfo);
        }

        public static IEnumerable<string> JoinQuotedStringSegments(IEnumerable<string> strEnumerable, out Queue<bool> stringQuoteInfo)
        {
            var output = new List<string>();
            stringQuoteInfo = new Queue<bool>();
            var queue = new Queue<string>(strEnumerable);

            var currentString = new List<string>();

            while (queue.Count > 0)
            {

                var s = queue.Dequeue();
                var building = currentString.Count > 0;
                if (building)
                    if (s.EndsWith(Quote))
                    {
                        currentString.Add(s.Substring(0, s.Length - 1));
                        output.Add(string.Join(" ", currentString));
                        stringQuoteInfo.Enqueue(true);
                        currentString.Clear();
                    }
                    else
                        currentString.Add(s);
                else if (s.StartsWith(Quote))
                {
                    if (s.EndsWith(Quote))
                    {
                        output.Add(s.Substring(1, s.Length - 2));
                        stringQuoteInfo.Enqueue(true);
                    }
                    else
                        currentString.Add(s.Substring(1));
                }
                else
                {
                    output.Add(s);
                    stringQuoteInfo.Enqueue(false);
                }

            }

            if (currentString.Count > 0)
            {
                output.Add(Quote + string.Join(" ", currentString));
                stringQuoteInfo.Enqueue(false);
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

        public static string JoinNotNulls(string separator, IEnumerable<string> str)
        {
            return string.Join(separator, str.Where((s => s != null)));
        }

        public static string RegexForAnyLiteral(IEnumerable<string> literals)
        {
            return string.Format("({0})", string.Join("|", literals.Select(Regex.Escape)));
        }

    }
}
