namespace WinMemoryChange
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
            this.button1 = new System.Windows.Forms.Button();
            this.txtBaseAddr = new System.Windows.Forms.TextBox();
            this.txtCodeAddr = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPId = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtKeyAddr = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(22, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "补丁";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtBaseAddr
            // 
            this.txtBaseAddr.Location = new System.Drawing.Point(100, 89);
            this.txtBaseAddr.Name = "txtBaseAddr";
            this.txtBaseAddr.Size = new System.Drawing.Size(100, 21);
            this.txtBaseAddr.TabIndex = 1;
            // 
            // txtCodeAddr
            // 
            this.txtCodeAddr.Location = new System.Drawing.Point(100, 122);
            this.txtCodeAddr.Name = "txtCodeAddr";
            this.txtCodeAddr.Size = new System.Drawing.Size(100, 21);
            this.txtCodeAddr.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(55, 92);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "基址：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(53, 125);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "码址：";
            // 
            // txtPId
            // 
            this.txtPId.Location = new System.Drawing.Point(100, 56);
            this.txtPId.Name = "txtPId";
            this.txtPId.Size = new System.Drawing.Size(100, 21);
            this.txtPId.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(55, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "PID：";
            // 
            // txtKeyAddr
            // 
            this.txtKeyAddr.Location = new System.Drawing.Point(100, 149);
            this.txtKeyAddr.Name = "txtKeyAddr";
            this.txtKeyAddr.Size = new System.Drawing.Size(100, 21);
            this.txtKeyAddr.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(25, 152);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 2;
            this.label4.Text = "关键地址：";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 191);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtKeyAddr);
            this.Controls.Add(this.txtCodeAddr);
            this.Controls.Add(this.txtPId);
            this.Controls.Add(this.txtBaseAddr);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtBaseAddr;
        private System.Windows.Forms.TextBox txtCodeAddr;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtPId;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtKeyAddr;
        private System.Windows.Forms.Label label4;
    }
}

