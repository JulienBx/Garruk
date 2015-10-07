using UnityEngine;
using TMPro;

public class DeckBoardDeckListInvitationPopUpController : MonoBehaviour 
{
	
	public Sprite[] sprites;
	private int id;
	
	void OnMouseOver()
	{
		gameObject.transform.GetComponent<SpriteRenderer> ().sprite = this.sprites [1];
		gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().color=new Color(0f,0f,0f);
	}
	void OnMouseExit()
	{
		gameObject.transform.GetComponent<SpriteRenderer> ().sprite = this.sprites [0];
		gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().color=new Color(1f,1f,1f);
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

