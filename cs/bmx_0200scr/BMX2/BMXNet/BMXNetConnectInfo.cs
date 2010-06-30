using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Soap;
using System.Windows.Forms;
using System.Security.Principal;
using System.Text;
using System.Security.Cryptography;
using System.Timers;
using System.Threading;


namespace IndianHealthService.BMXNet 
{
	public class BMXNetEventArgs : EventArgs
	{
		public string BMXParam;
		public string BMXEvent;
	}

	/// <summary>
	/// Contains methods and properties to support RPMS Login for .NET applications
	/// </summary>
	public class BMXNetConnectInfo : System.Windows.Forms.Control
	{

		/// <summary>
		/// Serializes RPMS server address and port
		/// </summary>
		[SerializableAttribute]
			private class ServerData
		{
			public string	m_sAddress = "";
			public int		m_nPort = 0;
            public string   m_sNamespace = "";
			
			public ServerData()
			{
			}

			public ServerData(string sAddress, int nPort)
			{
				this.m_nPort = nPort;
				this.m_sAddress = sAddress;
                this.m_sNamespace = "";
			}
			public ServerData(string sAddress, int nPort, string sNamespace)
			{
				this.m_nPort = nPort;
				this.m_sAddress = sAddress;
                this.m_sNamespace = sNamespace;
			}
        }

		public BMXNetConnectInfo()
		{
			m_BMXNetLib = new BMXNetLib();

			//Initialize BMXNetEvent timer
			m_timerEvent = new System.Timers.Timer();
			m_timerEvent.Elapsed+=new ElapsedEventHandler(OnEventTimer);
			m_timerEvent.Interval = 10000;
			m_timerEvent.Enabled = false;
		}

		#region BMXNetEvent

		private System.Timers.Timer m_timerEvent;
		public delegate void BMXNetEventDelegate(Object obj, BMXNetEventArgs e);
		public event BMXNetEventDelegate BMXNetEvent;

		/// <summary>
		/// Enables and disables event polling for the RPMS connection
		/// </summary>
		public bool EventPollingEnabled
		{
			get
			{
				return m_timerEvent.Enabled;
			}
			set
			{
//				Debug.Write("ConnectInfo handle: " + this.Handle.ToString() + "\n");
				//System.IntPtr pHandle = this.Handle;
				m_timerEvent.Enabled = value;
			}
		}
 
		/// <summary>
		/// Sets and retrieves the interval in milliseconds for RPMS event polling
		/// </summary>
		public double EventPollingInterval
		{
			get
			{
				return m_timerEvent.Interval;
			}
			set
			{
				m_timerEvent.Interval = value;
			}
		}

		/// <summary>
		/// Register interest in an RPMS event.
		/// </summary>
		/// <param name="EventName"></param>
		/// <returns></returns>
		public int SubscribeEvent(string EventName)
		{
			try
			{
				//System.IntPtr pHandle = this.Handle;
				DataTable dt;
				RPMSDataTableDelegate rdtd = new RPMSDataTableDelegate(RPMSDataTable);
                if (this.IsHandleCreated == false)
                {
                    this.CreateHandle();
                }
				dt = (DataTable) this.Invoke(rdtd, new object[] {"BMX EVENT REGISTER^" + EventName, "dt"});

//				dt = RPMSDataTable("BMX EVENT REGISTER^" + EventName, "dt");
				DataRow dr = dt.Rows[0];
				int nRet = (int) dr["ERRORID"];
				return nRet;
			}
			catch (Exception ex)
			{
				Debug.Write(ex.Message);
				return 99;
			}
		}

		/// <summary>
		/// Unregister an RPMS event
		/// </summary>
		/// <param name="EventName"></param>
		/// <returns></returns>
		public int UnSubscribeEvent(string EventName)
		{
			try
			{
				DataTable dt;
				RPMSDataTableDelegate rdtd = new RPMSDataTableDelegate(RPMSDataTable);
                if (this.IsHandleCreated == false)
                {
                    this.CreateHandle();
                }
				dt = (DataTable) this.Invoke(rdtd, new object[] {"BMX EVENT UNREGISTER^" + EventName, "dt"});
				
				DataRow dr = dt.Rows[0];
				int nRet = (int) dr["ERRORID"];
				return nRet;
			}
			catch (Exception ex)
			{
				Debug.Write(ex.Message);
				return 99;
			}
		}

		/// <summary>
		///  Raise an RPMS event
		/// </summary>
		/// <param name="EventName">The name of the event to raise</param>
		/// <param name="Param">Parameters associated with the event</param>
		/// <param name="RaiseBack">True if the event should be raised back to the caller</param>
		/// <returns></returns>
		public int RaiseEvent(string EventName, string Param, bool RaiseBack)
		{
			string sBack = (RaiseBack == true)?"TRUE":"FALSE";
			try
			{
				DataTable dt;
				RPMSDataTableDelegate rdtd = new RPMSDataTableDelegate(RPMSDataTable);
                if (this.IsHandleCreated == false)
                {
                    this.CreateHandle();
                }
				dt = (DataTable) this.Invoke(rdtd, new object[] {"BMX EVENT RAISE^" + EventName + "^" + Param + "^" + sBack + "^", "dt"});

				DataRow dr = dt.Rows[0];
				int nRet = (int) dr["ERRORID"];
				return nRet;
			}
			catch (Exception ex)
			{
				Debug.Write(ex.Message);
				return 99;
			}
		}

		/// <summary>
		/// Sets and retrieves the number of times that the Event Timer will generage a BMXNet AutoFire event.
		/// For example, if AutoFire == 3, then every 3rd time the Event Timer fires, it will generate an AutoFire event.
		/// </summary>
		public int AutoFire
		{
			get
			{
				return m_nAutoFireIncrements;
			}
			set
			{
				m_nAutoFireIncrements = value;
			}
		}

		//Retrieve events registered by this session
		private int m_nAutoFireIncrements = 0;
		private int m_nAutoFireCount = 0;

		private void OnEventTimer(object source, ElapsedEventArgs e)
		{
			try
			{
				this.bmxNetLib.BMXRWL.AcquireWriterLock(5);
				try
				{
					this.m_timerEvent.Enabled = false;

					Object obj = this;
					BMXNetEventArgs args = new BMXNetEventArgs();
					m_nAutoFireCount++;
					if ((m_nAutoFireIncrements > 0)&&(m_nAutoFireCount >= m_nAutoFireIncrements))
					{
						m_nAutoFireCount = 0;
						args.BMXEvent = "BMXNet AutoFire";
						args.BMXParam = "";
						if (BMXNetEvent != null)
						{
							BMXNetEvent(obj, args);
						}
						this.m_timerEvent.Enabled = true;
						return;
					}

					if (m_BMXNetLib.Connected == false)
					{
						this.m_timerEvent.Enabled = true;
						return;
					}

					DataTable dtEvents = new DataTable("BMXNetEvents");

					try
					{
                        if (this.IsHandleCreated == false)
                        {
                            this.CreateHandle();
                        }
                        RPMSDataTableDelegate rdtd = new RPMSDataTableDelegate(RPMSDataTable);
						dtEvents = (DataTable) this.Invoke(rdtd, new object[] {"BMX EVENT POLL", "BMXNetEvents"});
					}
					catch (Exception ex)
					{
						string sMsg = ex.Message;
						this.m_timerEvent.Enabled = true;
						return;
					}

					try
					{
						if (dtEvents.Rows.Count == 0)
						{
							this.m_timerEvent.Enabled = true;
							return;
						}
					}
					catch(Exception ex)
					{
						Debug.Write("upper Exception in BMXNetConnectInfo.OnEventTimer: " + ex.Message + "\n");
					}
					try
					{
						//If events exist, raise BMXNetEvent
						foreach (DataRow dr in dtEvents.Rows)
						{
							args.BMXEvent = dr["EVENT"].ToString();
							args.BMXParam = dr["PARAM"].ToString();
							if (BMXNetEvent != null)
							{
								BMXNetEvent(obj, args);
							}
						}
						this.m_timerEvent.Enabled = true;
						return;
					}
					catch(Exception ex)
					{
						Debug.Write("lower Exception in BMXNetConnectInfo.OnEventTimer: " + ex.Message + "\n");
					}
				}
				catch(Exception ex)
				{
					Debug.Write("Exception in BMXNetConnectInfo.OnEventTimer: " + ex.Message + "\n");
				}
				finally
				{
					this.bmxNetLib.BMXRWL.ReleaseWriterLock();
					this.m_timerEvent.Enabled = true;
				}
			}
			catch
			{
				Debug.Write("     OnEventTimer failed to obtain lock.\n");
			}
		}

		#endregion BMXNetEvent

		#region Fields

		private ServerData				m_ServerData;
		private string					m_sServerAddress;
		private int						m_nServerPort;
        private string                  m_sServerNamespace;
		private string					m_sDUZ2;
		private	int						m_nDivisionCount = 0;
		private	string					m_sUserName;
		private	string					m_sDivision;
		private string					m_sAppcontext;
		private BMXNetLib				m_BMXNetLib;
		private bool					m_bAutoServer = false;
		private bool					m_bAutoLogin = false;

		#endregion Fields

		#region Properties

//		/// <summary>
//		/// Set and retrieve the timeout in milliseconds for locking the transmit port.
//		/// If the transmit port is unavailable an ApplicationException will be thrown.
//		/// </summary>
//		public int TransmitLockTimeout
//		{
//			get
//			{
//				return m_nTransmitLockTimeout;
//			}
//			set
//			{
//				m_nTransmitLockTimeout = value;
//			}
//		}

        /// <summary>
        /// Set and retrieve the timeout, in milliseconds, to receive a response from the RPMS server.
        /// If the retrieve time exceeds the timeout, an exception will be thrown and the connection will be closed.
        /// The default is 30 seconds.
        /// </summary>
        public int ReceiveTimeout
        {
            get { return m_BMXNetLib.ReceiveTimeout; }
            set { m_BMXNetLib.ReceiveTimeout = value; }
        }

		public string MServerNameSpace
		{
			get
			{
                return m_BMXNetLib.MServerNamespace;
			}
			set
			{
                m_BMXNetLib.MServerNamespace = value;
			}
		}

		public BMXNetLib bmxNetLib
		{
			get
			{
				return m_BMXNetLib;
			}
		}

		public string AppContext
		{
			get
			{
				return m_sAppcontext;
			}
			set
			{
				if (m_BMXNetLib.Connected == true)
				{
					try
					{
						try
						{
							m_BMXNetLib.AppContext = value;
							m_sAppcontext = value;
						}
						catch (Exception ex)
						{
							Debug.Write(ex.Message);
							throw ex;
						}
						finally
						{
						}
					}
					catch (ApplicationException aex)
					{
						// The writer lock request timed out.
						Debug.Write("BMXNetConnectInfo.AppContext lock request timed out.\n");
						throw aex;
					}
				}//end if
			}//end set
		}

		public bool Connected
		{
			get
			{
				return m_BMXNetLib.Connected;
			}
		}

		public string UserName
		{
			get
			{
				return this.m_sUserName;
			}
		}

		public string DivisionName
		{
			get
			{
				return this.m_sDivision;
			}
		}

		/// <summary>
		/// Returns a string representation of DUZ
		/// </summary>
		public string DUZ
		{
			get
			{
				return this.bmxNetLib.DUZ;
			}
		}

		/// <summary>
		/// Sets and Returns DUZ(2)
		/// </summary>
		public string DUZ2
		{
			get
			{
				return m_sDUZ2;
			}
			set
			{
				try
				{
					//Set DUZ(2) in M partition
					DataTable dt = this.RPMSDataTable("BMXSetFac^" + value, "SetDUZ2");
					Debug.Assert(dt.Rows.Count == 1);
					DataRow dr = dt.Rows[0];
					string sDUZ2 = dr["FACILITY_IEN"].ToString();
					if (sDUZ2 != "0")
					{
						m_sDUZ2 = sDUZ2;
						this.m_sDivision = dr["FACILITY_NAME"].ToString();
					}
				}
				catch (Exception ex)
				{
					Debug.Write("DUZ2.Set failed: " + ex.Message + "\n");
				}
			}
		}

		/// <summary>
		/// Gets the address of the  RPMS Server
		/// </summary>
		public string MServerAddress
		{
			get
			{
				return this.m_sServerAddress;
			}
		}

		/// <summary>
		/// Gets the port on which the MServer is connected
		/// </summary>
		public int MServerPort
		{
			get
			{
				return this.m_nServerPort;
			}
		}

		public DataTable UserDivisions
		{
			get
			{
				DataTable dt = this.GetUserDivisions();
				return dt;
			}
		}

		#endregion Properties

		#region Methods

		public void CloseConnection()
		{
			//TODO: Make thread safe
            this.m_bAutoServer = false;
            this.m_bAutoLogin = false;
			m_BMXNetLib.CloseConnection();
		}

		/// <summary>
		/// For backwards compatibility.  Internally calls LoadConnectInfo()
		/// </summary>
		public void Login()
		{
			LoadConnectInfo();
		}

		/// <summary>
		/// Change the internet address and port of the RPMS server
		/// Throws a BMXNetException if user cancels
		/// </summary>
		public void ChangeServerInfo()
		{
			//Get existing values from isolated storage
			ServerData serverData = new ServerData();
			Stream stStorage = null;
			try
			{
				IsolatedStorageFile isStore = IsolatedStorageFile.GetUserStoreForAssembly();
				string sFileName = "mserver0200.dat";
				stStorage = new IsolatedStorageFileStream(sFileName, FileMode.Open, isStore);
				IFormatter formatter = new SoapFormatter();
				serverData = (ServerData) formatter.Deserialize(stStorage);
				stStorage.Close();
			}
			catch (Exception ex)
			{
				Debug.Write(ex.Message);
				if (stStorage != null)
					stStorage.Close();
			}

			try
			{
				DServerInfo dsi = new DServerInfo();
				dsi.InitializePage(serverData.m_sAddress,serverData.m_nPort, serverData.m_sNamespace);
				if (dsi.ShowDialog() != DialogResult.OK)
				{
					throw new BMXNetException("User cancelled.");
				}
				serverData.m_sAddress = dsi.MServerAddress;
				serverData.m_nPort = dsi.MServerPort;
                serverData.m_sNamespace = dsi.MServerNamespace;

				this.m_sServerAddress = dsi.MServerAddress;
				this.m_nServerPort = dsi.MServerPort;
                this.m_sServerNamespace = dsi.MServerNamespace;

				//Save port and address to isolated storage
				try
				{
                    string sFileName = "mserver0200.dat";
					IsolatedStorageFile isStore = IsolatedStorageFile.GetUserStoreForAssembly();
					stStorage = new IsolatedStorageFileStream(sFileName, FileMode.Create, isStore);
					IFormatter formatter = new SoapFormatter();
					formatter.Serialize(stStorage, serverData);
					stStorage.Close();
				}
				catch (Exception ex)
				{
					Debug.Write(ex.Message);
					if (stStorage != null)
						stStorage.Close();
				}

			}
			catch (Exception ex)
			{
				Debug.Write(ex.Message);
				if (stStorage != null)
					stStorage.Close();
				if (ex.Message == "User cancelled.")
				{
					throw ex;
				}
			}

		}

		private void GetServerInfo(ref int nPort, ref string sAddress, ref string sNamespace)
		{
			//Get values from isolated storage
			bool bLoaded = false;
			Stream stStorage = null;
			try
			{
				m_ServerData = new ServerData();
				IsolatedStorageFile isStore = IsolatedStorageFile.GetUserStoreForAssembly();
                string sFileName = "mserver0200.dat";
				stStorage = new IsolatedStorageFileStream(sFileName, FileMode.Open, isStore);
				IFormatter formatter = new SoapFormatter();
				m_ServerData = (ServerData) formatter.Deserialize(stStorage);
				stStorage.Close();
				sAddress = m_ServerData.m_sAddress;
				nPort = m_ServerData.m_nPort;
                sNamespace = m_ServerData.m_sNamespace;

				bLoaded = true;
				return;
			}
			catch (Exception ex)
			{
				Debug.Write(ex.Message);
				if (stStorage != null)
					stStorage.Close();
			}
			try
			{
				//If unable to deserialize, display dialog to collect values
				if (bLoaded == false)
				{
					DServerInfo dsi = new DServerInfo();
					dsi.InitializePage("",10501,"");
					if (dsi.ShowDialog() != DialogResult.OK)
					{
						throw new BMXNetException("Unable to get M Server information");
					}
					m_ServerData.m_sAddress = dsi.MServerAddress;
					m_ServerData.m_nPort = dsi.MServerPort;
                    m_ServerData.m_sNamespace = dsi.MServerNamespace;
				}

				sAddress = m_ServerData.m_sAddress;
				nPort = m_ServerData.m_nPort;
                sNamespace = m_ServerData.m_sNamespace;

				//Save port and address to isolated storage
				try
				{
                    string sFileName = "mserver0200.dat";
					IsolatedStorageFile isStore = IsolatedStorageFile.GetUserStoreForAssembly();
					stStorage = new IsolatedStorageFileStream(sFileName, FileMode.Create, isStore);
					IFormatter formatter = new SoapFormatter();
					formatter.Serialize(stStorage, m_ServerData);
					stStorage.Close();
				}
				catch (Exception ex)
				{
					Debug.Write(ex.Message);
					if (stStorage != null)
						stStorage.Close();
				}

				return;
			}
			catch (Exception ex)
			{
				Debug.Write(ex.Message);
				if (stStorage != null)
					stStorage.Close();
				throw ex;
			}

		}
		
		/// <summary>
		/// Called to connect to an M server
		/// Server address, port, Access and Verify codes will be
		/// prompted for depending on whether these values are
		/// cached.
		/// </summary>
		public void LoadConnectInfo()
		{
			m_bAutoServer = true;
			m_bAutoLogin = true;
			LoadConnectInfo("",0,"","");
		}

		/// <summary>
		/// Called to connect to the M server specified by
		/// server address and listener port.  The default namespace on the server will be used.
		/// Access and Verify codes will be prompted if 
		/// valid values for the current Windows Identity are
		/// not cached on the server.
		/// </summary>
		/// <param name="MServerAddress">The IP address or name of the MServer</param>
		/// <param name="Port">The port on which the BMXNet Monitor is listening</param>
        public void LoadConnectInfo(string MServerAddress, int Port)
        {
            LoadConnectInfo(MServerAddress, Port, "", "", "");
        }

		/// <summary>
		/// Called to connect to the M server specified by
		/// server address, listener port and namespace.
		/// Access and Verify codes will be prompted if 
		/// valid values for the current Windows Identity are
		/// not cached on the server.
		/// </summary>
		/// <param name="MServerAddress">The IP address or name of the MServer</param>
		/// <param name="Port">The port on which the BMXNet Monitor is listening</param>
        /// <param name="Namespace">The namespace in which the BMXNet application will run</param>
        public void LoadConnectInfo(string MServerAddress, int Port, string Namespace)
		{
			m_bAutoServer = false;
			m_bAutoLogin = true;
			LoadConnectInfo(MServerAddress, Port,"","", Namespace);
		}
		
		/// <summary>
		/// Called to connect to an M server
		/// using specific Access and Verify codes.
		/// Server address and port will be prompted if they
		/// are not cached in local storage.
		/// </summary>
		/// <param name="AccessCode">The user's access code</param>
		/// <param name="VerifyCode">The user's verify code</param>
		public void LoadConnectInfo(string AccessCode, string VerifyCode)
		{
			m_bAutoServer = true;
			m_bAutoLogin = false;
			LoadConnectInfo("", 0,AccessCode,VerifyCode);
		}

		/// <summary>
		/// Called to connect to a specific M server
		/// using specific Access and Verify codes.
		/// </summary>
		/// <param name="AccessCode">The user's access code</param>
		/// <param name="VerifyCode">The user's verify code</param>
		/// <param name="MServerAddress">The IP address or name of the MServer</param>
		/// <param name="Port">The port on which the BMXNet Monitor is listening</param>
        public void LoadConnectInfo(string MServerAddress, int Port,
            string AccessCode, string VerifyCode)
        {
            LoadConnectInfo(MServerAddress, Port, AccessCode, VerifyCode, "");
        }

        /// <summary>
        /// Called to connect to a specific namespace on the M server
        /// using specific Access and Verify codes.
        /// </summary>
        /// <param name="AccessCode">The user's access code</param>
        /// <param name="VerifyCode">The user's verify code</param>
        /// <param name="MServerAddress">The IP address or name of the MServer</param>
        /// <param name="Port">The port on which the BMXNet Monitor is listening</param>
        /// <param name="Namespace">The namespace in which the BMXNet application will run</param>
        public void LoadConnectInfo(string MServerAddress, int Port,
			string AccessCode, string VerifyCode, string Namespace)
        {		
			//Throws exception if unable to connect to RPMS

			/* 
			 * Get RPMS Server Address and Port from local storage.
			 * Prompt for them if they're not there.
			 *
			 * Throw exception if unable to get address/port
			*/

			if (m_bAutoServer == true)
			{
				string sAddress = "";
				int nPort = 0;
                string sNamespace = "";
				try
				{
					GetServerInfo(ref nPort, ref sAddress, ref sNamespace);
					m_nServerPort = nPort;
					m_sServerAddress = sAddress;
                    m_sServerNamespace = sNamespace;
				}
				catch (Exception ex)
				{
					Debug.Write(ex.Message);
					throw ex;
				}
			}
			else
			{
				m_sServerAddress = MServerAddress;
				m_nServerPort = Port;
                m_sServerNamespace = Namespace;
			}

			/*
			 * Connect to RPMS using current windows identity
			 * Execute BMXNetGetCodes(NTDomain/UserName) to get encrypted AV codes
			 * 
			 */

			m_BMXNetLib.CloseConnection();
			m_BMXNetLib.MServerPort = m_nServerPort;
            m_BMXNetLib.MServerNamespace = m_sServerNamespace;

			string sLoginError = "";
			WindowsIdentity winIdentity = WindowsIdentity.GetCurrent();
			string	sIdentName = winIdentity.Name;
			string	sIdentType = winIdentity.AuthenticationType;
			bool	bIdentIsAuth = winIdentity.IsAuthenticated;
			bool	bRet = false;
			if (m_bAutoLogin == true)
			{
				try 
				{
					//Attempt Auto-login using WindowsIdentity

					if (bIdentIsAuth == false)
					{
						throw new BMXNetException("Current Windows User is not authenticated");
					}
					bRet = m_BMXNetLib.OpenConnection(m_sServerAddress, winIdentity);
					
					try
					{
						string sDuz = m_BMXNetLib.DUZ;
						int nDuz = Convert.ToInt16(sDuz);
					}
					catch (Exception exCV)
					{
						Debug.Write("OpenConnection failed: " + exCV.Message);
						//Debug.Assert(false);
                        throw new Exception(exCV.Message);
					}
				}
				catch (Exception ex)
				{
					Debug.Write(ex.Message);
					sLoginError = ex.Message;
				}
			}

			if (m_BMXNetLib.Connected == false) //BMXNET Login failed or m_bAutoLogin == false
			{
				try
				{
					//If autologin was attempted and
					//error message anything other than
					//"invalid AV code pair"
					// or "User BMXNET,APPLICATION does not have access to option BMXRPC",
					// or current windows user is not authenticated or is a guest
					//then rethrow exception.
					if ((m_bAutoLogin == true)
						&&(BMXNetLib.FindSubString(sLoginError, "Not a valid ACCESS CODE/VERIFY CODE pair.") == -1)
						&&(BMXNetLib.FindSubString(sLoginError, "User BMXNET,APPLICATION does not have access to option BMXRPC") == -1)
						&&(BMXNetLib.FindSubString(sLoginError, "Not a valid Windows Identity map value.") == -1)
						&&(BMXNetLib.FindSubString(sLoginError, "Current Windows User is not authenticated") == -1)
						&&(BMXNetLib.FindSubString(sLoginError, "Windows Integrated Security Not Allowed on this port.") == -1)
						)
					{
						throw new BMXNetException(sLoginError);
					}

					//Display dialog to collect user-input AV
					
					DLoginInfo dLog = new DLoginInfo();
					DialogResult bStop = DialogResult.OK;
					m_BMXNetLib.CloseConnection();
					if ((AccessCode == "") && (VerifyCode == "")) 
					{
						//nothing was passed in, so display a dialog to collect AV codes
						do
						{
							dLog.InitializePage("","");
							bStop = dLog.ShowDialog();
							if (bStop == DialogResult.Cancel)
							{
								throw new BMXNetException("User cancelled login.");
							}
							try
							{
								string sTempAccess = dLog.AccessCode;
								string sTempVerify = dLog.VerifyCode;
								bRet = m_BMXNetLib.OpenConnection(m_sServerAddress, sTempAccess, sTempVerify);
							}
							catch (Exception ex)
							{
								Debug.Write(ex.Message);
								//MessageBox.Show(ex.Message, "RPMS Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                throw new Exception(ex.Message); ;
							}
						}while ((bStop == DialogResult.OK) && (m_BMXNetLib.Connected == false));
					}
					else //caller passed in AV codes
					{
						try
						{
							//Connect using caller's AV
							bRet = m_BMXNetLib.OpenConnection(m_sServerAddress, AccessCode, VerifyCode);
						}
						catch (Exception ex)
						{
							Debug.Write(ex.Message);
							//MessageBox.Show(ex.Message, "RPMS Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            throw new BMXNetException(ex.Message);
						}
					}

					//Map Windows Identity to logged-in RPMS user
					if ((bIdentIsAuth == true) && (m_BMXNetLib.Connected == true))
					{
						m_BMXNetLib.AppContext = "BMXRPC";
						string sRes = m_BMXNetLib.TransmitRPC("BMXNetSetUser", sIdentName);
						Debug.Write("LoadConnectInfo BMXNetSetUser returned " + sRes + "\n");
					}
				}
				catch (Exception ex)
				{
					Debug.Write(ex.Message);
					m_BMXNetLib.CloseConnection();
					throw ex; //this exception will be caught by the caller.
				}
			}//End if (m_BMXNetLib.Connected == false)

			try 
			{
				Debug.Assert(m_BMXNetLib.Connected == true);
				m_BMXNetLib.AppContext = "BMXRPC";
				string sRpc = "BMX USER";
				Debug.Assert(m_BMXNetLib.AppContext == "BMXRPC");

				bool bCtxt = false;
				int nCtxt = 0;
				do
				{
					try
					{
						m_sUserName = m_BMXNetLib.TransmitRPC(sRpc, this.DUZ);
						bCtxt = true;
						Debug.Write("BMXNet::LoadConnectInfo succeeded.\n");
					}
					catch (Exception ex)
					{
						Debug.Write("BMXNet::LoadConnectInfo retrying: " + ex.Message + "\n");
						m_BMXNetLib.AppContext = "BMXRPC";
						nCtxt++;
						if (nCtxt > 4)
							throw ex;
					}
				}while (bCtxt == false);


				System.Data.DataTable rsDivisions;
				rsDivisions =  this.GetUserDivisions();
				m_nDivisionCount = rsDivisions.Rows.Count;

				//The MOST_RECENT_LOOKUP field contains DUZ(2)
				foreach (System.Data.DataRow r in rsDivisions.Rows)
				{
					string sTemp = r["MOST_RECENT_LOOKUP"].ToString();
					if ((sTemp == "1") || (rsDivisions.Rows.Count == 1))
					{
						this.m_sDivision = r["FACILITY_NAME"].ToString();
						this.m_sDUZ2 = r["FACILITY_IEN"].ToString();
						break;
					}
				}
			}
			catch(Exception bmxEx)
			{
				m_BMXNetLib.CloseConnection();
				string sMessage =  bmxEx.Message + bmxEx.StackTrace;
				throw new BMXNetException(sMessage);
			}
			return;
		}

		private DataTable GetUserDivisions()
		{
			try
			{
				DataTable tb = this.RPMSDataTable("BMXGetFacRS^" + this.DUZ, "DivisionTable");
				return tb;
			}
			catch (Exception bmxEx)
			{
				string sMessage =  bmxEx.Message + bmxEx.StackTrace;
				throw new BMXNetException(sMessage);

			}
		}

		public void ChangeDivision(System.Windows.Forms.Form frmCaller)
		{
			DSelectDivision dsd = new DSelectDivision();
			dsd.InitializePage(UserDivisions, DUZ2);

			if (dsd.ShowDialog(frmCaller) == DialogResult.Cancel)
				return;

			if (dsd.DUZ2 != this.DUZ2)
			{
				DUZ2 = dsd.DUZ2;
			}
		}

		public string GetDSN(string sAppContext)
		{
			string sDsn = "Data source=";
			if (sAppContext == "")
				sAppContext = "BMXRPC";

			if (m_BMXNetLib.Connected == false)
				return sDsn.ToString();

			sDsn += this.m_sServerAddress ;
			sDsn += ";Location=";
			sDsn += this.m_nServerPort.ToString();
			sDsn += ";Extended Properties=";
			sDsn += sAppContext;
			
			return sDsn;
		}


		public bool Lock(string sVariable, string sIncrement, string sTimeOut)
		{
			bool			bRet = false;
			string			sErrorMessage = "";
			
			if (m_BMXNetLib.Connected == false)
			{
				return bRet;
			}
			try
			{
				bRet = this.bmxNetLib.Lock(sVariable, sIncrement, sTimeOut);
				return bRet;
			}
			catch(Exception ex)
			{
				sErrorMessage = "CGDocumentManager.RPMSDataTable error: " + ex.Message;
				throw ex;
			}
		}

		delegate DataTable RPMSDataTableDelegate(string CommandString, string TableName);
		delegate DataTable RPMSDataTableDelegate2(string CommandString, string TableName, DataSet dsDataSet);

		/// <summary>
		/// Creates and names a DataTable using the command in CommandString
		/// and the name in TableName.
		/// </summary>
		/// <param name="CommandString"> The SQL or RPC call</param>
		/// <param name="TableName">The name of the resulting table</param>
		/// <returns>
		/// Returns the resulting DataTable.
		/// </returns>
		public DataTable RPMSDataTable(string CommandString, string TableName)
		{
			return this.RPMSDataTable(CommandString, TableName, null);
		}

		/// <summary>
		/// Creates and names a DataTable using the command in CommandString
		/// and the name in TableName then adds it to DataSet. 
		/// </summary>
		/// <param name="CommandString">The SQL or RPC call</param>
		/// <param name="TableName">The name of the resulting table</param>
		/// <param name="dsDataSet">The dataset in which to place the table</param>
		/// <returns>
		/// Returns the resulting DataTable.
		/// </returns>
		public DataTable RPMSDataTable(string CommandString, string TableName, DataSet dsDataSet)
		{
			//Retrieves a recordset from RPMS
			//Debug.Assert(this.InvokeRequired == false);
			string			sErrorMessage = "";
			DataTable		dtResult = new DataTable(TableName);

			if (m_BMXNetLib.Connected == false)
				return dtResult;

			try
			{
				BMXNetConnection rpmsConn = new BMXNetConnection(m_BMXNetLib);
				BMXNetCommand cmd = (BMXNetCommand) rpmsConn.CreateCommand();
				BMXNetDataAdapter da = new BMXNetDataAdapter();

				cmd.CommandText = CommandString;
				da.SelectCommand = cmd;
				if (dsDataSet == null)
				{
					da.Fill(dtResult);
				}
				else
				{
					da.Fill(dsDataSet, TableName);
					dtResult = dsDataSet.Tables[TableName];
				}
				Debug.Write(dtResult.TableName + " DataTable retrieved\n");
				return dtResult;
			}
			catch (Exception ex)
			{
				sErrorMessage = "CGDocumentManager.RPMSDataTable error: " + ex.Message;
				throw ex;
			}
		}

		public int RPMSDataTableAsyncQue(string CommandString, string EventName)
		{
			try
			{
				string sCommand = "BMX ASYNC QUEUE^";
				//replace ^'s in CommandString with $c(30)'s
				char[] cDelim = new char[1];
				cDelim[0] = (char) 30;
				string sDelim = cDelim[0].ToString();
				CommandString = CommandString.Replace("^", sDelim);
				sCommand = sCommand + CommandString + "^" + EventName;

				DataTable dt = new DataTable();
				RPMSDataTableDelegate rdtd = new RPMSDataTableDelegate(RPMSDataTable);
                if (this.IsHandleCreated == false)
                {
                    this.CreateHandle();
                }

				dt = (DataTable) this.Invoke(rdtd, new object[] {sCommand, "Que"});

				DataRow dr = dt.Rows[0];
				int nErrorID = Convert.ToInt32(dr["ERRORID"]);
				int nParam = Convert.ToInt32(dr["PARAM"]);

				if (nErrorID == 0)
				{
					return 0;
				}
				else
				{
					return nParam;
				}
			}
			catch (Exception ex)
			{
				Debug.Write("RPMSDataTableAsyncQue  error: " + ex.Message + "\n");
				throw ex;
			}
		}

		public DataTable RPMSDataTableAsyncGet(string AsyncInfo, string TableName)
		{
			return RPMSDataTableAsyncGet(AsyncInfo, TableName, null);
		}

		public DataTable RPMSDataTableAsyncGet(string AsyncInfo, string TableName, DataSet dsDataSet)
		{
			try
			{
				string sCommand = "BMX ASYNC GET^" + AsyncInfo;

				DataTable dt;
				RPMSDataTableDelegate rdtd = new RPMSDataTableDelegate(RPMSDataTable);
				RPMSDataTableDelegate2 rdtd2 = new RPMSDataTableDelegate2(RPMSDataTable);
                if (this.IsHandleCreated == false)
                {
                    this.CreateHandle();
                }

				if (dsDataSet == null)
				{
					dt = (DataTable) this.Invoke(rdtd, new object[] {sCommand, TableName});
				}
				else
				{
					dt = (DataTable) this.Invoke(rdtd2, new object[] {sCommand, TableName, dsDataSet});
				}
				return dt;
			}
			catch (Exception ex)
			{
				Debug.Write("RPMSDataTableAsyncGet error: " + ex.Message + "\n");
				throw ex;
			}
		}

		#endregion Methods

	}
}
