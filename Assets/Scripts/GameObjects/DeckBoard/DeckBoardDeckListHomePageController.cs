using UnityEngine;
using TMPro;

public class DeckBoardDeckListHomePageController : MonoBehaviour 
{
	
	public Sprite[] sprites;
	private bool isHovered;
	private int id;
	
	void OnMouseOver()
	{
		if(!this.isHovered)
		{
			gameObject.transform.GetComponent<SpriteRenderer> ().sprite = this.sprites [1];
			gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().color=new Color(0f,0f,0f);
			this.isHovered=true;
		}
	}
	void OnMouseExit()
	{
		if(this.isHovered)
		{
			gameObject.transform.GetComponent<SpriteRenderer> ().sprite = this.sprites [0];
			gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().color=new Color(1f,1f,1f);
			this.isHovered=false;
		}
	}
	void OnMouseDown()
	{
		NewHomePageController.instance.selectDeck (this.id);
	}
	public void setId(int id)
	{
		this.id = id;
	}
}

