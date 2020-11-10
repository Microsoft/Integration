using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace Microsoft.BizTalk.Adapters.ODBC
{
	/// <summary>
	/// Summary description for MetaData.
	/// </summary>
	public class MetaData : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.TextBox textBox2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.RadioButton radioButton1;
		private System.Windows.Forms.RadioButton radioButton2;
		private System.Windows.Forms.Label label4;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public MetaData()
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
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.button3 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.button1 = new System.Windows.Forms.Button();
			this.textBox2 = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.radioButton1 = new System.Windows.Forms.RadioButton();
			this.radioButton2 = new System.Windows.Forms.RadioButton();
			this.label4 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(28, 40);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(152, 16);
			this.label2.TabIndex = 20;
			this.label2.Text = "Target Namespace:";
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(32, 56);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(448, 24);
			this.textBox1.TabIndex = 17;
			this.textBox1.Text = "";
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(388, 256);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(88, 24);
			this.button3.TabIndex = 16;
			this.button3.Text = "Finish";
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(292, 256);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(88, 24);
			this.button2.TabIndex = 15;
			this.button2.Text = "Next >";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(196, 256);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(88, 24);
			this.button1.TabIndex = 14;
			this.button1.Text = "< Bac&k";
			// 
			// textBox2
			// 
			this.textBox2.Location = new System.Drawing.Point(32, 200);
			this.textBox2.Multiline = true;
			this.textBox2.Name = "textBox2";
			this.textBox2.Size = new System.Drawing.Size(448, 24);
			this.textBox2.TabIndex = 21;
			this.textBox2.Text = "";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(32, 184);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(168, 16);
			this.label3.TabIndex = 22;
			this.label3.Text = "Document root element name:";
			// 
			// radioButton1
			// 
			this.radioButton1.Location = new System.Drawing.Point(56, 112);
			this.radioButton1.Name = "radioButton1";
			this.radioButton1.Size = new System.Drawing.Size(128, 24);
			this.radioButton1.TabIndex = 23;
			this.radioButton1.Text = "Receive";
			// 
			// radioButton2
			// 
			this.radioButton2.Location = new System.Drawing.Point(56, 136);
			this.radioButton2.Name = "radioButton2";
			this.radioButton2.Size = new System.Drawing.Size(112, 24);
			this.radioButton2.TabIndex = 24;
			this.radioButton2.Text = "Send";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(32, 96);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(152, 16);
			this.label4.TabIndex = 25;
			this.label4.Text = "Select port type:";
			// 
			// MetaData
			// 
			this.Controls.Add(this.label4);
			this.Controls.Add(this.radioButton2);
			this.Controls.Add(this.radioButton1);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.textBox2);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.button3);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Name = "MetaData";
			this.Size = new System.Drawing.Size(504, 288);
			this.ResumeLayout(false);

		}
		#endregion
	}
}
