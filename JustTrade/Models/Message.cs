using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JustTrade.Models
{
	public class Message
	{
		public enum Button
		{
			Ok, YesNo, RetryCancel, OkSendReport
		}

		public Message(string caption, string messageText, string description = null, Button buttons = Button.Ok) {
			Caption = caption;
			MessageText = messageText;
			Buttons = buttons;
			Description = description;
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