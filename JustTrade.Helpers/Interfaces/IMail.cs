namespace JustTrade.Helpers.Interfaces
{
	public interface IMail
	{
		void Send(string mailTo, string subject, string body);
	}
}
