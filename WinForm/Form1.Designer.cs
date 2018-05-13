namespace WinForm
{
    partial class MainForm
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
            this.checkBoxTest1 = new System.Windows.Forms.CheckBox();
            this.checkBoxTest2 = new System.Windows.Forms.CheckBox();
            this.checkBoxTest3 = new System.Windows.Forms.CheckBox();
            this.lblTest1 = new System.Windows.Forms.Label();
            this.lblTest2 = new System.Windows.Forms.Label();
            this.lblTest3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // checkBoxTest1
            // 
            this.checkBoxTest1.AutoSize = true;
            this.checkBoxTest1.Location = new System.Drawing.Point(12, 12);
            this.checkBoxTest1.Name = "checkBoxTest1";
            this.checkBoxTest1.Size = new System.Drawing.Size(56, 17);
            this.checkBoxTest1.TabIndex = 3;
            this.checkBoxTest1.Text = "Test 1";
            this.checkBoxTest1.UseVisualStyleBackColor = true;
            // 
            // checkBoxTest2
            // 
            this.checkBoxTest2.AutoSize = true;
            this.checkBoxTest2.Location = new System.Drawing.Point(12, 35);
            this.checkBoxTest2.Name = "checkBoxTest2";
            this.checkBoxTest2.Size = new System.Drawing.Size(56, 17);
            this.checkBoxTest2.TabIndex = 4;
            this.checkBoxTest2.Text = "Test 2";
            this.checkBoxTest2.UseVisualStyleBackColor = true;
            // 
            // checkBoxTest3
            // 
            this.checkBoxTest3.AutoSize = true;
            this.checkBoxTest3.Location = new System.Drawing.Point(12, 58);
            this.checkBoxTest3.Name = "checkBoxTest3";
            this.checkBoxTest3.Size = new System.Drawing.Size(56, 17);
            this.checkBoxTest3.TabIndex = 5;
            this.checkBoxTest3.Text = "Test 3";
            this.checkBoxTest3.UseVisualStyleBackColor = true;
            // 
            // lblTest1
            // 
            this.lblTest1.AutoSize = true;
            this.lblTest1.Location = new System.Drawing.Point(12, 112);
            this.lblTest1.Name = "lblTest1";
            this.lblTest1.Size = new System.Drawing.Size(35, 13);
            this.lblTest1.TabIndex = 6;
            this.lblTest1.Text = "label1";
            // 
            // lblTest2
            // 
            this.lblTest2.AutoSize = true;
            this.lblTest2.Location = new System.Drawing.Point(12, 138);
            this.lblTest2.Name = "lblTest2";
            this.lblTest2.Size = new System.Drawing.Size(35, 13);
            this.lblTest2.TabIndex = 7;
            this.lblTest2.Text = "label1";
            // 
            // lblTest3
            // 
            this.lblTest3.AutoSize = true;
            this.lblTest3.Location = new System.Drawing.Point(12, 164);
            this.lblTest3.Name = "lblTest3";
            this.lblTest3.Size = new System.Drawing.Size(35, 13);
            this.lblTest3.TabIndex = 8;
            this.lblTest3.Text = "label1";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(144, 193);
            this.Controls.Add(this.lblTest3);
            this.Controls.Add(this.lblTest2);
            this.Controls.Add(this.lblTest1);
            this.Controls.Add(this.checkBoxTest3);
            this.Controls.Add(this.checkBoxTest2);
            this.Controls.Add(this.checkBoxTest1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.Text = "WinForm";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.CheckBox checkBoxTest1;
        private System.Windows.Forms.CheckBox checkBoxTest2;
        private System.Windows.Forms.CheckBox checkBoxTest3;
        private System.Windows.Forms.Label lblTest1;
        private System.Windows.Forms.Label lblTest2;
        private System.Windows.Forms.Label lblTest3;
    }
}

