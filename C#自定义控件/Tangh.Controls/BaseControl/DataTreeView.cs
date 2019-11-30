using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Tangh.Controls.BaseControl
{
    public class DataTreeView : TreeView
    {
        public DataTreeView()
            : base()
        {
        }

        public void SetData<T>(T data)
        {
            // 读取包含Data的内容，并且设置指定长度。可以添加间隔。
            // TODO: because of this control is too hard, so let me have a lot of time to do.
        }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class DataTreeAttribute : Attribute
    {
        private int size = 0;

        public DataTreeAttribute(int size = 0)
        {
            this.size = size;
        }
    }
}
