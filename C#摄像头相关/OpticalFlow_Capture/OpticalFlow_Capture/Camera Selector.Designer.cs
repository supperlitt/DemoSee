namespace OpticalFlow_Capture_Farneback
{
    partial class Camera_Selector
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
            this.label1 = new System.Windows.Forms.Label();
            this.Camera_Selection = new System.Windows.Forms.ComboBox();
            this.BTN_Capture = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 16);
            this.label1.TabIndex = 11;
            this.label1.Text = "Camera";
            // 
            // Camera_Selection
            // 
            this.Camera_Selection.FormattingEnabled = true;
            this.Camera_Selection.Location = new System.Drawing.Point(15, 31);
            this.Camera_Selection.Name = "Camera_Selection";
            this.Camera_Selection.Size = new System.Drawing.Size(230, 21);
            this.Camera_Selection.TabIndex = 10;
            // 
            // BTN_Capture
            // 
            this.BTN_Capture.Enabled = false;
            this.BTN_Capture.Location = new System.Drawing.Point(253, 29);
            this.BTN_Capture.Name = "BTN_Capture";
            this.BTN_Capture.Size = new System.Drawing.Size(102, 23);
            this.BTN_Capture.TabIndex = 9;
            this.BTN_Capture.Text = "Start Capture";
            this.BTN_Capture.UseVisualStyleBackColor = true;
            this.BTN_Capture.Click += new System.EventHandler(this.BTN_Capture_Click);
            // 
            // Camera_Selector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(367, 64);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Camera_Selection);
            this.Controls.Add(this.BTN_Capture);
            this.Name = "Camera_Selector";
            this.Text = "Camera Selector";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox Camera_Selection;
        private System.Windows.Forms.Button BTN_Capture;
    }
}