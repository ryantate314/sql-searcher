using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLSearcher
{
    partial class SearchHistoryRepository
    {
        private const string FILENAME = "search-history.json";
        private const int BUFFER_SIZE = 5;

        private List<SearchInputs> _searchHistory;

        private int _currentBuffer;

        private Task _addSearchTask;
        private Task<List<SearchInputs>> _loadHistoryTask;

        public SearchHistoryRepository()
        {
            _currentBuffer = 0;
        }

        public async Task<List<SearchInputs>> GetSearchHistory()
        {
            if (_searchHistory == null)
            {
                try
                {
                    if (_loadHistoryTask != null)
                    {
                        await _loadHistoryTask;
                        _loadHistoryTask = null;
                    }

                    //Check if null because the previous task completing should have set the list
                    if (_searchHistory == null)
                    {
                        _loadHistoryTask = ReadSearchFile();
                        _searchHistory = await _loadHistoryTask;
                    }
                }
                catch
                {
                    _searchHistory = new List<SearchInputs>();
                }
            }

            return _searchHistory;
        }

        public async Task AddSearch(SearchInputs search)
        {
            _searchHistory.Add(search);
            _currentBuffer++;

            if (_currentBuffer > BUFFER_SIZE)
            {
                //Wait for the existing write to complete.
                if (_addSearchTask != null)
                {
                    await _addSearchTask;
                    _addSearchTask = null;
                }
                _addSearchTask = WriteSearchHistory();
                _currentBuffer = 0;
                await _addSearchTask;
            }
        }

        private async Task WriteSearchHistory()
        {
            var json = JsonConvert.SerializeObject(_searchHistory);
            await Task.Run(() => File.WriteAllText(FILENAME, json));
        }

        private async Task<List<SearchInputs>> ReadSearchFile()
        {
            var list = new List<SearchInputs>();

            if (File.Exists(FILENAME))
            {
                string json = await Task.Run(() =>
                {
                    return File.ReadAllText(FILENAME);
                });
                list = JsonConvert.DeserializeObject<List<SearchInputs>>(json);
            }

            return list;
        }

        public async Task Flush()
        {
            await WriteSearchHistory();
        }

    }
}
