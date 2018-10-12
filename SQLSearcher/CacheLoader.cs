using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SQLSearcher
{
    class CacheLoader : IDisposable
    {
        private TaskCompletionSource<int> _taskSource;
        private BackgroundWorker _worker;

        public Task Task
        {
            get
            {
                return _taskSource?.Task;
            }
        }


        public void CacheDatabases(SchemaRepository repo)
        {
            //if (_taskSource != null)
            //{
            //    _taskSource.TrySetCanceled();
            //}
            //_taskSource = new TaskCompletionSource<int>();
            //_worker = new BackgroundWorker();
            //_worker.DoWork += (o, a) =>
            //{
            //    repo.GetDatabases();
            //};
            //_worker.RunWorkerCompleted += (o, a) => {
            //    _taskSource.TrySetResult(0);
            //};
            //_worker.RunWorkerAsync();

            repo.GetDatabases();

            //return _taskSource.Task;
        }


        public void Dispose()
        {
            if (_worker != null)
            {
                _worker.Dispose();
            }
        }
    }
}
