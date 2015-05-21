using System;

namespace JastTrade
{
	public class FileNotFoundException : Exception
	{
		public FileNotFoundException ()
		{
		}

		public FileNotFoundException (string message) 
			:base(message)
		{

		}

		public FileNotFoundException (string message, Exception exception) 
			:base(message, exception)
		{

		}
	}
}

