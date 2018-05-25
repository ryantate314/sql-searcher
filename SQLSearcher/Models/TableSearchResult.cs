using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLSearcher.Models
{
    class TableSearchResult
    {
        public string Table { get; set; }
        public string Schema { get; set; }
        public string Database { get; set; }
        public string MatchReason { get; set; }
        public string DisplayName
        {
            get
            {
                return String.Format("{0}.{1}", Schema, Table);
            }
        }
        public string QualifiedName
        {
            get
            {
                return String.Format("[{0}].[{1}].[{2}]", Database, Schema, Table);
            }
        }
    }
}