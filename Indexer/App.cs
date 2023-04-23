using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Common;

namespace Indexer
{
    public class App
    {
        public void Run()
        {
            Database db = new Database();
            Crawler crawler = new Crawler(db);

            var directoryArray = new DirectoryInfo(Config.DataSourcePath).GetDirectories();
            var directories = new List<DirectoryInfo>(directoryArray).OrderBy(d => d.Name).AsEnumerable();
            if (Config.NumberOfFoldersToIndex > 0)
            {
                directories = directories.Take(Config.NumberOfFoldersToIndex);
            }

            DateTime start = DateTime.Now;
            foreach (var directory in directories)
            {
                crawler.IndexFilesIn(directory, new List<string> { ".txt"});
            }
            
            TimeSpan used = DateTime.Now - start;
            Console.WriteLine("DONE! used " + used.TotalMilliseconds);

            var all = db.GetAllWords();

            foreach (var p in all)
            {
                Console.WriteLine("<" + p.Key + ", " + p.Value + ">");
                break;
            }
        }
    }
}
