using System;
using System.Collections.Generic;
using System.Text;

internal class Program
{
    private static void Main(string[] args)
    {
        var input = Console.ReadLine();
        var inputLength = Console.ReadLine();
        Console.WriteLine(Urlify(input, Convert.ToInt32(inputLength)));
    }

    private static string Urlify(string input, int inputLength)
    {
        var output = new StringBuilder();
        var iteratedLength = 0;

        foreach(var inputChar in input)
        {
            iteratedLength += 1;

            if (inputChar == ' ')
                output.Append("%20");
            else
                output.Append(inputChar);
            
            if (iteratedLength == inputLength)
                break;
        }

        return output.ToString();
    }
}