using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SQLSearcher
{
    public partial class DefaultEditorMenuItem : ToolStripMenuItem
    {
        //
        // Summary:
        //     Gets or sets a value indicating whether the System.Windows.Forms.ToolStripMenuItem
        //     is checked.
        //
        // Returns:
        //     true if the System.Windows.Forms.ToolStripMenuItem is checked or is in an indeterminate
        //     state; otherwise, false. The default is false.
        [Bindable(true)]
        [DefaultValue("notepad++")]
        [RefreshProperties(RefreshProperties.All)]
        public string Command { get; set; }

        //
        // Summary:
        //     Exclusively checks this item among its siblings and notifies TempFileRepo to update options
        //
        // Parameters:
        //   e:
        //     An System.EventArgs that contains the event data.
        protected override void OnClick(EventArgs e)
        {
            Console.WriteLine("Hello World! " + Command);
            foreach (ToolStripMenuItem tsmi in GetCurrentParent().Items)
            {
                tsmi.Checked = this == tsmi;
            }

            TempFileRepo.commandName = Command;
        }
    }
}
