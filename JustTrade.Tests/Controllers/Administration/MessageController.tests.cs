
namespace JustTrade.Tests.Controllers.Administration
{
	using JustTrade.Controllers.Administration;
	using JustTrade.Tests.Tools;
	using NUnit.Framework;

	[TestFixture]
	public class MessageControllerTests
	{
		[Test]
		public void SendReport_CallMailSend() {
			var session = MockTools.SetupSession();
			var mail = session.SetupMail();
			const string head = "";
			const string body = "";
			var controller = new MessageController();
			controller.SendReport(head, body);
			mail.Verify(x => x.Send("JastTrage@gmail.com", head, body));
		}
	}
}
