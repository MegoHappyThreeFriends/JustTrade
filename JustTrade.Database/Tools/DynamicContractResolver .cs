using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace JustTrade.Database.Tools
{
	public class DynamicContractResolver : DefaultContractResolver
	{
		private readonly List<string> _propertyNameToExclude;

		public DynamicContractResolver(List<string> propertyNameToExclude)
		{
			_propertyNameToExclude = propertyNameToExclude;
			for (int i = 0; i < _propertyNameToExclude.Count; i++)
			{
				_propertyNameToExclude[0] = _propertyNameToExclude[0].ToLower();
			}
		}

		protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
		{
			IList<JsonProperty> properties = base.CreateProperties(type, memberSerialization);
			properties =
				properties.Where(p => _propertyNameToExclude.Contains(p.PropertyName)).ToList();
			return properties;
		}
	}
}
