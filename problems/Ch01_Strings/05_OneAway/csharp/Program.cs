using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

internal class Program
{
    private static void Main(string[] args)
    {
        var input1 = Console.ReadLine();
        var input2 = Console.ReadLine();
        Console.WriteLine(IsOneOrZeroAway(input1, input2));
    }

    private static string IsOneOrZeroAway(string input1, string input2)
    {
        if(Math.Abs(input1.Length - input2.Length) > 1)
        {
            return false.ToString().ToLower();
        }
        else
        {
            var charArray1 = input1.ToCharArray();
            var charArray2 = input2.ToCharArray();

            if(input1.Length == input2.Length)
            {
                if (GetDiffForEqualLengthInputs(charArray1, charArray2) > 1)
                {
                    return false.ToString().ToLower();
                }
            }
            else
            {
               if (GetDiffForUnEqualLengthInputs(charArray1, charArray2) > 1)
                {
                    return false.ToString().ToLower();
                }
            }

            return true.ToString().ToLower();
        }
    }

    private static int GetDiffForEqualLengthInputs(char[] charArray1, char[] charArray2)
    {
        var diffs = 0;

        for (int i=0; i<charArray1.Length; i++)
        {
            if (charArray1[i] != charArray2[i])
            {
                diffs += 1;
            }

            if (diffs > 1)
            {
                return diffs;
            }
        }

        return diffs;
    }

    private static int GetDiffForUnEqualLengthInputs(char[] charArray1, char[] charArray2)
    {
        var diffs = 0;
        
        var largerArray = charArray1.Length > charArray2.Length ? charArray1 : charArray2;
        var smallerArray = charArray1.Length < charArray2.Length ? charArray1 : charArray2;

        for (int smallerArrayIndex=0, largerArrayIndex=0; 
            largerArrayIndex<smallerArray.Length; )
        {
            if (largerArray[largerArrayIndex] != smallerArray[smallerArrayIndex])
            {
                diffs += 1;

                if (diffs > 1)
                {
                    return diffs;
                }

                largerArrayIndex += 1;
                continue;
            }

            largerArrayIndex += 1;
            smallerArrayIndex += 1;
        }

        return diffs;
    }
}