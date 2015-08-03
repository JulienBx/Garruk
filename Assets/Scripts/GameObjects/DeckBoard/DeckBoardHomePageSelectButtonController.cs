using UnityEngine;

public class DeckBoardHomePageSelectButtonController : MonoBehaviour 
{
	
	void OnMouseOver()
	{
		gameObject.GetComponent<SpriteRenderer>().color=new Color(155f/255f,220f/255f,1f);
		NewHomePageController.instance.mouseOnSelectDeckButton (true);
	}
	void OnMouseExit()
	{
		gameObject.GetComponent<SpriteRenderer>().color=new Color(1f,1f,1f);
		NewHomePageController.instance.mouseOnSelectDeckButton (false);
	}
	void OnMouseDown()
	{
		NewHomePageController.instance.displayDeckList ();	
	}
	
}

