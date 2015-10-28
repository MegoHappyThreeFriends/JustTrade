using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using JustTrade.Helpers.ExceptionCollection;
using NetTools;

namespace JustTrade.Helpers.ExtensionMethods
{
	public static class IpAdressStringExtension
	{
		public static List<IPAddress> ParceIpAddresses(this string strData)
		{
			const int maxIpCount = 255;
			List<IPAddress> ipAddresses = new List<IPAddress>();
			string[] ipItems = strData.Trim().Split(',');
			foreach (var ipItem in ipItems)
			{
				if (ipItem.Trim().Length == 0)
				{
					continue;
				}
				if (ipItem.Replace(" ", "") == "0.0.0.0")
				{
					ipAddresses.Add(IPAddress.Any);
					continue;
				}
				IPAddressRange range;
				try
				{
					range = IPAddressRange.Parse(ipItem);
				}
				catch (Exception ex)
				{
					throw new ParceException("Error parce ip address", ex);
				}
				if (range.Count() > maxIpCount)
				{
					throw new OutOfLimitException("IP range can't be more 255");
				}
				ipAddresses.AddRange(range);
			}
			if (ipAddresses.Contains(IPAddress.Any) && ipAddresses.Count > 1)
			{
				throw new HelpException("This rule does not make sense, because filter '0.0.0.0' will be allow all ip");
			}
			return ipAddresses;
		}
	}
}
