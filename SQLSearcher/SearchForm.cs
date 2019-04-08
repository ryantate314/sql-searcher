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

        public SearchForm()
        {
            InitializeComponent();
            _cacheLoader = new CacheLoader();

            NewRepo().Wait();
        }

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
            try
            {
                SearchResultViewModel result = await Task.Run(() => search.Execute(_repo));
                RenderResults(result);
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
                    var item = new ListViewItem(new[] { table.Database, table.Schema, table.Table, table.MatchReason });
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

        private static string GetConnectionString(string server)
        {
            return $"Data Source={server};Integrated Security=True;Pooling=False";
        }

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

        private void viewProcTextMenuStripOption_Click(object sender, EventArgs e)
        {
            var selected = procedureSearchResults.SelectedItems.OfType<ListViewItem>().SingleOrDefault();
            if (selected != null)
            {
                var searchResult = selected.Tag as StoredProcedureResult;
                string text = _repo.GetProcedureText(searchResult.Database, searchResult.Schema, searchResult.Name);
                string fileName = TempFileRepo.CreateNewFile(text);
                //TempFileRepo.StartSSMS(serverName.Text, searchResult.Database, fileName);
                TempFileRepo.StartNPP(fileName);
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
    }
}
