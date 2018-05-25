using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLSearcher.Models;

namespace SQLSearcher
{
    class MultiSearch : Search
    {
        private List<Search> _searches;

        public MultiSearch()
        {
            _searches = new List<Search>();
        }

        public void AddSearch(Search search)
        {
            _searches.Add(search);
        }

        public override SearchResultViewModel Execute(SchemaRepository repo)
        {
            SearchResultViewModel result = SearchResultViewModel.Empty;
            List<Task<SearchResultViewModel>> tasks = new List<Task<SearchResultViewModel>>(_searches.Count);
            foreach (var search in _searches)
            {
                tasks.Add(Task.Run(() => search.Execute(repo)));
            }

            Task.WaitAll(tasks.ToArray());

            foreach (var task in tasks)
            {
                result = result.Merge(task.Result);
            }

            return result;
        }

    }
}
