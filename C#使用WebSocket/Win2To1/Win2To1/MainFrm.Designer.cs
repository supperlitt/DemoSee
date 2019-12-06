namespace Win2To1
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
            this.txtServer = new System.Windows.Forms.TextBox();
            this.btnServer = new System.Windows.Forms.Button();
            this.txtClient = new System.Windows.Forms.TextBox();
            this.btnClient = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtServer
            // 
            this.txtServer.Location = new System.Drawing.Point(13, 34);
            this.txtServer.Multiline = true;
            this.txtServer.Name = "txtServer";
            this.txtServer.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtServer.Size = new System.Drawing.Size(457, 354);
            this.txtServer.TabIndex = 0;
            // 
            // btnServer
            // 
            this.btnServer.Location = new System.Drawing.Point(13, 5);
            this.btnServer.Name = "btnServer";
            this.btnServer.Size = new System.Drawing.Size(75, 23);
            this.btnServer.TabIndex = 1;
            this.btnServer.Text = "启动服务";
            this.btnServer.UseVisualStyleBackColor = true;
            this.btnServer.Click += new System.EventHandler(this.btnServer_Click);
            // 
            // txtClient
            // 
            this.txtClient.Location = new System.Drawing.Point(506, 34);
            this.txtClient.Multiline = true;
            this.txtClient.Name = "txtClient";
            this.txtClient.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtClient.Size = new System.Drawing.Size(457, 354);
            this.txtClient.TabIndex = 0;
            // 
            // btnClient
            // 
            this.btnClient.Location = new System.Drawing.Point(506, 5);
            this.btnClient.Name = "btnClient";
            this.btnClient.Size = new System.Drawing.Size(75, 23);
            this.btnClient.TabIndex = 1;
            this.btnClient.Text = "启动客户";
            this.btnClient.UseVisualStyleBackColor = true;
            this.btnClient.Click += new System.EventHandler(this.btnClient_Click);
            // 
            // MainFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(975, 400);
            this.Controls.Add(this.btnClient);
            this.Controls.Add(this.btnServer);
            this.Controls.Add(this.txtClient);
            this.Controls.Add(this.txtServer);
            this.Name = "MainFrm";
            this.Text = "websocketdll是应该是网上下的，具体哪里来的，不记得了。。。大体上就是集成了。websocket协议使用C#可以调用";
            this.Load += new System.EventHandler(this.MainFrm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtServer;
        private System.Windows.Forms.Button btnServer;
        private System.Windows.Forms.TextBox txtClient;
        private System.Windows.Forms.Button btnClient;
    }
}

