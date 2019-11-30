namespace WinTestCmd
{
    partial class Form1
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
            this.btnShell = new System.Windows.Forms.Button();
            this.txtResult = new System.Windows.Forms.TextBox();
            this.chkAsyc = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnShell
            // 
            this.btnShell.Location = new System.Drawing.Point(12, 12);
            this.btnShell.Name = "btnShell";
            this.btnShell.Size = new System.Drawing.Size(60, 23);
            this.btnShell.TabIndex = 0;
            this.btnShell.Text = "运行";
            this.btnShell.UseVisualStyleBackColor = true;
            this.btnShell.Click += new System.EventHandler(this.btnShell_Click);
            // 
            // txtResult
            // 
            this.txtResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtResult.Location = new System.Drawing.Point(12, 53);
            this.txtResult.Multiline = true;
            this.txtResult.Name = "txtResult";
            this.txtResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtResult.Size = new System.Drawing.Size(424, 197);
            this.txtResult.TabIndex = 1;
            // 
            // chkAsyc
            // 
            this.chkAsyc.AutoSize = true;
            this.chkAsyc.Location = new System.Drawing.Point(78, 16);
            this.chkAsyc.Name = "chkAsyc";
            this.chkAsyc.Size = new System.Drawing.Size(48, 16);
            this.chkAsyc.TabIndex = 3;
            this.chkAsyc.Text = "异步";
            this.chkAsyc.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(448, 262);
            this.Controls.Add(this.chkAsyc);
            this.Controls.Add(this.txtResult);
            this.Controls.Add(this.btnShell);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnShell;
        private System.Windows.Forms.TextBox txtResult;
        private System.Windows.Forms.CheckBox chkAsyc;
    }
}

