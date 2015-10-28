using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustTrade.Helpers.ExceptionCollection
{
	class HelpException : Exception
	{
		public HelpException(string message)
			: base(message)
		{
		}

		public HelpException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
