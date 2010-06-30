using System;
using System.Runtime.Serialization;

namespace IndianHealthService.BMXNet
{
	/// <summary>
	/// Custom exception class for BMXNet
	/// </summary>
	[Serializable]
	public class BMXNetException : System.ApplicationException
	{
		public BMXNetException()
		{

		}
		public BMXNetException(string message) : base(message)
		{

		}
		public BMXNetException(string message, Exception inner) : base(message, inner)
		{

		}	
		// deserialization constructor
		public BMXNetException(SerializationInfo info, 
			StreamingContext context):
		base(info, context)
		{

		}
	
	}
}
