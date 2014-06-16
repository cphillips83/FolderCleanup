using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace FolderCleanup
{
    class Program
    {
        static int Main(string[] args)
        {
            if (args.Length == 0)
            {
                ShowUsage();
                return -1;
            }

            var folder = args[0];
            if (!System.IO.Directory.Exists(folder))
            {
                Console.WriteLine("Folder not found.");
                ShowUsage();
                return -1;
            }


            var total = Stopwatch.StartNew();

            Console.WriteLine("Retrieving files");
            var files = System.IO.Directory.EnumerateFiles(folder);
            var ts = TimeSpan.FromDays(30);
            var dt = DateTime.UtcNow - ts;

            var deleted = 0;
            var index = 0;
            var update = Stopwatch.StartNew();
            //Console.WriteLine("Found {0} files...\n", files);
            
            foreach (var file in files)
            {
                index++;

                var fi = new FileInfo(file);
                if (fi.LastWriteTimeUtc < dt)
                {
                    System.IO.File.Delete(file);
                    deleted++;
                }

                if (update.Elapsed.TotalSeconds > 0.1)
                {
                    update.Reset();
                    update.Start();
                    Console.Write("\rProcessing {0}/{1} ...                      ", deleted, index);
                }
            }

            Console.WriteLine("\nDeleted {0} in {1}.", deleted, total.Elapsed);

            return 0;
        }

        static void ShowUsage()
        {
            Console.WriteLine();
            Console.WriteLine("Deletes all files in a specific folder that are older than 30 days");
            Console.WriteLine("Usage:  <folder> - folder to cleanup");
        }
    }
}
