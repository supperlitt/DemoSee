using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Tangh.Controls.BaseControl
{
    public partial class FileBrowse : UserControl
    {
        private string filter = string.Empty;

        public FileBrowse()
        {
            InitializeComponent();
        }

        public void SetFilter(string filter)
        {
            this.filter = filter;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (!string.IsNullOrEmpty(filter))
            {
                dialog.Filter = filter; // "压缩文件(*.zip)|*.zip";
            }

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.txtFileDir.Text = dialog.FileName;
            }
        }

        public string FilePath
        {
            get
            {
                return this.txtFileDir.Text;
            }
        }
    }
}
