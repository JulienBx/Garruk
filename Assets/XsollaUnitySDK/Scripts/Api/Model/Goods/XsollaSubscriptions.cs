using SimpleJSON;
using System.Collections.Generic;
using System.Text;

namespace Xsolla 
{
	public class XsollaSubscriptions : XsollaObjectsManager<XsollaSubscription>, IParseble {

//		private List<XsollaSubscription> subscriptionList;//"packages":[],
//		private XsollaApi api;//							"api":{

		public IParseble Parse (JSONNode subscriptionsNode)
		{
			var packagesNode = subscriptionsNode ["packages"];
			var enumerator = packagesNode.Childs.GetEnumerator ();
			while (enumerator.MoveNext()) 
			{
				AddItem(new XsollaSubscription().Parse(enumerator.Current) as XsollaSubscription);
			}
//			api = new XsollaApi().Parse(subscriptionsNode["api"]) as XsollaApi;
			return this;
		}
	}

	public class XsollaSubscription : IXsollaObject, IParseble
	{
		public string id { get; private set;}// 						"id":"5f23c3de",
		public float chargeAmount { get; private set;}//				"chargeAmount":19.99,
		public float chargeAmountWithoutDiscount{ get; private set;}//	"chargeAmountWithoutDiscount":19.99,
		public string chargeCurrency { get; private set;}//				"chargeCurrency":"USD",
		public int period { get; private set;}//						"period":1,
		public string periodUnit { get; private set;}//					"periodUnit":"month",
		public string name { get; private set;}//						"name":"Platinum VIP",
		public string description { get; private set;}//				"description":"10x more experience!",
		public int bonusOut { get; private set;}//						"bonusOut":0,
		public List<XsollaBonusItem> bonusItems { get; private set;}//	"bonusItems":[]

		public string GetBounusString()
		{
			if (bonusItems.Count > 0) {
				StringBuilder stringBuilder = new StringBuilder ();
				stringBuilder.Append ("<color=#2DAE7B>");
				stringBuilder.Append ("+ ");
				foreach (XsollaBonusItem bonusItem in bonusItems) {
					stringBuilder.Append (bonusItem.name).Append (" ");
				}
				stringBuilder.Append ("</color>");
				return stringBuilder.ToString ();
			} 
			else 
			{
				return "";
			}
		}

		public string GetPeriodString(string per)
		{
			return per + " " + period + " " + periodUnit;
		}

		public bool IsSpecial(){
			return chargeAmount != chargeAmountWithoutDiscount;
		}

		public string GetPriceString()
		{
			if (!IsSpecial()) {
				return CurrencyFormatter.FormatPrice(chargeCurrency, chargeAmount.ToString());
			} 
			else 
			{
				string oldPrice = CurrencyFormatter.FormatPrice(chargeCurrency, chargeAmountWithoutDiscount.ToString());
				string newPrice = CurrencyFormatter.FormatPrice(chargeCurrency, chargeAmount.ToString());
				return "<size=10><color=#a7a7a7>" + oldPrice + "</color></size>" + " " + newPrice;
			}
			
		}

		public string GetKey()
		{
			return id.ToString ();
		}

		public string GetName()
		{
			return name;
		}

		public IParseble Parse (JSONNode subscriptionNode)
		{
			id = subscriptionNode ["id"];
			chargeAmount = subscriptionNode ["chargeAmount"].AsFloat;
			chargeAmountWithoutDiscount = subscriptionNode ["chargeAmountWithoutDiscount"].AsFloat;
			chargeCurrency = subscriptionNode ["chargeCurrency"];
			period = subscriptionNode ["period"].AsInt;
			periodUnit = subscriptionNode ["periodUnit"];
			name = subscriptionNode ["name"];
			description = subscriptionNode ["description"];
			bonusOut = subscriptionNode ["bonusOut"].AsInt;
			bonusItems = XsollaBonusItem.ParseMany (subscriptionNode ["bonusItems"]);
			return this;
		}
	}
}
