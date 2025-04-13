namespace gameco
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
			this.components = new System.ComponentModel.Container();
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.turn = new System.Windows.Forms.Button();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.second1 = new System.Windows.Forms.Label();
			this.minute1 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.minute2 = new System.Windows.Forms.Label();
			this.second2 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.timer2 = new System.Windows.Forms.Timer(this.components);
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.mode = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.BackColor = System.Drawing.SystemColors.ActiveCaption;
			this.button1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.button1.Location = new System.Drawing.Point(4, 82);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(132, 39);
			this.button1.TabIndex = 1;
			this.button1.Text = "Chơi lại";
			this.button1.UseVisualStyleBackColor = false;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// button2
			// 
			this.button2.BackColor = System.Drawing.Color.IndianRed;
			this.button2.Location = new System.Drawing.Point(143, 82);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(112, 39);
			this.button2.TabIndex = 2;
			this.button2.Text = "Thoát";
			this.button2.UseVisualStyleBackColor = false;
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Cascadia Mono", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(17, 172);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(130, 44);
			this.label1.TabIndex = 3;
			this.label1.Text = "Tới lượt \r\nngười chơi :";
			this.label1.Click += new System.EventHandler(this.label1_Click);
			// 
			// turn
			// 
			this.turn.Location = new System.Drawing.Point(174, 173);
			this.turn.Name = "turn";
			this.turn.Size = new System.Drawing.Size(59, 43);
			this.turn.TabIndex = 4;
			this.turn.UseVisualStyleBackColor = true;
			// 
			// timer1
			// 
			this.timer1.Interval = 1000;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// second1
			// 
			this.second1.AutoSize = true;
			this.second1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.second1.Location = new System.Drawing.Point(144, 295);
			this.second1.Name = "second1";
			this.second1.Size = new System.Drawing.Size(27, 29);
			this.second1.TabIndex = 5;
			this.second1.Text = "0";
			this.second1.Click += new System.EventHandler(this.second_Click);
			// 
			// minute1
			// 
			this.minute1.AutoSize = true;
			this.minute1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.minute1.Location = new System.Drawing.Point(67, 295);
			this.minute1.Name = "minute1";
			this.minute1.Size = new System.Drawing.Size(27, 29);
			this.minute1.TabIndex = 6;
			this.minute1.Text = "0";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.Location = new System.Drawing.Point(110, 295);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(20, 29);
			this.label4.TabIndex = 7;
			this.label4.Text = ":";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(110, 391);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(20, 29);
			this.label2.TabIndex = 10;
			this.label2.Text = ":";
			// 
			// minute2
			// 
			this.minute2.AutoSize = true;
			this.minute2.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.minute2.Location = new System.Drawing.Point(67, 391);
			this.minute2.Name = "minute2";
			this.minute2.Size = new System.Drawing.Size(27, 29);
			this.minute2.TabIndex = 9;
			this.minute2.Text = "0";
			// 
			// second2
			// 
			this.second2.AutoSize = true;
			this.second2.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.second2.Location = new System.Drawing.Point(144, 391);
			this.second2.Name = "second2";
			this.second2.Size = new System.Drawing.Size(27, 29);
			this.second2.TabIndex = 8;
			this.second2.Text = "0";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Font = new System.Drawing.Font("Cascadia Mono", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label6.Location = new System.Drawing.Point(18, 254);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(225, 20);
			this.label6.TabIndex = 11;
			this.label6.Text = "Thời gian của bên xanh :";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Font = new System.Drawing.Font("Cascadia Mono", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label7.Location = new System.Drawing.Point(17, 354);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(207, 20);
			this.label7.TabIndex = 12;
			this.label7.Text = "Thời gian của bên đỏ :";
			this.label7.Click += new System.EventHandler(this.label7_Click);
			// 
			// timer2
			// 
			this.timer2.Interval = 1000;
			this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
			// 
			// groupBox1
			// 
			this.groupBox1.BackColor = System.Drawing.Color.LightGray;
			this.groupBox1.Controls.Add(this.mode);
			this.groupBox1.Controls.Add(this.button1);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.label7);
			this.groupBox1.Controls.Add(this.minute2);
			this.groupBox1.Controls.Add(this.button2);
			this.groupBox1.Controls.Add(this.second2);
			this.groupBox1.Controls.Add(this.label6);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.turn);
			this.groupBox1.Controls.Add(this.second1);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.minute1);
			this.groupBox1.Font = new System.Drawing.Font("Cascadia Mono", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBox1.Location = new System.Drawing.Point(715, 31);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(255, 494);
			this.groupBox1.TabIndex = 13;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Điều khiển";
			// 
			// mode
			// 
			this.mode.AutoSize = true;
			this.mode.Location = new System.Drawing.Point(19, 139);
			this.mode.Name = "mode";
			this.mode.Size = new System.Drawing.Size(140, 22);
			this.mode.TabIndex = 14;
			this.mode.Text = "Chế độ chơi: ";
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
			this.ClientSize = new System.Drawing.Size(982, 853);
			this.Controls.Add(this.groupBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Name = "Form1";
			this.Text = "Cờ gánh";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button turn;
		private System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.Label second1;
		private System.Windows.Forms.Label minute1;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label minute2;
		private System.Windows.Forms.Label second2;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Timer timer2;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label mode;
	}
}

