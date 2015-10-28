using System.Linq;
using System.Net;
using JustTrade.Helpers.ExceptionCollection;
using JustTrade.Helpers.ExtensionMethods;
using NUnit.Framework;

namespace JustTrade.Tests.Helpers
{
	[TestFixture]
	class IpAdressStringExtensionTests
	{
		[Test]
		public void ParceIpAddresses_ReturnCorrectValue_WhenOneIp()
		{
			string ipAddress = "127.0.0.1";
			var ipList = ipAddress.ParceIpAddresses();
			Assert.IsTrue(ipList.Count == 1);
			Assert.AreEqual(ipList.First(), IPAddress.Parse(ipAddress));
		}

		[Test]
		public void ParceIpAddresses_ReturnCorrectValue_WhenManyIp()
		{
			string ipAddress = "127.0.0.1, 192.168.2.1, 10.10.10.5";
			var ipList = ipAddress.ParceIpAddresses();
			Assert.IsTrue(ipList.Count == 3);
			Assert.AreEqual(ipList[0], IPAddress.Parse("127.0.0.1"));
			Assert.AreEqual(ipList[1], IPAddress.Parse("192.168.2.1"));
			Assert.AreEqual(ipList[2], IPAddress.Parse("10.10.10.5"));
		}

		[Test]
		public void ParceIpAddresses_ReturnCorrectValue_WhenIpRange()
		{
			string ipAddress = "192.168.2.1-192.168.2.5";
			var ipList = ipAddress.ParceIpAddresses();
			Assert.IsTrue(ipList.Count == 5);
			Assert.AreEqual(ipList.First(), IPAddress.Parse("192.168.2.1"));
			Assert.AreEqual(ipList.Last(), IPAddress.Parse("192.168.2.5"));
		}

		[Test]
		public void ParceIpAddresses_ReturnCorrectValue_WhenIpRangeWithOne()
		{
			string ipAddress = "192.168.2.1-192.168.2.5,10.10.10.5";
			var ipList = ipAddress.ParceIpAddresses();
			Assert.IsTrue(ipList.Count == 6);
			Assert.AreEqual(ipList.First(), IPAddress.Parse("192.168.2.1"));
			Assert.AreEqual(ipList[4], IPAddress.Parse("192.168.2.5"));
			Assert.AreEqual(ipList[5], IPAddress.Parse("10.10.10.5"));
		}

		[Test]
		public void ParceIpAddresses_ThrowsException_WhenInvalidIpAdressString()
		{
			string ipAddress = "192.168.2.900";
			Assert.Throws<ParceException>(() => ipAddress.ParceIpAddresses());
		}

		[Test]
		public void ParceIpAddresses_ThrowsException_WhenIpListOutOfLimit()
		{
			string ipAddress = "192.168.1.1-192.168.2.200";
			Assert.Throws<OutOfLimitException>(() => ipAddress.ParceIpAddresses());
		}

		[Test]
		public void ParceIpAddresses_ThrowsException_WhenRulsContainsAllowAllAndAnotherRules()
		{
			string ipAddress = "0.0.0.0, 192.168.1.1";
			Assert.Throws<HelpException>(() => ipAddress.ParceIpAddresses());
		}

	}
}
