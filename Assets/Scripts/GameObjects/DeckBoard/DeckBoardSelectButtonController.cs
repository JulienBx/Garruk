using UnityEngine;

public class DeckBoardSelectButtonController : MonoBehaviour 
{
	
	public Sprite[] sprites;
	
	void OnMouseOver()
	{
		gameObject.transform.GetComponent<SpriteRenderer> ().sprite = this.sprites [1];
		newMyGameController.instance.mouseOnSelectDeckButton (true);
	}
	void OnMouseExit()
	{
		gameObject.transform.GetComponent<SpriteRenderer> ().sprite = this.sprites [0];
		newMyGameController.instance.mouseOnSelectDeckButton (false);
	}
	void OnMouseDown()
	{
		newMyGameController.instance.displayDeckList ();	
	}

}

