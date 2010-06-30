using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using IndianHealthService.BMXNet;
using System.Security.Principal;
using System.Diagnostics;
using System.Xml.Xsl;
using System.Xml;

namespace IndianHealthService.BMXNet
{
	/// <summary>
	/// BMXNet demo form.
	/// </summary>
	public partial class frmBMXNetTest : System.Windows.Forms.Form
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmBMXNetTest()
		{
			InitializeComponent();
            m_ci = new BMXNetConnectInfo();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (m_ci != null)
					m_ci.CloseConnection();
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpaQuery = new System.Windows.Forms.TabPage();
            this.panGrid = new System.Windows.Forms.Panel();
            this.dataGrid2 = new System.Windows.Forms.DataGrid();
            this.panQuery = new System.Windows.Forms.Panel();
            this.cmdAddAll = new System.Windows.Forms.Button();
            this.cmdXML = new System.Windows.Forms.Button();
            this.cmdTest3 = new System.Windows.Forms.Button();
            this.cmdTest4 = new System.Windows.Forms.Button();
            this.cmdCancelChanges = new System.Windows.Forms.Button();
            this.cmdAcceptChanges = new System.Windows.Forms.Button();
            this.cmdExecuteQuery = new System.Windows.Forms.Button();
            this.txtCommand = new System.Windows.Forms.TextBox();
            this.tpaControls = new System.Windows.Forms.TabPage();
            this.grpControls = new System.Windows.Forms.GroupBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.lstCDSex = new System.Windows.Forms.ListBox();
            this.dtpCDDOB = new System.Windows.Forms.DateTimePicker();
            this.calCDDOB = new System.Windows.Forms.MonthCalendar();
            this.cboCDSelect = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtCDName = new System.Windows.Forms.TextBox();
            this.lblCDIntro = new System.Windows.Forms.Label();
            this.cmdCDLoad = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtCDSex = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtCDDOB = new System.Windows.Forms.TextBox();
            this.txtCDSSN = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tpaConnection = new System.Windows.Forms.TabPage();
            this.cmdStopLogging = new System.Windows.Forms.Button();
            this.cmdStartLogging = new System.Windows.Forms.Button();
            this.cmdTestReceiveTimeout = new System.Windows.Forms.Button();
            this.cmdTestSilent = new System.Windows.Forms.Button();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.txtDivision = new System.Windows.Forms.TextBox();
            this.cmdChangeDivision = new System.Windows.Forms.Button();
            this.cmdChangeContext = new System.Windows.Forms.Button();
            this.txtContext = new System.Windows.Forms.TextBox();
            this.cmdChangeUser = new System.Windows.Forms.Button();
            this.cmdChangeServer = new System.Windows.Forms.Button();
            this.tpaOther = new System.Windows.Forms.TabPage();
            this.label19 = new System.Windows.Forms.Label();
            this.grdAsyncResult = new System.Windows.Forms.DataGrid();
            this.label18 = new System.Windows.Forms.Label();
            this.txtAsyncCommand = new System.Windows.Forms.TextBox();
            this.cmdAsyncCall = new System.Windows.Forms.Button();
            this.label17 = new System.Windows.Forms.Label();
            this.txtEventMessages = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.chkEnableEvents = new System.Windows.Forms.CheckBox();
            this.nudEventPollingInterval = new System.Windows.Forms.NumericUpDown();
            this.chkEventRaiseBack = new System.Windows.Forms.CheckBox();
            this.label15 = new System.Windows.Forms.Label();
            this.txtEventParam = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.cmdUnregisterEvent = new System.Windows.Forms.Button();
            this.txtUnregisterEvent = new System.Windows.Forms.TextBox();
            this.cmdRaiseEvent = new System.Windows.Forms.Button();
            this.txtRaiseEvent = new System.Windows.Forms.TextBox();
            this.cmdRegisterEvent = new System.Windows.Forms.Button();
            this.txtRegisterEvent = new System.Windows.Forms.TextBox();
            this.grpPiece = new System.Windows.Forms.GroupBox();
            this.txtDelim = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtEnd = new System.Windows.Forms.TextBox();
            this.txtResult = new System.Windows.Forms.TextBox();
            this.txtNumber = new System.Windows.Forms.TextBox();
            this.txtInput = new System.Windows.Forms.TextBox();
            this.cmdTestPiece = new System.Windows.Forms.Button();
            this.cmdAcquireLock = new System.Windows.Forms.Button();
            this.cmdReleaseLock = new System.Windows.Forms.Button();
            this.cmdEventPollingInterval = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tpaQuery.SuspendLayout();
            this.panGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid2)).BeginInit();
            this.panQuery.SuspendLayout();
            this.tpaControls.SuspendLayout();
            this.grpControls.SuspendLayout();
            this.tpaConnection.SuspendLayout();
            this.tpaOther.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdAsyncResult)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudEventPollingInterval)).BeginInit();
            this.grpPiece.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tpaQuery);
            this.tabControl1.Controls.Add(this.tpaControls);
            this.tabControl1.Controls.Add(this.tpaConnection);
            this.tabControl1.Controls.Add(this.tpaOther);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(736, 566);
            this.tabControl1.TabIndex = 0;
            // 
            // tpaQuery
            // 
            this.tpaQuery.Controls.Add(this.panGrid);
            this.tpaQuery.Controls.Add(this.panQuery);
            this.tpaQuery.Location = new System.Drawing.Point(4, 22);
            this.tpaQuery.Name = "tpaQuery";
            this.tpaQuery.Size = new System.Drawing.Size(728, 540);
            this.tpaQuery.TabIndex = 0;
            this.tpaQuery.Text = "RPC and Query Testing";
            // 
            // panGrid
            // 
            this.panGrid.Controls.Add(this.dataGrid2);
            this.panGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panGrid.Location = new System.Drawing.Point(0, 216);
            this.panGrid.Name = "panGrid";
            this.panGrid.Size = new System.Drawing.Size(728, 324);
            this.panGrid.TabIndex = 43;
            // 
            // dataGrid2
            // 
            this.dataGrid2.AccessibleName = "DataGrid";
            this.dataGrid2.AccessibleRole = System.Windows.Forms.AccessibleRole.Table;
            this.dataGrid2.DataMember = "";
            this.dataGrid2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGrid2.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.dataGrid2.Location = new System.Drawing.Point(0, 0);
            this.dataGrid2.Name = "dataGrid2";
            this.dataGrid2.Size = new System.Drawing.Size(728, 324);
            this.dataGrid2.TabIndex = 40;
            this.dataGrid2.Navigate += new System.Windows.Forms.NavigateEventHandler(this.dataGrid2_Navigate);
            // 
            // panQuery
            // 
            this.panQuery.Controls.Add(this.cmdAddAll);
            this.panQuery.Controls.Add(this.cmdXML);
            this.panQuery.Controls.Add(this.cmdTest3);
            this.panQuery.Controls.Add(this.cmdTest4);
            this.panQuery.Controls.Add(this.cmdCancelChanges);
            this.panQuery.Controls.Add(this.cmdAcceptChanges);
            this.panQuery.Controls.Add(this.cmdExecuteQuery);
            this.panQuery.Controls.Add(this.txtCommand);
            this.panQuery.Dock = System.Windows.Forms.DockStyle.Top;
            this.panQuery.Location = new System.Drawing.Point(0, 0);
            this.panQuery.Name = "panQuery";
            this.panQuery.Size = new System.Drawing.Size(728, 216);
            this.panQuery.TabIndex = 42;
            // 
            // cmdAddAll
            // 
            this.cmdAddAll.Location = new System.Drawing.Point(344, 160);
            this.cmdAddAll.Name = "cmdAddAll";
            this.cmdAddAll.Size = new System.Drawing.Size(56, 32);
            this.cmdAddAll.TabIndex = 78;
            this.cmdAddAll.Text = "Add All";
            this.cmdAddAll.Click += new System.EventHandler(this.cmdAddAll_Click);
            // 
            // cmdXML
            // 
            this.cmdXML.Location = new System.Drawing.Point(448, 160);
            this.cmdXML.Name = "cmdXML";
            this.cmdXML.Size = new System.Drawing.Size(56, 32);
            this.cmdXML.TabIndex = 77;
            this.cmdXML.Text = "XML";
            this.cmdXML.Click += new System.EventHandler(this.cmdXML_Click);
            // 
            // cmdTest3
            // 
            this.cmdTest3.Location = new System.Drawing.Point(648, 160);
            this.cmdTest3.Name = "cmdTest3";
            this.cmdTest3.Size = new System.Drawing.Size(72, 32);
            this.cmdTest3.TabIndex = 76;
            this.cmdTest3.Text = "Low-Level TransmitRPC Demo";
            this.cmdTest3.Click += new System.EventHandler(this.cmdTest3_Click);
            // 
            // cmdTest4
            // 
            this.cmdTest4.Location = new System.Drawing.Point(560, 160);
            this.cmdTest4.Name = "cmdTest4";
            this.cmdTest4.Size = new System.Drawing.Size(80, 32);
            this.cmdTest4.TabIndex = 75;
            this.cmdTest4.Text = "Table Relations Demo";
            this.cmdTest4.Click += new System.EventHandler(this.cmdTest4_Click);
            // 
            // cmdCancelChanges
            // 
            this.cmdCancelChanges.Location = new System.Drawing.Point(232, 160);
            this.cmdCancelChanges.Name = "cmdCancelChanges";
            this.cmdCancelChanges.Size = new System.Drawing.Size(96, 32);
            this.cmdCancelChanges.TabIndex = 74;
            this.cmdCancelChanges.Text = "Cancel Changes";
            this.cmdCancelChanges.Click += new System.EventHandler(this.cmdCancelChanges_Click);
            // 
            // cmdAcceptChanges
            // 
            this.cmdAcceptChanges.Location = new System.Drawing.Point(120, 160);
            this.cmdAcceptChanges.Name = "cmdAcceptChanges";
            this.cmdAcceptChanges.Size = new System.Drawing.Size(96, 32);
            this.cmdAcceptChanges.TabIndex = 73;
            this.cmdAcceptChanges.Text = "Accept Changes";
            this.cmdAcceptChanges.Click += new System.EventHandler(this.cmdAcceptChanges_Click);
            // 
            // cmdExecuteQuery
            // 
            this.cmdExecuteQuery.Location = new System.Drawing.Point(16, 160);
            this.cmdExecuteQuery.Name = "cmdExecuteQuery";
            this.cmdExecuteQuery.Size = new System.Drawing.Size(96, 32);
            this.cmdExecuteQuery.TabIndex = 71;
            this.cmdExecuteQuery.Text = "Execute Query";
            this.cmdExecuteQuery.Click += new System.EventHandler(this.cmdExecuteQuery_Click);
            // 
            // txtCommand
            // 
            this.txtCommand.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtCommand.Location = new System.Drawing.Point(0, 0);
            this.txtCommand.Multiline = true;
            this.txtCommand.Name = "txtCommand";
            this.txtCommand.Size = new System.Drawing.Size(728, 152);
            this.txtCommand.TabIndex = 72;
            this.txtCommand.Text = "SELECT NAME, AGE, DOB FROM VA_PATIENT WHERE NAME LIKE \'END%\'";
            // 
            // tpaControls
            // 
            this.tpaControls.Controls.Add(this.grpControls);
            this.tpaControls.Location = new System.Drawing.Point(4, 22);
            this.tpaControls.Name = "tpaControls";
            this.tpaControls.Size = new System.Drawing.Size(728, 540);
            this.tpaControls.TabIndex = 3;
            this.tpaControls.Text = "Controls Demo";
            // 
            // grpControls
            // 
            this.grpControls.Controls.Add(this.label13);
            this.grpControls.Controls.Add(this.label9);
            this.grpControls.Controls.Add(this.lstCDSex);
            this.grpControls.Controls.Add(this.dtpCDDOB);
            this.grpControls.Controls.Add(this.calCDDOB);
            this.grpControls.Controls.Add(this.cboCDSelect);
            this.grpControls.Controls.Add(this.label1);
            this.grpControls.Controls.Add(this.txtCDName);
            this.grpControls.Controls.Add(this.lblCDIntro);
            this.grpControls.Controls.Add(this.cmdCDLoad);
            this.grpControls.Controls.Add(this.label2);
            this.grpControls.Controls.Add(this.txtCDSex);
            this.grpControls.Controls.Add(this.label3);
            this.grpControls.Controls.Add(this.txtCDDOB);
            this.grpControls.Controls.Add(this.txtCDSSN);
            this.grpControls.Controls.Add(this.label8);
            this.grpControls.Controls.Add(this.label10);
            this.grpControls.Controls.Add(this.label11);
            this.grpControls.Controls.Add(this.label12);
            this.grpControls.Controls.Add(this.label4);
            this.grpControls.Location = new System.Drawing.Point(0, 8);
            this.grpControls.Name = "grpControls";
            this.grpControls.Size = new System.Drawing.Size(704, 520);
            this.grpControls.TabIndex = 72;
            this.grpControls.TabStop = false;
            this.grpControls.Text = "Control demo";
            // 
            // label13
            // 
            this.label13.Location = new System.Drawing.Point(368, 128);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(176, 24);
            this.label13.TabIndex = 9;
            this.label13.Text = "<-- Select a patient after loading the dataset";
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(144, 80);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(312, 24);
            this.label9.TabIndex = 8;
            this.label9.Text = "<---Press this button to load first 50 patients from VA Patient file into a datas" +
                "et.";
            // 
            // lstCDSex
            // 
            this.lstCDSex.Items.AddRange(new object[] {
            "MALE",
            "FEMALE"});
            this.lstCDSex.Location = new System.Drawing.Point(224, 264);
            this.lstCDSex.Name = "lstCDSex";
            this.lstCDSex.Size = new System.Drawing.Size(112, 17);
            this.lstCDSex.TabIndex = 7;
            // 
            // dtpCDDOB
            // 
            this.dtpCDDOB.CustomFormat = "MMMM dd, yyyy";
            this.dtpCDDOB.Location = new System.Drawing.Point(72, 392);
            this.dtpCDDOB.Name = "dtpCDDOB";
            this.dtpCDDOB.Size = new System.Drawing.Size(264, 20);
            this.dtpCDDOB.TabIndex = 6;
            // 
            // calCDDOB
            // 
            this.calCDDOB.Location = new System.Drawing.Point(360, 344);
            this.calCDDOB.Name = "calCDDOB";
            this.calCDDOB.TabIndex = 5;
            // 
            // cboCDSelect
            // 
            this.cboCDSelect.Location = new System.Drawing.Point(96, 128);
            this.cboCDSelect.Name = "cboCDSelect";
            this.cboCDSelect.Size = new System.Drawing.Size(256, 21);
            this.cboCDSelect.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(16, 168);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 16);
            this.label1.TabIndex = 3;
            this.label1.Text = "Name:";
            // 
            // txtCDName
            // 
            this.txtCDName.Location = new System.Drawing.Point(72, 168);
            this.txtCDName.Name = "txtCDName";
            this.txtCDName.Size = new System.Drawing.Size(280, 20);
            this.txtCDName.TabIndex = 2;
            // 
            // lblCDIntro
            // 
            this.lblCDIntro.Location = new System.Drawing.Point(24, 24);
            this.lblCDIntro.Name = "lblCDIntro";
            this.lblCDIntro.Size = new System.Drawing.Size(376, 32);
            this.lblCDIntro.TabIndex = 1;
            this.lblCDIntro.Text = "This panel demonstrates how to load data into various kinds of controls from a da" +
                "taset.";
            // 
            // cmdCDLoad
            // 
            this.cmdCDLoad.Location = new System.Drawing.Point(32, 72);
            this.cmdCDLoad.Name = "cmdCDLoad";
            this.cmdCDLoad.Size = new System.Drawing.Size(96, 32);
            this.cmdCDLoad.TabIndex = 0;
            this.cmdCDLoad.Text = "Load Dataset";
            this.cmdCDLoad.Click += new System.EventHandler(this.cmdCDLoad_Click);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(8, 128);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 16);
            this.label2.TabIndex = 3;
            this.label2.Text = "Select Patient:";
            // 
            // txtCDSex
            // 
            this.txtCDSex.Location = new System.Drawing.Point(72, 264);
            this.txtCDSex.Name = "txtCDSex";
            this.txtCDSex.Size = new System.Drawing.Size(72, 20);
            this.txtCDSex.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(72, 240);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(128, 16);
            this.label3.TabIndex = 3;
            this.label3.Text = "Sex (Text Control):";
            // 
            // txtCDDOB
            // 
            this.txtCDDOB.Location = new System.Drawing.Point(72, 344);
            this.txtCDDOB.Name = "txtCDDOB";
            this.txtCDDOB.Size = new System.Drawing.Size(144, 20);
            this.txtCDDOB.TabIndex = 2;
            // 
            // txtCDSSN
            // 
            this.txtCDSSN.Location = new System.Drawing.Point(72, 200);
            this.txtCDSSN.Name = "txtCDSSN";
            this.txtCDSSN.Size = new System.Drawing.Size(280, 20);
            this.txtCDSSN.TabIndex = 2;
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(16, 200);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(40, 16);
            this.label8.TabIndex = 3;
            this.label8.Text = "SSN:";
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(72, 368);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(136, 16);
            this.label10.TabIndex = 3;
            this.label10.Text = "DOB (DateTime Picker):";
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(72, 320);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(128, 16);
            this.label11.TabIndex = 3;
            this.label11.Text = "DOB (Text Control):";
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(360, 320);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(200, 16);
            this.label12.TabIndex = 3;
            this.label12.Text = "DOB (Calendar Control):";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(224, 240);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(128, 16);
            this.label4.TabIndex = 3;
            this.label4.Text = "Sex (ListBox Control):";
            // 
            // tpaConnection
            // 
            this.tpaConnection.Controls.Add(this.cmdStopLogging);
            this.tpaConnection.Controls.Add(this.cmdStartLogging);
            this.tpaConnection.Controls.Add(this.cmdTestReceiveTimeout);
            this.tpaConnection.Controls.Add(this.cmdTestSilent);
            this.tpaConnection.Controls.Add(this.txtServer);
            this.tpaConnection.Controls.Add(this.txtUser);
            this.tpaConnection.Controls.Add(this.txtDivision);
            this.tpaConnection.Controls.Add(this.cmdChangeDivision);
            this.tpaConnection.Controls.Add(this.cmdChangeContext);
            this.tpaConnection.Controls.Add(this.txtContext);
            this.tpaConnection.Controls.Add(this.cmdChangeUser);
            this.tpaConnection.Controls.Add(this.cmdChangeServer);
            this.tpaConnection.Location = new System.Drawing.Point(4, 22);
            this.tpaConnection.Name = "tpaConnection";
            this.tpaConnection.Size = new System.Drawing.Size(605, 465);
            this.tpaConnection.TabIndex = 1;
            this.tpaConnection.Text = "Connection";
            // 
            // cmdStopLogging
            // 
            this.cmdStopLogging.Location = new System.Drawing.Point(16, 393);
            this.cmdStopLogging.Name = "cmdStopLogging";
            this.cmdStopLogging.Size = new System.Drawing.Size(152, 28);
            this.cmdStopLogging.TabIndex = 85;
            this.cmdStopLogging.Text = "Stop Logging";
            this.cmdStopLogging.UseVisualStyleBackColor = true;
            this.cmdStopLogging.Click += new System.EventHandler(this.cmdStopLogging_Click);
            // 
            // cmdStartLogging
            // 
            this.cmdStartLogging.Location = new System.Drawing.Point(16, 346);
            this.cmdStartLogging.Name = "cmdStartLogging";
            this.cmdStartLogging.Size = new System.Drawing.Size(152, 28);
            this.cmdStartLogging.TabIndex = 84;
            this.cmdStartLogging.Text = "Start Logging";
            this.cmdStartLogging.UseVisualStyleBackColor = true;
            this.cmdStartLogging.Click += new System.EventHandler(this.cmdStartLogging_Click);
            // 
            // cmdTestReceiveTimeout
            // 
            this.cmdTestReceiveTimeout.Location = new System.Drawing.Point(16, 294);
            this.cmdTestReceiveTimeout.Name = "cmdTestReceiveTimeout";
            this.cmdTestReceiveTimeout.Size = new System.Drawing.Size(152, 28);
            this.cmdTestReceiveTimeout.TabIndex = 83;
            this.cmdTestReceiveTimeout.Text = "Test Receive Timeout";
            this.cmdTestReceiveTimeout.UseVisualStyleBackColor = true;
            this.cmdTestReceiveTimeout.Visible = false;
            this.cmdTestReceiveTimeout.Click += new System.EventHandler(this.cmdTestReceiveTimeout_Click);
            // 
            // cmdTestSilent
            // 
            this.cmdTestSilent.Location = new System.Drawing.Point(16, 248);
            this.cmdTestSilent.Name = "cmdTestSilent";
            this.cmdTestSilent.Size = new System.Drawing.Size(152, 28);
            this.cmdTestSilent.TabIndex = 82;
            this.cmdTestSilent.Text = "Test Silent Login";
            this.cmdTestSilent.UseVisualStyleBackColor = true;
            this.cmdTestSilent.Visible = false;
            this.cmdTestSilent.Click += new System.EventHandler(this.cmdTestSilent_Click);
            // 
            // txtServer
            // 
            this.txtServer.Location = new System.Drawing.Point(176, 24);
            this.txtServer.Name = "txtServer";
            this.txtServer.ReadOnly = true;
            this.txtServer.Size = new System.Drawing.Size(200, 20);
            this.txtServer.TabIndex = 81;
            // 
            // txtUser
            // 
            this.txtUser.Location = new System.Drawing.Point(176, 64);
            this.txtUser.Name = "txtUser";
            this.txtUser.ReadOnly = true;
            this.txtUser.Size = new System.Drawing.Size(200, 20);
            this.txtUser.TabIndex = 80;
            // 
            // txtDivision
            // 
            this.txtDivision.Location = new System.Drawing.Point(176, 104);
            this.txtDivision.Name = "txtDivision";
            this.txtDivision.Size = new System.Drawing.Size(200, 20);
            this.txtDivision.TabIndex = 79;
            // 
            // cmdChangeDivision
            // 
            this.cmdChangeDivision.Location = new System.Drawing.Point(16, 96);
            this.cmdChangeDivision.Name = "cmdChangeDivision";
            this.cmdChangeDivision.Size = new System.Drawing.Size(152, 32);
            this.cmdChangeDivision.TabIndex = 78;
            this.cmdChangeDivision.Text = "Change Division";
            this.cmdChangeDivision.Click += new System.EventHandler(this.cmdChangeDivision_Click);
            // 
            // cmdChangeContext
            // 
            this.cmdChangeContext.Location = new System.Drawing.Point(16, 136);
            this.cmdChangeContext.Name = "cmdChangeContext";
            this.cmdChangeContext.Size = new System.Drawing.Size(152, 32);
            this.cmdChangeContext.TabIndex = 77;
            this.cmdChangeContext.Text = "Change Application Context";
            this.cmdChangeContext.Click += new System.EventHandler(this.cmdChangeContext_Click);
            // 
            // txtContext
            // 
            this.txtContext.Location = new System.Drawing.Point(176, 144);
            this.txtContext.Name = "txtContext";
            this.txtContext.Size = new System.Drawing.Size(200, 20);
            this.txtContext.TabIndex = 76;
            // 
            // cmdChangeUser
            // 
            this.cmdChangeUser.Location = new System.Drawing.Point(16, 56);
            this.cmdChangeUser.Name = "cmdChangeUser";
            this.cmdChangeUser.Size = new System.Drawing.Size(152, 32);
            this.cmdChangeUser.TabIndex = 74;
            this.cmdChangeUser.Text = "Change RPMS User Login";
            this.cmdChangeUser.Click += new System.EventHandler(this.cmdChangeUser_Click);
            // 
            // cmdChangeServer
            // 
            this.cmdChangeServer.Location = new System.Drawing.Point(16, 16);
            this.cmdChangeServer.Name = "cmdChangeServer";
            this.cmdChangeServer.Size = new System.Drawing.Size(152, 32);
            this.cmdChangeServer.TabIndex = 73;
            this.cmdChangeServer.Text = "Change RPMS Server";
            this.cmdChangeServer.Click += new System.EventHandler(this.cmdChangeServer_Click);
            // 
            // tpaOther
            // 
            this.tpaOther.Controls.Add(this.label19);
            this.tpaOther.Controls.Add(this.grdAsyncResult);
            this.tpaOther.Controls.Add(this.label18);
            this.tpaOther.Controls.Add(this.txtAsyncCommand);
            this.tpaOther.Controls.Add(this.cmdAsyncCall);
            this.tpaOther.Controls.Add(this.label17);
            this.tpaOther.Controls.Add(this.txtEventMessages);
            this.tpaOther.Controls.Add(this.label16);
            this.tpaOther.Controls.Add(this.chkEnableEvents);
            this.tpaOther.Controls.Add(this.nudEventPollingInterval);
            this.tpaOther.Controls.Add(this.chkEventRaiseBack);
            this.tpaOther.Controls.Add(this.label15);
            this.tpaOther.Controls.Add(this.txtEventParam);
            this.tpaOther.Controls.Add(this.label14);
            this.tpaOther.Controls.Add(this.cmdUnregisterEvent);
            this.tpaOther.Controls.Add(this.txtUnregisterEvent);
            this.tpaOther.Controls.Add(this.cmdRaiseEvent);
            this.tpaOther.Controls.Add(this.txtRaiseEvent);
            this.tpaOther.Controls.Add(this.cmdRegisterEvent);
            this.tpaOther.Controls.Add(this.txtRegisterEvent);
            this.tpaOther.Controls.Add(this.grpPiece);
            this.tpaOther.Controls.Add(this.cmdAcquireLock);
            this.tpaOther.Controls.Add(this.cmdReleaseLock);
            this.tpaOther.Controls.Add(this.cmdEventPollingInterval);
            this.tpaOther.Location = new System.Drawing.Point(4, 22);
            this.tpaOther.Name = "tpaOther";
            this.tpaOther.Size = new System.Drawing.Size(605, 465);
            this.tpaOther.TabIndex = 2;
            this.tpaOther.Text = "Events";
            // 
            // label19
            // 
            this.label19.Location = new System.Drawing.Point(416, 176);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(160, 16);
            this.label19.TabIndex = 107;
            this.label19.Text = "Asyncronous Result Set";
            // 
            // grdAsyncResult
            // 
            this.grdAsyncResult.DataMember = "";
            this.grdAsyncResult.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.grdAsyncResult.Location = new System.Drawing.Point(416, 192);
            this.grdAsyncResult.Name = "grdAsyncResult";
            this.grdAsyncResult.Size = new System.Drawing.Size(296, 304);
            this.grdAsyncResult.TabIndex = 106;
            // 
            // label18
            // 
            this.label18.Location = new System.Drawing.Point(416, 40);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(160, 16);
            this.label18.TabIndex = 105;
            this.label18.Text = "Command String:";
            // 
            // txtAsyncCommand
            // 
            this.txtAsyncCommand.Location = new System.Drawing.Point(416, 56);
            this.txtAsyncCommand.Multiline = true;
            this.txtAsyncCommand.Name = "txtAsyncCommand";
            this.txtAsyncCommand.Size = new System.Drawing.Size(288, 48);
            this.txtAsyncCommand.TabIndex = 104;
            this.txtAsyncCommand.Text = "BMX DEMO^W^15";
            // 
            // cmdAsyncCall
            // 
            this.cmdAsyncCall.Location = new System.Drawing.Point(416, 120);
            this.cmdAsyncCall.Name = "cmdAsyncCall";
            this.cmdAsyncCall.Size = new System.Drawing.Size(144, 32);
            this.cmdAsyncCall.TabIndex = 103;
            this.cmdAsyncCall.Text = "Make Asynchronous Call";
            this.cmdAsyncCall.Click += new System.EventHandler(this.cmdAsyncCall_Click);
            // 
            // label17
            // 
            this.label17.Location = new System.Drawing.Point(24, 288);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(160, 16);
            this.label17.TabIndex = 102;
            this.label17.Text = "Event Messages";
            // 
            // txtEventMessages
            // 
            this.txtEventMessages.Location = new System.Drawing.Point(24, 304);
            this.txtEventMessages.Multiline = true;
            this.txtEventMessages.Name = "txtEventMessages";
            this.txtEventMessages.Size = new System.Drawing.Size(360, 40);
            this.txtEventMessages.TabIndex = 101;
            // 
            // label16
            // 
            this.label16.Location = new System.Drawing.Point(272, 232);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(96, 16);
            this.label16.TabIndex = 100;
            this.label16.Text = "(milliseconds)";
            // 
            // chkEnableEvents
            // 
            this.chkEnableEvents.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkEnableEvents.Location = new System.Drawing.Point(184, 16);
            this.chkEnableEvents.Name = "chkEnableEvents";
            this.chkEnableEvents.Size = new System.Drawing.Size(104, 16);
            this.chkEnableEvents.TabIndex = 99;
            this.chkEnableEvents.Text = "Enable Events";
            this.chkEnableEvents.CheckedChanged += new System.EventHandler(this.chkEnableEvents_CheckedChanged);
            // 
            // nudEventPollingInterval
            // 
            this.nudEventPollingInterval.Increment = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.nudEventPollingInterval.Location = new System.Drawing.Point(192, 232);
            this.nudEventPollingInterval.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nudEventPollingInterval.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudEventPollingInterval.Name = "nudEventPollingInterval";
            this.nudEventPollingInterval.Size = new System.Drawing.Size(64, 20);
            this.nudEventPollingInterval.TabIndex = 98;
            this.nudEventPollingInterval.Value = new decimal(new int[] {
            3000,
            0,
            0,
            0});
            this.nudEventPollingInterval.ValueChanged += new System.EventHandler(this.nudEventPollingInterval_ValueChanged);
            // 
            // chkEventRaiseBack
            // 
            this.chkEventRaiseBack.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkEventRaiseBack.Checked = true;
            this.chkEventRaiseBack.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkEventRaiseBack.Location = new System.Drawing.Point(192, 160);
            this.chkEventRaiseBack.Name = "chkEventRaiseBack";
            this.chkEventRaiseBack.Size = new System.Drawing.Size(96, 16);
            this.chkEventRaiseBack.TabIndex = 97;
            this.chkEventRaiseBack.Text = "Raise Back";
            // 
            // label15
            // 
            this.label15.Location = new System.Drawing.Point(192, 128);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(48, 16);
            this.label15.TabIndex = 96;
            this.label15.Text = "Param";
            // 
            // txtEventParam
            // 
            this.txtEventParam.Location = new System.Drawing.Point(240, 128);
            this.txtEventParam.Name = "txtEventParam";
            this.txtEventParam.Size = new System.Drawing.Size(144, 20);
            this.txtEventParam.TabIndex = 95;
            // 
            // label14
            // 
            this.label14.Location = new System.Drawing.Point(192, 98);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(48, 16);
            this.label14.TabIndex = 94;
            this.label14.Text = "Event";
            // 
            // cmdUnregisterEvent
            // 
            this.cmdUnregisterEvent.Location = new System.Drawing.Point(24, 184);
            this.cmdUnregisterEvent.Name = "cmdUnregisterEvent";
            this.cmdUnregisterEvent.Size = new System.Drawing.Size(152, 32);
            this.cmdUnregisterEvent.TabIndex = 93;
            this.cmdUnregisterEvent.Text = "UnSubscribe Event";
            this.cmdUnregisterEvent.Click += new System.EventHandler(this.cmdUnregisterEvent_Click);
            // 
            // txtUnregisterEvent
            // 
            this.txtUnregisterEvent.Location = new System.Drawing.Point(184, 192);
            this.txtUnregisterEvent.Name = "txtUnregisterEvent";
            this.txtUnregisterEvent.Size = new System.Drawing.Size(200, 20);
            this.txtUnregisterEvent.TabIndex = 92;
            // 
            // cmdRaiseEvent
            // 
            this.cmdRaiseEvent.Location = new System.Drawing.Point(24, 88);
            this.cmdRaiseEvent.Name = "cmdRaiseEvent";
            this.cmdRaiseEvent.Size = new System.Drawing.Size(152, 32);
            this.cmdRaiseEvent.TabIndex = 91;
            this.cmdRaiseEvent.Text = "Raise Event";
            this.cmdRaiseEvent.Click += new System.EventHandler(this.cmdRaiseEvent_Click);
            // 
            // txtRaiseEvent
            // 
            this.txtRaiseEvent.Location = new System.Drawing.Point(240, 96);
            this.txtRaiseEvent.Name = "txtRaiseEvent";
            this.txtRaiseEvent.Size = new System.Drawing.Size(144, 20);
            this.txtRaiseEvent.TabIndex = 90;
            // 
            // cmdRegisterEvent
            // 
            this.cmdRegisterEvent.Location = new System.Drawing.Point(24, 48);
            this.cmdRegisterEvent.Name = "cmdRegisterEvent";
            this.cmdRegisterEvent.Size = new System.Drawing.Size(152, 32);
            this.cmdRegisterEvent.TabIndex = 89;
            this.cmdRegisterEvent.Text = "Subscribe Event";
            this.cmdRegisterEvent.Click += new System.EventHandler(this.cmdRegisterEvent_Click);
            // 
            // txtRegisterEvent
            // 
            this.txtRegisterEvent.Location = new System.Drawing.Point(184, 56);
            this.txtRegisterEvent.Name = "txtRegisterEvent";
            this.txtRegisterEvent.Size = new System.Drawing.Size(200, 20);
            this.txtRegisterEvent.TabIndex = 88;
            // 
            // grpPiece
            // 
            this.grpPiece.Controls.Add(this.txtDelim);
            this.grpPiece.Controls.Add(this.label7);
            this.grpPiece.Controls.Add(this.label6);
            this.grpPiece.Controls.Add(this.label5);
            this.grpPiece.Controls.Add(this.txtEnd);
            this.grpPiece.Controls.Add(this.txtResult);
            this.grpPiece.Controls.Add(this.txtNumber);
            this.grpPiece.Controls.Add(this.txtInput);
            this.grpPiece.Controls.Add(this.cmdTestPiece);
            this.grpPiece.Enabled = false;
            this.grpPiece.Location = new System.Drawing.Point(24, 408);
            this.grpPiece.Name = "grpPiece";
            this.grpPiece.Size = new System.Drawing.Size(264, 120);
            this.grpPiece.TabIndex = 71;
            this.grpPiece.TabStop = false;
            this.grpPiece.Text = "Piece Function Testing";
            this.grpPiece.Visible = false;
            // 
            // txtDelim
            // 
            this.txtDelim.Location = new System.Drawing.Point(24, 56);
            this.txtDelim.Name = "txtDelim";
            this.txtDelim.Size = new System.Drawing.Size(16, 20);
            this.txtDelim.TabIndex = 55;
            this.txtDelim.Text = "^";
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(96, 40);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(48, 16);
            this.label7.TabIndex = 54;
            this.label7.Text = "End:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(48, 40);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(48, 16);
            this.label6.TabIndex = 53;
            this.label6.Text = "Start:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(8, 40);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(48, 16);
            this.label5.TabIndex = 52;
            this.label5.Text = "Delim:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtEnd
            // 
            this.txtEnd.Location = new System.Drawing.Point(112, 56);
            this.txtEnd.Name = "txtEnd";
            this.txtEnd.Size = new System.Drawing.Size(32, 20);
            this.txtEnd.TabIndex = 45;
            this.txtEnd.Text = "3";
            // 
            // txtResult
            // 
            this.txtResult.Location = new System.Drawing.Point(16, 88);
            this.txtResult.Name = "txtResult";
            this.txtResult.Size = new System.Drawing.Size(208, 20);
            this.txtResult.TabIndex = 44;
            // 
            // txtNumber
            // 
            this.txtNumber.Location = new System.Drawing.Point(64, 56);
            this.txtNumber.Name = "txtNumber";
            this.txtNumber.Size = new System.Drawing.Size(32, 20);
            this.txtNumber.TabIndex = 43;
            this.txtNumber.Text = "2";
            // 
            // txtInput
            // 
            this.txtInput.Location = new System.Drawing.Point(16, 16);
            this.txtInput.Name = "txtInput";
            this.txtInput.Size = new System.Drawing.Size(200, 20);
            this.txtInput.TabIndex = 42;
            this.txtInput.Text = "1^2^3^4";
            // 
            // cmdTestPiece
            // 
            this.cmdTestPiece.Location = new System.Drawing.Point(152, 56);
            this.cmdTestPiece.Name = "cmdTestPiece";
            this.cmdTestPiece.Size = new System.Drawing.Size(72, 24);
            this.cmdTestPiece.TabIndex = 41;
            this.cmdTestPiece.Text = "Test Piece";
            this.cmdTestPiece.Click += new System.EventHandler(this.cmdTestPiece_Click_1);
            // 
            // cmdAcquireLock
            // 
            this.cmdAcquireLock.Location = new System.Drawing.Point(24, 360);
            this.cmdAcquireLock.Name = "cmdAcquireLock";
            this.cmdAcquireLock.Size = new System.Drawing.Size(104, 32);
            this.cmdAcquireLock.TabIndex = 89;
            this.cmdAcquireLock.Text = "Acquire Lock";
            this.cmdAcquireLock.Click += new System.EventHandler(this.cmdAcquireLock_Click);
            // 
            // cmdReleaseLock
            // 
            this.cmdReleaseLock.Location = new System.Drawing.Point(136, 360);
            this.cmdReleaseLock.Name = "cmdReleaseLock";
            this.cmdReleaseLock.Size = new System.Drawing.Size(88, 32);
            this.cmdReleaseLock.TabIndex = 89;
            this.cmdReleaseLock.Text = "Release Lock";
            this.cmdReleaseLock.Click += new System.EventHandler(this.cmdReleaseLock_Click);
            // 
            // cmdEventPollingInterval
            // 
            this.cmdEventPollingInterval.Location = new System.Drawing.Point(24, 226);
            this.cmdEventPollingInterval.Name = "cmdEventPollingInterval";
            this.cmdEventPollingInterval.Size = new System.Drawing.Size(152, 32);
            this.cmdEventPollingInterval.TabIndex = 93;
            this.cmdEventPollingInterval.Text = "Event Polling Interval";
            // 
            // frmBMXNetTest
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(736, 566);
            this.Controls.Add(this.tabControl1);
            this.Name = "frmBMXNetTest";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "BMXNet Test Application";
            this.Load += new System.EventHandler(this.frmBMXNetTest_Load);
            this.tabControl1.ResumeLayout(false);
            this.tpaQuery.ResumeLayout(false);
            this.panGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid2)).EndInit();
            this.panQuery.ResumeLayout(false);
            this.panQuery.PerformLayout();
            this.tpaControls.ResumeLayout(false);
            this.grpControls.ResumeLayout(false);
            this.grpControls.PerformLayout();
            this.tpaConnection.ResumeLayout(false);
            this.tpaConnection.PerformLayout();
            this.tpaOther.ResumeLayout(false);
            this.tpaOther.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdAsyncResult)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudEventPollingInterval)).EndInit();
            this.grpPiece.ResumeLayout(false);
            this.grpPiece.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
            try
            {
                Application.Run(new frmBMXNetTest());
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show("BMXNetTest Error: " + ex.Message, "BMXNetTest", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

		}

		private BMXNetConnectInfo m_ci;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.DataGrid dataGrid2;
		private System.Windows.Forms.Button cmdCancelChanges;
		private System.Windows.Forms.TextBox txtCommand;
		private System.Windows.Forms.TextBox txtContext;
		private System.Windows.Forms.Button cmdChangeUser;
		private System.Windows.Forms.Button cmdChangeServer;
		private System.Windows.Forms.Button cmdChangeContext;
		private System.Windows.Forms.Button cmdChangeDivision;
		private System.Windows.Forms.TextBox txtDivision;
		private System.Windows.Forms.TextBox txtUser;
		private System.Windows.Forms.TextBox txtServer;
		private System.Windows.Forms.TabPage tpaQuery;
		private System.Windows.Forms.Panel panGrid;
		private System.Windows.Forms.Panel panQuery;
		private System.Windows.Forms.TabPage tpaConnection;
		private System.Windows.Forms.TabPage tpaOther;
		private System.Windows.Forms.GroupBox grpPiece;
		private System.Windows.Forms.TextBox txtDelim;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox txtEnd;
		private System.Windows.Forms.TextBox txtResult;
		private System.Windows.Forms.TextBox txtNumber;
		private System.Windows.Forms.TextBox txtInput;
		private System.Windows.Forms.Button cmdTestPiece;
		private System.Windows.Forms.Button cmdTest4;
		private System.Windows.Forms.Button cmdTest3;
		private System.Windows.Forms.Button cmdExecuteQuery;
		private System.Windows.Forms.Button cmdAcceptChanges;
		private System.Windows.Forms.Button cmdXML;
		private System.Windows.Forms.GroupBox grpControls;
		private System.Windows.Forms.Button cmdCDLoad;
		private System.Windows.Forms.Label lblCDIntro;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtCDName;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.TextBox txtCDSex;
		private System.Windows.Forms.TextBox txtCDDOB;
		private System.Windows.Forms.TextBox txtCDSSN;
		private System.Windows.Forms.ComboBox cboCDSelect;
		private System.Windows.Forms.MonthCalendar calCDDOB;
		private System.Windows.Forms.TabPage tpaControls;
		private System.Windows.Forms.DateTimePicker dtpCDDOB;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.ListBox lstCDSex;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.Button cmdAddAll;
		private System.Windows.Forms.Button cmdUnregisterEvent;
		private System.Windows.Forms.TextBox txtUnregisterEvent;
		private System.Windows.Forms.Button cmdRaiseEvent;
		private System.Windows.Forms.TextBox txtRaiseEvent;
		private System.Windows.Forms.Button cmdRegisterEvent;
		private System.Windows.Forms.TextBox txtRegisterEvent;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.TextBox txtEventParam;
		private System.Windows.Forms.CheckBox chkEventRaiseBack;
		private System.Windows.Forms.Button cmdAcquireLock;
		private System.Windows.Forms.Button cmdReleaseLock;
		private System.Windows.Forms.Button cmdEventPollingInterval;
		private System.Windows.Forms.NumericUpDown nudEventPollingInterval;
		private System.Windows.Forms.CheckBox chkEnableEvents;
		private System.Windows.Forms.Label label16;
		private System.Windows.Forms.TextBox txtEventMessages;
		private System.Windows.Forms.Label label17;
		private System.Windows.Forms.Button cmdAsyncCall;
		private System.Windows.Forms.TextBox txtAsyncCommand;
		private System.Windows.Forms.Label label18;
		private System.Windows.Forms.DataGrid grdAsyncResult;
        private System.Windows.Forms.Label label19;
        private Button cmdTestSilent;
        private Button cmdTestReceiveTimeout;
        private Button cmdStopLogging;
        private Button cmdStartLogging;
		BMXNetDataAdapter m_da = new BMXNetDataAdapter();



		private void frmBMXNetTest_Load(object sender, System.EventArgs e)
		{
			//Basic steps to establish a BMXNet connection.
			//The first time a user connects, he will be prompted for
			//server info and passwords.
			//Subsequent connect requests will use cached information.
			//Note that LoadConnectInfo maintain's an internal copy 
			//of a BMXNetLib object which is accessible from the
			//bmxNetLib property
			//
			//Use the ADO.NET provider methods or the BMXNetLib TransmitRPC
			//method to create a connection and exchange data.
			if (m_ci == null)
				m_ci = new BMXNetConnectInfo();

			m_ci.EventPollingEnabled = false;
			m_ci.BMXNetEvent += new BMXNetConnectInfo.BMXNetEventDelegate(BMXNetEventHandler);
			try
			{
				//Use this overload to connect to the last M server
				//using Windows NT integrated security
				m_ci.LoadConnectInfo();

				//Use the following overload to force prompt for AV codes.
				//You can also use this overload if
				// you want to create your own dialog to collect
				// AV codes. You may then pass
				// the values you collected to LoadConnectInfo(access,verify)
				//
				//m_ci.LoadConnectInfo("","");

				m_ci.AppContext = "BMXRPC";
				txtContext.Text = m_ci.AppContext;
				this.txtServer.Text = m_ci.MServerAddress;
				this.txtUser.Text = m_ci.UserName;
				this.txtDivision.Text = m_ci.DivisionName;

			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "BMXNetTest");
			}
            txtCommand.Text = "BMX ADO SS^HW TEST^^~1~20~5";
            //txtCommand.Text = "SELECT NAME, AGE, DOB FROM PATIENT WHERE (NAME LIKE 'E%') AND (AGE BETWEEN 20 AND 60) MAXRECORDS:50";
//			this.txtCommand.Text= "BMXGetFacRS^408";
//			this.txtCommand.Text = "SELECT BMXIEN, NAME, AGE, DOB FROM VA_PATIENT WHERE (NAME LIKE 'E%') AND (AGE BETWEEN 20 AND 60) MAXRECORDS:50";
//			txtCommand.Text = "BMX TEST^S|30";
//			txtCommand.Text = "BMX ADO SS^IHS PATIENT^^~1~20~5";
//			txtCommand.Text = "BMX ADO SS^17^^~4~4~5~~~~~18,SUB";
//			//SIEN1,"","AA~3/21/1965~6/4/2004~5~~~~235|WT|C~11,.02"
//			txtCommand.Text = "BMX ADO SS^23^^AA~3/21/1965~6/4/2004~5~~~~235|WT|C~11,.02";
//			txtCommand.Text = "BMX ADO SS^17^^~4~9~~~~~~18,SUB";
//			txtCommand.Text = "BMX ADO SS^11^^~1~5~~~~~~25,.001,.02IEN,AA~1/1/1960~6/30/2004~~~~~|C";
//			txtCommand.Text = "BMX ADO SS^18^1,^~~~";
//			txtCommand.Text = "BMX ADO SS^PATIENT DEMOGRAPHICS^^~1~5~";
//			txtCommand.Text = @"BMX ADO SS^53^^~~~~~VMEAS~BMXADOFD~100002.1A||PU\60|WT\175|HT\70";
//			txtCommand.Text = "BMX ADO SS^3^^~1658~1658^11,.05IEN,.001";
//			BMX ADO SS^HW SD DEVICE1^^~~~
		}

		private void cmdTestPiece_Click_1(object sender, System.EventArgs e)
		{
			//Demo of BMXNet Piece function.  Note that only single-character
			//delimiters are supported.
			try
			{
				string sInput = txtInput.Text;
				string sDelim = txtDelim.Text;
				if (sDelim.Length > 1)
					throw new BMXNetException("BMXNet.Piece() supports only single-character delimiters.");
				int nNumber = Convert.ToInt16(txtNumber.Text);
				int nEnd = 0;
				string sOutput = "";
				if (txtEnd.Text != "")
				{
					nEnd = Convert.ToInt16(txtEnd.Text);
					sOutput = BMXNetLib.Piece(sInput, sDelim, nNumber, nEnd);
				}
				else
				{
					sOutput = BMXNetLib.Piece(sInput, sDelim, nNumber);
				}

				txtResult.Text = sOutput;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "BMXNetTest Error");
			}
		}

		private void cmdTest3_Click(object sender, System.EventArgs e)
		{
			//How to create a low-level socket connection using
			//BMXNetLib and transmit a simple RPC.

			//Always use try-catch blocks.  BMXNet uses exceptions to report
			//errors and connection failures.
			try
			{
				BMXNetLib bnLib;
				bnLib = new BMXNetLib();

				//Either collect your own connection information
				//or use the Windows Identity map to RPMS
				//and manually open a connection like this:
					//bnLib.MServerPort = nPort;
					//bnLib.OpenConnection(sIP, sAccess, sVerify);
					// -or-
					//bnLib.OpenConnection(sIP, WindowsIdentity.GetCurrent());
					//bnLib.AppContext = "BMXRPC";
					//If no exception thrown by OpenConnection, then connection
					//to rpms succeeded.  If OpenConnection fails, then an exception
					//is thrown.

				// -OR- Use BMXNetConnectInfo to establish connection
				//LoadConnectInfo will prompt for server and user info as needed
		
				bnLib.MServerPort = m_ci.MServerPort;
				bnLib.OpenConnection("127.0.0.1",WindowsIdentity.GetCurrent());
//				bnLib.OpenConnection("127.0.0.1","","");
				bnLib.AppContext = "BMXRPC";

				string sDUZ = bnLib.DUZ;
				string sUser = bnLib.TransmitRPC("BMX USER", sDUZ);
				if (sUser.StartsWith("M ERROR=") == true)
				{
					Exception ex = new Exception(sUser);
					throw ex;
				}
				MessageBox.Show("Connection to RPMS succeeded.  User name = " + sUser, "BMXNetLib Test");
				bnLib.CloseConnection();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "BMXNetTest Error");
			}

		}

		private void cmdTest4_Click(object sender, System.EventArgs e)
		{
			/*Demo of retrieving multiple related tables,
			 * joining them,
			 * and attaching them to a grid that can
			 * drill-down through the relations
			*/
			try
			{
				if (m_ci.Connected == false)
				{
					throw new BMXNetException("Not connected to RPMS.");
				}
				frmVisitDemo frmVD = new frmVisitDemo();
				frmVD.InitializePage(m_ci);
				frmVD.ShowDialog(this);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "BMXNetTest Error");
			}
			
		}

		private void cmdChangeServer_Click(object sender, System.EventArgs e)
		{
			//How to change the RPMS server in BMXNetConnectInfo		
			try
			{
				m_ci.ChangeServerInfo();
				m_ci.LoadConnectInfo();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "BMXNetTest");
			}

		}

		private void cmdChangeUser_Click(object sender, System.EventArgs e)
		{
			//How to change the RPMS user
			try
			{
				m_ci.LoadConnectInfo("","");
				m_ci.AppContext = "BMXRPC";
				MessageBox.Show("User changed to " + m_ci.UserName + " on Server " + m_ci.MServerAddress + ", on Port " + m_ci.MServerPort);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "BMXNetTest");
			}
		}

		private void cmdCancelChanges_Click(object sender, System.EventArgs e)
		{
			DataTable dt = (DataTable) dataGrid2.DataSource;
			dt.RejectChanges();
		}

		private void cmdChangeContext_Click(object sender, System.EventArgs e)
		{
			try
			{
				m_ci.AppContext = txtContext.Text;
				MessageBox.Show("Context changed to " + m_ci.AppContext);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void cmdChangeDivision_Click(object sender, System.EventArgs e)
		{			

			/*How to change user division:
			 * You can either create your own dialog, populate
			 * it with using the UserDivisions datatable and DUZ2 string
			 *  from ConnectInfo
			 * and then manually reset ConnectInfo's DUZ2 property,
			 * -OR-
			 * you can invoke ConnectInfo's ChangeDivision method
			 * which will display its own dialog for you.  The
			 * commented code below shows the 'manual' way (you will have to
			 * first create a 'DSelectDivision' dialog class)
			 */

			if (m_ci.Connected == false)
			{
				throw new BMXNetException("Not connected to RPMS.");
			}

//			DSelectDivision dsd = new DSelectDivision();
//			dsd.InitializePage(m_ci.UserDivisions, m_ci.DUZ2);
//			if (dsd.ShowDialog(this) == DialogResult.Cancel)
//				return;
//			m_ci.DUZ2 = dsd.DUZ2;
			
			m_ci.ChangeDivision(this);
			this.txtDivision.Text = m_ci.DivisionName;
		}

		private void cmdExecuteQuery_Click(object sender, System.EventArgs e)
		{
			//This is the 'Execute' button on the form and demonstrates
			//how to use the BMXNet ADO.NET Data Provider to 
			//retrieve an RPMS ADO.NET datatable.
            MessageBox.Show("TEST");
            try
            {
                if (m_ci.Connected == false)
                {
                    throw new Exception("Not connected to RPMS.");
                }
                //m_ci.bmxNetLib.StartLog("TestLog.txt");
                //m_ci.bmxNetLib.StartLog();
                m_ci.bmxNetLib.BMXRWL.AcquireWriterLock(5);
                dataGrid2.DataSource = m_ci.RPMSDataTable(txtCommand.Text, "SampleTable");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            finally
            {
                if (m_ci.bmxNetLib.BMXRWL.IsWriterLockHeld == true)
                {
                    m_ci.bmxNetLib.BMXRWL.ReleaseWriterLock();
                }
                //m_ci.bmxNetLib.StopLog();

            }
		}

		/// <summary>
		/// How to Accept Updates to an updateable recordset
		/// using the BMXNetDataAdapter's Update method
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void cmdAcceptChanges_Click(object sender, System.EventArgs e)
		{
           int nTimeOut = m_ci.ReceiveTimeout;
           try
            {
                m_ci.ReceiveTimeout = (90 * 1000);
                DataTable dtPrimary = (DataTable)dataGrid2.DataSource;
                DataSet ds = dtPrimary.DataSet;
                if ((dtPrimary.DataSet is DataSet) == false)
                {
                    ds = new DataSet();
                    ds.Tables.Add(dtPrimary);
                }
                foreach (DataTable dt in ds.Tables)
                {
                    BMXNetConnection conn = new BMXNetConnection(m_ci.bmxNetLib);
                    DataTable dtc = dt.GetChanges();
                    if (dtc != null)
                    {

                        BMXNetCommand bmxSelectCmd = (BMXNetCommand)conn.CreateCommand();
                        bmxSelectCmd.CommandText = txtCommand.Text;
                        m_da.SelectCommand = bmxSelectCmd;

                        DataTable dtSchema = m_da.FillSchema(dtc, SchemaType.Source);

                        //Build UPDATE command based on info stored in table's extended property set
                        BMXNetCommand bmxUpdateCmd = new BMXNetCommand();
                        bmxUpdateCmd.Connection = conn;

                        //Call BMXBuildUpdateCommand(dtSchema) to set up parameters
                        bmxUpdateCmd.BMXBuildUpdateCommand(dtSchema);

                        //Link command to data adapeter
                        m_da.UpdateCommand = bmxUpdateCmd;

                        //Call adapter's Update method
                        m_da.Update(dtc);

                        //Accept the changes to the datagrid's datatable
                        dt.AcceptChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                m_ci.ReceiveTimeout = nTimeOut;
            }
		}

		private void dataGrid2_Navigate(object sender, System.Windows.Forms.NavigateEventArgs ne)
		{
			string sDirection = (ne.Forward == true)?"forward":"backward";
			string myString = "Navigate event raised, moved " + sDirection;
			Debug.Write(myString + "\n");
			
		}

		private void cmdXML_Click(object sender, System.EventArgs e)
		{
			//Writes contents of the datagrid's current dataset to XML file
			//Also writes dataset structure to XSD schema file
			//Transforms the datetime data in the XML file to a format acceptable to
			//	MS Excel by removing the UTC time conversion info from the end of the datetime string
            try
            {
                DataTable dtPrimary = (DataTable)dataGrid2.DataSource;
                DataSet ds = dtPrimary.DataSet;

                if ((dtPrimary.DataSet is DataSet) == false)
                {
                    ds = new DataSet();
                    ds.Tables.Add(dtPrimary);
                }

                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Filter = "XML Files (*.xml)|*.xml|All files (*.*)|*.*";
                saveFileDialog1.FilterIndex = 0;
                saveFileDialog1.RestoreDirectory = true;
                saveFileDialog1.AddExtension = true;
                saveFileDialog1.DefaultExt = "xml";

                if (saveFileDialog1.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                //create file names based on user input.
                //sFile = the name of the XML file created by the dataset export
                //sSchemaFile = the schema generated by the dataset
                //xXSL file = the transform file applied to sFile
                //sFileOut = the final transformed XML file
                string sFile = saveFileDialog1.FileName;
                string sSchemaFile = sFile.Remove(sFile.Length - 4, 4);
                sSchemaFile = sSchemaFile + "Schema.xsd";
                string sFileOut = sFile;
                sFile = sFile.Remove(sFile.Length - 4, 4);
                sFile = sFile + "Input.xml";
                string sXSLFile = sSchemaFile.Remove(sSchemaFile.Length - 10, 10);
                sXSLFile = sXSLFile + "Transform.xslt";

                // Open file to which to write schema
                System.IO.FileStream fsSchema = new System.IO.FileStream
                    (sSchemaFile, System.IO.FileMode.Create);
                System.Xml.XmlTextWriter schemaWriter =
                    new System.Xml.XmlTextWriter(fsSchema, System.Text.Encoding.ASCII);
                ds.WriteXmlSchema(schemaWriter);
                schemaWriter.Close();

                // Write out the dataset to xml
                System.IO.FileStream myFileStream = new System.IO.FileStream
                    (sFile, System.IO.FileMode.Create);
                System.Xml.XmlTextWriter myXmlWriter =
                    new System.Xml.XmlTextWriter(myFileStream, System.Text.Encoding.ASCII);
                ds.WriteXml(myXmlWriter);
                myXmlWriter.Close();

                //Create and apply an xslt file to reformat dates to be Excel compatible
                //First create the xslt transform
                this.CreateXSL(sSchemaFile, sXSLFile);
                //Apply the transform
                System.Xml.XmlResolver xmlr = null;
                XslTransform xslt = new XslTransform();
                //System.Xml.Xsl.XslCompiledTransform xslt = new XslCompiledTransform();
                try
                {
                    xslt.Load(sXSLFile, xmlr);
                    //xslt.Load(sXSLFile, null , xmlr);

                    xslt.Transform(sFile, sFileOut, xmlr);
                    //xslt.Transform(
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
		}

		private void CreateXSL(string sXsdFile, string sXsltFile)
		{
			//Given an xsd file, create an xsl transform to apply to xlm file
			//that will transform xml date format to Excel dates by removing UTC information
			//from the xml datetime value.

			//Open the xsd input file and xsl output file
			//and associate with XML reader and writer

			string sFileIn = sXsdFile;
			string sFileOut = sXsltFile;

			//Open XMLTextWriter
			System.IO.FileStream fsOut = new System.IO.FileStream
				(sFileOut, System.IO.FileMode.Create);
			System.Xml.XmlTextWriter xwOut = 
				new System.Xml.XmlTextWriter(fsOut, System.Text.Encoding.ASCII);
	
			//Write to the output file
			xwOut.Formatting=Formatting.Indented;
		
			xwOut.WriteStartElement("xsl", "stylesheet", "http://www.w3.org/1999/XSL/Transform");
			xwOut.WriteAttributeString("xmlns","msxsl",null, "urn:schemas-microsoft-com:xslt");
			xwOut.WriteAttributeString("xmlns","bmx",null, "http://bmx.ihs.gov");
			xwOut.WriteAttributeString("version", "1.0");

			xwOut.WriteStartElement( "msxsl", "script", null);
			xwOut.WriteAttributeString("implements-prefix", "bmx");
			xwOut.WriteAttributeString("language", "C#");
			
			string sCode = "\npublic string ExcelDate(string sXMLDate) {\n";
			sCode += "string sConverted;\n";
			sCode += "sConverted = sXMLDate.Remove(sXMLDate.Length - 15,15);\n";
			sCode += "return sConverted;\n";
			sCode += "}\n";
			xwOut.WriteCData(sCode);
			xwOut.WriteFullEndElement(); //msxsl script

			xwOut.WriteStartElement("xsl", "output", null);
			xwOut.WriteAttributeString("method", "xml");
			xwOut.WriteAttributeString("indent", "yes");
			xwOut.WriteEndElement();

			xwOut.WriteStartElement("xsl", "template", null);
			xwOut.WriteAttributeString("match", "@*|node()");
			xwOut.WriteStartElement("xsl", "copy", null);
			xwOut.WriteStartElement("xsl", "apply-templates", null);
			xwOut.WriteAttributeString("select", "@*|node()");
			xwOut.WriteEndElement(); //apply templates
			xwOut.WriteFullEndElement(); //copy
			xwOut.WriteFullEndElement(); //template

			ArrayList alFieldNames = new ArrayList();

			//read from schema file
			//Open XMLTextReader
			System.IO.FileStream fsIn = new System.IO.FileStream
				(sFileIn, System.IO.FileMode.Open);
			System.Xml.XmlTextReader xrIn = 
				new System.Xml.XmlTextReader(fsIn);

			string sFieldName = "";
			while (xrIn.Read())
			{
				if (xrIn.NodeType == XmlNodeType.Element)
				{
					switch (xrIn.LocalName)
					{
						case "element":
							if (xrIn.MoveToAttribute("name"))
							{
								sFieldName = xrIn.Value;
								if (xrIn.MoveToAttribute("type"))
								{
									if (xrIn.Value == "xs:dateTime")
									{
										alFieldNames.Add(sFieldName);
									}
								}
							}
							else
							{
								xrIn.Skip();
							}
							break;
					}//switch
				}//if
			}//while

			//build conversion templates
			for (int j=0; j < alFieldNames.Count; j++)
			{
				sFieldName = alFieldNames[j].ToString();
				xwOut.WriteStartElement("xsl", "template", null);
				xwOut.WriteAttributeString("match", sFieldName);

				xwOut.WriteStartElement("xsl", "element", null);
				xwOut.WriteAttributeString("name", sFieldName);

				xwOut.WriteStartElement("xsl", "value-of", null);
				xwOut.WriteAttributeString("select", "bmx:ExcelDate(.)");
	
				xwOut.WriteEndElement(); //value-of
				xwOut.WriteFullEndElement(); //element
				xwOut.WriteFullEndElement(); //template
			}

			xwOut.WriteFullEndElement(); //stylesheet
			xwOut.Close();
		}

		/*
		 * Controls Demo
		 * Demonstrates how to populate controls from a datatable
		 */
		private void cmdCDLoad_Click(object sender, System.EventArgs e)
		{
			try
			{
				if (m_ci.Connected == false)
				{
					throw new BMXNetException("Not connected to RPMS.");
				}

				string sCommandText = "BMX ADO SS^PATIENT DEMOGRAPHICS^^~1~50~";
				DataTable dt = m_ci.RPMSDataTable(sCommandText, "ControlsDemo");

				txtCDName.DataBindings.Clear();
				txtCDSex.DataBindings.Clear();
				txtCDDOB.DataBindings.Clear();
				txtCDSSN.DataBindings.Clear();
				calCDDOB.DataBindings.Clear();
				dtpCDDOB.DataBindings.Clear();
				lstCDSex.DataBindings.Clear();

				DataView dvCDPat = new DataView(dt);
				dvCDPat.Sort = "NAME ASC";

				cboCDSelect.DataSource = dvCDPat;
				cboCDSelect.DisplayMember = "NAME";
				cboCDSelect.ValueMember = "BMXIEN";

				txtCDName.DataBindings.Add("Text", dvCDPat, "NAME");
				txtCDSex.DataBindings.Add("Text", dvCDPat, "SEX");
				txtCDDOB.DataBindings.Add("Text", dvCDPat, "DOB");
				txtCDSSN.DataBindings.Add("Text", dvCDPat, "SSN");

				calCDDOB.DataBindings.Add("SelectionStart", dvCDPat, "DOB");
				calCDDOB.DataBindings.Add("SelectionEnd", dvCDPat, "DOB");

				dtpCDDOB.DataBindings.Add("Value", dvCDPat, "DOB");

				lstCDSex.DataBindings.Add("SelectedItem", dvCDPat, "SEX");
			
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
				return;
			}			
		}

		private void cmdAddAll_Click(object sender, System.EventArgs e)
		{
			/*
					 * How to Accept Updates to an updateable recordset
					 * using the new schema format and the data adapter's Update method
					 */

			try
			{
				DataTable dtPrimary = (DataTable) dataGrid2.DataSource;
				DataSet ds = dtPrimary.DataSet;



				foreach (DataTable dt in ds.Tables)
				{

					BMXNetConnection conn = new BMXNetConnection(m_ci.bmxNetLib);

					object[] oVal = new object[dt.Columns.Count];
					//Call adapter's Update method
					int nRowCount = dt.Rows.Count;
					for (int j=0; j < nRowCount; j++ )
					{
							DataRow r = dt.Rows[j];
						for (int k = 0; k < dt.Columns.Count; k++)
						{
							oVal[k] = r[dt.Columns[k].ColumnName];
						}
						DataRow rAdd = dt.Rows.Add(oVal);
					}

					DataTable dtc = dt.GetChanges();					
					if (dtc != null)
					{
						DataTable dtSchema = m_da.FillSchema(dtc, SchemaType.Source);

						//Build UPDATE command based on info stored in table's extended property set
						BMXNetCommand bmxUpdateCmd = new BMXNetCommand();
						bmxUpdateCmd.Connection = conn;

						//Call BMXBuildUpdateCommand(dtSchema) to set up parameters
						bmxUpdateCmd.BMXBuildUpdateCommand(dtSchema);

						//Link command to data adapter
						m_da.UpdateCommand = bmxUpdateCmd;

						//Call adapter's Update method
						m_da.Update(dtc);

						//Accept the changes to the datagrid's datatable
						dt.RejectChanges();
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}		
		}

		#region Event Handling and Asynchronous Calls

		private string  m_sTask;
		private void cmdAsyncCall_Click(object sender, System.EventArgs e)
		{
			try
			{
				//Use the windows handle to create a unique event name
                Process pCurrent = Process.GetCurrentProcess();
                string sID = Environment.MachineName + "-";
                sID += pCurrent.Id.ToString();
				m_ci.SubscribeEvent("BMX ASYNC RESULT READY" + sID ); // this.Handle.ToString());
				//nTask corresponds to TaskMan's ZTSK
                int nTask = m_ci.RPMSDataTableAsyncQue(this.txtAsyncCommand.Text, "BMX ASYNC RESULT READY" + sID) ;//this.Handle.ToString());
				//Save ZTSK for later comparison
				m_sTask = nTask.ToString();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}		
		}

		private void BMXNetEventHandler(Object obj, BMXNet.BMXNetEventArgs e)
		{
			try
			{
				//BMXNet events arrive on a separate thread from the UI thread,
				//thus UI methods should be called using BeginInvoke or Invoke
				UpdateEventMessageTextDelegate dlgUEMT = new UpdateEventMessageTextDelegate(UpdateEventMessageText);
				this.Invoke(dlgUEMT, new object[] {e.BMXEvent, e.BMXParam});

                Process pCurrent = Process.GetCurrentProcess();
                string sID = Environment.MachineName + "-";
                sID += pCurrent.Id.ToString();
                if (e.BMXEvent == "BMX ASYNC RESULT READY" + sID)
				{
                    string sTask = BMXNetLib.Piece(e.BMXParam, "~", 1);
					string sAsyncInfo = BMXNetLib.Piece(e.BMXParam,"~",2);
					DataTable dtAsyncGrid = m_ci.RPMSDataTableAsyncGet(sAsyncInfo, "AsyncData");
					UpdateAsyncGridDelegate uagd = new UpdateAsyncGridDelegate(UpdateAsyncGrid);
					this.BeginInvoke(uagd, new object[] {sTask, dtAsyncGrid});
				}
			}
			catch (Exception ex)
			{
				Debug.Write("BMXNetEventHandler exception: " + ex.Message + "\n");
			}		
		}

		delegate void UpdateAsyncGridDelegate(string sTask, DataTable dt);
		private void UpdateAsyncGrid(string sTask, DataTable dt)
		{
			try
			{
				//Assert that invoke is not required in any UI method that may be called
				//by a worker thread (e.g. from an event)
				Debug.Assert(this.InvokeRequired == false);

				//Check that this is the same job that we tasked
				if (m_sTask != sTask)
					return;

				grdAsyncResult.DataSource = dt;
				m_ci.UnSubscribeEvent("BMX ASYNC RESULT READY" + this.Handle.ToString());
			}
			catch (Exception ex)
			{
				Debug.Write("UpdateAsyncGrid exception: " + ex.Message + "\n");
			}
		}
		
		delegate void UpdateEventMessageTextDelegate(string EventName, string Param);

		private void UpdateEventMessageText(string EventName, string Param)
		{
			Debug.Assert(this.InvokeRequired == false);
			txtEventMessages.Text = "BMXNet event " + EventName + " fired with parameter " + Param;
			if (txtEventMessages.BackColor == System.Drawing.Color.LightGreen)
			{
				txtEventMessages.BackColor = System.Drawing.Color.LightCoral; 
			}
			else
			{
				txtEventMessages.BackColor = System.Drawing.Color.LightGreen; 
			}		
		}

		private void cmdRegisterEvent_Click(object sender, System.EventArgs e)
		{
			m_ci.SubscribeEvent(this.txtRegisterEvent.Text);
		}

		private void cmdUnregisterEvent_Click(object sender, System.EventArgs e)
		{
			m_ci.UnSubscribeEvent(this.txtUnregisterEvent.Text);
		}

		private void cmdRaiseEvent_Click(object sender, System.EventArgs e)
		{
			m_ci.RaiseEvent(this.txtRaiseEvent.Text, txtEventParam.Text, chkEventRaiseBack.Checked);
		}

		private void cmdAcquireLock_Click(object sender, System.EventArgs e)
		{
			try
			{
				m_ci.bmxNetLib.BMXRWL.AcquireWriterLock(5);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void cmdReleaseLock_Click(object sender, System.EventArgs e)
		{
			try
			{
				m_ci.bmxNetLib.BMXRWL.ReleaseWriterLock();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void chkEnableEvents_CheckedChanged(object sender, System.EventArgs e)
		{
			m_ci.EventPollingInterval = Convert.ToInt16(nudEventPollingInterval.Value);
			m_ci.EventPollingEnabled = chkEnableEvents.Checked;
		}

		private void nudEventPollingInterval_ValueChanged(object sender, System.EventArgs e)
		{
			m_ci.EventPollingInterval = Convert.ToInt16(nudEventPollingInterval.Value);	
		}

		#endregion Event Handling

        private void cmdTestSilent_Click(object sender, EventArgs e)
        {
            try
            {
                this.m_ci.CloseConnection();

                string sA = "";
                string sV = "";
                string sAddress = "";
                int nPort = 10501;
                string sN = "";

                m_ci.LoadConnectInfo(sAddress, nPort, sA, sV, sN);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void cmdTestReceiveTimeout_Click(object sender, EventArgs e)
        {
            int nTest = 5000;
            this.m_ci.ReceiveTimeout = nTest;
        }

        private void cmdStartLogging_Click(object sender, EventArgs e)
        {
            try
            {
                m_ci.bmxNetLib.StartLog();
            }
            catch (Exception ex)
            { 
                MessageBox.Show("Unable to start logging:  " + ex.Message);
            }
        }

        private void cmdStopLogging_Click(object sender, EventArgs e)
        {
            try
            {
                m_ci.bmxNetLib.StopLog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to stop logging:  " + ex.Message);
            }
        }



    }
}
