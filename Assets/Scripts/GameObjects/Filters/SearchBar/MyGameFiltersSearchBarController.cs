using UnityEngine;

public class MyGameFiltersSearchBarController : MonoBehaviour 
{
	void OnMouseDown()
	{
		newMyGameController.instance.searchingSkill ();	
	}
	void OnMouseOver()
	{
		newMyGameController.instance.mouseOnSearchBar (true);
	}
	void OnMouseExit()
	{
		newMyGameController.instance.mouseOnSearchBar (false);
	}
}

