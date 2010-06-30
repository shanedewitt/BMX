using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace IndianHealthService.BMXNet
{
	/// <summary>
	/// Summary description for frmVisitDemo.
	/// </summary>
	public class frmVisitDemo : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TextBox txtPatName;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button cmdViewVisitInfo;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.DataGrid dataGrid1;

		private System.ComponentModel.Container components = null;

		public frmVisitDemo()
		{
			InitializeComponent();
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.txtPatName = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.cmdViewVisitInfo = new System.Windows.Forms.Button();
			this.panel1 = new System.Windows.Forms.Panel();
			this.panel2 = new System.Windows.Forms.Panel();
			this.dataGrid1 = new System.Windows.Forms.DataGrid();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).BeginInit();
			this.SuspendLayout();
			// 
			// txtPatName
			// 
			this.txtPatName.Location = new System.Drawing.Point(120, 24);
			this.txtPatName.Name = "txtPatName";
			this.txtPatName.Size = new System.Drawing.Size(152, 20);
			this.txtPatName.TabIndex = 0;
			this.txtPatName.Text = "W";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(104, 24);
			this.label1.TabIndex = 1;
			this.label1.Text = "First few characters of patient name:";
			// 
			// cmdViewVisitInfo
			// 
			this.cmdViewVisitInfo.Location = new System.Drawing.Point(304, 24);
			this.cmdViewVisitInfo.Name = "cmdViewVisitInfo";
			this.cmdViewVisitInfo.Size = new System.Drawing.Size(112, 24);
			this.cmdViewVisitInfo.TabIndex = 2;
			this.cmdViewVisitInfo.Text = "View Patient Visits";
			this.cmdViewVisitInfo.Click += new System.EventHandler(this.cmdViewVisitInfo_Click);
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.cmdViewVisitInfo);
			this.panel1.Controls.Add(this.txtPatName);
			this.panel1.Controls.Add(this.label1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(584, 72);
			this.panel1.TabIndex = 3;
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.dataGrid1);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel2.Location = new System.Drawing.Point(0, 72);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(584, 246);
			this.panel2.TabIndex = 4;
			// 
			// dataGrid1
			// 
			this.dataGrid1.AccessibleName = "DataGrid";
			this.dataGrid1.AccessibleRole = System.Windows.Forms.AccessibleRole.Table;
			this.dataGrid1.DataMember = "";
			this.dataGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dataGrid1.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.dataGrid1.Location = new System.Drawing.Point(0, 0);
			this.dataGrid1.Name = "dataGrid1";
			this.dataGrid1.Size = new System.Drawing.Size(584, 246);
			this.dataGrid1.TabIndex = 0;
			// 
			// frmVisitDemo
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(584, 318);
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.panel1);
			this.Name = "frmVisitDemo";
			this.Text = "frmVisitDemo";
			this.panel1.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		#region Fields
		BMXNetConnectInfo m_ci;
		#endregion Fields


		public void InitializePage(BMXNetConnectInfo ci)
		{
			m_ci = ci;
			CreateGridStyles();
		}

		private void cmdViewVisitInfo_Click(object sender, System.EventArgs e)
		{
			if (m_ci.Connected == false)
			{
				MessageBox.Show("Must connect to RPMS first");
				return;
			}
			try
			{
				//Declare new dataset and connection variables
				DataSet m_dsGlobal = new DataSet("Global");

				//Retrieve RPMS Records from PATIENT file
                string sCmd = "SELECT BMXIEN DFN, NAME, AGE, SEX, DOB FROM PATIENT WHERE NAME LIKE '" + txtPatName.Text + "%'";
                m_ci.RPMSDataTable(sCmd, "Patient", m_dsGlobal);

				//Build Primary Key for Patient table
				DataTable dtGroups = m_dsGlobal.Tables["Patient"];
				DataColumn dcKey = dtGroups.Columns["DFN"];
				DataColumn[] dcKeys = new DataColumn[1];
				dcKeys[0] = dcKey;
				dtGroups.PrimaryKey = dcKeys;

				//Retrieve RPMS Records from VISIT file
				sCmd = @"SELECT INTERNAL[PATIENT_NAME] PDFN, BMXIEN VDFN, VISIT/ADMIT_DATE&TIME, CLINIC, LOC._OF_ENCOUNTER FROM VISIT WHERE PATIENT_NAME LIKE '" + txtPatName.Text + "%'";
                m_ci.RPMSDataTable(sCmd, "Visit", m_dsGlobal);

				//Build Primary Key for Visit table
				DataTable dtAGTypes = m_dsGlobal.Tables["Visit"];
				DataColumn dcGTKey = dtAGTypes.Columns["VDFN"];
				DataColumn[] dcGTKeys = new DataColumn[1];
				dcGTKeys[0] = dcGTKey;
				dtAGTypes.PrimaryKey = dcGTKeys;

				//Build Data Relationship between Patient and Visit tables
				DataRelation dr = new DataRelation("PatientVisit",	//Relation Name
					m_dsGlobal.Tables["Patient"].Columns["DFN"],	//Parent
					m_dsGlobal.Tables["Visit"].Columns["PDFN"]);	//Child
				m_dsGlobal.Relations.Add(dr);

				this.dataGrid1.DataSource = m_dsGlobal.Tables["Patient"];
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "BMXNetTest Error");
			}
		}

		private void CreateGridStyles()
		{
			//Create DataGridTableStyle for Patient table
			DataGridTableStyle tsRU = new DataGridTableStyle();
			tsRU.MappingName = "Patient";
			tsRU.ReadOnly = true;

			// Add DFN column style.
			DataGridColumnStyle colDFN = new DataGridTextBoxColumn();
			colDFN.MappingName = "DFN";
			colDFN.HeaderText = "DFN";
			colDFN.Width = 0;
			tsRU.GridColumnStyles.Add(colDFN);

			// Add NAME column style.
			DataGridColumnStyle colRUID = new DataGridTextBoxColumn();
			colRUID.MappingName = "NAME";
			colRUID.HeaderText = "Patient Name";
			colRUID.Width = 250;
			tsRU.GridColumnStyles.Add(colRUID);

			// Add AGE column style.
			DataGridColumnStyle colRUserID = new DataGridTextBoxColumn();
			colRUserID.MappingName = "AGE";
			colRUserID.HeaderText = "Age";
			colRUserID.Width = 50;
			tsRU.GridColumnStyles.Add(colRUserID);

			// Add SEX column style.
			DataGridColumnStyle colSex = new DataGridTextBoxColumn();
			colSex.MappingName = "SEX";
			colSex.HeaderText = "Sex";
			colSex.Width = 50;
			tsRU.GridColumnStyles.Add(colSex);

			// Add DOB column style.
			DataGridColumnStyle colDOB = new DataGridTextBoxColumn();
			colDOB.MappingName = "DOB";
			colDOB.HeaderText = "Date of Birth";
			colDOB.Width = 100;
			tsRU.GridColumnStyles.Add(colDOB);

			this.dataGrid1.TableStyles.Add(tsRU);

			//Create DataGridTableStyle for Visit table
			DataGridTableStyle tsVisit = new DataGridTableStyle();
			tsVisit.MappingName = "Visit";
			tsVisit.ReadOnly = true;

			// Add VISIT/ADMIT_DATE&TIME column style
			DataGridColumnStyle colVDate = new DataGridTextBoxColumn();
			colVDate.MappingName = "VISIT/ADMIT_DATE&TIME";
			colVDate.HeaderText = "Visit Date & Time";
			colVDate.Width = 150;
			tsVisit.GridColumnStyles.Add(colVDate);

			// Add LOC._OF_ENCOUNTER column style
			DataGridColumnStyle colLocation = new DataGridTextBoxColumn();
			colLocation.MappingName = "LOC._OF_ENCOUNTER";
			colLocation.HeaderText = "Location of Encounter";
			colLocation.Width = 200;
			tsVisit.GridColumnStyles.Add(colLocation);

			// Add CLINIC column style
			DataGridColumnStyle colClinic = new DataGridTextBoxColumn();
			colClinic.MappingName = "CLINIC";
			colClinic.HeaderText = "Clinic";
			colClinic.Width = 100;
			tsVisit.GridColumnStyles.Add(colClinic);

			this.dataGrid1.TableStyles.Add(tsVisit);

		}

	}
}
