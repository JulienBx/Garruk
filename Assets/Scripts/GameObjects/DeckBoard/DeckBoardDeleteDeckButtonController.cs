using UnityEngine;
using TMPro;

public class DeckBoardDeleteDeckButtonController : MonoBehaviour 
{
	public Sprite[] sprites;
	
	void OnMouseOver()
	{
		gameObject.GetComponent<SpriteRenderer>().color=new Color(155f/255f,220f/255f,1f);
		gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().color=new Color(155f/255f,220f/255f,1f);
	}
	void OnMouseExit()
	{
		gameObject.GetComponent<SpriteRenderer>().color=new Color(1f,1f,1f);
		gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().color=new Color(1f,1f,1f);
	}
	void OnMouseDown()
	{
		newMyGameController.instance.displayDeleteDeckPopUp ();
	}
}

