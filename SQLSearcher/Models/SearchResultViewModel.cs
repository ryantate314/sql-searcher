using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLSearcher.Models
{
    class SearchResultViewModel
    {
        public IEnumerable<TableSearchResult> TableResults { get; set; }
        public IEnumerable<ColumnSearchResult> ColumnResults { get; set; }
        public IEnumerable<StoredProcedureResult> ProcedureResults { get; set; }

        public SearchResultViewModel()
        {
            TableResults = new List<TableSearchResult>();
            ColumnResults = new List<ColumnSearchResult>();
            ProcedureResults = new List<StoredProcedureResult>();
        }

        public SearchResultViewModel Merge(SearchResultViewModel other)
        {
            var result = new SearchResultViewModel();

            var tableResults = new List<TableSearchResult>(TableResults.Count() + other.TableResults.Count());
            tableResults.AddRange(TableResults);
            tableResults.AddRange(other.TableResults);
            result.TableResults = tableResults;

            var columnResults = new List<ColumnSearchResult>();
            columnResults.AddRange(ColumnResults);
            columnResults.AddRange(other.ColumnResults);
            result.ColumnResults = columnResults;

            var procedureResults = new List<StoredProcedureResult>();
            procedureResults.AddRange(procedureResults);
            procedureResults.AddRange(other.ProcedureResults);
            result.ProcedureResults = procedureResults;

            return result;
        }

        private readonly static SearchResultViewModel _empty = new SearchResultViewModel();
        public static SearchResultViewModel Empty
        {
            get
            {
                return _empty;
            }
        }

    }
}
