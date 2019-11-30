using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Tangh.Controls
{
    public class QuickListView : ListView
    {
        public QuickListView()
            : base()
        {
            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            UpdateStyles();
        }

        public void InitColumns(Dictionary<string, int> dic)
        {
            this.View = System.Windows.Forms.View.Details;
            foreach (var item in dic)
            {
                this.Columns.Add(item.Key, item.Value, HorizontalAlignment.Left);
            }
        }

        private void Update(int index, Dictionary<int, string> dic, Dictionary<int, Color> colorDic = null)
        {
            this.Invoke(new Action<ListView>(p =>
            {
                p.BeginUpdate();
                foreach (var item in dic)
                {
                    p.Items[index].SubItems[item.Key].Text = item.Value;
                    if (colorDic != null && colorDic.ContainsKey(item.Key))
                    {
                        p.Items[index].SubItems[item.Key].ForeColor = colorDic[item.Key];
                    }
                }

                p.EndUpdate();
            }), this);
        }

        public void SetData(List<string[]> list)
        {
            this.Invoke(new Action<ListView>(p =>
            {
                p.BeginUpdate();
                foreach (var array in list)
                {
                    ListViewItem item = new ListViewItem(array);

                    p.Items.Add(item);
                }

                p.EndUpdate();
            }), this);
        }
    }
}
