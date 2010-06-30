using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace IndianHealthService.BMXNet
{
	/// <summary>
	/// Summary description for DSelectDivision.
	/// </summary>
	public partial class DSelectDivision : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Panel pnlPageBottom;
		private System.Windows.Forms.Button cmdCancel;
		private System.Windows.Forms.Button cmdOK;
		private System.Windows.Forms.Panel pnlDescription;
		private System.Windows.Forms.GroupBox grpDescriptionResourceGroup;
		private System.Windows.Forms.Label lblDescriptionResourceGroup;
		private System.Windows.Forms.ListBox lstDivision;
		private System.Windows.Forms.Label label1;
		private System.ComponentModel.Container components = null;

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.pnlPageBottom = new System.Windows.Forms.Panel();
			this.cmdCancel = new System.Windows.Forms.Button();
			this.cmdOK = new System.Windows.Forms.Button();
			this.pnlDescription = new System.Windows.Forms.Panel();
			this.grpDescriptionResourceGroup = new System.Windows.Forms.GroupBox();
			this.lblDescriptionResourceGroup = new System.Windows.Forms.Label();
			this.lstDivision = new System.Windows.Forms.ListBox();
			this.label1 = new System.Windows.Forms.Label();
			this.pnlPageBottom.SuspendLayout();
			this.pnlDescription.SuspendLayout();
			this.grpDescriptionResourceGroup.SuspendLayout();
			this.SuspendLayout();
			// 
			// pnlPageBottom
			// 
			this.pnlPageBottom.Controls.Add(this.cmdCancel);
			this.pnlPageBottom.Controls.Add(this.cmdOK);
			this.pnlPageBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.pnlPageBottom.Location = new System.Drawing.Point(0, 262);
			this.pnlPageBottom.Name = "pnlPageBottom";
			this.pnlPageBottom.Size = new System.Drawing.Size(456, 40);
			this.pnlPageBottom.TabIndex = 7;
			// 
			// cmdCancel
			// 
			this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cmdCancel.Location = new System.Drawing.Point(376, 8);
			this.cmdCancel.Name = "cmdCancel";
			this.cmdCancel.Size = new System.Drawing.Size(56, 24);
			this.cmdCancel.TabIndex = 4;
			this.cmdCancel.Text = "Cancel";
			// 
			// cmdOK
			// 
			this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.cmdOK.Location = new System.Drawing.Point(296, 8);
			this.cmdOK.Name = "cmdOK";
			this.cmdOK.Size = new System.Drawing.Size(64, 24);
			this.cmdOK.TabIndex = 3;
			this.cmdOK.Text = "OK";
			this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
			// 
			// pnlDescription
			// 
			this.pnlDescription.Controls.Add(this.grpDescriptionResourceGroup);
			this.pnlDescription.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.pnlDescription.Location = new System.Drawing.Point(0, 190);
			this.pnlDescription.Name = "pnlDescription";
			this.pnlDescription.Size = new System.Drawing.Size(456, 72);
			this.pnlDescription.TabIndex = 8;
			// 
			// grpDescriptionResourceGroup
			// 
			this.grpDescriptionResourceGroup.Controls.Add(this.lblDescriptionResourceGroup);
			this.grpDescriptionResourceGroup.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grpDescriptionResourceGroup.Location = new System.Drawing.Point(0, 0);
			this.grpDescriptionResourceGroup.Name = "grpDescriptionResourceGroup";
			this.grpDescriptionResourceGroup.Size = new System.Drawing.Size(456, 72);
			this.grpDescriptionResourceGroup.TabIndex = 1;
			this.grpDescriptionResourceGroup.TabStop = false;
			this.grpDescriptionResourceGroup.Text = "Description";
			// 
			// lblDescriptionResourceGroup
			// 
			this.lblDescriptionResourceGroup.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblDescriptionResourceGroup.Location = new System.Drawing.Point(3, 16);
			this.lblDescriptionResourceGroup.Name = "lblDescriptionResourceGroup";
			this.lblDescriptionResourceGroup.Size = new System.Drawing.Size(450, 53);
			this.lblDescriptionResourceGroup.TabIndex = 0;
			this.lblDescriptionResourceGroup.Text = "Use this panel to select the Division.";
			// 
			// lstDivision
			// 
			this.lstDivision.Location = new System.Drawing.Point(40, 48);
			this.lstDivision.Name = "lstDivision";
			this.lstDivision.Size = new System.Drawing.Size(384, 121);
			this.lstDivision.TabIndex = 9;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(40, 24);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(240, 16);
			this.label1.TabIndex = 10;
			this.label1.Text = "Select Division:";
			// 
			// DSelectDivision
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(456, 302);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.lstDivision);
			this.Controls.Add(this.pnlDescription);
			this.Controls.Add(this.pnlPageBottom);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "DSelectDivision";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Select Division";
			this.pnlPageBottom.ResumeLayout(false);
			this.pnlDescription.ResumeLayout(false);
			this.grpDescriptionResourceGroup.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		#region Fields

		private DataTable		m_dtDivisions;
		private string			m_sDUZ2;
		private string			m_sDivisionName;

		#endregion Fields

		#region Properties
		
		public string DivisionName
		{
			get
			{
				return this.m_sDivisionName;
			}
		}

		public string DUZ2
		{
			get
			{
				return this.m_sDUZ2;
			}
		}
		#endregion Properties

		#region Methods

		public void InitializePage(DataTable dtDivisions, string sDUZ2)
		{
			m_dtDivisions = dtDivisions;
			m_sDUZ2 = sDUZ2;
			UpdateDialogData(true);
		}
	
		/// <summary>
		/// If b is true, moves member vars into control data
		/// otherwise, moves control data into member vars
		/// </summary>
		/// <param name="b"></param>
		private void UpdateDialogData(bool b)
		{
			if (b == true) //move member vars into controls
			{
				this.lstDivision.DataSource = m_dtDivisions;
				lstDivision.DisplayMember = "FACILITY_NAME";
				lstDivision.ValueMember = "FACILITY_IEN";
				lstDivision.SelectedValue = m_sDUZ2;
			}
			else //move control data into member vars
			{
				this.m_sDUZ2 = lstDivision.SelectedValue.ToString();
				this.m_sDivisionName = lstDivision.Text;
			}
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

		public DSelectDivision()
		{
			InitializeComponent();
		}

		#endregion Methods

		private void cmdOK_Click(object sender, System.EventArgs e)
		{
			this.UpdateDialogData(false);
			return;
		}
	}
}
