using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Xsolla 
{
	public class ShopViewController : MonoBehaviour {

		public ScrollableListCustom menu;
		public RadioGroupController radioGroup;
		public Text title;
		public GridView content;

		public PricePointsAdapter pAdapter;
		public SubscriptionsAdapter sAdapter;
		public GoodsAdapter gAdapter;

		public void OpenPricepoints(string title, XsollaPricepointsManager pricepoints, string virtualCurrencyName, string buyBtnText)
		{
			Resizer.ResizeToParrent (gameObject);
			menu.transform.parent.parent.gameObject.SetActive (false);
			SetTitle (title);
			pAdapter.SetManager (pricepoints, virtualCurrencyName, buyBtnText);
			if (pAdapter.OnBuyPricepoints == null) {
				pAdapter.OnBuyPricepoints += (outAmount) => {
					Dictionary<string, object> map = new Dictionary<string, object> (1);
					map.Add ("out", outAmount);
					OpenPaymentMethods (map);
				};
			}
			DrawContent (pAdapter, 3);
		}
		
		public void OpenSubscriptions(string title, XsollaSubscriptions subscriptions)
		{
			Resizer.ResizeToParrent (gameObject);
			menu.transform.parent.parent.gameObject.SetActive (false);
			SetTitle (title);
			sAdapter.SetManager (subscriptions);
			if (sAdapter.OnBuySubscription == null) {
				sAdapter.OnBuySubscription += (subscriptionId) => {
					Dictionary<string, object> map = new Dictionary<string, object> (1);
					map.Add ("id_package", subscriptionId);
					OpenPaymentMethods (map);
				};
			}
			DrawContent (sAdapter, 1);
		}
		
		public void OpenGoods(XsollaGroupsManager groups)
		{
			Resizer.ResizeToParrent (gameObject);
			menu.transform.parent.parent.gameObject.SetActive (true);
			SetTitle (groups.GetItemByPosition(0).name);
			menu.SetData ((groupId) => {
				XsollaGoodsGroup group = groups.GetItemByKey(groupId);
				radioGroup.SelectItem (groups.GetItemsList().IndexOf(group));
				ChooseItemsGroup(group.id, group.name);
			}, groups.GetNamesDict ());
			radioGroup.SetButtons(menu.GetItems ());
			radioGroup.SelectItem (0);
		}

		public void UpdateGoods(XsollaGoodsManager goods, string buyBtnText)
		{
			gAdapter.SetManager (goods, buyBtnText);
			if (gAdapter.OnBuy == null) {
				gAdapter.OnBuy += (sku) => {
					Dictionary<string, object> map = new Dictionary<string, object> (1);
					map.Add (sku, 1);
					OpenPaymentMethods (map);
				};
			}
			if (gAdapter.OnFavorite == null) {
				gAdapter.OnFavorite += (isFavorite, sku, id) => {
					Dictionary<string, object> map = new Dictionary<string, object> (1);
					map.Add ("is_favorite", isFavorite ? 1 : 0);
					map.Add (sku, 1);
					map.Add ("virtual_item_id", id);
					SetFavorite (map);
				};
			}
			DrawContent (gAdapter, 3);
		}

		public void SetTitle(string s)
		{
			title.text = s;
		}

		private void DrawContent(IBaseAdapter adapter, int columnsCount){
			content.SetAdapter (adapter, columnsCount);
		}

		private void ChooseItemsGroup(long groupId, string groupName)
		{
			SetTitle (groupName);
			gameObject.GetComponentInParent<XsollaPaystationController> ().LoadGoods (groupId);	
		}

		private void OpenPaymentMethods(Dictionary<string, object> purchase)
		{
			gameObject.GetComponentInParent<XsollaPaystationController> ().ChooseItem (purchase);
		}

		private void SetFavorite(Dictionary<string, object> purchase)
		{
			gameObject.GetComponentInParent<XsollaPaystationController> ().SetFavorite (purchase);
		}

	}
}