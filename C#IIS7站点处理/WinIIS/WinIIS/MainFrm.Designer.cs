﻿namespace WinIIS
{
    partial class MainFrm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnLoad = new System.Windows.Forms.Button();
            this.lstSite = new System.Windows.Forms.ListView();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnReStart = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(12, 12);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(62, 23);
            this.btnLoad.TabIndex = 1;
            this.btnLoad.Text = "加载站点";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // lstSite
            // 
            this.lstSite.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstSite.Location = new System.Drawing.Point(12, 41);
            this.lstSite.Name = "lstSite";
            this.lstSite.Size = new System.Drawing.Size(580, 209);
            this.lstSite.TabIndex = 2;
            this.lstSite.UseCompatibleStateImageBehavior = false;
            this.lstSite.View = System.Windows.Forms.View.Details;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(101, 12);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(62, 23);
            this.btnAdd.TabIndex = 1;
            this.btnAdd.Text = "添加站点";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnReStart
            // 
            this.btnReStart.Location = new System.Drawing.Point(192, 12);
            this.btnReStart.Name = "btnReStart";
            this.btnReStart.Size = new System.Drawing.Size(62, 23);
            this.btnReStart.TabIndex = 1;
            this.btnReStart.Text = "回收站点";
            this.btnReStart.UseVisualStyleBackColor = true;
            this.btnReStart.Click += new System.EventHandler(this.btnReStart_Click);
            // 
            // MainFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(605, 262);
            this.Controls.Add(this.lstSite);
            this.Controls.Add(this.btnReStart);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnLoad);
            this.Name = "MainFrm";
            this.Text = "针对IIS7处理，IIS6需要通过WMI或者其他方案";
            this.Load += new System.EventHandler(this.MainFrm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.ListView lstSite;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnReStart;
    }
}

