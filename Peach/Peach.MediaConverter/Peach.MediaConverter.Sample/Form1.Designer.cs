namespace Peach.MediaConverter.Sample
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics;
    public partial class Form1 : System.Windows.Forms.Form
    {
        //Form overrides dispose to clean up the component list.
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing && components != null)
                {
                    components.Dispose();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        //Required by the Windows Form Designer

        private System.ComponentModel.IContainer components;
        //NOTE: The following procedure is required by the Windows Form Designer
        //It can be modified using the Windows Form Designer.  
        //Do not modify it using the code editor.
        private void InitializeComponent()
        {
            this.ProgressBar = new System.Windows.Forms.ProgressBar();
            this.StatusLbl = new System.Windows.Forms.Label();
            this.Label2 = new System.Windows.Forms.Label();
            this.Button1 = new System.Windows.Forms.Button();
            this.Label3 = new System.Windows.Forms.Label();
            this.Label4 = new System.Windows.Forms.Label();
            this.Label1 = new System.Windows.Forms.Label();
            this.Button2 = new System.Windows.Forms.Button();
            this.ListBox1 = new System.Windows.Forms.CheckedListBox();
            this.Button3 = new System.Windows.Forms.Button();
            this.Label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ProgressBar
            // 
            this.ProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ProgressBar.Location = new System.Drawing.Point(12, 493);
            this.ProgressBar.Name = "ProgressBar";
            this.ProgressBar.Size = new System.Drawing.Size(824, 23);
            this.ProgressBar.TabIndex = 0;
            // 
            // StatusLbl
            // 
            this.StatusLbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.StatusLbl.AutoSize = true;
            this.StatusLbl.Location = new System.Drawing.Point(12, 477);
            this.StatusLbl.Name = "StatusLbl";
            this.StatusLbl.Size = new System.Drawing.Size(40, 13);
            this.StatusLbl.TabIndex = 1;
            this.StatusLbl.Text = "Status:";
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Location = new System.Drawing.Point(12, 9);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(102, 13);
            this.Label2.TabIndex = 2;
            this.Label2.Text = "Select output folder:";
            // 
            // Button1
            // 
            this.Button1.Location = new System.Drawing.Point(12, 25);
            this.Button1.Name = "Button1";
            this.Button1.Size = new System.Drawing.Size(75, 23);
            this.Button1.TabIndex = 3;
            this.Button1.Text = "Output folder:";
            this.Button1.UseVisualStyleBackColor = true;
            this.Button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // Label3
            // 
            this.Label3.AutoSize = true;
            this.Label3.Location = new System.Drawing.Point(93, 30);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(77, 13);
            this.Label3.TabIndex = 4;
            this.Label3.Text = "Path of output:";
            // 
            // Label4
            // 
            this.Label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Label4.Location = new System.Drawing.Point(588, 41);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(257, 40);
            this.Label4.TabIndex = 5;
            this.Label4.Text = "Please make sure you have files in the box, and click start!";
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(12, 68);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(291, 13);
            this.Label1.TabIndex = 7;
            this.Label1.Text = "Please drag and drop a folder or files into the box to encode!";
            // 
            // Button2
            // 
            this.Button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Button2.Location = new System.Drawing.Point(761, 58);
            this.Button2.Name = "Button2";
            this.Button2.Size = new System.Drawing.Size(75, 23);
            this.Button2.TabIndex = 9;
            this.Button2.Text = "START ENCODING!";
            this.Button2.UseVisualStyleBackColor = true;
            this.Button2.Click += new System.EventHandler(this.Button2_Click);
            // 
            // ListBox1
            // 
            this.ListBox1.AllowDrop = true;
            this.ListBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ListBox1.FormattingEnabled = true;
            this.ListBox1.Location = new System.Drawing.Point(15, 84);
            this.ListBox1.Name = "ListBox1";
            this.ListBox1.Size = new System.Drawing.Size(821, 394);
            this.ListBox1.TabIndex = 11;
            this.ListBox1.DragDrop += new System.Windows.Forms.DragEventHandler(this.ListBox1_DragDrop);
            this.ListBox1.DragEnter += new System.Windows.Forms.DragEventHandler(this.ListBox1_DragEnter);
            // 
            // Button3
            // 
            this.Button3.Location = new System.Drawing.Point(365, 30);
            this.Button3.Name = "Button3";
            this.Button3.Size = new System.Drawing.Size(95, 23);
            this.Button3.TabIndex = 12;
            this.Button3.Text = "convert audio";
            this.Button3.UseVisualStyleBackColor = true;
            this.Button3.Click += new System.EventHandler(this.Button3_Click);
            // 
            // Label5
            // 
            this.Label5.AutoSize = true;
            this.Label5.Location = new System.Drawing.Point(311, 9);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(235, 13);
            this.Label5.TabIndex = 13;
            this.Label5.Text = "ONLY IF U HAVE JUST AUDIO FILES IN BOX! ";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(848, 520);
            this.Controls.Add(this.Label5);
            this.Controls.Add(this.Button3);
            this.Controls.Add(this.ListBox1);
            this.Controls.Add(this.Button2);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.Label4);
            this.Controls.Add(this.Label3);
            this.Controls.Add(this.Button1);
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.StatusLbl);
            this.Controls.Add(this.ProgressBar);
            this.Name = "Form1";
            this.Text = "Encoder sample.";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        internal System.Windows.Forms.ProgressBar ProgressBar;
        internal System.Windows.Forms.Label StatusLbl;
        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.Button Button1;
        internal System.Windows.Forms.Label Label3;
        internal System.Windows.Forms.Label Label4;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.Button Button2;
        internal System.Windows.Forms.CheckedListBox ListBox1;
        internal System.Windows.Forms.Button Button3;

        internal System.Windows.Forms.Label Label5;
    }
}

