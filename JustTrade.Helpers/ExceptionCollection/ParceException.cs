using System;

namespace JustTrade.Helpers.ExceptionCollection
{
	class ParceException : System.Exception
	{
		public ParceException(string message)
			: base(String.Format("Parse object is invalid: {0}", message))
		{
		}

		public ParceException(string message, Exception innerException)
			: base(String.Format("Parse object is invalid: {0}", message), innerException)
		{
		}
	}
}
