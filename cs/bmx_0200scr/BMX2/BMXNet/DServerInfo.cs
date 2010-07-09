using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;

namespace IndianHealthService.BMXNet
{
	/// <summary>
	/// Prompts for RPMS Server address and port
	/// Obtains current values, if any, from isolated storage
	/// and uses them as defaults.
	/// If OK, then writes values to isolated storage and returns
	/// Address and Port as properties.
	/// 
	/// </summary>
    public partial class DServerInfo : System.Windows.Forms.Form
	{

		private System.Windows.Forms.Panel pnlPageBottom;
		private System.Windows.Forms.Button cmdCancel;
		private System.Windows.Forms.Button cmdOK;
		private System.Windows.Forms.Panel pnlDescription;
		private System.Windows.Forms.GroupBox grpDescriptionResourceGroup;
		private System.Windows.Forms.Label lblDescriptionResourceGroup;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtAddress;
		private System.Windows.Forms.TextBox txtPort;
        private TextBox txtNamespace;
        private Label label3;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public DServerInfo()
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DServerInfo));
            this.pnlPageBottom = new System.Windows.Forms.Panel();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.pnlDescription = new System.Windows.Forms.Panel();
            this.grpDescriptionResourceGroup = new System.Windows.Forms.GroupBox();
            this.lblDescriptionResourceGroup = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.txtNamespace = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
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
            this.pnlPageBottom.Location = new System.Drawing.Point(0, 203);
            this.pnlPageBottom.Name = "pnlPageBottom";
            this.pnlPageBottom.Size = new System.Drawing.Size(488, 40);
            this.pnlPageBottom.TabIndex = 5;
            // 
            // cmdCancel
            // 
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(376, 8);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(56, 24);
            this.cmdCancel.TabIndex = 2;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // cmdOK
            // 
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Location = new System.Drawing.Point(296, 8);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(64, 24);
            this.cmdOK.TabIndex = 1;
            this.cmdOK.Text = "OK";
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // pnlDescription
            // 
            this.pnlDescription.Controls.Add(this.grpDescriptionResourceGroup);
            this.pnlDescription.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlDescription.Location = new System.Drawing.Point(0, 131);
            this.pnlDescription.Name = "pnlDescription";
            this.pnlDescription.Size = new System.Drawing.Size(488, 72);
            this.pnlDescription.TabIndex = 6;
            // 
            // grpDescriptionResourceGroup
            // 
            this.grpDescriptionResourceGroup.Controls.Add(this.lblDescriptionResourceGroup);
            this.grpDescriptionResourceGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpDescriptionResourceGroup.Location = new System.Drawing.Point(0, 0);
            this.grpDescriptionResourceGroup.Name = "grpDescriptionResourceGroup";
            this.grpDescriptionResourceGroup.Size = new System.Drawing.Size(488, 72);
            this.grpDescriptionResourceGroup.TabIndex = 1;
            this.grpDescriptionResourceGroup.TabStop = false;
            this.grpDescriptionResourceGroup.Text = "Description";
            // 
            // lblDescriptionResourceGroup
            // 
            this.lblDescriptionResourceGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDescriptionResourceGroup.Location = new System.Drawing.Point(3, 16);
            this.lblDescriptionResourceGroup.Name = "lblDescriptionResourceGroup";
            this.lblDescriptionResourceGroup.Size = new System.Drawing.Size(482, 53);
            this.lblDescriptionResourceGroup.TabIndex = 0;
            this.lblDescriptionResourceGroup.Text = resources.GetString("lblDescriptionResourceGroup.Text");
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(24, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(128, 16);
            this.label1.TabIndex = 7;
            this.label1.Text = "VISTA Server Address:";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(24, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(128, 16);
            this.label2.TabIndex = 8;
            this.label2.Text = "Server Port:";
            // 
            // txtAddress
            // 
            this.txtAddress.Location = new System.Drawing.Point(160, 24);
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(280, 20);
            this.txtAddress.TabIndex = 9;
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(160, 56);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(72, 20);
            this.txtPort.TabIndex = 10;
            // 
            // txtNamespace
            // 
            this.txtNamespace.Location = new System.Drawing.Point(160, 89);
            this.txtNamespace.Name = "txtNamespace";
            this.txtNamespace.Size = new System.Drawing.Size(72, 20);
            this.txtNamespace.TabIndex = 12;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(24, 89);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(128, 16);
            this.label3.TabIndex = 11;
            this.label3.Text = "Server Namespace:";
            // 
            // DServerInfo
            // 
            this.AcceptButton = this.cmdOK;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(488, 243);
            this.Controls.Add(this.txtNamespace);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.txtAddress);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pnlDescription);
            this.Controls.Add(this.pnlPageBottom);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "DServerInfo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "VISTA Server Address";
            this.Load += new System.EventHandler(this.DServerInfo_Load);
            this.pnlPageBottom.ResumeLayout(false);
            this.pnlDescription.ResumeLayout(false);
            this.grpDescriptionResourceGroup.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		#region Fields
		string	m_sAddress;
		int		m_nPort;
        string  m_sNamespace;
		#endregion Fields

		#region Methods

		public void InitializePage(string sAddress, int nPort, string sNamespace)
		{
			m_sAddress = sAddress;
			m_nPort = nPort;
            m_sNamespace = sNamespace;
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
				txtAddress.Text = m_sAddress;
				txtPort.Text = m_nPort.ToString();
                txtNamespace.Text = m_sNamespace;

			}
			else //move control data into member vars
			{
				try
				{
					m_nPort = Convert.ToInt16(txtPort.Text);
				}
				catch (Exception ex)
				{
					Debug.Write(ex.Message);
					m_nPort = 0;
				}
				m_sAddress = txtAddress.Text;
                m_sNamespace = txtNamespace.Text;

			}
		}

		private void cmdOK_Click(object sender, System.EventArgs e)
		{
			this.UpdateDialogData(false);
			return;
		}

		private void cmdCancel_Click(object sender, System.EventArgs e)
		{
			return;
		}

		private void DServerInfo_Load(object sender, System.EventArgs e)
		{
			this.Activate();
		}
		#endregion Methods

		#region Properties

		/// <summary>
		/// Gets/sets the internet address of the RPMS Server
		/// </summary>
		public string MServerAddress
		{
			get
			{
				return m_sAddress;
			}
			set
			{
				m_sAddress = value;
			}
		}

        /// <summary>
        /// Gets/sets the namespace of the RPMS Server
        /// </summary>
        public string MServerNamespace
        {
            get
            {
                return m_sNamespace;
            }
            set
            {
                m_sNamespace = value;
            }
        }

		/// <summary>
		/// Gets/sets the TCP/IP Port of the RPMS Server
		/// </summary>
		public int MServerPort
		{
			get
			{
				return m_nPort;
			}
			set
			{
				m_nPort = value;
			}
	    }   
		#endregion Properties
	}
}
