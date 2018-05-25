using Dapper;
using SQLSearcher.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace SQLSearcher
{
    class SchemaRepository
    {
        private string _connectionString;
        public string ConnectionString
        {
            get
            {
                return _connectionString;
            }
        }

        public SchemaRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        private class ColumnResult
        {
            public string Schema { get; set; }
            public string Table { get; set; }
            public string Column { get; set; }
            public string Type { get; set; }
            public bool Nullable { get; set; }
            public int CharacterLength { get; set; }
            public bool IsNullable { get; set; }
            public bool IsPrimaryKey { get; set; }
            public string FK_Constraint { get; set; }
            public string PK_Column { get; set; }
            public string PK_Table { get; set; }
        }


        private class TableResult
        {
            public string Schema { get; set; }
            public string Table { get; set; }
        }

        private class ProcedureResult
        {
            public string Schema { get; set; }
            public string Name { get; set; }
            public DateTime DateCreated { get; set; }
            public DateTime DateModified { get; set; }
        }


        private IEnumerable<string> _databases;
        public IEnumerable<string> GetDatabases()
        {
            if (_databases == null)
            {
                using (var con = new SqlConnection(_connectionString))
                {
                    _databases = con.Query<string>("SELECT [name] FROM master.sys.databases ORDER BY [name];");
                }
            }
            return _databases;
        }

        public SearchResultViewModel Search(string database, string columnSearch, string tableSearch, string procedureSearch, string schemaSearch)
        {
            string query =
@"
USE [" + database + "];" + @"

SET NOCOUNT ON;

SELECT      
			  s.[name] AS [Schema]
			, t.[name] AS [Table]
			, c.[name]  AS [Column]
            , IC.data_type AS [Type]
			, CASE IC.is_nullable
				WHEN 'YES' THEN 1
				ELSE 0
			END AS Nullable
FROM        sys.columns c
JOIN        sys.tables  t   ON c.object_id = t.object_id
JOIN		sys.schemas s   ON t.schema_id = s.schema_id
JOIN		INFORMATION_SCHEMA.COLUMNS IC ON s.[name] = IC.table_schema AND t.[name] = IC.table_name AND c.[name] = IC.column_name
WHERE       c.name LIKE @columnSearch
        AND s.name LIKE @schemaSearch
        AND t.name LIKE @tableSearch
ORDER BY    [Schema], [Table], [Column];


SELECT      
			  s.[name] AS [Schema]
			, t.[name] AS [Table]
FROM        sys.tables t
JOIN		sys.schemas s   ON t.schema_id = s.schema_id
WHERE       t.[name] LIKE @tableSearch
        AND s.name LIKE @schemaSearch
ORDER BY    [Table];

SELECT 
	  s.[name] AS [Schema]
	, p.[name] AS [Procedure]
    --, m.[definition] AS [Definition]
FROM 
    sys.procedures p
    JOIN sys.schemas s ON p.schema_id = s.schema_id
    JOIN sys.sql_modules m ON p.object_id = m.object_id
WHERE 
        p.[name] LIKE @procedureSearch
    AND s.name LIKE @schemaSearch
ORDER BY [Schema], [Procedure];
";
            using (var con = new SqlConnection(_connectionString))
            {
                using (GridReader grid = con.QueryMultiple(query, new
                {
                    columnSearch = SurroundInPercent(columnSearch),
                    tableSearch = SurroundInPercent(tableSearch),
                    procedureSearch = SurroundInPercent(procedureSearch),
                    schemaSearch = SurroundInPercent(schemaSearch)
                }))
                {


                    var model = new SearchResultViewModel();
                    //Columns
                    model.ColumnResults = grid.Read<ColumnResult>()
                        .Select(x => new ColumnSearchResult()
                        {
                            Column = x.Column,
                            Database = database,
                            Reason = "Column name contains '" + columnSearch + "'.",
                            Schema = x.Schema,
                            Table = x.Table,
                            Type = x.Type
                        });

                    //Tables
                    model.TableResults = grid.Read<TableResult>()
                        .Select(x => new TableSearchResult()
                        {
                            Database = database,
                            MatchReason = "Table name contains '" + tableSearch + "'.",
                            Schema = x.Schema,
                            Table = x.Table
                        });

                    //Procedures


                    return model;
                }//End using GridReader
            }//End using SqlConnection
        }

        public IEnumerable<ColumnSearchResult> SearchColumns(string database, string schema, string table, string column)
        {
            string query =
@"
USE [" + database + "];" + @"

SET NOCOUNT ON;


SELECT 
	  IC.TABLE_SCHEMA [Schema]
	, IC.TABLE_NAME [Table]
	, IC.COLUMN_NAME [Column]
	, IC.DATA_TYPE Type
	, IC.CHARACTER_MAXIMUM_LENGTH CharacterLength
	, CASE IC.IS_NULLABLE
		WHEN 'YES' THEN 1
		ELSE 0
	END IsNullable
	, CASE
		WHEN PK.CONSTRAINT_NAME IS NULL THEN 0
		ELSE 1
	END AS IsPrimaryKey
	--FK Column will be shown if the column is also a primary key
	, CASE
		WHEN RC.CONSTRAINT_NAME IS NULL THEN NULL
		ELSE RC.CONSTRAINT_NAME
	END FK_Constraint
    , FPK.TABLE_NAME PK_Table
    , FPKU.COLUMN_NAME PK_Column
FROM
		   INFORMATION_SCHEMA.COLUMNS IC
LEFT JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE CU ON
	    CU.TABLE_SCHEMA = IC.TABLE_SCHEMA
	AND CU.TABLE_NAME = IC.TABLE_NAME
	AND CU.COLUMN_NAME = IC.COLUMN_NAME
LEFT JOIN INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS RC ON
		CU.CONSTRAINT_SCHEMA = RC.CONSTRAINT_SCHEMA
	AND CU.CONSTRAINT_NAME = RC.CONSTRAINT_NAME
LEFT JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS FK ON
		RC.CONSTRAINT_SCHEMA = FK.CONSTRAINT_SCHEMA
	AND RC.CONSTRAINT_NAME = FK.CONSTRAINT_NAME
	AND FK.CONSTRAINT_TYPE = 'Foreign Key'
LEFT JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS FPK ON
	    RC.UNIQUE_CONSTRAINT_SCHEMA = FPK.CONSTRAINT_SCHEMA
	AND RC.UNIQUE_CONSTRAINT_NAME = FPK.CONSTRAINT_NAME
	AND FPK.CONSTRAINT_TYPE = 'PRIMARY KEY'
--Foreign Key Referenced Column
LEFT JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE FPKU ON
	    FPK.CONSTRAINT_NAME = FPKU.CONSTRAINT_NAME
	AND FPK.CONSTRAINT_SCHEMA = FPKU.CONSTRAINT_SCHEMA
--Primary Key
LEFT JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS PK ON
	    CU.TABLE_SCHEMA = PK.TABLE_SCHEMA
	AND CU.TABLE_NAME = PK.TABLE_NAME
	AND PK.CONSTRAINT_TYPE = 'Primary Key'
LEFT JOIN INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE PKU ON
		PK.CONSTRAINT_SCHEMA = PKU.CONSTRAINT_SCHEMA
	AND PK.CONSTRAINT_NAME = PKU.CONSTRAINT_NAME
WHERE
	--If Primary Key is null, then Primary Key Column must also be null (def of implication)
	    NOT (PK.CONSTRAINT_NAME IS NULL AND PKU.CONSTRAINT_NAME IS NOT NULL)
    AND IC.COLUMN_NAME LIKE @column
    AND IC.TABLE_SCHEMA LIKE @schema
    AND IC.TABLE_NAME LIKE @table
ORDER BY IC.TABLE_SCHEMA, IC.TABLE_NAME, IC.ORDINAL_POSITION
;";
            using (var con = new SqlConnection(_connectionString))
            {
                IEnumerable<ColumnResult> result = con.Query<ColumnResult>(query, new
                {
                    column = SurroundInPercent(column),
                    schema = SurroundInPercent(schema),
                    table = SurroundInPercent(table)
                });
                return result.Select(x => new ColumnSearchResult()
                {
                    Database = database,
                    Column = x.Column,
                    Reason = "Column name contains '" + column + "'.",
                    Schema = x.Schema,
                    Table = x.Table,
                    Type = x.Type,
                    CharacterLength = x.CharacterLength,
                    IsNullable = x.IsNullable,
                    IsPrimaryKey = x.IsPrimaryKey,
                    FK_Constraint = x.FK_Constraint,
                    PK_Column = x.PK_Column,
                    PK_Table = x.PK_Table
                });
            }
        }

        public IEnumerable<StoredProcedureResult> SearchProcedures(string database, string schema, string procedure)
        {
            string query = @"
USE [" + database + @"];

SELECT
      s.[name] AS [Schema]
    , p.[name] AS [Name]
    , o.create_date DateCreated
    , o.modify_date DateModified
FROM
    sys.procedures p
    JOIN sys.schemas s ON p.schema_id = s.schema_id
    JOIN sys.objects o ON p.object_id = o.object_id
WHERE
        s.[name] LIKE @schema
    AND p.[name] LIKE @procedure;
";
            using (var con = new SqlConnection(_connectionString))
            {
                return con.Query<ProcedureResult>(query, new
                {
                    schema = SurroundInPercent(schema),
                    procedure = SurroundInPercent(procedure)
                }).Select(x => new StoredProcedureResult()
                {
                    Database = database,
                    DateCreated = x.DateCreated,
                    DateModified = x.DateModified,
                    Name = x.Name,
                    Schema = x.Schema
                });
            }
        }

        public string GetProcedureText(string database, string schema, string procedure)
        {
            string query =
$"USE [{database}]; EXEC sp_helptext N'{database}.{schema}.{procedure}';";
            using (var con = new SqlConnection(_connectionString))
            {
                var lines = con.Query<string>(query);
                return String.Join("", lines);
            }
        }

        private string SurroundInPercent(string source)
        {
            string result = "%";
            if (source == null)
            {
                //Keep search term NULL to prevent matching ALL fields
                result = null;
            }
            else if (source.Length > 0)
            {
                result = $"%{source}%";
            }
            return result;
        }
    }
}
