using System;
using System.Collections.Generic;
using System.Text;

namespace SayiTahmin
{
    public static class MyExtensions
    {
        static Random rnd = new Random();
        static List<int> spaces, chars;


        public static string RemoveWhitespace(this StringBuilder str) => str.ToString().RemoveWhitespace();
        public static string RemoveWhitespace(this string str)
        {
            return string.Join("", str.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
        }

        public static void FindSpaces(this StringBuilder str) => str.ToString().FindSpaces();
        public static void FindSpaces(this string str)
        {
            spaces = str.FindAllSpaces();
        }

        public static void FindChars(this StringBuilder str) => str.ToString().FindChars();
        public static void FindChars(this string str)
        {
            chars = str.FindAllChars();
        }
        public static int NextChar(this StringBuilder str)
        {
            return Next(chars);
        }
        public static int NextSpace(this StringBuilder str)
        {
            return Next(spaces);
        }

        public static int Next(List<int> list)
        {
            if (list.Count == 0)
                return -1;
            int i = rnd.Next(list.Count);
            int retval = list[i];
            list.RemoveAt(i);
            return retval;
        }

        public static List<int> FindAllSpaces(this string searched)
        {
            int startIndex = -1;
            List<int> result = new List<int>();
            // Search for all occurrences of the target.
            while (true)
            {
                startIndex = searched.IndexOf(
                    ' ', startIndex + 1,
                    searched.Length - startIndex - 1);

                // Exit the loop if the target is not found.
                if (startIndex < 0)
                    break;
                result.Add(startIndex);
            }
            return result;
        }
        public static List<int> FindAllChars(this string searched)
        {
            List<int> result = new List<int>();
            // Search for all occurrences of the target.
            for (int i = 0; i < searched.Length; ++i)
            {
                if (!Char.IsWhiteSpace(searched[i]))
                    result.Add(i);
            }
            return result;
        }
    }    
}
