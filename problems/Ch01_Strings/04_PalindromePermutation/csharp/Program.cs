using System;
using System.Collections.Generic;
using System.Text;

internal class Program
{
    private static void Main(string[] args)
    {
        var input = Console.ReadLine();
        Console.WriteLine(IsPalindromePermutation(input));
    }

    private static string IsPalindromePermutation(string input)
    {
        var chars = input.ToCharArray();
        var charsCounter = new Dictionary<char, int>();
        
        foreach(var c in chars)
        {
            if (c == ' ')
            {
                continue;
            }

            var lowerC = char.ToLower(c);
            
            if (charsCounter.ContainsKey(lowerC))
            {
                charsCounter[lowerC] += 1;
            }
            else
            {
                charsCounter.Add(lowerC, 1);
            }
        }

        var noOfOddCountChars = 0;

        foreach(var kvp in charsCounter)
        {
            if (kvp.Value%2 == 1)
            {
                noOfOddCountChars += 1;
            }

            if (noOfOddCountChars > 1)
            {
                return false.ToString().ToLower();
            }
        }

        return true.ToString().ToLower();
    }
}