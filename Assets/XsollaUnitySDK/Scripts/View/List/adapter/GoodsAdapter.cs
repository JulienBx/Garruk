

using UnityEngine;
using UnityEngine.UI;
using System;

namespace Xsolla 
{
	public class GoodsAdapter : IBaseAdapter 
	{
		private GameObject shopItemPrefab;

		private XsollaGoodsManager manager;
		private string textValue = "Coins";
		private string _buyBtnText = "Buy";
		int current = 0;

		public Action<string> OnBuy; 
		public Action<bool, string, long> OnFavorite; 

		public void Start()
		{
			shopItemPrefab = Resources.Load("Prefabs/SimpleView/_ScreenShop/_ShopItemGood") as GameObject;
		}
		
		public override int GetElementType(int id)
		{
			return 0;
		}
		
		public override int GetCount() 
		{
			return manager.GetCount ();
		}

		
		public override GameObject GetView(int position)
		{
			GameObject shopItemInstance = Instantiate(shopItemPrefab) as GameObject;
			shopItemInstance.name = "ShopItemGood " + position;
			XsollaShopItem item = manager.GetItemByPosition (position);//manager.GetItemByPosition (position);
			ShopItemViewAdapter itemAdapter = shopItemInstance.GetComponent<ShopItemViewAdapter>();
			itemAdapter.SetPrice (item.GetPriceString());
			itemAdapter.SetSpecial (item.GetBounusString());
			itemAdapter.SetDesc (item.GetDescription());
			itemAdapter.SetName (item.GetName());
			itemAdapter.SetFullDesc (item.GetLongDescription());
			itemAdapter.SetBuyText ("Buy");
			itemAdapter.SetImage (item.GetImageUrl());
			itemAdapter.SetFavorite (item.IsFavorite());
			itemAdapter.SetOnClickListener(() => OnClickBuy("sku[" + item.GetKey() + "]"));
			itemAdapter.SetOnFavoriteChanged((b) => OnClickFavorite(b, "sku[" + item.GetKey() + "]", item.GetId()));
			itemAdapter.SetLabel (item.GetAdvertisementType (), item.GetLabel());
			return shopItemInstance;
		}

		private void OnClickFavorite(bool isFavorite, string sku, long virtualItemId){
			if (OnFavorite != null) 
			{
				OnFavorite(isFavorite, sku, virtualItemId);
			}
		}

		private void OnClickBuy (string sku){
			if (OnBuy != null) 
			{
				OnBuy(sku);
			}
		}

		public override GameObject GetPrefab ()
		{
			return shopItemPrefab;
		}

		public void SetManager(XsollaGoodsManager pricepoints, string buyBtnText)
		{
			_buyBtnText = buyBtnText;
			manager = pricepoints;
		}

		public override GameObject GetNext ()
		{
			if (current < manager.GetCount ()) 
			{
				GameObject go = GetView (current);
				current ++;
				return go;
			}
			return null;
		}
	}
}
