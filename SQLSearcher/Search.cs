using SQLSearcher.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLSearcher
{
    abstract class Search
    {
        public abstract SearchResultViewModel Execute(SchemaRepository repo);

        protected bool DatabaseExists(string database, SchemaRepository repo)
        {
            return repo.GetDatabases().Contains(database, StringComparer.CurrentCultureIgnoreCase);
        }

        protected static string RemoveWildcard(string s)
        {
            if (s == null)
            {
                return null;
            }
            return s.Replace("*", "");
        }
    }
}
