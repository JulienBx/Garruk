using UnityEngine;
using TMPro;

public class DeckBoardDeckListInvitationPopUpController : MonoBehaviour 
{
	
	public Sprite[] sprites;
	private int id;
	
	void OnMouseOver()
	{
		gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().color=ApplicationDesignRules.blueColor;
	}
	void OnMouseExit()
	{
		gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
	}
	void OnMouseDown()
	{
		InvitationPopUpController.instance.selectDeck (this.id);
	}
	public void setId(int id)
	{
		this.id = id;
	}
}

