using SQLSearcher.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLSearcher
{
    class SearchParser
    {
        private static string DefaultDatabase = "master";

        public static Search Parse(string search)
        {
            //database.schema.table.column
            //schema.table
            //table.column
            //database.*.*.column
            //database.schema.table
            //column
            //table
            //stored procedure

            MultiSearch result = new MultiSearch();

            string[] split = search.Split('.');
            switch (split.Length)
            {
                //case 0:
                //    return null;
                case 1:
                    //table
                    result.AddSearch(new TableSearch(DefaultDatabase, "*", split[0]));
                    //column
                    result.AddSearch(new ColumnSearch(DefaultDatabase, "*", "*", split[0]));
                    //stored procedure
                    result.AddSearch(new ProcedureSearch(DefaultDatabase, "*", split[0]));
                    break;
                case 2:
                    //schema.table
                    result.AddSearch(new TableSearch(DefaultDatabase, split[0], split[1]));
                    //table.column
                    result.AddSearch(new ColumnSearch(DefaultDatabase, "*", split[0], split[1]));
                    //schema.procedure
                    result.AddSearch(new ProcedureSearch(DefaultDatabase, split[0], split[1]));
                    break;
                case 3:
                    //database.schema.table
                    result.AddSearch(new TableSearch(split[0], split[1], split[2]));
                    //schema.table.column
                    result.AddSearch(new ColumnSearch(DefaultDatabase, split[0], split[1], split[2]));
                    //database.schema.procedure
                    result.AddSearch(new ProcedureSearch(split[0], split[1], split[2]));
                    break;
                case 4:
                    //database.schema.table.column
                    result.AddSearch(new ColumnSearch(split[0], split[1], split[2], split[3]));
                    break;
            }

            return result;
        }

        public static string GenerateSearch(string database, string schema, string table, string column, string procedure)
        {
            string search = "";

            if (!String.IsNullOrEmpty(column))
            {
                search = $"{database}.{schema}.{table}.{column}";
            }
            else if (!String.IsNullOrEmpty(procedure))
            {
                search = $"{database}.{schema}.{procedure}";
            }
            else
            {
                search = $"{database}.{schema}.{table}.*";
            }

            return search;
        }
    }
}
