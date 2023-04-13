using System;
using System.IO;
using Common;
using Microsoft.Data.Sqlite;

namespace Indexer
{
    class Program
    {
        static void Main(string[] args)
        {
            new App().Run();
            //new Renamer().Crawl(new DirectoryInfo(Config.DataSourcePath));
        }
    }
}