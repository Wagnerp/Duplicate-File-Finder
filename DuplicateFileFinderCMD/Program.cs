using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using DuplicateFileFinderLib;

namespace DuplicateFileFinderCMD
{
    class Program
    {
        static void Main(string[] args)
        {
            string dir = "";
            if (args.Count() == 0)
            {
                Console.Write("Directory? ");
                dir = Console.ReadLine();
                if (dir.Equals(""))
                {
                    dir = Directory.GetCurrentDirectory();
                }
            }
            else if (args.Count() == 1)
            {
                dir = args[0];
            }
            else
            {
                Console.WriteLine("Too many arguments");
                Environment.Exit(-1);
            }
            if (!Directory.Exists(dir))
            {
                Console.WriteLine("Directory doesn't exist");
                Environment.Exit(-1);
            }
            string output = DuplicateFinder.FindDuplicates(dir);
            Console.WriteLine(output);
            Console.WriteLine();
            Console.Write("Save to file? (y/n)");
            //Console.ReadKey() doesn't work over putty
            //ConsoleKey ck = Console.ReadKey().Key;
            //if (true || ck == ConsoleKey.Y)
            if (true)
            {
                Console.Write("Filename? (dupes.txt) ");
                string filename = Console.ReadLine();
                if (filename.Equals("")) { filename = "dupes.txt"; }
                TextWriter tw = new StreamWriter(filename);
                // write a line of text to the file
                tw.WriteLine(output);
                // close the stream
                tw.Close();
            }
        }
    }
}
