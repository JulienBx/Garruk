using UnityEngine;

public class DeckBoardDeckListController : MonoBehaviour 
{
	
	public Sprite[] sprites;
	public int id;
	
	void OnMouseOver()
	{
		gameObject.transform.GetComponent<SpriteRenderer> ().sprite = this.sprites [1];
		gameObject.transform.FindChild("Title").GetComponent<TextMesh>().color=new Color(0f,0f,0f);
	}
	void OnMouseExit()
	{
		gameObject.transform.GetComponent<SpriteRenderer> ().sprite = this.sprites [0];
		gameObject.transform.FindChild("Title").GetComponent<TextMesh>().color=new Color(1f,1f,1f);
	}
	void OnMouseDown()
	{
		newMyGameController.instance.selectDeck (this.id);
	}
	public void setId(int id)
	{
		this.id = id;
	}
}
