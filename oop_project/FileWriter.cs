using System;
using System.Collections.Generic;
using System.IO;

namespace oop_project
{
    public static class FileWriter
    {
        public static void WriteLines(string path, IEnumerable<string> lines)
        {
            File.WriteAllLines(path, lines);
        }

        public static IEnumerable<string> ReadLines(string path)
        {
            if (!File.Exists(path))
                return Array.Empty<string>();
            return File.ReadLines(path);
        }

        public static void WriteString(string path, string content)
        {
            File.WriteAllText(path, content);
        }

        public static string ReadString(string path)
        {
            if (!File.Exists(path))
                return string.Empty;
            return File.ReadAllText(path);
        }
    }
} 