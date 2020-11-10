using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace Microsoft.BizTalk.Adapters.ODBC
{
	/// <summary>
	/// Summary description for QueryType.
	/// </summary>
	public class ConnectionString : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button button4;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button1;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ConnectionString()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call

		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.button4 = new System.Windows.Forms.Button();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.button3 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.button1 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(36, 44);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(152, 16);
			this.label2.TabIndex = 13;
			this.label2.Text = "Connection string:";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(36, 12);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(248, 16);
			this.label1.TabIndex = 12;
			this.label1.Text = "To set connection string, click set";
			// 
			// button4
			// 
			this.button4.Location = new System.Drawing.Point(396, 28);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size(80, 24);
			this.button4.TabIndex = 11;
			this.button4.Text = "&Set";
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(36, 68);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.ReadOnly = true;
			this.textBox1.Size = new System.Drawing.Size(448, 168);
			this.textBox1.TabIndex = 10;
			this.textBox1.Text = "";
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(396, 260);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(88, 24);
			this.button3.TabIndex = 9;
			this.button3.Text = "Finish";
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(300, 260);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(88, 24);
			this.button2.TabIndex = 8;
			this.button2.Text = "Next >";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(204, 260);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(88, 24);
			this.button1.TabIndex = 7;
			this.button1.Text = "< Bac&k";
			// 
			// ConnectionString
			// 
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.button4);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.button3);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Name = "ConnectionString";
			this.Size = new System.Drawing.Size(520, 296);
			this.ResumeLayout(false);

		}
		#endregion
	}
}
