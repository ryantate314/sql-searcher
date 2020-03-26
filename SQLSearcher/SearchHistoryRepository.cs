using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SQLSearcher
{
    partial class SearchHistoryRepository
    {
        private const string FILENAME = "search-history.json";
        private const int BUFFER_SIZE = 5;

        private List<SearchInputs> _searchHistory;

        private int _currentBuffer;

        private Task<List<SearchInputs>> _loadHistoryTask;
        private BetterSemaphore _saveHistorySemoaphore; //Locks saving history to a file
        private BetterSemaphore _modifyHistorySemaphore; //Locks access for adding records to the stack

        public SearchHistoryRepository()
        {
            _currentBuffer = 0;
            _saveHistorySemoaphore = new BetterSemaphore(1, 1);
            _modifyHistorySemaphore = new BetterSemaphore(1, 1);
        }

        public async Task<List<SearchInputs>> GetSearchHistory()
        {
            if (_searchHistory == null)
            {
                try
                {
                    //Check if a file read is already in progress
                    if (_loadHistoryTask != null)
                    {
                        await _loadHistoryTask;
                        _loadHistoryTask = null;
                    }

                    //Check if still null, because the previous task completing should have set the value
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

        /// <summary>
        /// Add the provided search to the top of the history stack. This is a partially-async
        /// method. The response can be discarded unless the calling code needs to ensure all
        /// file writes are complete.
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public Task AddSearch(SearchInputs search)
        {
            using (_modifyHistorySemaphore.Lock())
            {
                _searchHistory.Add(search);
                _currentBuffer++;
            }

            //Flush the buffer to the history file
            if (_currentBuffer > BUFFER_SIZE)
            {
                return WriteSearchHistory(() => _currentBuffer > BUFFER_SIZE);
            }
            return Task.CompletedTask;
        }

        /// <summary>
        /// Write the current search history to disk.
        /// </summary>
        /// <param name="proceed">Determine if the action still needs to be performed after mutex is achieved. Cannot modify the history log.</param>
        /// <returns></returns>
        private async Task WriteSearchHistory(Func<bool> proceed = null)
        {
            //Wait for any existing writes to complete.
            using (await _saveHistorySemoaphore.LockAsync())
            {
                bool goAhead = false;
                string json = "";
                int currentBuffer = 0;

                //Place a lock on adding any more searches until we mark down the buffer size and serialize the data
                using (await _modifyHistorySemaphore.LockAsync())
                {
                    currentBuffer = _currentBuffer;
                    
                    //Determine if conditions are still good to go while we have a lock on the search history
                    goAhead = proceed == null || proceed.Invoke();

                    if (goAhead)
                    {
                        json = JsonConvert.SerializeObject(_searchHistory);
                    }
                }

                //If conditions are still good
                if (goAhead)
                {
                    await Task.Run(() => File.WriteAllText(FILENAME, json));

                    //Decrement buffer count. Not setting to 0 in case more searches were added while we were saving.
                    _currentBuffer -= currentBuffer;
                }
            }//End save history lock
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
