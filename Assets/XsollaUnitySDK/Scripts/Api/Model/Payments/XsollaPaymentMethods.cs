using UnityEngine;
using System.Collections;
using SimpleJSON;
using System.Collections.Generic;

namespace Xsolla 
{
	public class XsollaPaymentMethods : XsollaObjectsManager<XsollaPaymentMethod>, IParseble 
	{

//		private List<XsollaPaymentMethod> paymentList;//	"instances" : [ ... ],
		private object lastPayment;//						"lastPayment" : null,
		private XsollaApi api;//							"api" : { ... }

		public object GetLastPayment(){
			return lastPayment;
		}

		public List<XsollaPaymentMethod> GetSortedItems(string s)
		{
			return itemsList.FindAll (delegate(XsollaPaymentMethod xpm) {
				return xpm.name.ToLower().StartsWith (s.ToLower());
			});
		}

		public List<XsollaPaymentMethod> GetRecomendedItems()
		{
			return GetItemsList().FindAll (delegate(XsollaPaymentMethod xpm) {
				return xpm.isHidden == 0 && xpm.isRecommended == 1;
			});//((xpm) => {return xpm.isHidden == 0 && xpm.isRecommended == 1;});

		}

		public IParseble Parse (JSONNode paymentListNode)
		{
			IEnumerator<JSONNode> paymentListEnumerator =  paymentListNode["instances"].Childs.GetEnumerator();
			while(paymentListEnumerator.MoveNext())
			{
				XsollaPaymentMethod method = new XsollaPaymentMethod().Parse(paymentListEnumerator.Current) as XsollaPaymentMethod;
				if(method.id != 64 && method.id != 1738){
					AddItem(method);
				}
			}
			lastPayment = null;//paymentListNode["lastPayment"];
			api = new XsollaApi().Parse(paymentListNode["api"]) as XsollaApi;
			return this;
		}
	}

	public class XsollaPaymentMethod : IXsollaObject, IParseble
	{
		public long id {get; private set;}//				"id" : 24,
		public int isHidden {get; private set;}//			"hidden" : 0, show 0
		public string aliases {get; private set;}//			"aliases" : ,
		public int isRecommended {get; private set;}//		"recommended" : 1, show 1
		public int[] cat {get; private set;}//				"cat" : [ ... ],
		public string recurrentType {get; private set;}//	"recurrent_type" : charge,
		public string name {get; private set;}//			"name" : PayPal,
		public string imgUrl {get; private set;}//			"imgUrl" : //xsolla.cachefly.net/paymentoptions/paystation/theme_33/143x83/24.1421413162.png

		public string GetImageUrl()
		{
			if(imgUrl.StartsWith("https:"))
				return imgUrl;
			else 
				return "https:" + imgUrl;
		}

		public string GetKey()
		{
			return id.ToString ();
		}

		public string GetName()
		{
			return name;
		}

		public IParseble Parse (JSONNode paymentMethodNode)
		{
			id = paymentMethodNode ["id"].AsInt;
			isHidden = paymentMethodNode["hidden"].AsInt;
			aliases = paymentMethodNode["aliases"];
			isRecommended = paymentMethodNode["recommended"].AsInt;
			IEnumerator<JSONNode> catEnumerator =  paymentMethodNode["cat"].Childs.GetEnumerator();
			cat = new int[paymentMethodNode["cat"].Count];
			int counter = 0;
			while (catEnumerator.MoveNext()) 
			{
				cat[counter] = catEnumerator.Current.AsInt;
				counter++;
			}
			recurrentType = paymentMethodNode["recurrentType"];
			name = paymentMethodNode ["name"];
			imgUrl = paymentMethodNode["imgUrl"];
			return this;
		}

		public override string ToString ()
		{
			return string.Format ("[XsollaPaymentMethod: id={0}, isHidden={1}, aliases={2}, isRecommended={3}, cat={4}, recurrentType={5}, name={6}, imgUrl={7}]", id, isHidden, aliases, isRecommended, cat, recurrentType, name, imgUrl);
		}
	}
}
