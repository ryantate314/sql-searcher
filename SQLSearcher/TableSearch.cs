using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLSearcher.Models;

namespace SQLSearcher
{
    class TableSearch : Search
    {
        private string _database;
        private string _schema;
        private string _tableName;

        public TableSearch(string database, string schema, string tableName): base()
        {
            _database = RemoveWildcard(database);
            _schema = RemoveWildcard(schema);
            _tableName = RemoveWildcard(tableName);
        }


        public override SearchResultViewModel Execute(SchemaRepository repo)
        {
            if (!DatabaseExists(_database, repo)) { return new SearchResultViewModel(); }
            return repo.Search(_database, null, _tableName, null, _schema);
        }
    }
}
