namespace WindowsFormsApplication6
{
    partial class Form1
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
            this.encryp = new System.Windows.Forms.Button();
            this.LoadPublicKey = new System.Windows.Forms.Button();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // encryp
            // 
            this.encryp.Location = new System.Drawing.Point(103, 101);
            this.encryp.Name = "encryp";
            this.encryp.Size = new System.Drawing.Size(75, 23);
            this.encryp.TabIndex = 0;
            this.encryp.Text = "encryp";
            this.encryp.UseVisualStyleBackColor = true;
            this.encryp.Click += new System.EventHandler(this.encryp_Click);
            // 
            // LoadPublicKey
            // 
            this.LoadPublicKey.Location = new System.Drawing.Point(91, 22);
            this.LoadPublicKey.Name = "LoadPublicKey";
            this.LoadPublicKey.Size = new System.Drawing.Size(109, 23);
            this.LoadPublicKey.TabIndex = 1;
            this.LoadPublicKey.Text = "LoadPublicKey";
            this.LoadPublicKey.UseVisualStyleBackColor = true;
            this.LoadPublicKey.Click += new System.EventHandler(this.LoadPublicKey_Click);
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(103, 65);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(133, 20);
            this.textBox3.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 65);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "enter yu text??!";
            // 
            // button3
            // 
            this.button3.Enabled = false;
            this.button3.Location = new System.Drawing.Point(103, 205);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 6;
            this.button3.Text = "Encrypt file";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.LoadPublicKey);
            this.Controls.Add(this.encryp);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        #endregion

        private System.Windows.Forms.Button encryp;
        private System.Windows.Forms.Button LoadPublicKey;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button3;
    }
}

