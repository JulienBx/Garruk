
using System.Collections.Generic;

namespace Xsolla 
{
	public class XsollaResult
	{

		public string invoice{ get; set;}
		public XsollaStatusData.Status status{ get; set;}
		public Dictionary<string, object> purchases;

		public XsollaResult(){}

		public XsollaResult(Dictionary<string, object> purchases){
			this.purchases = purchases;
		}

	}
}
 