using UnityEngine;
using System.Collections;
using SimpleJSON;
using System.Collections.Generic;

namespace Xsolla {
	public class XsollaCountries : XsollaObjectsManager<XsollaCountry>, IParseble 
	{

		private string iso;//					"iso" : US,
//		private List<object> countryList;//		"countryList" : +[ ... ],
//		private XsollaApi api;//				"api" : +{ ... }

		public string GetIso(){
			return iso;
		}

		public IParseble Parse(JSONNode countriesNode)
		{
			iso = countriesNode["iso"];
			var countriesEnumerator = countriesNode["countryList"].Childs.GetEnumerator();
			while(countriesEnumerator.MoveNext()){
				var country = new XsollaCountry();
				AddItem(country.Parse(countriesEnumerator.Current) as XsollaCountry);
			}
//			api = new XsollaApi().Parse(countriesNode["api"]) as XsollaApi;
			return this;
		}
	}

	public class XsollaCountry : IXsollaObject, IParseble
	{

		public string iso {get; private set;}//			"ISO" : AX,
		public string aliases {get; private set;}//		"aliases" : ,
		public string name {get; private set;}//		"name" : Aaland Islands

		public string GetKey()
		{
			return iso;
		}

		public string GetName()
		{
			return name;
		}

		public IParseble Parse(JSONNode countryNode)
		{
			iso = countryNode["ISO"];
			aliases = countryNode["aliases"];
			name = countryNode["name"];
			return this;
		}
	}
}
