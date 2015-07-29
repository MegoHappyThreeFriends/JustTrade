using System;

namespace JustTrade.Tools.Attributes
{
	using NUnit.Framework.Constraints;

	[System.AttributeUsage(AttributeTargets.Method)]
	public class FreeAccess : Attribute
	{
	}
}