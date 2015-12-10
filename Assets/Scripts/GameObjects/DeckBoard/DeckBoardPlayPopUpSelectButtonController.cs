using UnityEngine;

public class DeckBoardPlayPopUpSelectButtonController : MonoBehaviour 
{
	
	void OnMouseOver()
	{
		gameObject.GetComponent<SpriteRenderer>().color=ApplicationDesignRules.blueColor;
		PlayPopUpController.instance.mouseOnSelectDeckButton (true);
	}
	void OnMouseExit()
	{
		gameObject.GetComponent<SpriteRenderer>().color=new Color(1f,1f,1f);
		PlayPopUpController.instance.mouseOnSelectDeckButton (false);
	}
	void OnMouseDown()
	{
		PlayPopUpController.instance.displayDeckList ();	
	}
	
}

