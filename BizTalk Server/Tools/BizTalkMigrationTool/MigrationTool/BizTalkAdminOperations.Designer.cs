namespace MigrationTool
{
    partial class BizTalkAdminOperations
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BizTalkAdminOperations));
            this.txtConnectionString = new System.Windows.Forms.TextBox();
            this.richTextBoxLogs = new System.Windows.Forms.RichTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.rbWebsiteYes = new System.Windows.Forms.RadioButton();
            this.rbWebsiteNo = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.rbAppPoolYes = new System.Windows.Forms.RadioButton();
            this.rbAppPoolNo = new System.Windows.Forms.RadioButton();
            this.label6 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.rbCertificateYes = new System.Windows.Forms.RadioButton();
            this.rbCertificateNo = new System.Windows.Forms.RadioButton();
            this.label7 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.rbHandlersYes = new System.Windows.Forms.RadioButton();
            this.rbHandlersNo = new System.Windows.Forms.RadioButton();
            this.label8 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.rbHostInstanceYes = new System.Windows.Forms.RadioButton();
            this.rbHostInstanceNo = new System.Windows.Forms.RadioButton();
            this.label9 = new System.Windows.Forms.Label();
            this.panel7 = new System.Windows.Forms.Panel();
            this.rbBizTalkAssembliesYes = new System.Windows.Forms.RadioButton();
            this.rbBizTalkAssembliesNo = new System.Windows.Forms.RadioButton();
            this.label11 = new System.Windows.Forms.Label();
            this.panel8 = new System.Windows.Forms.Panel();
            this.rbGlobalPartyBindingYes = new System.Windows.Forms.RadioButton();
            this.rbGlobalPartyBindingNo = new System.Windows.Forms.RadioButton();
            this.label12 = new System.Windows.Forms.Label();
            this.panel9 = new System.Windows.Forms.Panel();
            this.rbBizTalkAppNo = new System.Windows.Forms.RadioButton();
            this.rbBizTalkAppYes = new System.Windows.Forms.RadioButton();
            this.label13 = new System.Windows.Forms.Label();
            this.btnExportOperations = new System.Windows.Forms.Button();
            this.btnImportOperations = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.rbFileCopyYes = new System.Windows.Forms.RadioButton();
            this.rbFileCopyNo = new System.Windows.Forms.RadioButton();
            this.label14 = new System.Windows.Forms.Label();
            this.txtConnectionStringDst = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.cmbBoxServerDst = new System.Windows.Forms.ComboBox();
            this.panelLoginDialog = new System.Windows.Forms.Panel();
            this.label18 = new System.Windows.Forms.Label();
            this.lblLoginDialog = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.panel11 = new System.Windows.Forms.Panel();
            this.btnSettings = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.panel14 = new System.Windows.Forms.Panel();
            this.rbServicesYes = new System.Windows.Forms.RadioButton();
            this.rbServicesNo = new System.Windows.Forms.RadioButton();
            this.panel13 = new System.Windows.Forms.Panel();
            this.rbApp = new System.Windows.Forms.RadioButton();
            this.rbBizTalk = new System.Windows.Forms.RadioButton();
            this.label4 = new System.Windows.Forms.Label();
            this.panel12 = new System.Windows.Forms.Panel();
            this.rbMigrate = new System.Windows.Forms.RadioButton();
            this.rbBackup = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel10 = new System.Windows.Forms.Panel();
            this.rbBamYes = new System.Windows.Forms.RadioButton();
            this.rbBamNo = new System.Windows.Forms.RadioButton();
            this.cmbBoxServerSrc = new System.Windows.Forms.ComboBox();
            this.lblMsg = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel8.SuspendLayout();
            this.panel9.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panelLoginDialog.SuspendLayout();
            this.panel11.SuspendLayout();
            this.panel14.SuspendLayout();
            this.panel13.SuspendLayout();
            this.panel12.SuspendLayout();
            this.panel10.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtConnectionString
            // 
            this.txtConnectionString.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtConnectionString.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtConnectionString.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtConnectionString.Location = new System.Drawing.Point(209, 84);
            this.txtConnectionString.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.txtConnectionString.Name = "txtConnectionString";
            this.txtConnectionString.Size = new System.Drawing.Size(389, 29);
            this.txtConnectionString.TabIndex = 3;
            this.txtConnectionString.Text = "SQL INSTANCE";
            this.txtConnectionString.Validating += new System.ComponentModel.CancelEventHandler(this.txtConnectionString_Validating);
            // 
            // richTextBoxLogs
            // 
            this.richTextBoxLogs.BackColor = System.Drawing.SystemColors.Window;
            this.richTextBoxLogs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.richTextBoxLogs.Font = new System.Drawing.Font("Calibri", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBoxLogs.Location = new System.Drawing.Point(386, 184);
            this.richTextBoxLogs.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.richTextBoxLogs.Name = "richTextBoxLogs";
            this.richTextBoxLogs.ReadOnly = true;
            this.richTextBoxLogs.Size = new System.Drawing.Size(527, 437);
            this.richTextBoxLogs.TabIndex = 25;
            this.richTextBoxLogs.Text = "";
            this.richTextBoxLogs.TextChanged += new System.EventHandler(this.richTextBoxLogs_TextChanged_1);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(20, 90);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(202, 25);
            this.label2.TabIndex = 30;
            this.label2.Text = "BizTalk Group (Source)";
            // 
            // rbWebsiteYes
            // 
            this.rbWebsiteYes.AutoSize = true;
            this.rbWebsiteYes.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.rbWebsiteYes.Location = new System.Drawing.Point(13, 5);
            this.rbWebsiteYes.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.rbWebsiteYes.Name = "rbWebsiteYes";
            this.rbWebsiteYes.Size = new System.Drawing.Size(64, 29);
            this.rbWebsiteYes.TabIndex = 12;
            this.rbWebsiteYes.Text = "Yes";
            this.rbWebsiteYes.UseVisualStyleBackColor = true;
            this.rbWebsiteYes.CheckedChanged += new System.EventHandler(this.rbWebsiteYes_CheckedChanged);
            // 
            // rbWebsiteNo
            // 
            this.rbWebsiteNo.AutoSize = true;
            this.rbWebsiteNo.Checked = true;
            this.rbWebsiteNo.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.rbWebsiteNo.Location = new System.Drawing.Point(103, 6);
            this.rbWebsiteNo.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.rbWebsiteNo.Name = "rbWebsiteNo";
            this.rbWebsiteNo.Size = new System.Drawing.Size(62, 29);
            this.rbWebsiteNo.TabIndex = 13;
            this.rbWebsiteNo.TabStop = true;
            this.rbWebsiteNo.Text = "No";
            this.rbWebsiteNo.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rbWebsiteYes);
            this.panel1.Controls.Add(this.rbWebsiteNo);
            this.panel1.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.panel1.Location = new System.Drawing.Point(198, 293);
            this.panel1.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(169, 29);
            this.panel1.TabIndex = 9;
            this.panel1.TabStop = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.rbAppPoolYes);
            this.panel2.Controls.Add(this.rbAppPoolNo);
            this.panel2.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.panel2.Location = new System.Drawing.Point(198, 249);
            this.panel2.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(169, 29);
            this.panel2.TabIndex = 8;
            this.panel2.TabStop = true;
            // 
            // rbAppPoolYes
            // 
            this.rbAppPoolYes.AutoSize = true;
            this.rbAppPoolYes.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.rbAppPoolYes.Location = new System.Drawing.Point(13, 5);
            this.rbAppPoolYes.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.rbAppPoolYes.Name = "rbAppPoolYes";
            this.rbAppPoolYes.Size = new System.Drawing.Size(64, 29);
            this.rbAppPoolYes.TabIndex = 10;
            this.rbAppPoolYes.Text = "Yes";
            this.rbAppPoolYes.UseVisualStyleBackColor = true;
            this.rbAppPoolYes.CheckedChanged += new System.EventHandler(this.rbAppPoolYes_CheckedChanged);
            // 
            // rbAppPoolNo
            // 
            this.rbAppPoolNo.AutoSize = true;
            this.rbAppPoolNo.Checked = true;
            this.rbAppPoolNo.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.rbAppPoolNo.Location = new System.Drawing.Point(103, 5);
            this.rbAppPoolNo.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.rbAppPoolNo.Name = "rbAppPoolNo";
            this.rbAppPoolNo.Size = new System.Drawing.Size(62, 29);
            this.rbAppPoolNo.TabIndex = 11;
            this.rbAppPoolNo.TabStop = true;
            this.rbAppPoolNo.Text = "No";
            this.rbAppPoolNo.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.Location = new System.Drawing.Point(20, 258);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(88, 25);
            this.label6.TabIndex = 38;
            this.label6.Text = "App Pool";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.rbCertificateYes);
            this.panel3.Controls.Add(this.rbCertificateNo);
            this.panel3.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.panel3.Location = new System.Drawing.Point(198, 335);
            this.panel3.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(169, 29);
            this.panel3.TabIndex = 10;
            this.panel3.TabStop = true;
            // 
            // rbCertificateYes
            // 
            this.rbCertificateYes.AutoSize = true;
            this.rbCertificateYes.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.rbCertificateYes.Location = new System.Drawing.Point(13, 5);
            this.rbCertificateYes.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.rbCertificateYes.Name = "rbCertificateYes";
            this.rbCertificateYes.Size = new System.Drawing.Size(64, 29);
            this.rbCertificateYes.TabIndex = 14;
            this.rbCertificateYes.Text = "Yes";
            this.rbCertificateYes.UseVisualStyleBackColor = true;
            this.rbCertificateYes.CheckedChanged += new System.EventHandler(this.rbCertificateYes_CheckedChanged);
            // 
            // rbCertificateNo
            // 
            this.rbCertificateNo.AutoSize = true;
            this.rbCertificateNo.Checked = true;
            this.rbCertificateNo.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.rbCertificateNo.Location = new System.Drawing.Point(104, 7);
            this.rbCertificateNo.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.rbCertificateNo.Name = "rbCertificateNo";
            this.rbCertificateNo.Size = new System.Drawing.Size(62, 29);
            this.rbCertificateNo.TabIndex = 15;
            this.rbCertificateNo.TabStop = true;
            this.rbCertificateNo.Text = "No";
            this.rbCertificateNo.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.label7.ForeColor = System.Drawing.Color.Black;
            this.label7.Location = new System.Drawing.Point(20, 345);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(98, 25);
            this.label7.TabIndex = 38;
            this.label7.Text = "Certificate";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.rbHandlersYes);
            this.panel4.Controls.Add(this.rbHandlersNo);
            this.panel4.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.panel4.Location = new System.Drawing.Point(198, 421);
            this.panel4.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(169, 29);
            this.panel4.TabIndex = 12;
            // 
            // rbHandlersYes
            // 
            this.rbHandlersYes.AutoSize = true;
            this.rbHandlersYes.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.rbHandlersYes.Location = new System.Drawing.Point(13, 5);
            this.rbHandlersYes.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.rbHandlersYes.Name = "rbHandlersYes";
            this.rbHandlersYes.Size = new System.Drawing.Size(64, 29);
            this.rbHandlersYes.TabIndex = 18;
            this.rbHandlersYes.Text = "Yes";
            this.rbHandlersYes.UseVisualStyleBackColor = true;
            this.rbHandlersYes.CheckedChanged += new System.EventHandler(this.rbHandlersYes_CheckedChanged);
            // 
            // rbHandlersNo
            // 
            this.rbHandlersNo.AutoSize = true;
            this.rbHandlersNo.Checked = true;
            this.rbHandlersNo.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.rbHandlersNo.Location = new System.Drawing.Point(104, 5);
            this.rbHandlersNo.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.rbHandlersNo.Name = "rbHandlersNo";
            this.rbHandlersNo.Size = new System.Drawing.Size(62, 29);
            this.rbHandlersNo.TabIndex = 19;
            this.rbHandlersNo.TabStop = true;
            this.rbHandlersNo.Text = "No";
            this.rbHandlersNo.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.label8.ForeColor = System.Drawing.Color.Black;
            this.label8.Location = new System.Drawing.Point(20, 429);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(87, 25);
            this.label8.TabIndex = 42;
            this.label8.Text = "Handlers";
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.rbHostInstanceYes);
            this.panel5.Controls.Add(this.rbHostInstanceNo);
            this.panel5.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.panel5.Location = new System.Drawing.Point(198, 378);
            this.panel5.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(169, 29);
            this.panel5.TabIndex = 11;
            this.panel5.TabStop = true;
            // 
            // rbHostInstanceYes
            // 
            this.rbHostInstanceYes.AutoSize = true;
            this.rbHostInstanceYes.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.rbHostInstanceYes.Location = new System.Drawing.Point(13, 5);
            this.rbHostInstanceYes.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.rbHostInstanceYes.Name = "rbHostInstanceYes";
            this.rbHostInstanceYes.Size = new System.Drawing.Size(64, 29);
            this.rbHostInstanceYes.TabIndex = 16;
            this.rbHostInstanceYes.Text = "Yes";
            this.rbHostInstanceYes.UseVisualStyleBackColor = true;
            this.rbHostInstanceYes.CheckedChanged += new System.EventHandler(this.rbHostInstanceYes_CheckedChanged);
            // 
            // rbHostInstanceNo
            // 
            this.rbHostInstanceNo.AutoSize = true;
            this.rbHostInstanceNo.Checked = true;
            this.rbHostInstanceNo.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.rbHostInstanceNo.Location = new System.Drawing.Point(104, 5);
            this.rbHostInstanceNo.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.rbHostInstanceNo.Name = "rbHostInstanceNo";
            this.rbHostInstanceNo.Size = new System.Drawing.Size(62, 29);
            this.rbHostInstanceNo.TabIndex = 17;
            this.rbHostInstanceNo.TabStop = true;
            this.rbHostInstanceNo.Text = "No";
            this.rbHostInstanceNo.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.label9.ForeColor = System.Drawing.Color.Black;
            this.label9.Location = new System.Drawing.Point(20, 386);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(50, 25);
            this.label9.TabIndex = 43;
            this.label9.Text = "Host";
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.rbBizTalkAssembliesYes);
            this.panel7.Controls.Add(this.rbBizTalkAssembliesNo);
            this.panel7.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.panel7.Location = new System.Drawing.Point(198, 509);
            this.panel7.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(169, 29);
            this.panel7.TabIndex = 14;
            this.panel7.TabStop = true;
            // 
            // rbBizTalkAssembliesYes
            // 
            this.rbBizTalkAssembliesYes.AutoSize = true;
            this.rbBizTalkAssembliesYes.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.rbBizTalkAssembliesYes.Location = new System.Drawing.Point(13, 6);
            this.rbBizTalkAssembliesYes.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.rbBizTalkAssembliesYes.Name = "rbBizTalkAssembliesYes";
            this.rbBizTalkAssembliesYes.Size = new System.Drawing.Size(64, 29);
            this.rbBizTalkAssembliesYes.TabIndex = 22;
            this.rbBizTalkAssembliesYes.Text = "Yes";
            this.rbBizTalkAssembliesYes.UseVisualStyleBackColor = true;
            this.rbBizTalkAssembliesYes.CheckedChanged += new System.EventHandler(this.rbBizTalkAssembliesYes_CheckedChanged);
            // 
            // rbBizTalkAssembliesNo
            // 
            this.rbBizTalkAssembliesNo.AutoSize = true;
            this.rbBizTalkAssembliesNo.Checked = true;
            this.rbBizTalkAssembliesNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbBizTalkAssembliesNo.Location = new System.Drawing.Point(104, 6);
            this.rbBizTalkAssembliesNo.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.rbBizTalkAssembliesNo.Name = "rbBizTalkAssembliesNo";
            this.rbBizTalkAssembliesNo.Size = new System.Drawing.Size(62, 29);
            this.rbBizTalkAssembliesNo.TabIndex = 23;
            this.rbBizTalkAssembliesNo.TabStop = true;
            this.rbBizTalkAssembliesNo.Text = "No";
            this.rbBizTalkAssembliesNo.UseVisualStyleBackColor = true;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.label11.ForeColor = System.Drawing.Color.Black;
            this.label11.Location = new System.Drawing.Point(20, 518);
            this.label11.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(153, 25);
            this.label11.TabIndex = 42;
            this.label11.Text = "Assemblies (DLL)";
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.rbGlobalPartyBindingYes);
            this.panel8.Controls.Add(this.rbGlobalPartyBindingNo);
            this.panel8.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.panel8.Location = new System.Drawing.Point(198, 552);
            this.panel8.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(169, 29);
            this.panel8.TabIndex = 15;
            this.panel8.TabStop = true;
            // 
            // rbGlobalPartyBindingYes
            // 
            this.rbGlobalPartyBindingYes.AutoSize = true;
            this.rbGlobalPartyBindingYes.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.rbGlobalPartyBindingYes.Location = new System.Drawing.Point(13, 4);
            this.rbGlobalPartyBindingYes.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.rbGlobalPartyBindingYes.Name = "rbGlobalPartyBindingYes";
            this.rbGlobalPartyBindingYes.Size = new System.Drawing.Size(64, 29);
            this.rbGlobalPartyBindingYes.TabIndex = 24;
            this.rbGlobalPartyBindingYes.Text = "Yes";
            this.rbGlobalPartyBindingYes.UseVisualStyleBackColor = true;
            this.rbGlobalPartyBindingYes.CheckedChanged += new System.EventHandler(this.rbGlobalPartyBindingYes_CheckedChanged);
            // 
            // rbGlobalPartyBindingNo
            // 
            this.rbGlobalPartyBindingNo.AutoSize = true;
            this.rbGlobalPartyBindingNo.Checked = true;
            this.rbGlobalPartyBindingNo.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.rbGlobalPartyBindingNo.Location = new System.Drawing.Point(103, 5);
            this.rbGlobalPartyBindingNo.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.rbGlobalPartyBindingNo.Name = "rbGlobalPartyBindingNo";
            this.rbGlobalPartyBindingNo.Size = new System.Drawing.Size(62, 29);
            this.rbGlobalPartyBindingNo.TabIndex = 25;
            this.rbGlobalPartyBindingNo.TabStop = true;
            this.rbGlobalPartyBindingNo.Text = "No";
            this.rbGlobalPartyBindingNo.UseVisualStyleBackColor = true;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.label12.ForeColor = System.Drawing.Color.Black;
            this.label12.Location = new System.Drawing.Point(20, 559);
            this.label12.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(184, 25);
            this.label12.TabIndex = 40;
            this.label12.Text = "Global Party Binding";
            // 
            // panel9
            // 
            this.panel9.Controls.Add(this.rbBizTalkAppNo);
            this.panel9.Controls.Add(this.rbBizTalkAppYes);
            this.panel9.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.panel9.Location = new System.Drawing.Point(198, 465);
            this.panel9.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(169, 29);
            this.panel9.TabIndex = 13;
            this.panel9.TabStop = true;
            // 
            // rbBizTalkAppNo
            // 
            this.rbBizTalkAppNo.AutoSize = true;
            this.rbBizTalkAppNo.Checked = true;
            this.rbBizTalkAppNo.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.rbBizTalkAppNo.Location = new System.Drawing.Point(104, 4);
            this.rbBizTalkAppNo.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.rbBizTalkAppNo.Name = "rbBizTalkAppNo";
            this.rbBizTalkAppNo.Size = new System.Drawing.Size(62, 29);
            this.rbBizTalkAppNo.TabIndex = 21;
            this.rbBizTalkAppNo.TabStop = true;
            this.rbBizTalkAppNo.Text = "No";
            this.rbBizTalkAppNo.UseVisualStyleBackColor = true;
            // 
            // rbBizTalkAppYes
            // 
            this.rbBizTalkAppYes.AutoSize = true;
            this.rbBizTalkAppYes.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbBizTalkAppYes.Location = new System.Drawing.Point(13, 6);
            this.rbBizTalkAppYes.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.rbBizTalkAppYes.Name = "rbBizTalkAppYes";
            this.rbBizTalkAppYes.Size = new System.Drawing.Size(71, 29);
            this.rbBizTalkAppYes.TabIndex = 20;
            this.rbBizTalkAppYes.Text = "Yes";
            this.rbBizTalkAppYes.UseVisualStyleBackColor = true;
            this.rbBizTalkAppYes.CheckedChanged += new System.EventHandler(this.rbBizTalkAppYes_CheckedChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.label13.ForeColor = System.Drawing.Color.Black;
            this.label13.Location = new System.Drawing.Point(20, 472);
            this.label13.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(170, 25);
            this.label13.TabIndex = 44;
            this.label13.Text = "BizTalk Application";
            // 
            // btnExportOperations
            // 
            this.btnExportOperations.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(128)))), ((int)(((byte)(193)))));
            this.btnExportOperations.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportOperations.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.btnExportOperations.ForeColor = System.Drawing.Color.White;
            this.btnExportOperations.Location = new System.Drawing.Point(773, 76);
            this.btnExportOperations.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.btnExportOperations.Name = "btnExportOperations";
            this.btnExportOperations.Size = new System.Drawing.Size(124, 34);
            this.btnExportOperations.TabIndex = 17;
            this.btnExportOperations.Text = "Export";
            this.btnExportOperations.UseVisualStyleBackColor = false;
            this.btnExportOperations.Click += new System.EventHandler(this.btnExportOperations_Click);
            // 
            // btnImportOperations
            // 
            this.btnImportOperations.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(128)))), ((int)(((byte)(193)))));
            this.btnImportOperations.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnImportOperations.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.btnImportOperations.ForeColor = System.Drawing.Color.White;
            this.btnImportOperations.Location = new System.Drawing.Point(772, 121);
            this.btnImportOperations.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.btnImportOperations.Name = "btnImportOperations";
            this.btnImportOperations.Size = new System.Drawing.Size(124, 34);
            this.btnImportOperations.TabIndex = 18;
            this.btnImportOperations.Text = "Import";
            this.btnImportOperations.UseVisualStyleBackColor = false;
            this.btnImportOperations.Click += new System.EventHandler(this.btnImportOperations_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.label10.ForeColor = System.Drawing.Color.Black;
            this.label10.Location = new System.Drawing.Point(384, 159);
            this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(169, 25);
            this.label10.TabIndex = 49;
            this.label10.Text = "Progress Summary";
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.rbFileCopyYes);
            this.panel6.Controls.Add(this.rbFileCopyNo);
            this.panel6.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.panel6.Location = new System.Drawing.Point(198, 209);
            this.panel6.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(170, 29);
            this.panel6.TabIndex = 7;
            this.panel6.TabStop = true;
            // 
            // rbFileCopyYes
            // 
            this.rbFileCopyYes.AutoSize = true;
            this.rbFileCopyYes.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.rbFileCopyYes.Location = new System.Drawing.Point(13, 5);
            this.rbFileCopyYes.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.rbFileCopyYes.Name = "rbFileCopyYes";
            this.rbFileCopyYes.Size = new System.Drawing.Size(64, 29);
            this.rbFileCopyYes.TabIndex = 0;
            this.rbFileCopyYes.TabStop = true;
            this.rbFileCopyYes.Text = "Yes";
            this.rbFileCopyYes.UseVisualStyleBackColor = true;
            this.rbFileCopyYes.CheckedChanged += new System.EventHandler(this.rbFileCopyYes_CheckedChanged);
            // 
            // rbFileCopyNo
            // 
            this.rbFileCopyNo.AutoSize = true;
            this.rbFileCopyNo.Checked = true;
            this.rbFileCopyNo.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.rbFileCopyNo.Location = new System.Drawing.Point(104, 4);
            this.rbFileCopyNo.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.rbFileCopyNo.Name = "rbFileCopyNo";
            this.rbFileCopyNo.Size = new System.Drawing.Size(62, 29);
            this.rbFileCopyNo.TabIndex = 1;
            this.rbFileCopyNo.TabStop = true;
            this.rbFileCopyNo.Text = "No";
            this.rbFileCopyNo.UseVisualStyleBackColor = true;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.label14.ForeColor = System.Drawing.Color.Black;
            this.label14.Location = new System.Drawing.Point(20, 218);
            this.label14.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(104, 25);
            this.label14.TabIndex = 40;
            this.label14.Text = "File Folder ";
            // 
            // txtConnectionStringDst
            // 
            this.txtConnectionStringDst.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtConnectionStringDst.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtConnectionStringDst.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtConnectionStringDst.Location = new System.Drawing.Point(209, 125);
            this.txtConnectionStringDst.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.txtConnectionStringDst.Name = "txtConnectionStringDst";
            this.txtConnectionStringDst.Size = new System.Drawing.Size(389, 29);
            this.txtConnectionStringDst.TabIndex = 5;
            this.txtConnectionStringDst.Text = "SQL INSTANCE";
            this.txtConnectionStringDst.Validating += new System.ComponentModel.CancelEventHandler(this.txtConnectionStringDst_Validating);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.label15.ForeColor = System.Drawing.Color.Black;
            this.label15.Location = new System.Drawing.Point(20, 129);
            this.label15.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(240, 25);
            this.label15.TabIndex = 54;
            this.label15.Text = "BizTalk Group (Destination)";
            // 
            // cmbBoxServerDst
            // 
            this.cmbBoxServerDst.DropDownHeight = 80;
            this.cmbBoxServerDst.FormattingEnabled = true;
            this.cmbBoxServerDst.IntegralHeight = false;
            this.cmbBoxServerDst.ItemHeight = 21;
            this.cmbBoxServerDst.Location = new System.Drawing.Point(608, 125);
            this.cmbBoxServerDst.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.cmbBoxServerDst.Name = "cmbBoxServerDst";
            this.cmbBoxServerDst.Size = new System.Drawing.Size(140, 29);
            this.cmbBoxServerDst.TabIndex = 6;
            this.cmbBoxServerDst.Visible = false;
            this.cmbBoxServerDst.SelectedIndexChanged += new System.EventHandler(this.cmbBoxServerDst_SelectedIndexChanged);
            // 
            // panelLoginDialog
            // 
            this.panelLoginDialog.BackColor = System.Drawing.SystemColors.ControlLight;
            this.panelLoginDialog.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelLoginDialog.Controls.Add(this.label18);
            this.panelLoginDialog.Controls.Add(this.lblLoginDialog);
            this.panelLoginDialog.Controls.Add(this.label17);
            this.panelLoginDialog.Controls.Add(this.label16);
            this.panelLoginDialog.Controls.Add(this.txtPassword);
            this.panelLoginDialog.Controls.Add(this.txtUserName);
            this.panelLoginDialog.Controls.Add(this.btnSubmit);
            this.panelLoginDialog.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panelLoginDialog.Location = new System.Drawing.Point(398, 218);
            this.panelLoginDialog.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.panelLoginDialog.Name = "panelLoginDialog";
            this.panelLoginDialog.Size = new System.Drawing.Size(339, 220);
            this.panelLoginDialog.TabIndex = 56;
            this.panelLoginDialog.Visible = false;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.ForeColor = System.Drawing.Color.Red;
            this.label18.Location = new System.Drawing.Point(10, 176);
            this.label18.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(69, 25);
            this.label18.TabIndex = 6;
            this.label18.Text = "label18";
            this.label18.Visible = false;
            // 
            // lblLoginDialog
            // 
            this.lblLoginDialog.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLoginDialog.Location = new System.Drawing.Point(24, 8);
            this.lblLoginDialog.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblLoginDialog.Name = "lblLoginDialog";
            this.lblLoginDialog.Size = new System.Drawing.Size(260, 54);
            this.lblLoginDialog.TabIndex = 5;
            this.lblLoginDialog.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.label17.Location = new System.Drawing.Point(20, 100);
            this.label17.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(91, 25);
            this.label17.TabIndex = 4;
            this.label17.Text = "Password";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.label16.Location = new System.Drawing.Point(20, 67);
            this.label16.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(105, 25);
            this.label16.TabIndex = 3;
            this.label16.Text = "User Name";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(107, 97);
            this.txtPassword.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(175, 29);
            this.txtPassword.TabIndex = 2;
            this.txtPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPassword_KeyDown);
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(107, 64);
            this.txtUserName.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(175, 29);
            this.txtUserName.TabIndex = 1;
            // 
            // btnSubmit
            // 
            this.btnSubmit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(128)))), ((int)(((byte)(193)))));
            this.btnSubmit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSubmit.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.btnSubmit.ForeColor = System.Drawing.Color.White;
            this.btnSubmit.Location = new System.Drawing.Point(107, 127);
            this.btnSubmit.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(105, 42);
            this.btnSubmit.TabIndex = 0;
            this.btnSubmit.Text = "Submit";
            this.btnSubmit.UseVisualStyleBackColor = false;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // panel11
            // 
            this.panel11.Controls.Add(this.btnSettings);
            this.panel11.Controls.Add(this.label5);
            this.panel11.Controls.Add(this.panel14);
            this.panel11.Controls.Add(this.panel13);
            this.panel11.Controls.Add(this.label4);
            this.panel11.Controls.Add(this.panel12);
            this.panel11.Controls.Add(this.label3);
            this.panel11.Controls.Add(this.label1);
            this.panel11.Controls.Add(this.panel10);
            this.panel11.Controls.Add(this.cmbBoxServerSrc);
            this.panel11.Controls.Add(this.label2);
            this.panel11.Controls.Add(this.panelLoginDialog);
            this.panel11.Controls.Add(this.txtConnectionString);
            this.panel11.Controls.Add(this.cmbBoxServerDst);
            this.panel11.Controls.Add(this.lblMsg);
            this.panel11.Controls.Add(this.label15);
            this.panel11.Controls.Add(this.txtConnectionStringDst);
            this.panel11.Controls.Add(this.panel1);
            this.panel11.Controls.Add(this.label6);
            this.panel11.Controls.Add(this.panel6);
            this.panel11.Controls.Add(this.label9);
            this.panel11.Controls.Add(this.label14);
            this.panel11.Controls.Add(this.panel2);
            this.panel11.Controls.Add(this.panel5);
            this.panel11.Controls.Add(this.label10);
            this.panel11.Controls.Add(this.label7);
            this.panel11.Controls.Add(this.btnExportOperations);
            this.panel11.Controls.Add(this.label8);
            this.panel11.Controls.Add(this.btnImportOperations);
            this.panel11.Controls.Add(this.label12);
            this.panel11.Controls.Add(this.panel9);
            this.panel11.Controls.Add(this.panel8);
            this.panel11.Controls.Add(this.label13);
            this.panel11.Controls.Add(this.panel3);
            this.panel11.Controls.Add(this.panel7);
            this.panel11.Controls.Add(this.label11);
            this.panel11.Controls.Add(this.panel4);
            this.panel11.Controls.Add(this.richTextBoxLogs);
            this.panel11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel11.Location = new System.Drawing.Point(0, 0);
            this.panel11.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.panel11.Name = "panel11";
            this.panel11.Size = new System.Drawing.Size(920, 630);
            this.panel11.TabIndex = 0;
            this.panel11.TabStop = true;
            // 
            // btnSettings
            // 
            this.btnSettings.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(128)))), ((int)(((byte)(193)))));
            this.btnSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSettings.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.btnSettings.ForeColor = System.Drawing.Color.White;
            this.btnSettings.Location = new System.Drawing.Point(772, 31);
            this.btnSettings.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(124, 34);
            this.btnSettings.TabIndex = 67;
            this.btnSettings.Text = "Settings";
            this.btnSettings.UseVisualStyleBackColor = false;
            this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(20, 175);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(163, 25);
            this.label5.TabIndex = 66;
            this.label5.Text = "Windows Services";
            // 
            // panel14
            // 
            this.panel14.Controls.Add(this.rbServicesYes);
            this.panel14.Controls.Add(this.rbServicesNo);
            this.panel14.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.panel14.Location = new System.Drawing.Point(198, 165);
            this.panel14.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.panel14.Name = "panel14";
            this.panel14.Size = new System.Drawing.Size(170, 29);
            this.panel14.TabIndex = 65;
            this.panel14.TabStop = true;
            // 
            // rbServicesYes
            // 
            this.rbServicesYes.AutoSize = true;
            this.rbServicesYes.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.rbServicesYes.Location = new System.Drawing.Point(13, 3);
            this.rbServicesYes.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.rbServicesYes.Name = "rbServicesYes";
            this.rbServicesYes.Size = new System.Drawing.Size(64, 29);
            this.rbServicesYes.TabIndex = 26;
            this.rbServicesYes.Text = "Yes";
            this.rbServicesYes.UseVisualStyleBackColor = true;
            this.rbServicesYes.CheckedChanged += new System.EventHandler(this.rbServicesYes_CheckedChanged);
            // 
            // rbServicesNo
            // 
            this.rbServicesNo.AutoSize = true;
            this.rbServicesNo.Checked = true;
            this.rbServicesNo.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.rbServicesNo.Location = new System.Drawing.Point(104, 3);
            this.rbServicesNo.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.rbServicesNo.Name = "rbServicesNo";
            this.rbServicesNo.Size = new System.Drawing.Size(62, 29);
            this.rbServicesNo.TabIndex = 27;
            this.rbServicesNo.TabStop = true;
            this.rbServicesNo.Text = "No";
            this.rbServicesNo.UseVisualStyleBackColor = true;
            // 
            // panel13
            // 
            this.panel13.Controls.Add(this.rbApp);
            this.panel13.Controls.Add(this.rbBizTalk);
            this.panel13.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.panel13.Location = new System.Drawing.Point(197, 42);
            this.panel13.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.panel13.Name = "panel13";
            this.panel13.Size = new System.Drawing.Size(170, 29);
            this.panel13.TabIndex = 2;
            this.panel13.TabStop = true;
            // 
            // rbApp
            // 
            this.rbApp.AutoSize = true;
            this.rbApp.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.rbApp.Location = new System.Drawing.Point(13, 5);
            this.rbApp.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.rbApp.Name = "rbApp";
            this.rbApp.Size = new System.Drawing.Size(71, 29);
            this.rbApp.TabIndex = 0;
            this.rbApp.TabStop = true;
            this.rbApp.Text = "App";
            this.rbApp.UseVisualStyleBackColor = true;
            this.rbApp.CheckedChanged += new System.EventHandler(this.rbApp_CheckedChanged);
            // 
            // rbBizTalk
            // 
            this.rbBizTalk.AutoSize = true;
            this.rbBizTalk.Checked = true;
            this.rbBizTalk.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.rbBizTalk.Location = new System.Drawing.Point(104, 5);
            this.rbBizTalk.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.rbBizTalk.Name = "rbBizTalk";
            this.rbBizTalk.Size = new System.Drawing.Size(94, 29);
            this.rbBizTalk.TabIndex = 1;
            this.rbBizTalk.TabStop = true;
            this.rbBizTalk.Text = "BizTalk";
            this.rbBizTalk.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(20, 49);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(109, 25);
            this.label4.TabIndex = 63;
            this.label4.Text = "Server Type";
            // 
            // panel12
            // 
            this.panel12.Controls.Add(this.rbMigrate);
            this.panel12.Controls.Add(this.rbBackup);
            this.panel12.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.panel12.Location = new System.Drawing.Point(198, 6);
            this.panel12.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.panel12.Name = "panel12";
            this.panel12.Size = new System.Drawing.Size(170, 29);
            this.panel12.TabIndex = 1;
            this.panel12.TabStop = true;
            // 
            // rbMigrate
            // 
            this.rbMigrate.AutoSize = true;
            this.rbMigrate.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.rbMigrate.Location = new System.Drawing.Point(13, 5);
            this.rbMigrate.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.rbMigrate.Name = "rbMigrate";
            this.rbMigrate.Size = new System.Drawing.Size(103, 29);
            this.rbMigrate.TabIndex = 0;
            this.rbMigrate.TabStop = true;
            this.rbMigrate.Text = "Migrate";
            this.rbMigrate.UseVisualStyleBackColor = true;
            this.rbMigrate.CheckedChanged += new System.EventHandler(this.rbMigrate_CheckedChanged);
            // 
            // rbBackup
            // 
            this.rbBackup.AutoSize = true;
            this.rbBackup.Checked = true;
            this.rbBackup.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.rbBackup.Location = new System.Drawing.Point(104, 5);
            this.rbBackup.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.rbBackup.Name = "rbBackup";
            this.rbBackup.Size = new System.Drawing.Size(98, 29);
            this.rbBackup.TabIndex = 1;
            this.rbBackup.TabStop = true;
            this.rbBackup.Text = "Backup";
            this.rbBackup.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(20, 13);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 25);
            this.label3.TabIndex = 61;
            this.label3.Text = "Mode";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(19, 604);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(140, 25);
            this.label1.TabIndex = 59;
            this.label1.Text = "BAM Definition";
            // 
            // panel10
            // 
            this.panel10.Controls.Add(this.rbBamYes);
            this.panel10.Controls.Add(this.rbBamNo);
            this.panel10.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.panel10.Location = new System.Drawing.Point(197, 596);
            this.panel10.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(170, 29);
            this.panel10.TabIndex = 16;
            this.panel10.TabStop = true;
            // 
            // rbBamYes
            // 
            this.rbBamYes.AutoSize = true;
            this.rbBamYes.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.rbBamYes.Location = new System.Drawing.Point(13, 3);
            this.rbBamYes.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.rbBamYes.Name = "rbBamYes";
            this.rbBamYes.Size = new System.Drawing.Size(64, 29);
            this.rbBamYes.TabIndex = 26;
            this.rbBamYes.Text = "Yes";
            this.rbBamYes.UseVisualStyleBackColor = true;
            this.rbBamYes.CheckedChanged += new System.EventHandler(this.rbBamYes_CheckedChanged);
            // 
            // rbBamNo
            // 
            this.rbBamNo.AutoSize = true;
            this.rbBamNo.Checked = true;
            this.rbBamNo.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.rbBamNo.Location = new System.Drawing.Point(104, 3);
            this.rbBamNo.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.rbBamNo.Name = "rbBamNo";
            this.rbBamNo.Size = new System.Drawing.Size(62, 29);
            this.rbBamNo.TabIndex = 27;
            this.rbBamNo.TabStop = true;
            this.rbBamNo.Text = "No";
            this.rbBamNo.UseVisualStyleBackColor = true;
            // 
            // cmbBoxServerSrc
            // 
            this.cmbBoxServerSrc.DropDownHeight = 80;
            this.cmbBoxServerSrc.FormattingEnabled = true;
            this.cmbBoxServerSrc.IntegralHeight = false;
            this.cmbBoxServerSrc.ItemHeight = 21;
            this.cmbBoxServerSrc.Location = new System.Drawing.Point(608, 83);
            this.cmbBoxServerSrc.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.cmbBoxServerSrc.Name = "cmbBoxServerSrc";
            this.cmbBoxServerSrc.Size = new System.Drawing.Size(140, 29);
            this.cmbBoxServerSrc.TabIndex = 4;
            this.cmbBoxServerSrc.Visible = false;
            this.cmbBoxServerSrc.SelectedIndexChanged += new System.EventHandler(this.cmbBoxServerSrc_SelectedIndexChanged);
            // 
            // lblMsg
            // 
            this.lblMsg.AutoSize = true;
            this.lblMsg.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblMsg.ForeColor = System.Drawing.Color.Black;
            this.lblMsg.Location = new System.Drawing.Point(20, 300);
            this.lblMsg.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Size = new System.Drawing.Size(138, 25);
            this.lblMsg.TabIndex = 3;
            this.lblMsg.Text = "WebSites/Apps";
            // 
            // BizTalkAdminOperations
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(920, 630);
            this.Controls.Add(this.panel11);
            this.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.ForeColor = System.Drawing.Color.Black;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.MaximizeBox = false;
            this.Name = "BizTalkAdminOperations";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Migration Tool";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.BizTalkAdminOperations_FormClosing);
            this.Load += new System.EventHandler(this.BizTalkAdminOperations_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.panel8.ResumeLayout(false);
            this.panel8.PerformLayout();
            this.panel9.ResumeLayout(false);
            this.panel9.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panelLoginDialog.ResumeLayout(false);
            this.panelLoginDialog.PerformLayout();
            this.panel11.ResumeLayout(false);
            this.panel11.PerformLayout();
            this.panel14.ResumeLayout(false);
            this.panel14.PerformLayout();
            this.panel13.ResumeLayout(false);
            this.panel13.PerformLayout();
            this.panel12.ResumeLayout(false);
            this.panel12.PerformLayout();
            this.panel10.ResumeLayout(false);
            this.panel10.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtConnectionString;
        private System.Windows.Forms.RichTextBox richTextBoxLogs;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton rbWebsiteYes;
        private System.Windows.Forms.RadioButton rbWebsiteNo;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RadioButton rbAppPoolYes;
        private System.Windows.Forms.RadioButton rbAppPoolNo;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.RadioButton rbCertificateYes;
        private System.Windows.Forms.RadioButton rbCertificateNo;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.RadioButton rbHandlersYes;
        private System.Windows.Forms.RadioButton rbHandlersNo;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.RadioButton rbHostInstanceYes;
        private System.Windows.Forms.RadioButton rbHostInstanceNo;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.RadioButton rbBizTalkAssembliesYes;
        private System.Windows.Forms.RadioButton rbBizTalkAssembliesNo;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.RadioButton rbGlobalPartyBindingYes;
        private System.Windows.Forms.RadioButton rbGlobalPartyBindingNo;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.RadioButton rbBizTalkAppYes;
        private System.Windows.Forms.RadioButton rbBizTalkAppNo;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button btnExportOperations;
        private System.Windows.Forms.Button btnImportOperations;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.RadioButton rbFileCopyYes;
        private System.Windows.Forms.RadioButton rbFileCopyNo;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtConnectionStringDst;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.ComboBox cmbBoxServerDst;
        private System.Windows.Forms.Panel panelLoginDialog;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.Label lblLoginDialog;
        private System.Windows.Forms.Panel panel11;
        private System.Windows.Forms.ComboBox cmbBoxServerSrc;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel10;
        private System.Windows.Forms.RadioButton rbBamYes;
        private System.Windows.Forms.RadioButton rbBamNo;
        private System.Windows.Forms.Panel panel13;
        private System.Windows.Forms.RadioButton rbApp;
        private System.Windows.Forms.RadioButton rbBizTalk;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel12;
        private System.Windows.Forms.RadioButton rbMigrate;
        private System.Windows.Forms.RadioButton rbBackup;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblMsg;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panel14;
        private System.Windows.Forms.RadioButton rbServicesYes;
        private System.Windows.Forms.RadioButton rbServicesNo;
        private System.Windows.Forms.Button btnSettings;
    }
}

