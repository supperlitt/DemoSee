namespace ToText
{
    partial class Form2
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
            this.button1 = new System.Windows.Forms.Button();
            this.txt_result = new System.Windows.Forms.TextBox();
            this.txt_imgpath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_imgpath = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cmb_languagetype = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(217, 55);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(57, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "转换";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txt_result
            // 
            this.txt_result.Location = new System.Drawing.Point(12, 93);
            this.txt_result.Multiline = true;
            this.txt_result.Name = "txt_result";
            this.txt_result.Size = new System.Drawing.Size(465, 381);
            this.txt_result.TabIndex = 1;
            // 
            // txt_imgpath
            // 
            this.txt_imgpath.Location = new System.Drawing.Point(77, 17);
            this.txt_imgpath.Name = "txt_imgpath";
            this.txt_imgpath.Size = new System.Drawing.Size(319, 21);
            this.txt_imgpath.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "选择图片：";
            // 
            // btn_imgpath
            // 
            this.btn_imgpath.Location = new System.Drawing.Point(402, 15);
            this.btn_imgpath.Name = "btn_imgpath";
            this.btn_imgpath.Size = new System.Drawing.Size(75, 23);
            this.btn_imgpath.TabIndex = 4;
            this.btn_imgpath.Text = "浏览...";
            this.btn_imgpath.UseVisualStyleBackColor = true;
            this.btn_imgpath.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.SaddleBrown;
            this.label2.Location = new System.Drawing.Point(280, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(197, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "office 2007 document imaging实现";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 60);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "语言类型：";
            // 
            // cmb_languagetype
            // 
            this.cmb_languagetype.FormattingEnabled = true;
            this.cmb_languagetype.Location = new System.Drawing.Point(77, 55);
            this.cmb_languagetype.Name = "cmb_languagetype";
            this.cmb_languagetype.Size = new System.Drawing.Size(134, 20);
            this.cmb_languagetype.TabIndex = 8;
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(490, 486);
            this.Controls.Add(this.cmb_languagetype);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btn_imgpath);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txt_imgpath);
            this.Controls.Add(this.txt_result);
            this.Controls.Add(this.button1);
            this.Name = "Form2";
            this.Text = "OCR 图片转化器 1.0 【Stone】";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txt_result;
        private System.Windows.Forms.TextBox txt_imgpath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_imgpath;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmb_languagetype;
    }
}