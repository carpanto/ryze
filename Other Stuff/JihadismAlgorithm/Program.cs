// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs.cs" company="JihadAlgorithm">
//      Copyright (c) JihadAlgorithm. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace JihadAlgorithm
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.Clear();
            var rand = new Random();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Welcome to Jihadism Algorithm");
            Console.WriteLine("Please enter your name");
            Console.ForegroundColor = ConsoleColor.Magenta;
            var Name = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("======================");
            var jihadnum = rand.Next(100);
            Console.WriteLine(Name + " | You're " + jihadnum + "% Jihad");
            Console.WriteLine(Name + " | You're " + (100 - jihadnum) + "% Christian");

            var key = Console.ReadKey();
            if (key.Key != ConsoleKey.Escape)
                Main(null);
        }
    }
}