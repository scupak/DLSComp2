using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using Microsoft.Data.Sqlite;

namespace ConsoleSearch
{
    public class Database
    {
        private SqliteConnection _connection;
        public Database()
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder();
            connectionStringBuilder.DataSource = Config.DatabasePath;
            
            _connection = new SqliteConnection(connectionStringBuilder.ConnectionString);
            _connection.Open();
        }

        private async Task Execute(string sql)
        {
            var cmd = _connection.CreateCommand();
            cmd.CommandText = sql;
           await cmd.ExecuteNonQueryAsync();
        }

        // key is the id of the document, the value is number of search words in the document
        public async Task<List<KeyValuePair<int, int>>> GetDocuments(List<int> wordIds)
        {
            var res = new List<KeyValuePair<int, int>>();

            var sql = "SELECT docId, COUNT(wordId) as count FROM Occ where ";
            sql += "wordId in " + AsString(wordIds) + " GROUP BY docId ";
            sql += "ORDER BY count DESC;";

            var selectCmd = _connection.CreateCommand();
            selectCmd.CommandText = sql;

            using (var reader =  await selectCmd.ExecuteReaderAsync())
            {
                while (reader.Read())
                {
                    var docId = reader.GetInt32(0);
                    var count = reader.GetInt32(1);
                   
                    res.Add(new KeyValuePair<int, int>(docId, count));
                }
            }

            return res;
        }

        private string AsString(List<int> x)
        {
            return string.Concat("(", string.Join(',', x.Select(i => i.ToString())), ")");
            string res = "(";

            for (int i = 0; i < x.Count - 1; i++)
                res += x[i] + ",";

            if (x.Count > 0)
                res += x[x.Count - 1];

            res += ")";

            return res;
        }
        /*
         * SELECT wordId, COUNT(docId) as count
FROM Occ
where wordId in (2,3)
GROUP BY wordId
ORDER BY COUNT(docId) DESC;
        */


        public async Task<Dictionary<string, int>> GetAllWords()
        {
            Dictionary<string, int> res = new Dictionary<string, int>();
      
            var selectCmd = _connection.CreateCommand();
            selectCmd.CommandText = "SELECT * FROM word";

            using (var reader = await selectCmd.ExecuteReaderAsync())
            {
                while (reader.Read())
                {
                    var id = reader.GetInt32(0);
                    var w = reader.GetString(1);
                    
                    res.Add(w, id);
                }
            }
            return res;
        }

        public async Task<List<string>> GetDocDetails(List<int> docIds)
        {
            List<string> res = new List<string>();

            var selectCmd = _connection.CreateCommand();
            selectCmd.CommandText = "SELECT * FROM document where id in " + AsString(docIds);

            using (var reader = await selectCmd.ExecuteReaderAsync())
            {
                while (reader.Read())
                {
                    var id = reader.GetInt32(0);
                    var url = reader.GetString(1);

                    res.Add(url);
                }
            }
            return res;
        }
    }
}
