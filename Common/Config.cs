namespace Common
{
    public static class Config
    {
        public static string DatabasePath { get; } = "/data/database.db";
        public static string DataSourcePath { get; } = "/data/maildir";
        public static int NumberOfFoldersToIndex { get; } = 10; // Use 0 or less for indexing all folders
    }
}