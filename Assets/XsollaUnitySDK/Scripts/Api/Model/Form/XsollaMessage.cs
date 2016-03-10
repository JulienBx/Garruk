using UnityEngine;
using System.Collections;
using SimpleJSON;

namespace Xsolla {

	public class XsollaMessage : IParseble{
		public string code {get; private set;}		//"code":"statustext",
		public string message {get; private set;}	//"message":"The invoice was issued. After payment click to verify"

		public IParseble Parse(JSONNode messageNode)
		{
			this.code = messageNode ["code"].Value;
			this.message = messageNode ["message"].Value;
			return this;
		}

	}

}
