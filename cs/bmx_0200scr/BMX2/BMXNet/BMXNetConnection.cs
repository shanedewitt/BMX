using System;
using System.Data;
using System.Diagnostics;
using System.Reflection;
using System.Security.Principal;

namespace IndianHealthService.BMXNet
{
	public class BMXNetConnection : System.Data.Common.DbConnection, IDbConnection
	{
		private ConnectionState		m_state;
		private string				m_sConnString;
		private BMXNetLib			m_rpx;
		private	RPMSDb				m_RPMSDb;
		private int					m_nTimeout;
		private string				m_sDatabase;

		private string				m_sAccess;
		private string				m_sVerify;
		private int					m_nPort;
		private string				m_sAddress;
		private string				m_sAppContext;
		private bool				m_bUseWinIdent;

		// default constructor.
		public BMXNetConnection()
		{
			// Initialize the connection object into the closed state.
			m_state = ConnectionState.Closed;
			this.m_rpx = new BMXNetLib();
		}
    
		public BMXNetConnection(string sConnString)
		{
			// Initialize the connection object into a closed state.
			m_state = ConnectionState.Closed;
			this.ConnectionString = sConnString;
			this.m_rpx = new BMXNetLib();
		}

		public BMXNetConnection(BMXNetLib bmxLib)
		{
			if (bmxLib.Connected == true)
			{
				m_state = ConnectionState.Open;
				m_rpx = bmxLib;
				m_RPMSDb = new RPMSDb(m_rpx);

			}
			else
			{
				m_state = ConnectionState.Closed;
			}
		}

		/****
		 * IMPLEMENT THE REQUIRED PROPERTIES.
		 ****/
		override public string ConnectionString
		{
			get
			{
				return m_sConnString;
			}
			set
			{
				m_nTimeout = 0;
				try 
				{
					// Parse values from connect string
					m_sConnString = value;
					string sSemi = ";";
					string sEq = "=";
					string sU = "^";
					char[] cSemi = sSemi.ToCharArray();
					char[] cEq = sEq.ToCharArray();
					char[] cU = sU.ToCharArray();
					string [] saSplit = m_sConnString.Split(cSemi);
					string [] saProp;
					string [] saTemp;
					string sPropName;
					string sPropVal;
					for (int j = 0; j<saSplit.Length; j++)
					{
						saProp = saSplit[j].Split(cEq);
						if (saProp.Length != 2)
						{
							//throw invalid parameter exception								
						}
						sPropName = saProp[0];
						sPropVal = saProp[1];
						sPropName = sPropName.ToUpper();
						if (sPropName == "PASSWORD")
						{
							saTemp = sPropVal.Split(cU);
							if (saTemp.Length != 2)
							{
								//throw invalid parameter exception								
							}

							m_sAccess = saTemp[0];
							m_sVerify = saTemp[1];
						}
						if (sPropName == "ACCESS CODE")
						{
							m_sAccess = sPropVal;
						}
						if (sPropName == "VERIFY CODE")
						{
							m_sVerify = sPropVal;
						}
						if ((sPropName == "LOCATION") || (sPropName == "PORT"))
						{
							m_nPort = Convert.ToInt16(sPropVal);
						}
						if (sPropName == "DATA SOURCE")
						{
							m_sAddress = sPropVal;
							m_sDatabase = sPropVal;
						}
						if (sPropName == "EXTENDED PROPERTIES")
						{
							m_sAppContext = sPropVal;
						}
						if (sPropName == "WINIDENT")
						{
							m_bUseWinIdent = false;
							sPropVal = sPropVal.ToUpper();
							if (sPropVal == "TRUE")
								m_bUseWinIdent = true;
						}
					}

				}
				catch (Exception e)
				{
					//re-throw exception
					throw e;
				}

			}
		}

		public override int ConnectionTimeout
		{
			get
			{
				// Returns the connection time-out value set in the connection
				// string. Zero indicates an indefinite time-out period.
				return m_nTimeout;
			}
		}

		override public string Database
		{
			get
			{
				// Returns an initial database as set in the connection string.
				// An empty string indicates not set - do not return a null reference.
				return m_sDatabase;
			}
		}

		override public ConnectionState State
		{
			get { return m_state; }
		}

		public BMXNetLib bmxNetLib
		{
			get
			{
				return this.m_rpx;
			}
		}

		/****
		 * IMPLEMENT THE REQUIRED METHODS.
		 ****/

		new public IDbTransaction BeginTransaction()
		{
			throw new NotSupportedException();
		}

        protected override System.Data.Common.DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
        {
            throw new Exception("The method or operation is not implemented.");
        }

		new public IDbTransaction BeginTransaction(IsolationLevel level)
		{
			throw new NotSupportedException();
		}

		override public void ChangeDatabase(string dbName)
		{
			//should dbname include address, port, access & verify?
			/*
			 * Change the database setting on the back-end. Note that it is a method
			 * and not a property because the operation requires an expensive
			 * round trip.
			 */
		}

		override public void Open()
		{
			/*
			 * Open the RPMS connection and set the ConnectionState
			 * property. 
			 */
			//If the connection is already open, then return.
			//If you wan't to re-open an already open connection,
			//you must first close it.
			if (m_rpx.Connected == true)
				return;

			try 
			{
				m_state = ConnectionState.Connecting;
				m_rpx = new BMXNetLib();

				m_rpx.MServerPort = m_nPort;
				bool bRet = false;
				if (this.m_bUseWinIdent == true)
				{
					WindowsIdentity winIdent = WindowsIdentity.GetCurrent();
					bRet = m_rpx.OpenConnection(m_sAddress, winIdent);
				}
				else
				{
					bRet = m_rpx.OpenConnection(m_sAddress, m_sAccess, m_sVerify);
				}
				if (bRet == true)
				{
					m_state = ConnectionState.Open;
				}
				else
				{
					m_state = ConnectionState.Closed;
					return;
				}
				m_RPMSDb = new RPMSDb(m_rpx);

			}
			catch (Exception ex)
			{
				string s = ex.Message + ex.StackTrace;
				throw new BMXNetException(s);
			}
		}

		override public void Close()
		{
			/*
			 * Close the rpms connection and set the ConnectionState
			 * property. 
			 */
			try
			{
				if (m_state == ConnectionState.Closed)
					return;
				m_rpx.CloseConnection();
				m_state = ConnectionState.Closed;
			}
			catch (Exception ex)
			{
				string s = ex.Message + ex.StackTrace;
				Debug.Write(s);
			}
		}

		new public IDbCommand CreateCommand()
		{
			// Return a new instance of a command object.
			BMXNetCommand comm =  new BMXNetCommand();
			comm.Connection = this;
			return comm;
		}

        override protected System.Data.Common.DbCommand CreateDbCommand()
        {
            // Return a new instance of a command object.
            BMXNetCommand comm = new BMXNetCommand();
            comm.Connection = this;
            return comm;
        }

        public override string DataSource
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public override string ServerVersion
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

		/*
		 * Implementation specific properties / methods.
		 */
		internal RPMSDb RPMSDb
		{
			get { return m_RPMSDb; }
		}

		void IDisposable.Dispose() 
		{
			this.Dispose(true);
			System.GC.SuppressFinalize(this);
		}

        //protected override void Dispose(bool disposing) 
        //{
        //    /*
        //     * Dispose of the object and perform any cleanup.
        //     */

        //    if (m_state == ConnectionState.Open)
        //    {
        //        this.Close();
        //    }
        //}

	}
}
