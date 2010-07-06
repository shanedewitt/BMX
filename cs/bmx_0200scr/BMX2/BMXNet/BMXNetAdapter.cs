using System;
using System.Data;
using System.Data.Common;

namespace IndianHealthService.BMXNet
{
	public class BMXNetDataAdapter : DbDataAdapter, IDbDataAdapter
	{
		private BMXNetCommand m_selectCommand;
		private BMXNetCommand m_insertCommand;
		private BMXNetCommand m_updateCommand;
		private BMXNetCommand m_deleteCommand;

		/*
		 * Inherit from Component through DbDataAdapter. The event
		 * mechanism is designed to work with the Component.Events
		 * property. These variables are the keys used to find the
		 * events in the components list of events.
		 */
		static private readonly object EventRowUpdated = new object(); 
		static private readonly object EventRowUpdating = new object(); 

		public BMXNetDataAdapter()
		{
		}

        new public BMXNetCommand SelectCommand
        {
            get { return m_selectCommand; }
            set { m_selectCommand = value; }
        }

        IDbCommand IDbDataAdapter.SelectCommand
        {
            get { return m_selectCommand; }
            set { m_selectCommand = (BMXNetCommand)value; }
        }

		new public BMXNetCommand InsertCommand 
		{
			get { return m_insertCommand; }
			set { m_insertCommand = value; }
		}

		IDbCommand IDbDataAdapter.InsertCommand 
		{
			get { return m_insertCommand; }
			set { m_insertCommand = (BMXNetCommand)value; }
		}

		new public BMXNetCommand UpdateCommand 
		{
			get { return m_updateCommand; }
			set { m_updateCommand = value; }
		}

		IDbCommand IDbDataAdapter.UpdateCommand 
		{
			get { return m_updateCommand; }
			set { m_updateCommand = (BMXNetCommand)value; }
		}

		new public BMXNetCommand DeleteCommand 
		{
			get { return m_deleteCommand; }
			set { m_deleteCommand = value; }
		}

		IDbCommand IDbDataAdapter.DeleteCommand 
		{
			get { return m_deleteCommand; }
			set { m_deleteCommand = (BMXNetCommand)value; }
		}

		/*
		 * Implement abstract methods inherited from DbDataAdapter.
		 */

		public override int Fill(DataSet ds)
		{
			//The inital call to base.fill calls the RPC which loads up the array
			//After base.fill returns, create a datareader
			BMXNetConnection bmxConn = (BMXNetConnection) this.SelectCommand.Connection;
			RPMSDb bmxDB = bmxConn.RPMSDb;

			DataTable dt = new DataTable();

			//Execute the RPC call
			base.Fill(dt);
			//Get the table name
			dt.TableName = bmxDB.ResultSets[0].fmFileID;
			dt.ExtendedProperties.Add("fmSeed", bmxDB.ResultSets[0].fmSeed);

			
			string sParentTable = dt.TableName;

			//Add the first table to the DataSet
			ds.Tables.Add(dt);

			//If bmxDB resultset array count is greater than 1
			int nSets = bmxDB.ResultSets.GetUpperBound(0) + 1;

			if (nSets > 1)
			{
				//Set primary key for first table
				string sKeyField = bmxDB.ResultSets[0].fmKeyField;
				DataColumn dcKey = dt.Columns[sKeyField];
				DataColumn[] dcKeys = new DataColumn[1];
				dcKeys[0] = dcKey;
				dt.PrimaryKey = dcKeys;

				string[] sRelations = new string[nSets];

				//loop and get the rest of the tables
				for (int k = 1; k < nSets; k++)
				{
					//Increment the current recordset counter in bmxDB
					bmxDB.CurrentRecordSet++;
					//Create the next table
					dt = new DataTable();
					//Fill it
					base.Fill(dt);
					//Get the table name
					string sChildTable = bmxDB.ResultSets[k].fmFileID;
					dt.TableName = sChildTable;
					//Add it to the dataset
					ds.Tables.Add(dt);

					//Get the foreign key field
					string sForeignKey = bmxDB.ResultSets[k].fmForeignKey;

                    //Set the data relationship
					string sParentKey;
					sParentKey = "BMXIEN";
					sParentKey = "PATIENT_IEN";

					DataRelation dr = new DataRelation("Relation" + k.ToString() ,	//Relation Name
						ds.Tables[sParentTable].Columns[sParentKey],  //;	//parent
						ds.Tables[sChildTable].Columns[sForeignKey]) ;//child
					ds.Relations.Add(dr);
				}
				bmxDB.CurrentRecordSet = 0;				  
			}
			return dt.Rows.Count;
		}

		override protected DataTable FillSchema(
			DataTable dataTable,
			SchemaType schemaType,
			IDbCommand command,
			CommandBehavior behavior
			)
		{
			behavior = CommandBehavior.SchemaOnly;
			BMXNetDataReader dReader =(BMXNetDataReader) command.ExecuteReader(behavior);
			DataTable dtSchema = dReader.GetSchemaTable();
			return dtSchema;
		}

		override protected int Update(
			DataRow[] dataRows,
			DataTableMapping tableMapping
			)
		{
			//Build UpdateCommand's m_sCmdText using UpdateCommand's parameters
			// and data in dataRows;
			// execute non query and increment nRet;

			string sCmd = "";
			int nRecordsAffected = 0;

			//Get recordset-level info from parameters
			BMXNetParameter parm = (BMXNetParameter) UpdateCommand.Parameters[0];
			string sFileID = parm.SourceColumn;
			parm = (BMXNetParameter) UpdateCommand.Parameters[1];
			string sKeyField = parm.SourceColumn;
			string sKeyID = "";
			char[] cRecDelim = new char[1];
			cRecDelim[0] = (char) 30;

			string sValue = "";
			string sFMFieldID;
			string sColumnName;
			int		nColIndex;

			//Process deletions
			foreach (DataRow r in dataRows)
			{
				if (r.RowState == DataRowState.Deleted)
				{
					r.RejectChanges(); //so that I can get to the row id
					//Build DAS
					char cSep = Convert.ToChar(",");
					string[] saKeyFields = sKeyField.Split(cSep);
					string sTmp = "";
					for (int j = 0; j < saKeyFields.GetLength(0); j++)
					{
                        if (saKeyFields[j] != "")
                        {
                            if (j > 0)
                                sTmp = sTmp + ",";
                            if (j == saKeyFields.GetLength(0) - 1)
                                sTmp += "-";
                            sTmp += r[saKeyFields[j]];
                        }
					}
					sCmd = sTmp;
					sCmd = sFileID + "^" + sCmd + "^";
					UpdateCommand.CommandText = "UPDATE " + sCmd;
					int nRet = this.UpdateCommand.ExecuteNonQuery();
					r.Delete();
					nRecordsAffected += nRet;
				}
			}

			//Process Edits and Adds
			foreach (DataRow r in dataRows)
			{
				if (r.RowState != DataRowState.Deleted)
				{
					string	sMsg = "";
					sKeyID = "";
					for (int j=2; j < UpdateCommand.Parameters.Count; j++)
					{
						parm = (BMXNetParameter) UpdateCommand.Parameters[j];
						sColumnName = parm.ParameterName;
						sFMFieldID = parm.SourceColumn;
						//Find a column id in r whose column name is sColumnName
						nColIndex = -1;
						for (int k = 0; k < r.Table.Columns.Count; k ++)
						{
							if (r.Table.Columns[k].ColumnName == sColumnName)
							{
								nColIndex = k;
								break;
							}
						}
						if (nColIndex > -1) 
						{
							if (r.ItemArray[nColIndex].GetType() == typeof(System.DateTime))
							{
								DateTime dValue = (DateTime) r.ItemArray[nColIndex];
								if ((dValue.Minute == 0) && (dValue.Hour == 0))
								{
									sValue = dValue.ToString("M-d-yyyy");
								}
								else
								{
									sValue = dValue.ToString("M-d-yyyy@HH:mm");
								}
							}
							else
							{
								sValue = r.ItemArray[nColIndex].ToString();
							}
							if (parm.IsKey == false)
							{
								if (sMsg != "")
									sMsg += (char) 30;
								sMsg += sFMFieldID + "|" + sValue;
							}
						}
						switch (sFMFieldID)
						{
							case ".0001":
								if (sKeyID == "")
								{
									sKeyID = sValue + ",";
								}
								else
								{
									sKeyID = sValue + "," + sKeyID;
								}
								break;
							case ".001":
								if (sKeyID == "")
								{
									sKeyID = sValue;
								}
								else
								{
									sKeyID = sKeyID + sValue;
								}
								break;
							default:
								break;

						}
					}
					sCmd = sFileID + "^" + sKeyID + "^" + sMsg;
					UpdateCommand.CommandText = "UPDATE " + sCmd;
					int nRet = this.UpdateCommand.ExecuteNonQuery();
					nRecordsAffected += nRet;
				}//end if RowState != deleted
			}//end for

			return nRecordsAffected;
		}

		override protected RowUpdatedEventArgs CreateRowUpdatedEvent(DataRow dataRow, IDbCommand command, StatementType statementType, DataTableMapping tableMapping)
		{
			return new BMXNetRowUpdatedEventArgs(dataRow, command, statementType, tableMapping);
		}

		override protected RowUpdatingEventArgs CreateRowUpdatingEvent(DataRow dataRow, IDbCommand command, StatementType statementType, DataTableMapping tableMapping)
		{
			return new BMXNetRowUpdatingEventArgs(dataRow, command, statementType, tableMapping);
		}

		override protected void OnRowUpdating(RowUpdatingEventArgs value)
		{
			BMXNetRowUpdatingEventHandler handler = (BMXNetRowUpdatingEventHandler) Events[EventRowUpdating];
			if ((null != handler) && (value is BMXNetRowUpdatingEventArgs)) 
			{
				handler(this, (BMXNetRowUpdatingEventArgs) value);
			}
		}

		override protected void OnRowUpdated(RowUpdatedEventArgs value)
		{
			BMXNetRowUpdatedEventHandler handler = (BMXNetRowUpdatedEventHandler) Events[EventRowUpdated];
			if ((null != handler) && (value is BMXNetRowUpdatedEventArgs)) 
			{
				handler(this, (BMXNetRowUpdatedEventArgs) value);
			}
		}

		public event BMXNetRowUpdatingEventHandler RowUpdating
		{
			add { Events.AddHandler(EventRowUpdating, value); }
			remove { Events.RemoveHandler(EventRowUpdating, value); }
		}

		public event BMXNetRowUpdatedEventHandler RowUpdated
		{
			add { Events.AddHandler(EventRowUpdated, value); }
			remove { Events.RemoveHandler(EventRowUpdated, value); }
		}
	}

	public delegate void BMXNetRowUpdatingEventHandler(object sender, BMXNetRowUpdatingEventArgs e);
	public delegate void BMXNetRowUpdatedEventHandler(object sender, BMXNetRowUpdatedEventArgs e);

	public class BMXNetRowUpdatingEventArgs : RowUpdatingEventArgs
	{
		public BMXNetRowUpdatingEventArgs(DataRow row, IDbCommand command, StatementType statementType, DataTableMapping tableMapping) 
			: base(row, command, statementType, tableMapping) 
		{
		}

		// Hide the inherited implementation of the command property.
		new public BMXNetCommand Command
		{
			get  { return (BMXNetCommand)base.Command; }
			set  { base.Command = value; }
		}
	}

	public class BMXNetRowUpdatedEventArgs : RowUpdatedEventArgs
	{
		public BMXNetRowUpdatedEventArgs(DataRow row, IDbCommand command, StatementType statementType, DataTableMapping tableMapping)
			: base(row, command, statementType, tableMapping) 
		{
		}

		// Hide the inherited implementation of the command property.
		new public BMXNetCommand Command
		{
			get  { return (BMXNetCommand)base.Command; }
		}
	}
}
