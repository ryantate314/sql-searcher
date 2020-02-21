namespace SQLSearcher
{
    partial class SearchForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.ColumnHeader databaseColumn;
            System.Windows.Forms.ColumnHeader schemaColumn;
            System.Windows.Forms.ColumnHeader tableCoumn;
            System.Windows.Forms.ColumnHeader reasonColumn;
            System.Windows.Forms.ColumnHeader columnDatabaseColumn;
            this.serverName = new System.Windows.Forms.TextBox();
            this.searchBox = new System.Windows.Forms.TextBox();
            this.searchResultsTabControl = new System.Windows.Forms.TabControl();
            this.tableResultPage = new System.Windows.Forms.TabPage();
            this.tableSearchResults = new System.Windows.Forms.ListView();
            this.typeColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnResultPage = new System.Windows.Forms.TabPage();
            this.columnSearchResults = new System.Windows.Forms.ListView();
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.fkColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.procedureResultPage = new System.Windows.Forms.TabPage();
            this.procedureSearchResults = new System.Windows.Forms.ListView();
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader10 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.procResultContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.viewProcTextMenuStripOption = new System.Windows.Forms.ToolStripMenuItem();
            this.tableResultContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.viewColumnsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectTop1000ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showViewCreateScriptMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.searchButton = new System.Windows.Forms.Button();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.backButton = new System.Windows.Forms.Button();
            this.backToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.forwardButton = new System.Windows.Forms.Button();
            this.frequentServerPanel = new System.Windows.Forms.Panel();
            databaseColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            schemaColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            tableCoumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            reasonColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            columnDatabaseColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.searchResultsTabControl.SuspendLayout();
            this.tableResultPage.SuspendLayout();
            this.columnResultPage.SuspendLayout();
            this.procedureResultPage.SuspendLayout();
            this.procResultContextMenu.SuspendLayout();
            this.tableResultContextMenu.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // databaseColumn
            // 
            databaseColumn.Text = "Database";
            databaseColumn.Width = 70;
            // 
            // schemaColumn
            // 
            schemaColumn.Text = "Schema";
            schemaColumn.Width = 67;
            // 
            // tableCoumn
            // 
            tableCoumn.Text = "Table";
            tableCoumn.Width = 202;
            // 
            // reasonColumn
            // 
            reasonColumn.Text = "Reason";
            reasonColumn.Width = 406;
            // 
            // columnDatabaseColumn
            // 
            columnDatabaseColumn.Text = "Database";
            columnDatabaseColumn.Width = 70;
            // 
            // serverName
            // 
            this.serverName.Location = new System.Drawing.Point(12, 34);
            this.serverName.Name = "serverName";
            this.serverName.Size = new System.Drawing.Size(171, 20);
            this.serverName.TabIndex = 0;
            this.serverName.Leave += new System.EventHandler(this.ServerName_Leave);
            // 
            // searchBox
            // 
            this.searchBox.Location = new System.Drawing.Point(12, 112);
            this.searchBox.Name = "searchBox";
            this.searchBox.Size = new System.Drawing.Size(363, 20);
            this.searchBox.TabIndex = 1;
            this.searchBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.searchBox_KeyDown);
            // 
            // searchResultsTabControl
            // 
            this.searchResultsTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.searchResultsTabControl.Controls.Add(this.tableResultPage);
            this.searchResultsTabControl.Controls.Add(this.columnResultPage);
            this.searchResultsTabControl.Controls.Add(this.procedureResultPage);
            this.searchResultsTabControl.Location = new System.Drawing.Point(0, 217);
            this.searchResultsTabControl.Name = "searchResultsTabControl";
            this.searchResultsTabControl.SelectedIndex = 0;
            this.searchResultsTabControl.Size = new System.Drawing.Size(935, 348);
            this.searchResultsTabControl.TabIndex = 2;
            // 
            // tableResultPage
            // 
            this.tableResultPage.Controls.Add(this.tableSearchResults);
            this.tableResultPage.Location = new System.Drawing.Point(4, 22);
            this.tableResultPage.Name = "tableResultPage";
            this.tableResultPage.Padding = new System.Windows.Forms.Padding(3);
            this.tableResultPage.Size = new System.Drawing.Size(927, 322);
            this.tableResultPage.TabIndex = 0;
            this.tableResultPage.Text = "Table";
            this.tableResultPage.UseVisualStyleBackColor = true;
            // 
            // tableSearchResults
            // 
            this.tableSearchResults.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            databaseColumn,
            schemaColumn,
            tableCoumn,
            this.typeColumn,
            reasonColumn});
            this.tableSearchResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableSearchResults.FullRowSelect = true;
            this.tableSearchResults.HideSelection = false;
            this.tableSearchResults.Location = new System.Drawing.Point(3, 3);
            this.tableSearchResults.Name = "tableSearchResults";
            this.tableSearchResults.Size = new System.Drawing.Size(921, 316);
            this.tableSearchResults.TabIndex = 0;
            this.tableSearchResults.UseCompatibleStateImageBehavior = false;
            this.tableSearchResults.View = System.Windows.Forms.View.Details;
            this.tableSearchResults.MouseClick += new System.Windows.Forms.MouseEventHandler(this.tableSearchResults_MouseClick);
            // 
            // typeColumn
            // 
            this.typeColumn.Text = "Type";
            this.typeColumn.Width = 74;
            // 
            // columnResultPage
            // 
            this.columnResultPage.Controls.Add(this.columnSearchResults);
            this.columnResultPage.Location = new System.Drawing.Point(4, 22);
            this.columnResultPage.Name = "columnResultPage";
            this.columnResultPage.Size = new System.Drawing.Size(927, 322);
            this.columnResultPage.TabIndex = 2;
            this.columnResultPage.Text = "Columns";
            this.columnResultPage.UseVisualStyleBackColor = true;
            // 
            // columnSearchResults
            // 
            this.columnSearchResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.columnSearchResults.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            columnDatabaseColumn,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader1,
            this.columnHeader5,
            this.fkColumn,
            this.columnHeader4});
            this.columnSearchResults.FullRowSelect = true;
            this.columnSearchResults.HideSelection = false;
            this.columnSearchResults.Location = new System.Drawing.Point(3, 3);
            this.columnSearchResults.Name = "columnSearchResults";
            this.columnSearchResults.Size = new System.Drawing.Size(927, 316);
            this.columnSearchResults.TabIndex = 1;
            this.columnSearchResults.UseCompatibleStateImageBehavior = false;
            this.columnSearchResults.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Schema";
            this.columnHeader2.Width = 67;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Table";
            this.columnHeader3.Width = 135;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Column";
            this.columnHeader1.Width = 98;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Type";
            this.columnHeader5.Width = 108;
            // 
            // fkColumn
            // 
            this.fkColumn.Text = "Foreign Key";
            this.fkColumn.Width = 159;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Reason";
            this.columnHeader4.Width = 269;
            // 
            // procedureResultPage
            // 
            this.procedureResultPage.Controls.Add(this.procedureSearchResults);
            this.procedureResultPage.Location = new System.Drawing.Point(4, 22);
            this.procedureResultPage.Name = "procedureResultPage";
            this.procedureResultPage.Padding = new System.Windows.Forms.Padding(3);
            this.procedureResultPage.Size = new System.Drawing.Size(927, 322);
            this.procedureResultPage.TabIndex = 1;
            this.procedureResultPage.Text = "Stored Procedures";
            this.procedureResultPage.UseVisualStyleBackColor = true;
            // 
            // procedureSearchResults
            // 
            this.procedureSearchResults.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader6,
            this.columnHeader7,
            this.columnHeader8,
            this.columnHeader9,
            this.columnHeader10});
            this.procedureSearchResults.ContextMenuStrip = this.procResultContextMenu;
            this.procedureSearchResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.procedureSearchResults.FullRowSelect = true;
            this.procedureSearchResults.HideSelection = false;
            this.procedureSearchResults.Location = new System.Drawing.Point(3, 3);
            this.procedureSearchResults.Name = "procedureSearchResults";
            this.procedureSearchResults.Size = new System.Drawing.Size(921, 316);
            this.procedureSearchResults.TabIndex = 1;
            this.procedureSearchResults.UseCompatibleStateImageBehavior = false;
            this.procedureSearchResults.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Database";
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Schema";
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "Procedure";
            this.columnHeader8.Width = 188;
            // 
            // columnHeader9
            // 
            this.columnHeader9.Text = "DateCreated";
            this.columnHeader9.Width = 122;
            // 
            // columnHeader10
            // 
            this.columnHeader10.Text = "DateModified";
            this.columnHeader10.Width = 127;
            // 
            // procResultContextMenu
            // 
            this.procResultContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewProcTextMenuStripOption});
            this.procResultContextMenu.Name = "procResultContextMenu";
            this.procResultContextMenu.Size = new System.Drawing.Size(124, 26);
            // 
            // viewProcTextMenuStripOption
            // 
            this.viewProcTextMenuStripOption.Name = "viewProcTextMenuStripOption";
            this.viewProcTextMenuStripOption.Size = new System.Drawing.Size(123, 22);
            this.viewProcTextMenuStripOption.Text = "View Text";
            this.viewProcTextMenuStripOption.Click += new System.EventHandler(this.viewProcTextMenuStripOption_Click);
            // 
            // tableResultContextMenu
            // 
            this.tableResultContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewColumnsToolStripMenuItem,
            this.selectTop1000ToolStripMenuItem,
            this.showViewCreateScriptMenuItem});
            this.tableResultContextMenu.Name = "tableResultContextMenu";
            this.tableResultContextMenu.Size = new System.Drawing.Size(174, 70);
            // 
            // viewColumnsToolStripMenuItem
            // 
            this.viewColumnsToolStripMenuItem.Name = "viewColumnsToolStripMenuItem";
            this.viewColumnsToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.viewColumnsToolStripMenuItem.Text = "View Columns";
            this.viewColumnsToolStripMenuItem.Click += new System.EventHandler(this.ViewColumnsToolStripMenuItem_Click);
            // 
            // selectTop1000ToolStripMenuItem
            // 
            this.selectTop1000ToolStripMenuItem.Enabled = false;
            this.selectTop1000ToolStripMenuItem.Name = "selectTop1000ToolStripMenuItem";
            this.selectTop1000ToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.selectTop1000ToolStripMenuItem.Text = "Select Top 1000";
            // 
            // showViewCreateScriptMenuItem
            // 
            this.showViewCreateScriptMenuItem.Name = "showViewCreateScriptMenuItem";
            this.showViewCreateScriptMenuItem.Size = new System.Drawing.Size(173, 22);
            this.showViewCreateScriptMenuItem.Text = "Show Create Script";
            this.showViewCreateScriptMenuItem.Click += new System.EventHandler(this.showCreateScriptToolStripMenuItem_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 135);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(198, 52);
            this.label1.TabIndex = 3;
            this.label1.Text = "Example Searches:\r\n\r\nSearch columns: DbName.dbo.*.chart\r\nSearch tables and procs:" +
    " DbName.dbo.*";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 96);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Search Term";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Server";
            // 
            // searchButton
            // 
            this.searchButton.Location = new System.Drawing.Point(381, 111);
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(75, 23);
            this.searchButton.TabIndex = 6;
            this.searchButton.Text = "Search";
            this.searchButton.UseVisualStyleBackColor = true;
            this.searchButton.Click += new System.EventHandler(this.SearchButton_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 568);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(935, 22);
            this.statusStrip.TabIndex = 7;
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // backButton
            // 
            this.backButton.Enabled = false;
            this.backButton.Location = new System.Drawing.Point(462, 112);
            this.backButton.Name = "backButton";
            this.backButton.Size = new System.Drawing.Size(18, 23);
            this.backButton.TabIndex = 8;
            this.backButton.Text = "<";
            this.backToolTip.SetToolTip(this.backButton, "Back (Alt + Left Arrow)");
            this.backButton.UseVisualStyleBackColor = true;
            this.backButton.Click += new System.EventHandler(this.backButton_Click);
            // 
            // forwardButton
            // 
            this.forwardButton.Enabled = false;
            this.forwardButton.Location = new System.Drawing.Point(482, 112);
            this.forwardButton.Name = "forwardButton";
            this.forwardButton.Size = new System.Drawing.Size(18, 23);
            this.forwardButton.TabIndex = 9;
            this.forwardButton.Text = ">";
            this.backToolTip.SetToolTip(this.forwardButton, "Forward (Alt + Right Arrow)");
            this.forwardButton.UseVisualStyleBackColor = true;
            this.forwardButton.Click += new System.EventHandler(this.forwardButton_Click);
            // 
            // frequentServerPanel
            // 
            this.frequentServerPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.frequentServerPanel.Location = new System.Drawing.Point(12, 60);
            this.frequentServerPanel.Name = "frequentServerPanel";
            this.frequentServerPanel.Size = new System.Drawing.Size(916, 30);
            this.frequentServerPanel.TabIndex = 10;
            // 
            // SearchForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(935, 590);
            this.Controls.Add(this.frequentServerPanel);
            this.Controls.Add(this.forwardButton);
            this.Controls.Add(this.backButton);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.searchButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.searchResultsTabControl);
            this.Controls.Add(this.searchBox);
            this.Controls.Add(this.serverName);
            this.KeyPreview = true;
            this.Name = "SearchForm";
            this.Text = "SearchForm";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SearchForm_KeyDown);
            this.searchResultsTabControl.ResumeLayout(false);
            this.tableResultPage.ResumeLayout(false);
            this.columnResultPage.ResumeLayout(false);
            this.procedureResultPage.ResumeLayout(false);
            this.procResultContextMenu.ResumeLayout(false);
            this.tableResultContextMenu.ResumeLayout(false);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox serverName;
        private System.Windows.Forms.TextBox searchBox;
        private System.Windows.Forms.TabControl searchResultsTabControl;
        private System.Windows.Forms.TabPage tableResultPage;
        private System.Windows.Forms.ListView tableSearchResults;
        private System.Windows.Forms.TabPage procedureResultPage;
        private System.Windows.Forms.ListView procedureSearchResults;
        private System.Windows.Forms.ContextMenuStrip tableResultContextMenu;
        private System.Windows.Forms.ToolStripMenuItem viewColumnsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectTop1000ToolStripMenuItem;
        private System.Windows.Forms.TabPage columnResultPage;
        private System.Windows.Forms.ListView columnSearchResults;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.ColumnHeader columnHeader9;
        private System.Windows.Forms.ColumnHeader columnHeader10;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button searchButton;
        private System.Windows.Forms.ContextMenuStrip procResultContextMenu;
        private System.Windows.Forms.ToolStripMenuItem viewProcTextMenuStripOption;
        private System.Windows.Forms.ColumnHeader fkColumn;
        private System.Windows.Forms.ToolStripMenuItem showViewCreateScriptMenuItem;
        private System.Windows.Forms.ColumnHeader typeColumn;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.Button backButton;
        private System.Windows.Forms.ToolTip backToolTip;
        private System.Windows.Forms.Button forwardButton;
        private System.Windows.Forms.Panel frequentServerPanel;
    }
}