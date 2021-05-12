using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace ChartLoader.NET.Utils
{
    /// <summary>
    /// Reads and manipulates external files.
    /// </summary>
    public class IO
    {
        /// <summary>
        /// Reads a string array from file.
        /// </summary>
        /// <param name="filePath">Provide the filepath at which to load your file.</param>
        /// <returns>string[]</returns>
        public static string[] ReadFile(string filePath)
        {
            string[] lines = new string[0];
            try
            {
                Console.WriteLine("Reading: " + filePath);
                lines = File.ReadAllLines(filePath);
                
            }
            finally
            {
                Console.WriteLine("Reading: " + lines.Length + " lines.");
            }
            return lines;
        }
    }
}
