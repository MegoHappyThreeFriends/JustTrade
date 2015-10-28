using System;

namespace JustTrade.Helpers.ExceptionCollection
{
	class OutOfLimitException : System.Exception
	{
		public OutOfLimitException(string message)
			: base(String.Format("Object out of limit: {0}", message))
		{
		}

		public OutOfLimitException(string message, Exception innerException)
			: base(String.Format("Object out of limit: {0}", message), innerException)
		{
		}
	}
}
