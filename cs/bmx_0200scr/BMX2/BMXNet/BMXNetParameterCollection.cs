using System;
using System.Data;
using System.Collections;
using System.Globalization;

namespace IndianHealthService.BMXNet
{
	/*
	 * Because IDataParameterCollection is primarily an IList,
	 * the sample can use an existing class for most of the implementation.
	 */
	public class BMXNetParameterCollection : ArrayList, IDataParameterCollection
	{
		public object this[string index] 
		{
			get
			{
				return this[IndexOf(index)];
			}
			set
			{
				this[IndexOf(index)] = value;
			}
		}

		public bool Contains(string parameterName)
		{
			return(-1 != IndexOf(parameterName));
		}

		public int IndexOf(string parameterName)
		{
			int index = 0;
			foreach(BMXNetParameter item in this) 
			{
				if (0 == _cultureAwareCompare(item.ParameterName, parameterName))
				{
					return index;
				}
				index++;
			}
			return -1;
		}

		public void RemoveAt(string parameterName)
		{
			RemoveAt(IndexOf(parameterName));
		}

		public override int Add(object value)
		{
			return Add((BMXNetParameter)value);
		}

		public int Add(BMXNetParameter value)
		{
			if (((BMXNetParameter)value).ParameterName != null)
			{
				return base.Add(value);
			}
			else
				throw new ArgumentException("parameter must be named");
		}

		public int Add(string parameterName, DbType type)
		{
			return Add(new BMXNetParameter(parameterName, type));
		}

		public int Add(string parameterName, object value)
		{
			return Add(new BMXNetParameter(parameterName, value));
		}

		public int Add(string parameterName, DbType dbType, string sourceColumn)
		{
			return Add(new BMXNetParameter(parameterName, dbType, sourceColumn));
		}

		private int _cultureAwareCompare(string strA, string strB)
		{
			return CultureInfo.CurrentCulture.CompareInfo.Compare(strA, strB, CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth | CompareOptions.IgnoreCase);
		}
	}
}