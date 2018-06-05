using System.Collections.Generic;
using System.IO;

namespace CodeDomExtTests.TestClasses
{
    public static class StreamUtilities
    {
        public static StreamWriter GetStreamWriter()
        {
            return new StreamWriter("testsFile.txt") {AutoFlush = true};
        }

        public static IEnumerable<string> ReadStream()
        {
            IList<string> res = new List<string>();
            string prev;

            using (StreamReader sr = new StreamReader("testsFile.txt"))
            {
                do
                {
                    prev = sr.ReadLine();
                    res.Add(prev);
                } while (prev != null);
            }

            res.RemoveAt(res.Count - 1);
            return res;
        }
    }
}