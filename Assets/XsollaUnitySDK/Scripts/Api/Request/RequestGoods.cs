using UnityEngine;
using System.Collections;
using SimpleJSON;

namespace Xsolla {
	public class RequestGoods : BaseWWWRequest {
		
		public RequestGoods(int type) : base(type){}
		
		protected override string GetMethod ()
		{
			return "/paystation2/api/virtualitems/items";
		}
		
		protected override object[] ParseResult (JSONNode rootNode)
		{
			var data = new XsollaGoodsManager().Parse(rootNode) as XsollaGoodsManager;
			return new object[]{data};
		}

	}
}
