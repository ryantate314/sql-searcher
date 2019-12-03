using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLSearcher.Models;

namespace SQLSearcher
{
    class FullSearch : Search
    {
        private string _database;
        private string _schema;
        private string _table;
        private string _columnSearch;
        private string _procedureSearch;

        public FullSearch(string database, string schema, string table, string columnSearch, string procedureSearch)
        {
            _database = RemoveWildcard(database);
            _schema = RemoveWildcard(schema);
            _table = RemoveWildcard(table);
            _columnSearch = RemoveWildcard(columnSearch);
            _procedureSearch = RemoveWildcard(procedureSearch);
        }

        public override SearchResultViewModel Execute(SchemaRepository repo)
        {
            return repo.Search(_database, _columnSearch, _table, _procedureSearch, _schema);
        }
    }
}
