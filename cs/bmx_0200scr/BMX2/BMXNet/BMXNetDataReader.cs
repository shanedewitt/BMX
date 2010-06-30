using System;
using System.Data;
using System.Globalization;

namespace IndianHealthService.BMXNet
{
	public class BMXNetDataReader : IDataReader
	{
		// The DataReader should always be open when returned to the user.
		private bool m_fOpen = true;

		// Keep track of the results and position
		// within the resultset (starts prior to first record).
		private RPMSDb.RPMSDbResultSet  m_resultset;
		private static int  m_STARTPOS  = -1;
		private int      m_nPos    = m_STARTPOS;

		/* 
		 * Keep track of the connection in order to implement the
		 * CommandBehavior.CloseConnection flag. A null reference means
		 * normal behavior (do not automatically close).
		 */
		private BMXNetConnection m_connection = null;

		/*
		 * Because the user should not be able to directly create a 
		 * DataReader object, the constructors are
		 * marked as internal.
		 */
		internal BMXNetDataReader(RPMSDb.RPMSDbResultSet resultset)
		{
			m_resultset   = resultset;
		}

		internal BMXNetDataReader(RPMSDb.RPMSDbResultSet resultset, BMXNetConnection connection)
		{
			m_resultset    = resultset;
			m_connection  = connection;
		}

		/****
		 * METHODS / PROPERTIES FROM IDataReader.
		 ****/
		public int Depth 
		{
			/*
			 * Always return a value of zero if nesting is not supported.
			 */
			get { return 0;  }
		}

		public bool IsClosed
		{
			/*
			 * Keep track of the reader state - some methods should be
			 * disallowed if the reader is closed.
			 */
			get  { return !m_fOpen; }
		}

		public int RecordsAffected 
		{
			/*
			 * RecordsAffected is only applicable to batch statements
			 * that include inserts/updates/deletes. BMXNet always
			 * returns -1.
			 */
			get { return -1; }
		}

		public void Close()
		{
			/*
			 * Close the reader. BMXNet only changes the state,
			 * but an actual implementation would also clean up any 
			 * resources used by the operation. For example,
			 * cleaning up any resources waiting for data to be
			 * returned by the server.
			 */
			m_fOpen = false;
		}

		public bool NextResult()
		{
			// BMXNet only returns a single resultset. However,
			// DbDataAdapter expects NextResult to return a value.
			return false;
		}

		public bool Read()
		{
			// Return true if it is possible to advance and if you are still positioned
			// on a valid row. Because the data array in the resultset
			// is two-dimensional, you must divide by the number of columns.
			if (++m_nPos >= m_resultset.data.Length / m_resultset.metaData.Length)
				return false;
			else
				return true;
		}

		public DataTable GetSchemaTable()
		{
			DataTable dtSchema = new DataTable();

			dtSchema.Columns.Add("ColumnName", typeof(System.String));
			dtSchema.Columns.Add("ColumnSize", typeof(Int32));
			dtSchema.Columns.Add("ColumnOrdinal", typeof(Int32));
			dtSchema.Columns.Add("NumericPrecision", typeof(Int16));
			dtSchema.Columns.Add("NumericScale", typeof(Int16));
			dtSchema.Columns.Add("DataType", typeof(System.Type));
			dtSchema.Columns.Add("AllowDBNull", typeof(bool));
			dtSchema.Columns.Add("IsReadOnly", typeof(bool));
			dtSchema.Columns.Add("IsUnique", typeof(bool));
			dtSchema.Columns.Add("IsRowVersion", typeof(bool));
			dtSchema.Columns.Add("IsKey", typeof(bool));
			dtSchema.Columns.Add("IsAutoIncrement", typeof(bool));
			dtSchema.Columns.Add("IsLong", typeof(bool));
			dtSchema.Columns.Add("BaseTableName", typeof(System.String));
			dtSchema.Columns.Add("BaseColumnName", typeof(System.String));

			dtSchema.ExtendedProperties.Add("BMXTable", m_resultset.fmFileID);
			dtSchema.ExtendedProperties.Add("BMXKey", m_resultset.fmKeyField);

			for (int i=0; i < m_resultset.metaData.GetLength(0); i++)
			{
				DataRow r = dtSchema.NewRow();
				r["BaseTableName"] = m_resultset.fmFileID;
				r["BaseColumnName"] = m_resultset.metaData[i].fmFieldID;
				r["ColumnName"] = m_resultset.metaData[i].name;
				r["ColumnSize"] = m_resultset.metaData[i].maxSize;
				r["ColumnOrdinal"] = i;			
				r["NumericPrecision"] = 0;
				r["NumericScale"] = 0;
				r["DataType"] = m_resultset.metaData[i].type;
				r["AllowDBNull"] = false;
				r["IsReadOnly"] = m_resultset.metaData[i].fmReadOnly;
				r["IsUnique"] = false;
				if (m_resultset.metaData[i].name == "BMXIEN")
					r["IsUnique"] = true;
				r["IsRowVersion"] = false;
				r["IsKey"] = m_resultset.metaData[i].fmKeyField;
				r["IsAutoIncrement"] = false;
				r["IsLong"] = false;

				dtSchema.Rows.Add(r);
			}
			return dtSchema;
		}

		/****
		 * METHODS / PROPERTIES FROM IDataRecord.
		 ****/
		public int FieldCount
		{
			// Return the count of the number of columns, which in
			// this case is the size of the column metadata
			// array.
			get { return m_resultset.metaData.Length; }
		}

		public String GetName(int i)
		{
			return m_resultset.metaData[i].name;
		}

		public String GetDataTypeName(int i)
		{
			/*
			 * Usually this would return the name of the type
			 * as used on the back end, for example 'smallint' or 'varchar'.
			 * BMXNet returns the simple name of the .NET Framework type.
			 */
			return m_resultset.metaData[i].type.Name;
		}

		public Type GetFieldType(int i)
		{
			// Return the actual Type class for the data type.
			return m_resultset.metaData[i].type;
		}

		public Object GetValue(int i)
		{
			return m_resultset.data[m_nPos, i];
		}

		public int GetValues(object[] values)
		{
			int i = 0, j = 0;
			for ( ; i < values.Length && j < m_resultset.metaData.Length; i++, j++)
			{
				values[i] = m_resultset.data[m_nPos, j];
			}

			return i;
		}

		public int GetOrdinal(string name)
		{
			// Look for the ordinal of the column with the same name and return it.
			for (int i = 0; i < m_resultset.metaData.Length; i++)
			{
				if (0 == _cultureAwareCompare(name, m_resultset.metaData[i].name))
				{
					return i;
				}
			}

			// Throw an exception if the ordinal cannot be found.
			throw new IndexOutOfRangeException("Could not find specified column in results");
		}

		public object this [ int i ]
		{
			get { return m_resultset.data[m_nPos, i]; }
		}

		public object this [ String name ]
		{
			// Look up the ordinal and return 
			// the value at that position.
			get { return this[GetOrdinal(name)]; }
		}

		public bool GetBoolean(int i)
		{
			/*
			 * Force the cast to return the type. InvalidCastException
			 * should be thrown if the data is not already of the correct type.
			 */
			return (bool)m_resultset.data[m_nPos, i];
		}

		public byte GetByte(int i)
		{
			/*
			 * Force the cast to return the type. InvalidCastException
			 * should be thrown if the data is not already of the correct type.
			 */
			return (byte)m_resultset.data[m_nPos, i];
		}

		public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
		{
			// BMXNet does not support this method.
			throw new NotSupportedException("GetBytes not supported.");
		}

		public char GetChar(int i)
		{
			/*
			 * Force the cast to return the type. InvalidCastException
			 * should be thrown if the data is not already of the correct type.
			 */
			return (char)m_resultset.data[m_nPos, i];
		}

		public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
		{
			// BMXNet does not support this method.
			throw new NotSupportedException("GetChars not supported.");
		}

		public Guid GetGuid(int i)
		{
			/*
			 * Force the cast to return the type. InvalidCastException
			 * should be thrown if the data is not already of the correct type.
			 */
			return (Guid)m_resultset.data[m_nPos, i];
		}

		public Int16 GetInt16(int i)
		{
			/*
			 * Force the cast to return the type. InvalidCastException
			 * should be thrown if the data is not already of the correct type.
			 */
			return (Int16)m_resultset.data[m_nPos, i];
		}

		public Int32 GetInt32(int i)
		{
			/*
			 * Force the cast to return the type. InvalidCastException
			 * should be thrown if the data is not already of the correct type.
			 */
			return (Int32)m_resultset.data[m_nPos, i];
		}

		public Int64 GetInt64(int i)
		{
			/*
			 * Force the cast to return the type. InvalidCastException
			 * should be thrown if the data is not already of the correct type.
			 */
			return (Int64)m_resultset.data[m_nPos, i];
		}

		public float GetFloat(int i)
		{
			/*
			 * Force the cast to return the type. InvalidCastException
			 * should be thrown if the data is not already of the correct type.
			 */
			return (float)m_resultset.data[m_nPos, i];
		}

		public double GetDouble(int i)
		{
			/*
			 * Force the cast to return the type. InvalidCastException
			 * should be thrown if the data is not already of the correct type.
			 */
			return (double)m_resultset.data[m_nPos, i];
		}

		public String GetString(int i)
		{
			/*
			 * Force the cast to return the type. InvalidCastException
			 * should be thrown if the data is not already of the correct type.
			 */
			return (String)m_resultset.data[m_nPos, i];
		}

		public Decimal GetDecimal(int i)
		{
			/*
			 * Force the cast to return the type. InvalidCastException
			 * should be thrown if the data is not already of the correct type.
			 */
			return (Decimal)m_resultset.data[m_nPos, i];
		}

		public DateTime GetDateTime(int i)
		{
			/*
			 * Force the cast to return the type. InvalidCastException
			 * should be thrown if the data is not already of the correct type.
			*/
			return (DateTime)m_resultset.data[m_nPos, i];
		}

		public IDataReader GetData(int i)
		{
			/*
			 * BMXNet code does not support this method. Need,
			 * to implement this in order to expose nested tables and
			 * other hierarchical data.
			 */
			throw new NotSupportedException("GetData not supported.");
		}

		public bool IsDBNull(int i)
		{
			return m_resultset.data[m_nPos, i] == DBNull.Value;
		}

		/*
		 * Implementation specific methods.
		 */
		private int _cultureAwareCompare(string strA, string strB)
		{
			return CultureInfo.CurrentCulture.CompareInfo.Compare(strA, strB, CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth | CompareOptions.IgnoreCase);
		}

		void IDisposable.Dispose() 
		{
			this.Dispose(true);
			System.GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing) 
		{
			if (disposing) 
			{
				try 
				{
					this.Close();
				}
				catch (Exception e) 
				{
					throw new SystemException("An exception of type " + e.GetType() + 
						" was encountered while closing the BMXNetDataReader.");
				}
			}
		}

	}
}