using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace IndianHealthService.BMXNet
{
	/// <summary>
	/// Summary description for DLoginInfo.
	/// </summary>
	public class DLoginInfo : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Panel pnlPageBottom;
		private System.Windows.Forms.Button cmdCancel;
		private System.Windows.Forms.Button cmdOK;
		private System.Windows.Forms.Panel pnlDescription;
		private System.Windows.Forms.GroupBox grpDescriptionResourceGroup;
		private System.Windows.Forms.Label lblDescriptionResourceGroup;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtVerify;
		private System.Windows.Forms.TextBox txtAccess;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public DLoginInfo()
		{
			InitializeComponent();
		}

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
            this.txtVerify = new System.Windows.Forms.TextBox();
            this.txtAccess = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
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
            this.pnlPageBottom.Location = new System.Drawing.Point(0, 174);
            this.pnlPageBottom.Name = "pnlPageBottom";
            this.pnlPageBottom.Size = new System.Drawing.Size(448, 40);
            this.pnlPageBottom.TabIndex = 6;
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
            this.pnlDescription.Location = new System.Drawing.Point(0, 102);
            this.pnlDescription.Name = "pnlDescription";
            this.pnlDescription.Size = new System.Drawing.Size(448, 72);
            this.pnlDescription.TabIndex = 7;
            // 
            // grpDescriptionResourceGroup
            // 
            this.grpDescriptionResourceGroup.Controls.Add(this.lblDescriptionResourceGroup);
            this.grpDescriptionResourceGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpDescriptionResourceGroup.Location = new System.Drawing.Point(0, 0);
            this.grpDescriptionResourceGroup.Name = "grpDescriptionResourceGroup";
            this.grpDescriptionResourceGroup.Size = new System.Drawing.Size(448, 72);
            this.grpDescriptionResourceGroup.TabIndex = 1;
            this.grpDescriptionResourceGroup.TabStop = false;
            this.grpDescriptionResourceGroup.Text = "Description";
            // 
            // lblDescriptionResourceGroup
            // 
            this.lblDescriptionResourceGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDescriptionResourceGroup.Location = new System.Drawing.Point(3, 16);
            this.lblDescriptionResourceGroup.Name = "lblDescriptionResourceGroup";
            this.lblDescriptionResourceGroup.Size = new System.Drawing.Size(442, 53);
            this.lblDescriptionResourceGroup.TabIndex = 0;
            this.lblDescriptionResourceGroup.Text = "Use this panel to enter the Access and Verify codes that you use to log in to the" +
                " VISTA server. ";
            // 
            // txtVerify
            // 
            this.txtVerify.Location = new System.Drawing.Point(160, 56);
            this.txtVerify.Name = "txtVerify";
            this.txtVerify.PasswordChar = '*';
            this.txtVerify.Size = new System.Drawing.Size(160, 20);
            this.txtVerify.TabIndex = 2;
            // 
            // txtAccess
            // 
            this.txtAccess.Location = new System.Drawing.Point(160, 24);
            this.txtAccess.Name = "txtAccess";
            this.txtAccess.PasswordChar = '*';
            this.txtAccess.Size = new System.Drawing.Size(160, 20);
            this.txtAccess.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(24, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(128, 16);
            this.label2.TabIndex = 12;
            this.label2.Text = "VISTA Verify Code:";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(24, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(128, 16);
            this.label1.TabIndex = 11;
            this.label1.Text = "VISTA Access Code:";
            // 
            // DLoginInfo
            // 
            this.AcceptButton = this.cmdOK;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(448, 214);
            this.Controls.Add(this.txtVerify);
            this.Controls.Add(this.txtAccess);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pnlDescription);
            this.Controls.Add(this.pnlPageBottom);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "DLoginInfo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "VISTA Login";
            this.Load += new System.EventHandler(this.DLoginInfo_Load);
            this.Activated += new System.EventHandler(this.DLoginInfo_Activated);
            this.pnlPageBottom.ResumeLayout(false);
            this.pnlDescription.ResumeLayout(false);
            this.grpDescriptionResourceGroup.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		#region Fields
		string	m_sAccess;
		string	m_sVerify;
		#endregion Fields

		#region Methods

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

		public void InitializePage(string sAccess, string sVerify)
		{
			m_sAccess = sAccess;
			m_sVerify = sVerify;
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
				txtAccess.Text = m_sAccess;
				txtVerify.Text = m_sVerify;

			}
			else //move control data into member vars
			{
				m_sVerify = txtVerify.Text;
				m_sAccess = txtAccess.Text;
			}
		}

		private void cmdOK_Click(object sender, System.EventArgs e)
		{
			this.UpdateDialogData(false);
			return;
		}

		#endregion Methods

		private void DLoginInfo_Activated(object sender, System.EventArgs e)
		{
			txtAccess.Focus();
		}

		private void DLoginInfo_Load(object sender, System.EventArgs e)
		{
			this.Activate();
		}


		#region Properties

		/// <summary>
		/// Gets the access code entered by the user.
		/// </summary>
		public string AccessCode
		{
			get
			{
				return m_sAccess;
			}
		}

		/// <summary>
		/// Gets the verify code entered by the user.
		/// </summary>
		public string VerifyCode
		{
			get
			{
				return m_sVerify;
			}
		}
		#endregion Properties


	}
}
