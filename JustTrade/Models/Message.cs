namespace JustTrade.Models
{
	using System.ComponentModel.Design.Serialization;

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

		public string Serialize() {
			return Newtonsoft.Json.JsonConvert.SerializeObject(this);
		}

		public static Message Deserialize(string data) {
			return Newtonsoft.Json.JsonConvert.DeserializeObject<Message>(data);
		}
	}
}