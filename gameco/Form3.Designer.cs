namespace gameco
{
	partial class Form3
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
			this.easy_btn = new System.Windows.Forms.Button();
			this.normal_btn = new System.Windows.Forms.Button();
			this.hard_btn = new System.Windows.Forms.Button();
			this.exit_btn = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// easy_btn
			// 
			this.easy_btn.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.easy_btn.Location = new System.Drawing.Point(63, 74);
			this.easy_btn.Name = "easy_btn";
			this.easy_btn.Size = new System.Drawing.Size(131, 54);
			this.easy_btn.TabIndex = 0;
			this.easy_btn.Text = "Dễ";
			this.easy_btn.UseVisualStyleBackColor = true;
			this.easy_btn.Click += new System.EventHandler(this.easy_btn_Click);
			// 
			// normal_btn
			// 
			this.normal_btn.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.normal_btn.Location = new System.Drawing.Point(63, 154);
			this.normal_btn.Name = "normal_btn";
			this.normal_btn.Size = new System.Drawing.Size(131, 53);
			this.normal_btn.TabIndex = 1;
			this.normal_btn.Text = "Khá";
			this.normal_btn.UseVisualStyleBackColor = true;
			this.normal_btn.Click += new System.EventHandler(this.normal_btn_Click);
			// 
			// hard_btn
			// 
			this.hard_btn.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.hard_btn.Location = new System.Drawing.Point(63, 233);
			this.hard_btn.Name = "hard_btn";
			this.hard_btn.Size = new System.Drawing.Size(131, 54);
			this.hard_btn.TabIndex = 2;
			this.hard_btn.Text = "Khó";
			this.hard_btn.UseVisualStyleBackColor = true;
			this.hard_btn.Click += new System.EventHandler(this.button3_Click);
			// 
			// exit_btn
			// 
			this.exit_btn.Location = new System.Drawing.Point(91, 323);
			this.exit_btn.Name = "exit_btn";
			this.exit_btn.Size = new System.Drawing.Size(78, 35);
			this.exit_btn.TabIndex = 3;
			this.exit_btn.Text = "Thoát";
			this.exit_btn.UseVisualStyleBackColor = true;
			this.exit_btn.Click += new System.EventHandler(this.exit_btn_Click);
			// 
			// Form3
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(278, 427);
			this.Controls.Add(this.exit_btn);
			this.Controls.Add(this.hard_btn);
			this.Controls.Add(this.normal_btn);
			this.Controls.Add(this.easy_btn);
			this.Name = "Form3";
			this.Text = "Chọn độ khó";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button easy_btn;
		private System.Windows.Forms.Button normal_btn;
		private System.Windows.Forms.Button hard_btn;
		private System.Windows.Forms.Button exit_btn;
	}
}