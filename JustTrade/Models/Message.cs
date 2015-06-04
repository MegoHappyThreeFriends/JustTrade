using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JustTrade.Models
{
	using NUnit.Framework;

	public class Message
	{
		public enum Button
		{
			Ok, YesNo, RetryCancel, OkSendReport
		}

		public Message(string caption, string messageText, string description = null, bool isError=false, Button buttons = Button.Ok) {
			Caption = caption;
			MessageText = messageText;
			Buttons = buttons;
			Description = description;
			IsError = isError;
		}

		public bool IsError {
			get;
			set;
		}

		public string Caption {
			get;
			set;
		}

		public string MessageText {
			get;
			set;
		}

		public string Description {
			get;
			set;
		}

		public Button Buttons {
			get;
			set;
		}

	}
}