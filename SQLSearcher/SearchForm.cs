using SQLSearcher.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SQLSearcher
{
    public partial class SearchForm : Form
    {
        private SchemaRepository _repo;
        private CacheLoader _cacheLoader;

        private bool _searchDisabled;

        private class SearchInputs
        {
            public string Server { get; set; }
            public string Search { get; set; }
        }
        private List<SearchInputs> _searchHistory;
        private int _searchHistoryIndex;

        public SearchForm()
        {
            InitializeComponent();
            _cacheLoader = new CacheLoader();
            _searchHistory = new List<SearchInputs>();
            _searchHistoryIndex = -1;

            NewRepo().Wait();
        }

        /// <summary>
        /// Perform the search by pressing Enter or F5 inside of the search text box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void searchBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.F5)
            {
                e.Handled = e.SuppressKeyPress = true; //Prevent beep
                await PerformSearch();
            }
        }

        private async void SearchForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                await PerformSearch();
            }
            else if (e.KeyCode == Keys.J && e.Control)
            {
                e.Handled = e.SuppressKeyPress = true; //Prevent beep
                searchResultsTabControl.SelectedTab = tableResultPage;
                searchBox.Focus();
            }
            else if (e.KeyCode == Keys.K && e.Control)
            {
                e.Handled = e.SuppressKeyPress = true; //Prevent beep
                searchResultsTabControl.SelectedTab = columnResultPage;
                searchBox.Focus();
            }
            else if (e.KeyCode == Keys.L && e.Control)
            {
                e.Handled = e.SuppressKeyPress = true; //Prevent beep
                searchResultsTabControl.SelectedTab = procedureResultPage;
                searchBox.Focus();
            }
            else if (e.KeyCode == Keys.Left && e.Alt)
            {
                PreviousSearch();
            }
            else if (e.KeyCode == Keys.Right && e.Alt)
            {
                NextSearch();
            }
        }

        private async Task PerformSearch()
        {
            if (_searchDisabled)
            {
                return;
            }

            if (_repo.ConnectionString != GetConnectionString(serverName.Text))
            {
                bool serverNameCorrect = await NewRepo();
                if (!serverNameCorrect)
                {
                    return;
                }
            }
            Search search = SearchParser.Parse(searchBox.Text);
            if (search == null)
            {
                MessageBox.Show("Invalid Search Terms");
                return;
            }

            Lock("Searching...");

            AddSearchToHistory(serverName.Text, searchBox.Text);

            try
            {
                SearchResultViewModel result = await Task.Run(() => search.Execute(_repo));
                RenderResults(result);
                RenderFrequentServers();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error performing query: " + ex.Message, "Query Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Unlock();
            }
        }

        private void RenderResults(SearchResultViewModel model)
        {
            RenderTableResults(model.TableResults);
            RenderColumnResults(model.ColumnResults);
            RenderProcedureResults(model.ProcedureResults);

            TabPage firstTabWithResults = null;
            if (model.TableResults.Count() > 0)
            {
                firstTabWithResults = tableResultPage;
            }
            else if (model.ColumnResults.Count() > 0)
            {
                firstTabWithResults = columnResultPage;
            }
            else if (model.ProcedureResults.Count() > 0)
            {
                firstTabWithResults = procedureResultPage;
            }

            bool currentTabHasResults =
                (searchResultsTabControl.SelectedTab == tableResultPage && model.TableResults.Count() > 0)
                || (searchResultsTabControl.SelectedTab == columnResultPage && model.ColumnResults.Count() > 0)
                || (searchResultsTabControl.SelectedTab == procedureResultPage && model.ProcedureResults.Count() > 0);
            if (!currentTabHasResults && firstTabWithResults != null)
            {
                searchResultsTabControl.SelectedTab = firstTabWithResults;
            }
        }

        private void RenderTableResults(IEnumerable<TableSearchResult> results)
        {
            tableSearchResults.BeginUpdate();
            try
            {
                tableSearchResults.Items.Clear();
                foreach (var table in results)
                {
                    var item = new ListViewItem(new[] { table.Database, table.Schema, table.Table, table.Type.ToString(), table.MatchReason });
                    item.Tag = table;
                    tableSearchResults.Items.Add(item);
                }
            }
            finally
            {
                tableSearchResults.EndUpdate();
            }
        }

        private void RenderColumnResults(IEnumerable<ColumnSearchResult> results)
        {
            columnSearchResults.BeginUpdate();
            try
            {
                columnSearchResults.Items.Clear();
                foreach (var column in results)
                {
                    var item = new ListViewItem(new[] { column.Database, column.Schema, column.Table, column.Column, column.DisplayType, column.FKString, column.Reason });
                    item.UseItemStyleForSubItems = false;//Allow sub-item override
                    item.Tag = column;
                    if (column.IsPrimaryKey)
                    {
                        item.SubItems[3].Font = new Font(item.SubItems[3].Font, FontStyle.Bold);
                    }
                    columnSearchResults.Items.Add(item);
                }
            }
            finally
            {
                columnSearchResults.EndUpdate();
            }
        }

        private void RenderProcedureResults(IEnumerable<StoredProcedureResult> results)
        {
            procedureSearchResults.BeginUpdate();
            try
            {
                procedureSearchResults.Items.Clear();
                foreach (var procedure in results)
                {
                    var item = new ListViewItem(new[] { procedure.Database, procedure.Schema, procedure.Name, procedure.DateCreated.ToString(), procedure.DateModified.ToString() });
                    item.Tag = procedure;
                    procedureSearchResults.Items.Add(item);
                }
            }
            finally
            {
                procedureSearchResults.EndUpdate();
            }

        }

        /// <summary>
        /// Generate a SQL connection string based on the provider server hostname/instance.
        /// </summary>
        /// <param name="server">The server name in the format {hostname}[\instance]</param>
        /// <returns></returns>
        private static string GetConnectionString(string server)
        {
            return $"Data Source={server};Integrated Security=True;Pooling=False";
        }

        /// <summary>
        /// Generate and perform a search for the currently selected table's column list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ViewColumnsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var selected = tableSearchResults.SelectedItems.OfType<ListViewItem>().SingleOrDefault();
            if (selected != null)
            {
                var searchResult = selected.Tag as TableSearchResult;
                searchBox.Text = SearchParser.GenerateSearch(searchResult.Database, searchResult.Schema, searchResult.Table, null, null);
                await PerformSearch();
            }
        }

        /// <summary>
        /// Connect to a new database.
        /// </summary>
        /// <returns></returns>
        private async Task<bool> NewRepo()
        {
            bool success = false;
            _repo = new SchemaRepository(GetConnectionString(serverName.Text));
            if (!String.IsNullOrWhiteSpace(serverName.Text))
            {
                Lock("Fetching database names...");
                //Cache database names
                try
                {
                    await Task.Run(() => _cacheLoader.CacheDatabases(_repo));
                    success = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error querying database list: " + ex.Message, "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    success = false;
                }
                finally
                {
                    Unlock();
                }
            }
            return success;
        }

        /// <summary>
        /// Server name text box on-blur event. Update the database connection on change.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ServerName_Leave(object sender, EventArgs e)
        {
            if (_repo.ConnectionString != GetConnectionString(serverName.Text) && !_searchDisabled)
            {
                await NewRepo();
            }
        }

        private async void SearchButton_Click(object sender, EventArgs e)
        {
            await PerformSearch();
        }

        /// <summary>
        /// View procedure definition.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void viewProcTextMenuStripOption_Click(object sender, EventArgs e)
        {
            var selected = procedureSearchResults.SelectedItems.OfType<ListViewItem>().SingleOrDefault();
            if (selected != null)
            {
                var searchResult = selected.Tag as StoredProcedureResult;
                ShowHelpText(searchResult.Database, searchResult.Schema, searchResult.Name);
            }
        }

        /// <summary>
        /// Show view create script.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void showCreateScriptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var selected = tableSearchResults.SelectedItems.OfType<ListViewItem>().SingleOrDefault();
            if (selected != null)
            {
                var searchResult = selected.Tag as TableSearchResult;
                ShowHelpText(searchResult.Database, searchResult.Schema, searchResult.Table);
            }
        }

        /// <summary>
        /// Generate the definition of the provided object and open it in a text editor.
        /// </summary>
        /// <param name="database"></param>
        /// <param name="schema"></param>
        /// <param name="obj"></param>
        private void ShowHelpText(string database, string schema, string obj)
        {
            string text = _repo.GetHelpText(database, schema, obj);
            try
            {
                string fileName = TempFileRepo.CreateNewFile(text, GenerateFileName(database, schema, obj));
                TempFileRepo.StartNPP(fileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error saving temp file", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Generate a temporary file name for the provided info.
        /// </summary>
        /// <param name="database"></param>
        /// <param name="schema"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private string GenerateFileName(string database, string schema, string name)
        {
            string sanitizedServerName = serverName.Text.Replace("\\", "-");
            return $"{sanitizedServerName}.{database}.{schema}.{name}.sql";
        }

        /// <summary>
        /// Custom event handle to display context menu. Needed to disable certain menu items which depend on the type of row.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tableSearchResults_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                //Get selected item and check which type it is
                var selected = tableSearchResults.SelectedItems.OfType<ListViewItem>().SingleOrDefault();
                if (selected != null)
                {
                    var searchResult = (TableSearchResult)selected.Tag;
                    if (searchResult.Type == TableType.Table)
                    {
                        showViewCreateScriptMenuItem.Enabled = false;
                    }
                    else
                    {
                        showViewCreateScriptMenuItem.Enabled = true;
                    }
                    tableResultContextMenu.Show(tableSearchResults, e.X, e.Y);
                }
            }
        }

        private void Lock(string message)
        {
            statusLabel.Text = message;
            _searchDisabled = true;
            searchButton.Enabled = false;
        }

        private void Unlock()
        {
            statusLabel.Text = "";
            _searchDisabled = false;
            searchButton.Enabled = true;
        }

        private void PreviousSearch()
        {
            if (_searchHistoryIndex > 0)
            {
                _searchHistoryIndex--;
                searchBox.Text = _searchHistory[_searchHistoryIndex].Search;
                serverName.Text = _searchHistory[_searchHistoryIndex].Server;
                forwardButton.Enabled = true;
            }
            
            if (_searchHistoryIndex == 0)
            {
                backButton.Enabled = false;
            }

            searchBox.Focus();
        }

        private void NextSearch()
        {
            if (_searchHistoryIndex < _searchHistory.Count - 1)
            {
                _searchHistoryIndex++;
                searchBox.Text = _searchHistory[_searchHistoryIndex].Search;
                serverName.Text = _searchHistory[_searchHistoryIndex].Server;
                forwardButton.Enabled = true;
                backButton.Enabled = true;
            }
            else if (_searchHistoryIndex == _searchHistory.Count - 1)
            {
                //Top of the stack. Clear the text box
                _searchHistoryIndex++;
                searchBox.Text = "";
                forwardButton.Enabled = false;
                backButton.Enabled = true;
            }
            searchBox.Focus();
        }

        private void AddSearchToHistory(string server, string searchTerm)
        {
            //Add search to top of history stack
            _searchHistory.Add(new SearchInputs()
            {
                Server = server,
                Search = searchTerm
            });
            _searchHistoryIndex = _searchHistory.Count - 1;
            forwardButton.Enabled = false;
            backButton.Enabled = true;
        }

        private void backButton_Click(object sender, EventArgs e)
        {
            PreviousSearch();
        }

        private void forwardButton_Click(object sender, EventArgs e)
        {
            NextSearch();
        }

        private void RenderFrequentServers()
        {
            var counts = _searchHistory.Select(x => x.Server)
                .GroupBy(x => x)
                .ToDictionary(x => x.Key, x => x.Count());

            var top = counts.OrderBy(x => x.Value)
                .Select(x => x.Key)
                .Take(4);

            frequentServerPanel.Controls.Clear();
            foreach (string server in top)
            {
                var button = new Button();
                button.Text = server;
                button.Tag = server;
                button.Click += (sender, e) =>
                {
                    serverName.Text = (string)button.Tag;
                };
                frequentServerPanel.Controls.Add(button);
            }
        }
    }
}
