namespace JustTrade.Helpers
{
	using System.Net;
	using System.Net.Mail;
	using System.Reflection;
	using JustTrade.Helpers.Interfaces;

	public class Mail : IMail
	{
		public void Send(string mailTo, string subject, string body) {
			var version = Assembly.GetExecutingAssembly().GetName().Version;
			var fromAddress = new MailAddress("JastTrage@gmail.com", "JastTrade_v" + version);
			var toAddress = new MailAddress(mailTo, mailTo);
			var smtp = new SmtpClient {
				Host = AppSettings.MailHost,
				Port = AppSettings.MailPort,
				EnableSsl = AppSettings.MailSSL,
				DeliveryMethod = SmtpDeliveryMethod.Network,
				UseDefaultCredentials = false,
				Credentials = new NetworkCredential(AppSettings.MailUser, AppSettings.MailPassword)
			};
			using (var message = new MailMessage(fromAddress, toAddress) {
				Subject = subject,
				Body = body
			}) {
				smtp.Send(message);
			}
		}
	}
}
