using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

/**
 * The while loop represents the game.
 * Each iteration represents a turn of the game
 * where you are given inputs (the heights of the mountains)
 * and where you have to print an output (the index of the mountain to fire on)
 * The inputs you are given are automatically updated according to your last actions.
 **/
class Player
{
    static void Main(string[] args)
    {
        // game loop
        while (true)
        {
            int indexToDestroy = -1;
            int mountToDestroySize = -1;

            for (int i = 0; i < 8; i++)
            {
                int mountainH = int.Parse(Console.ReadLine()); // represents the height of one mountain.
                Console.Error.WriteLine("Index: " + i + " Height: " + mountainH.ToString());

                if (mountainH > mountToDestroySize)
                {
                    indexToDestroy = i;
                    mountToDestroySize = mountainH;
                }
            }

            // Write an action using Console.WriteLine()
            // To debug: Console.Error.WriteLine("Debug messages...");

            Console.WriteLine(indexToDestroy); // The index of the mountain to fire on.
        }
    }
}