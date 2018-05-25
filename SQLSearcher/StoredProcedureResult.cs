using System;

namespace SQLSearcher
{
    class StoredProcedureResult
    {
        public string Database { get; set; }
        public string Schema { get; set; }
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
    }
}