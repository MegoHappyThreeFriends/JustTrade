using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using NetTools;

namespace JustTrade.Helpers.ExtensionMethods
{
	public static class IpAdressStringExtension
	{
		public static List<IPAddress> ParceIpAddresses(this string strData)
		{
			const int maxIpCount = 255;
			List<IPAddress> ipAddresses = new List<IPAddress>();
			string[] ipItems = strData.Split(',');
			foreach (var ipItem in ipItems)
			{
				IPAddressRange range;
				try
				{
					range = IPAddressRange.Parse(ipItem);
				}
				catch (Exception ex)
				{
					throw new Exception("Error parce ip address", ex);
				}
				if (range.Count() > maxIpCount)
				{
					throw new Exception("IP range can't be more 255");
				}
				ipAddresses.AddRange(range);
			}
			if (ipAddresses.Contains(IPAddress.Parse("0.0.0.0")) && ipAddresses.Count > 1)
			{
				throw new Exception("This rule does not make sense, because filter '0.0.0.0' will be allow all ip");
			}
			return ipAddresses;
		}
	}
}
