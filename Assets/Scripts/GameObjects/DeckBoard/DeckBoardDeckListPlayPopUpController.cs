using UnityEngine;
using TMPro;

public class DeckBoardDeckListPlayPopUpController : MonoBehaviour 
{
	
	public Sprite[] sprites;
	private int id;
	
	void OnMouseOver()
	{
		if(!ApplicationDesignRules.isMobileScreen)
		{
			gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().color=ApplicationDesignRules.blueColor;
		}
	}
	void OnMouseExit()
	{
		if(!ApplicationDesignRules.isMobileScreen)
		{
			gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		}
	}
	void OnMouseDown()
	{
		PlayPopUpController.instance.selectDeck (this.id);
	}
	public void setId(int id)
	{
		this.id = id;
	}
}

