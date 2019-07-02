namespace VolleyBall_Automated_Analysis_GradProject
{
    partial class Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.Process_Button = new System.Windows.Forms.Button();
            this.Browse_Button = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.InitializeButton = new System.Windows.Forms.Button();
            this.exitButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Process_Button
            // 
            this.Process_Button.Location = new System.Drawing.Point(458, 196);
            this.Process_Button.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Process_Button.Name = "Process_Button";
            this.Process_Button.Size = new System.Drawing.Size(66, 34);
            this.Process_Button.TabIndex = 3;
            this.Process_Button.Text = "Process";
            this.Process_Button.UseVisualStyleBackColor = true;
            this.Process_Button.Click += new System.EventHandler(this.Process_Button_Click);
            // 
            // Browse_Button
            // 
            this.Browse_Button.Location = new System.Drawing.Point(458, 82);
            this.Browse_Button.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Browse_Button.Name = "Browse_Button";
            this.Browse_Button.Size = new System.Drawing.Size(66, 33);
            this.Browse_Button.TabIndex = 2;
            this.Browse_Button.Text = "Browse";
            this.Browse_Button.UseVisualStyleBackColor = true;
            this.Browse_Button.Click += new System.EventHandler(this.Browse_Button_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(-1, -2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(476, 52);
            this.label1.TabIndex = 4;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // InitializeButton
            // 
            this.InitializeButton.Location = new System.Drawing.Point(458, 140);
            this.InitializeButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.InitializeButton.Name = "InitializeButton";
            this.InitializeButton.Size = new System.Drawing.Size(66, 34);
            this.InitializeButton.TabIndex = 5;
            this.InitializeButton.Text = "Initialize";
            this.InitializeButton.UseVisualStyleBackColor = true;
            this.InitializeButton.Click += new System.EventHandler(this.Initialize_Click);
            // 
            // exitButton
            // 
            this.exitButton.Location = new System.Drawing.Point(487, 11);
            this.exitButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(37, 21);
            this.exitButton.TabIndex = 6;
            this.exitButton.Text = "Exit";
            this.exitButton.UseVisualStyleBackColor = true;
            this.exitButton.Click += new System.EventHandler(this.exitButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(536, 365);
            this.Controls.Add(this.exitButton);
            this.Controls.Add(this.InitializeButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Process_Button);
            this.Controls.Add(this.Browse_Button);
            this.Name = "Form1";
            this.Text = "Volleyball Player Automated Analysis";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Process_Button;
        private System.Windows.Forms.Button Browse_Button;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button InitializeButton;
        private System.Windows.Forms.Button exitButton;
    }
}

