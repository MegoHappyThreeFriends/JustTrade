namespace JustTrade.Helpers
{
	public interface IMail
	{
		void Send(string mailTo, string subject, string body);
	}
}
