using System;
using System.Data;
using System.Text;
using System.Diagnostics;

namespace IndianHealthService.BMXNet
{
	/*
	 * This class executes a query or RPC against the connected
	 * RPMS database and formats the return value as a RPMSDbResultSet
	 */
	public class RPMSDb
	{
		private string							m_sSelectCmd = "SELECT ";
		private string							m_sUpdateCmd = "UPDATE ";
		private string							m_sCmd = "";
		private BMXNetLib						m_rpx;

		private string							m_sQueryType = "";
		private RPMSDbResultSet[]				m_aResultSets = null;
		private int								m_nCurrentSet = 0;

		public class RPMSDbResultSet
		{
			public struct MetaData
			{
				public string	name;
				public Type		type;
				public int		maxSize;

				public string	fmFieldID;
				public bool		fmReadOnly;
				public bool		fmKeyField;
			}

			public int			recordsAffected;
			public MetaData[]	metaData;
			public object[,]	data;

			public string	fmKeyField;
			public string	fmFileID;
			public string	fmForeignKey = "";
			public int		fmSeed = 0;
		}
		
		//Constructor
		public RPMSDb(BMXNetLib rpx)
		{
			m_rpx = rpx;
		}
		
		public void Execute(string sCmd, out RPMSDbResultSet resultset)
		{
			/*
			 * SELECT, UPDATE and RPC operations.
			 */
			if (0 == String.Compare(sCmd, 0, m_sSelectCmd, 0, m_sSelectCmd.Length, true))
			{
				m_sCmd = sCmd;
				m_sQueryType = "SELECT";
				_executeSelect(out resultset);
			}
			else if (0 == String.Compare(sCmd, 0, m_sUpdateCmd, 0, m_sUpdateCmd.Length, true))
			{
				m_sCmd = sCmd;
				m_sQueryType = "UPDATE";
				_executeUpdate(out resultset);
			}
			else //RPC Call
			{
				m_sQueryType = "RPC";
				m_sCmd = sCmd;
				_executeSelect(out resultset);
			}
		}

		private void _executeSelect(out RPMSDbResultSet resultset)
		{
			if (m_nCurrentSet == 0)
				_resultsetCreate();

			// Return the current resultset.
			resultset = m_aResultSets[m_nCurrentSet];
		}

		private void _executeUpdate(out RPMSDbResultSet resultset)
		{
			string sRPC;

            /* /* 3110109 -- smh Commented out for performance
            string sOldContext = m_rpx.AppContext;

            if (m_rpx.AppContext != "BMXRPC")
            {
                m_rpx.AppContext = "BMXRPC";
            }
             */

			sRPC = "BMX UPDATE";
			int nStart = 7;
			m_sCmd = m_sCmd.Substring(nStart);
			string sResult = m_rpx.TransmitRPC( sRPC,  m_sCmd);

            /* /* 3110109 -- smh Commented out for performance
            if (sOldContext != m_rpx.AppContext)
            {
                m_rpx.AppContext = sOldContext;
            }
             */

			resultset = new RPMSDbResultSet();
			resultset.recordsAffected = 1; //Make this the return value of the call
		}

		private string CallRPX()
		{
			//Make rpx call
			string sRPC;
			string sParam;
            /* /* 3110109 -- smh Commented out for performance issues.
            string sOldContext = m_rpx.AppContext;
            */
            if (m_sQueryType == "SELECT")
			{
                /*/* 3110109 -- smh Commented out for performance issues.
                if (m_rpx.AppContext != "BMXRPC")
                {
                    m_rpx.AppContext = "BMXRPC";
                }
                 */
				sRPC = "BMX SQL";
				sParam = m_sCmd;
			}
			else //it's an RPC (stored procedure)
			{
				sRPC = BMXNetLib.Piece(m_sCmd, "^", 1);
				sParam = BMXNetLib.Piece(m_sCmd, "^", 2 , 99);
			}
			
			string sResult = m_rpx.TransmitRPC( sRPC,  sParam);
			
            /*
			if (sOldContext != m_rpx.AppContext)
			{
				m_rpx.AppContext = sOldContext;
			}
             */
			
			return sResult;
		}

		private void ProcessHeader(string sHeader, int nSet, out int numCols)
		{
			//Modifies m_aResultSet[nSet]
			string sFldDelim = "^";
			char[] cFldDelim = sFldDelim.ToCharArray();
			string sZero = "0";
			char[] cZero = sZero.ToCharArray();
			string sBar = "|";
			char[] cBar = sBar.ToCharArray();

			int j;
			if (sHeader.StartsWith("@@@meta@@@") == true)
			{
				//New style -- ColumnHeaderInfo in [1]
				//Parse out data location or update logic here
				//RecordInfo ^ Column1 info ^ column2 info ^ ... ^ column n info
				string sType;
				string sMaxSize;
				string sFieldName;
				string sReadOnly;
				string	sKeyField;
				string	sFileID;
				string	sSeed = "";
				int nMaxSize;
				Type tType;

				int nTemp = 10; //length of @@@meta@@@
                //actual header
				sHeader = sHeader.Substring(10,(sHeader.Length - nTemp));
				string[] sRecordSetInfo = sHeader.Split(cFldDelim);

                //substract one because 1st item is RecordId|File# -- rest is columns
				numCols = sRecordSetInfo.GetLength(0)-1;
				m_aResultSets[nSet].metaData = new RPMSDbResultSet.MetaData[numCols];

				//Set FileID
                //First ^-Piece is recordset-level info: RecordIdentifier|File#
				string[] sRecordInfo = sRecordSetInfo[0].Split(cBar);
				m_aResultSets[nSet].fmFileID = sRecordInfo[1];

                //What is the seed???
				if (sRecordInfo.GetLength(0) > 2)
				{
					sSeed = sRecordInfo[2];
					sSeed = (sSeed == "")?"0":sSeed;
					try
					{
						m_aResultSets[nSet].fmSeed = Convert.ToInt32(sSeed);
					}
					catch (Exception ex)
					{
						Debug.Assert(false, "BMXNet.RMSPDb: 'Seed' value failed to convert to integer.");
						Debug.Write(ex.Message + "\n");
					}
				}

                // Foreign key is included
				if (sRecordInfo.GetLength(0) > 3)
				{
					m_aResultSets[nSet].fmForeignKey = sRecordInfo[4];

				}

					m_aResultSets[nSet].fmKeyField = "";
				//2nd through nth ^-Pieces are Column info: Fileman File#|FileMan Field#|DataType|Field Length|Column Name|IsReadOnly|IsKeyField|????
				for (j=1; j < sRecordSetInfo.GetLength(0); j++)
				{
					string[] sColumnInfo = sRecordSetInfo[j].Split(cBar);
					//Field 0 = FileID
					//Field 1 = FieldID
					//Field 2 = DataType
					//Field 3 = Length
					//Field 4 = ColumnName
					//Field 5 = IsReadOnly
					//Field 6 = IsKeyField
                    //Field 7 {MISSING}
					sFileID = sColumnInfo[0];
					string sFieldID = sColumnInfo[1];

					
					switch (sFieldID)
					{
						case ".001":
							m_aResultSets[nSet].fmKeyField += "," + sColumnInfo[4];
							break;
						case ".0001":
							m_aResultSets[nSet].fmKeyField = sColumnInfo[4];
							break;
						default:
							break;
					}

					sType = sColumnInfo[2];
					switch (sType)
					{
						case "D":   
							tType = typeof(DateTime);
							break;
						case "I":
							tType = typeof(Int32);
							break;
						case "N":
							tType = typeof(System.Double);
							break;
						default:
							tType = typeof(String);
							break;
					}
					sMaxSize = sColumnInfo[3];
					nMaxSize = Convert.ToInt16(sMaxSize);
					sFieldName = sColumnInfo[4];

					if (m_aResultSets[nSet].fmForeignKey == sFieldID)
					{
						m_aResultSets[nSet].fmForeignKey = sFieldName;
					}

					sReadOnly = sColumnInfo[5];
					sKeyField = sColumnInfo[6];
					_resultsetFillColumn(nSet, j-1, sFieldName, tType, nMaxSize, sFieldID, sReadOnly, sKeyField);
				}
			}
			else
			{
				//Old style column header info
				
				string sField;
				string sType;
				string sMaxSize;
				string sFieldName;
				int nMaxSize;
				Type tType;

				string[] saHeader = sHeader.Split(cFldDelim);
				numCols = saHeader.GetLength(0);
				m_aResultSets[nSet].metaData = new RPMSDbResultSet.MetaData[numCols];
				for (j=0; j < numCols; j++)
				{
					sField = saHeader[j];
					sType = sField.Substring(0,1);
					if (sType == "D")
					{
						tType = typeof(DateTime);
					}
					else if(sType == "I")
					{
						tType = typeof(Int32);
					}
					else if(sType == "N")
					{
						tType = typeof(System.Double);
					}
					else
					{
						tType = typeof(String);
					}
					sMaxSize = sField.Substring(1,5);
					sMaxSize = sMaxSize.TrimStart(cZero);
					nMaxSize = Convert.ToInt16(sMaxSize);
					sFieldName = sField.Substring(6, sField.Length - 6);
					_resultsetFillColumn(nSet, j, sFieldName, tType, nMaxSize);
				}
			}
		}

		private void ProcessRecords(string[] sResultArray, int nRecords, int nSet, int numCols, int nStartIndex)
		{

			string sFldDelim = "^";
			char[] cFldDelim = sFldDelim.ToCharArray();
            // nRecords-1 because last record is empty (Where $C(31) (end of record) is)
			m_aResultSets[nSet].data = new object[nRecords-1, numCols];
			string[] saRecord;
			int j;
			for (j = nStartIndex; j < nRecords + nStartIndex -1; j++)
			{
				saRecord = sResultArray[j].Split(cFldDelim);
				for (int k = 0; k< saRecord.GetLength(0); k++)
				{
                    //Date Time validation
                    //TODO: Support Fileman DateTime
					if (m_aResultSets[nSet].metaData[k].type == typeof(DateTime))
					{
						if (saRecord[k] == "")
						{
							m_aResultSets[nSet].data[j-nStartIndex, k] = null;
						}
						else
						{
							try
							{
								m_aResultSets[nSet].data[j-nStartIndex, k] = Convert.ToDateTime(saRecord[k]);
							}
							catch (Exception ex)
							{
								Debug.Write(ex.Message);
								m_aResultSets[nSet].data[j-nStartIndex, k] = null;
							}
						}
					}
					else if (m_aResultSets[nSet].metaData[k].type == typeof(Int32))
					{
						if (saRecord[k] == "")
						{
							m_aResultSets[nSet].data[j-nStartIndex, k] = null;
						}
						else
						{
							try
							{
								m_aResultSets[nSet].data[j-nStartIndex, k] = Convert.ToInt32(saRecord[k]);
							}
							catch (Exception ex)
							{
								Debug.Write(ex.Message);
								m_aResultSets[nSet].data[j-nStartIndex, k] = null;
							}
						}
					}
						//TODO: Double datatype here
					else //it's a string
					{
						m_aResultSets[nSet].data[j-nStartIndex, k] = saRecord[k];
					}
				}
			}		
		}

		

		private void _resultsetCreate()
		{
			m_nCurrentSet = 0;
			char[] cRecDelim = new char[1];
			cRecDelim[0] = (char) 30;

			char cEof = (char) 31;
			string sEof = cEof.ToString();

			string sFldDelim = "^";
			char[] cFldDelim = sFldDelim.ToCharArray();
			string sZero = "0";
			char[] cZero = sZero.ToCharArray();
			string sBar = "|";
			char[] cBar = sBar.ToCharArray();

			string sResult = this.CallRPX();

			string[] sResultArray = sResult.Split(cRecDelim);
			int nTotalRecords = sResultArray.GetLength(0);

			//Get set count and record count
			int[]	naRecords;			//Count of records for each set
			int[]	naHeaderIndex;		//Location (index) of header records in sResultArray
			int		nRecordSetCount;	//Count of recordsets

            //Gets Records[sets] (val is number of records for each set), Headers[sets] (val is header location in array), and number of record sets.
			IndexRecords(sResultArray, out naRecords, out naHeaderIndex, out nRecordSetCount);
            //Create array of result sets
			m_aResultSets = new RPMSDbResultSet[nRecordSetCount];

			for (int nSet = 0; nSet < nRecordSetCount; nSet++)
			{
				int nHeaderIndex = naHeaderIndex[nSet];
				int nRecords = naRecords[nSet];
				int numCols = 0;
				m_aResultSets[nSet] = new RPMSDbResultSet();
				m_aResultSets[nSet].recordsAffected = 0;

				//process header
				string sHeader = sResultArray[nHeaderIndex];
				ProcessHeader(sHeader, nSet, out numCols);

				//process records
				this.ProcessRecords(sResultArray, nRecords, nSet, numCols, nHeaderIndex+1);

			}
		}

		private void IndexRecords(string[] sResultArray, 
			out int[] naRecordsOut, 
			out int[] naHeaderIndexOut,
			out int nSets)
		{

			int[] naHeaderIndex = new int[999];
			int nTotalRecords = sResultArray.GetLength(0);
			nSets = 0;
			int[] naRecords = new int[999];

			if (sResultArray[0].StartsWith("@@@meta@@@") == false)
			{
				//this is an old-style header with only one record
				nSets = 1;
				naHeaderIndex[0] = 0;
				naRecords[0]=nTotalRecords - 1;
			}
			else
			{

				//Count the number of record sets and record the index of each header record
				for (int k = 0; k < nTotalRecords; k++)
				{
					if (sResultArray[k].StartsWith("@@@meta@@@") == true)
					{
						naHeaderIndex[nSets] = k;
						nSets++;
					}
				}
				//Calculate record count for each set
				for (int k = 0; k < nSets - 1; k++)
				{
					naRecords[k] = naHeaderIndex[k+1] - naHeaderIndex[k] ;// - 1;
				}
				naRecords[nSets-1] = nTotalRecords - naHeaderIndex[nSets - 1] - 1;
			}

			naRecordsOut = new int[nSets];
			naHeaderIndexOut = new int[nSets];
			for (int k = 0; k < nSets; k++)
			{
				naRecordsOut[k] = naRecords[k];
				naHeaderIndexOut[k] = naHeaderIndex[k];
			}
		}

		private void _resultsetFillColumn(int nSet, int nIdx, string name, Type type, int maxSize)
		{
			m_aResultSets[nSet].metaData[nIdx].name    = name;
			m_aResultSets[nSet].metaData[nIdx].type    = type;
			m_aResultSets[nSet].metaData[nIdx].maxSize  = maxSize;
		}

		private void _resultsetFillColumn(int nSet, int nIdx, string name, Type type, int maxSize, 
			string sFieldID, string sReadOnly, string sKeyField)
		{
			m_aResultSets[nSet].metaData[nIdx].name    = name;
			m_aResultSets[nSet].metaData[nIdx].type    = type;
			m_aResultSets[nSet].metaData[nIdx].maxSize  = maxSize;

			m_aResultSets[nSet].metaData[nIdx].fmFieldID = sFieldID;
			m_aResultSets[nSet].metaData[nIdx].fmReadOnly = (sReadOnly == "FALSE")?false:true;
			m_aResultSets[nSet].metaData[nIdx].fmKeyField = (sKeyField == "TRUE")?true:false;

		}

		
		/// <summary>
		/// Returns the array of RMPSResultSets retrieved from RPMS
		/// </summary>
		public RPMSDbResultSet[] ResultSets
		{
			get
			{
				return m_aResultSets;
			}
		}

		/// <summary>
		/// Sets and Returns the current recordset
		/// </summary>
		public int CurrentRecordSet
		{
			get
			{
				return m_nCurrentSet;
			}
			set
			{
				m_nCurrentSet = value;
			}
		}


	}
}