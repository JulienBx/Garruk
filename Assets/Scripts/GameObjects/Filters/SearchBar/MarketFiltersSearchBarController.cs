using UnityEngine;

public class MarketFiltersSearchBarController : MonoBehaviour 
{
	void OnMouseDown()
	{
		NewMarketController.instance.searchingSkill ();	
	}
	void OnMouseOver()
	{
		NewMarketController.instance.mouseOnSearchBar (true);
	}
	void OnMouseExit()
	{
		NewMarketController.instance.mouseOnSearchBar (false);
	}
}

