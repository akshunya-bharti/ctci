using System;
using System.Collections.Generic;

internal class Program
{
    static List<char> charList = new List<char>();

    private static void Main(string[] args)
    {
        var line = Console.ReadLine();
        
        foreach (var character in line)
        {
            if (IsParsedAlready(character))
            {
                Console.WriteLine("false");
                return;
            }
        }

        Console.WriteLine("true");
    }

    private static bool IsParsedAlready(char c)
    {
        if (charList.Count != 0)
        {
            if (charList.Contains(c))
            {
                return true;
            }
        }

        charList.Add(c);
        return false;
    }
}