using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        // Get the directory where the executable is located
        string startPath = AppDomain.CurrentDomain.BaseDirectory;
        Console.WriteLine("Starting Path: " + startPath);

        List<string> lastFoldersWithFiles = FindLastFoldersWithFiles(startPath, 0, out int maxDepth);

        if (lastFoldersWithFiles.Count > 0)
        {
            Console.WriteLine("Last folders with files:");
            foreach (var folder in lastFoldersWithFiles)
            {
                Console.WriteLine(folder);
            }

            // Join the paths to write all of them to the file
            string content = string.Join(Environment.NewLine, lastFoldersWithFiles);
            CreateTextFile(startPath, "LastFoldersWithFiles.txt", content);
        }
        else
        {
            Console.WriteLine("No folder with files found.");
            CreateTextFile(startPath, "LastFoldersWithFiles.txt", "No folder with files found.");
        }

        Console.ReadLine(); // Keeps console window open
    }

    static List<string> FindLastFoldersWithFiles(string path, int depth, out int maxDepth)
    {
        List<string> foldersWithFiles = new List<string>();
        maxDepth = depth;

        foreach (string directory in Directory.GetDirectories(path))
        {
            int subMaxDepth;
            // Recursively find folders with files and track the maximum depth reached
            var subFoldersWithFiles = FindLastFoldersWithFiles(directory, depth + 1, out subMaxDepth);

            // Update maxDepth if a deeper level was reached in the current directory
            if (subMaxDepth > maxDepth)
            {
                maxDepth = subMaxDepth;
                foldersWithFiles = subFoldersWithFiles;
            }
            else if (subMaxDepth == maxDepth)
            {
                foldersWithFiles.AddRange(subFoldersWithFiles);
            }
        }

        // If there are files in the current folder (ignore subdirectories), consider it a candidate
        if (Directory.GetFiles(path).Length > 0)
        {
            if (depth == maxDepth)
            {
                foldersWithFiles.Add(path);
            }
            else if (depth > maxDepth)
            {
                maxDepth = depth;
                foldersWithFiles = new List<string> { path };
            }
        }

        return foldersWithFiles;
    }

    static void CreateTextFile(string directory, string fileName, string content)
    {
        // Combine directory and filename to create the file path
        string filePath = Path.Combine(directory, fileName);

        // Write the content to the text file
        File.WriteAllText(filePath, content);
        Console.WriteLine($"File '{fileName}' created at: {filePath}");
    }
}
