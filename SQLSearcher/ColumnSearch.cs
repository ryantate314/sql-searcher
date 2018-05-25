using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLSearcher.Models;

namespace SQLSearcher
{
    class ColumnSearch : Search
    {
        private string _database;
        private string _schema;
        private string _table;
        private string _columnSearch;


        public ColumnSearch(string database, string schema, string table, string columnSearch)
        {
            _database = RemoveWildcard(database);
            _schema = RemoveWildcard(schema);
            _table = RemoveWildcard(table);
            _columnSearch = RemoveWildcard(columnSearch);
        }

        public override SearchResultViewModel Execute(SchemaRepository repo)
        {
            var result = new SearchResultViewModel();
            if (DatabaseExists(_database, repo))
            {
                result.ColumnResults = repo.SearchColumns(_database, _schema, _table, _columnSearch);
            }
            return result;
        }
    }
}
