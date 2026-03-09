using System;
using System.Collections.Generic;

internal class Program
{
    private static void Main(string[] args)
    {
        var string1 = Console.ReadLine();
        var string2 = Console.ReadLine();

        string output = IsPermutation(string1, string2).ToString();
        Console.WriteLine(output.ToLower());
    }

    private static bool IsPermutation(string string1, string string2)
    {
        if (string1.Length != string2.Length)
            return false;

        var string1Chars = string1.ToCharArray().ToList();

        foreach(var character in string2)
        {
            var indexInString1 = string1Chars.IndexOf(character);
            if (indexInString1 > -1)
            {
                string1Chars.RemoveAt(indexInString1);
            }
        }

        if (string1Chars.Count > 0)
            return false;

        return true;
    }
}