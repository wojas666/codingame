using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
class Solution
{
    static void Main(string[] args)
    {
        int n = int.Parse(Console.ReadLine()); // the number of temperatures to analyse
        string temps = Console.ReadLine(); // the n temperatures expressed as integers ranging from -273 to 5526
        string[] t = temps.Split(' ');
        int[] values = new int[n];

        for (int i = 0; i < t.Length; i++)
        {
            if (n > 0)
            {
                int.TryParse(t[i], out values[i]);
            }
        }

        Temperatures temperatures = new Temperatures(values);

        Console.WriteLine(temperatures.GetTheClosestValue(0).ToString());
    }
}

public class Temperatures
{
    public int[] Values { get; private set; }


    public Temperatures(int[] values)
    {
        Values = values;
    }

    public int GetTheClosestValue(int expected)
    {
        int index = -1;
        int bestDifference = int.MaxValue;

        if (Values.Length > 0)
        {
            for (int i = 0; i < Values.Length; i++)
            {
                int difference = int.MaxValue;

                if (Values[i] > 0)
                    difference = expected + Values[i];
                else
                    difference = expected - Values[i];

                if (index != -1)
                {
                    if (bestDifference > difference)
                    {
                        bestDifference = difference;
                        index = i;
                    }
                    if (bestDifference == difference)
                    {
                        if (Values[i] > Values[index])
                        {
                            bestDifference = difference;
                            index = i;
                        }
                    }
                }
                else
                {
                    bestDifference = difference;
                    index = i;
                }
            }
        }
        else
            return expected;

        return Values[index];
    }
}