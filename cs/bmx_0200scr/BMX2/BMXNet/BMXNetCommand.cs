using System;
using System.Data;
using System.Data.Common;

namespace IndianHealthService.BMXNet
{
    public class BMXNetCommand : System.Data.Common.DbCommand, IDbCommand
	{
		BMXNetConnection			m_connection;
		BMXNetTransaction			m_txn;
		string						m_sCmdText;
		UpdateRowSource				m_updatedRowSource = UpdateRowSource.None;
		BMXNetParameterCollection	m_parameters = new BMXNetParameterCollection();

		// the default constructor
		public BMXNetCommand()
		{
		}

		// other constructors
		public BMXNetCommand(string cmdText)
		{
			m_sCmdText = cmdText;
		}

		public BMXNetCommand(string cmdText, BMXNetConnection connection)
		{
			m_sCmdText    = cmdText;
			m_connection  = connection;
		}

		public BMXNetCommand(string cmdText, BMXNetConnection connection, BMXNetTransaction txn)
		{
			m_sCmdText    = cmdText;
			m_connection  = connection;
			m_txn      = txn;
		}

		/****
		 * IMPLEMENT THE REQUIRED PROPERTIES.
		 ****/
		override public string CommandText
		{
			get { return m_sCmdText;  }
			set  { m_sCmdText = value;  }
		}

        override public int CommandTimeout
		{
			/*
			 * BMXNet does not support a command time-out. As a result,
			 * for the get, zero is returned because zero indicates an indefinite
			 * time-out period. For the set, throw an exception.
			 */
			get  { return 0; }
			set  { if (value != 0) throw new NotSupportedException(); }
		}

        override public CommandType CommandType
		{
			/*
			 * BMXNet only supports CommandType.Text.
			 */
			get { return CommandType.Text; }
			set { if (value != CommandType.Text) throw new NotSupportedException(); }
		}

        protected override DbConnection DbConnection
        {
            get
            {
                return m_connection;
            }
            set
            {
                if (m_connection != value)
                    this.Transaction = null;

                m_connection = (BMXNetConnection)value;
            }
        }

        new public IDbConnection Connection
        {
            /*
             * The user should be able to set or change the connection at 
             * any time.
             */
            get { return m_connection; }
            set
            {
                /*
                 * The connection is associated with the transaction
                 * so set the transaction object to return a null reference if the connection 
                 * is reset.
                 */
                if (m_connection != value)
                    this.Transaction = null;

                m_connection = (BMXNetConnection)value;
            }
        }

		new public BMXNetParameterCollection Parameters
		{
			get  { return m_parameters; }
		}

		IDataParameterCollection IDbCommand.Parameters
		{
			get  { return m_parameters; }
		}

        override protected DbParameterCollection DbParameterCollection
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

		new public IDbTransaction Transaction
		{
			/*
			 * Set the transaction. Consider additional steps to ensure that the transaction
			 * is compatible with the connection, because the two are usually linked.
			 */
			get { return m_txn; }
			set { m_txn = (BMXNetTransaction)value; }
		}

        override protected DbTransaction DbTransaction
        {
            /*
             * Set the transaction. Consider additional steps to ensure that the transaction
             * is compatible with the connection, because the two are usually linked.
             */
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        override public bool DesignTimeVisible
        {
            get
            {return false ; }
            set { ;}
        }

        override public UpdateRowSource UpdatedRowSource
		{
			get { return m_updatedRowSource;  }
			set { m_updatedRowSource = value; }
		}


		/****
		 * IMPLEMENT THE REQUIRED METHODS.
		 ****/
		override public void Cancel()
		{
			// BMXNet does not support canceling a command
			// once it has been initiated.
			throw new NotSupportedException();
		}

        new public IDbDataParameter CreateParameter()
		{
			return (IDbDataParameter)(new BMXNetParameter());
		}

        override protected DbParameter CreateDbParameter()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        override public int ExecuteNonQuery()
		{
			/*
			 * ExecuteNonQuery is intended for commands that do
			 * not return results, instead returning only the number
			 * of records affected.
			 */
      
			// There must be a valid and open connection.
			if (m_connection == null || m_connection.State != ConnectionState.Open)
				throw new InvalidOperationException("Connection must valid and open");

			// Execute the command.
			RPMSDb.RPMSDbResultSet resultset;
			m_connection.RPMSDb.Execute(m_sCmdText, out resultset);

			// Return the number of records affected.
			return resultset.recordsAffected;
		}

        new public IDataReader ExecuteReader()
		{
			/*
			 * ExecuteReader should retrieve results from the data source
			 * and return a DataReader that allows the user to process 
			 * the results.
			 */
			// There must be a valid and open connection.
			if (m_connection == null || m_connection.State != ConnectionState.Open)
				throw new InvalidOperationException("Connection must valid and open");

			// Execute the command.
			RPMSDb.RPMSDbResultSet resultset;
			m_connection.RPMSDb.Execute(m_sCmdText, out resultset);

			return new BMXNetDataReader(resultset);
		}

        new public IDataReader ExecuteReader(CommandBehavior behavior)
		{
			/*
			 * ExecuteReader should retrieve results from the data source
			 * and return a DataReader that allows the user to process 
			 * the results.
			 */

			// There must be a valid and open connection.
			if (m_connection == null || m_connection.State != ConnectionState.Open)
				throw new InvalidOperationException("Connection must valid and open");

			/*If SchemaOnly or KeyInfo behavior, Set BMXSchema flag
			 *execute the command, then unset the BMXSchema flag
			 *Otherwise, just Execute the command.
			 */
			RPMSDb.RPMSDbResultSet resultset;
			if (((behavior & (CommandBehavior.SchemaOnly | CommandBehavior.KeyInfo)) > 0))
			{
				m_connection.bmxNetLib.TransmitRPC("BMX SCHEMA ONLY", "TRUE");
				m_connection.RPMSDb.Execute(m_sCmdText, out resultset);
				m_connection.bmxNetLib.TransmitRPC("BMX SCHEMA ONLY", "FALSE");
			}
			else
			{
				m_connection.RPMSDb.Execute(m_sCmdText, out resultset);
			}

			/*
			 * The only CommandBehavior option supported by BMXNet
			 * is the automatic closing of the connection
			 * when the user is done with the reader.
			 */
			if (behavior == CommandBehavior.CloseConnection)
				return new BMXNetDataReader(resultset, m_connection);
			else
				return new BMXNetDataReader(resultset);
		}

        override protected DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        override public object ExecuteScalar()
		{
			/*
			 * ExecuteScalar assumes that the command will return a single
			 * row with a single column, or if more rows/columns are returned
			 * it will return the first column of the first row.
			 */

			// There must be a valid and open connection.
			if (m_connection == null || m_connection.State != ConnectionState.Open)
				throw new InvalidOperationException("Connection must valid and open");

			// Execute the command.
			RPMSDb.RPMSDbResultSet resultset;
			m_connection.RPMSDb.Execute(m_sCmdText, out resultset);

			// Return the first column of the first row.
			// Return a null reference if there is no data.
			if (resultset.data.Length == 0)
				return null;

			return resultset.data[0, 0];
		}

        override public void Prepare()
		{
			// BMXNet Prepare is a no-op.
		}

		void IDisposable.Dispose() 
		{
			this.Dispose(true);
			System.GC.SuppressFinalize(this);
		}

        //private void Dispose(bool disposing) 
        //{
        //    /*
        //     * Dispose of the object and perform any cleanup.
        //     */
        //}

		/****
		 * IMPLEMENT BMX-Specific METHODS.
		 ****/
		public void BMXBuildUpdateCommand(DataTable dtSchema)
		{
			string sText = "UPDATE ";
			sText += "@File, ";
			sText += "@Record, ";

			//Build Parameters array
			BMXNetParameter[] parms = new BMXNetParameter[dtSchema.Rows.Count+2];

			parms[0] = new BMXNetParameter("@File", DbType.String);
			parms[0].SourceVersion = DataRowVersion.Original;
			parms[0].SourceColumn = dtSchema.ExtendedProperties["BMXTable"].ToString();
			Parameters.Add(parms[0]);

			parms[1] = new BMXNetParameter("@Record", DbType.String);
			parms[1].SourceVersion = DataRowVersion.Original;
			parms[1].SourceColumn = dtSchema.ExtendedProperties["BMXKey"].ToString();;
			Parameters.Add(parms[1]);

			int i = 1;
			foreach (DataRow r in dtSchema.Rows)
			{
				//Make a parameter for the Key Field and all non-ReadOnly fields
				if (  ((bool) r["IsReadOnly"] == false) || ( (bool) r["IsKey"] == true ) )
				{
					i++;
					parms[i] = new BMXNetParameter(r["ColumnName"].ToString(), DbType.String);
					parms[i].SourceVersion = DataRowVersion.Current;
					parms[i].SourceColumn = r["BaseColumnName"].ToString(); //FM FieldNumber
					parms[i].IsKey = Convert.ToBoolean(r["IsKey"]);
					Parameters.Add(parms[i]);
				}
			}
		}
	}
}