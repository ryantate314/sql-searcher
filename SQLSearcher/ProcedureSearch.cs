using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLSearcher.Models;

namespace SQLSearcher
{
    class ProcedureSearch : Search
    {
        private string _database;
        private string _schema;
        private string _procedure;

        public ProcedureSearch(string database, string schema, string procedure) : base()
        {
            _database = RemoveWildcard(database);
            _schema = RemoveWildcard(schema);
            _procedure = RemoveWildcard(procedure);
        }


        public override SearchResultViewModel Execute(SchemaRepository repo)
        {
            var result = new SearchResultViewModel();
            if (DatabaseExists(_database, repo))
            {
                result.ProcedureResults = repo.SearchProcedures(_database, _schema, _procedure);
            }
            return result;
        }
    }
}
