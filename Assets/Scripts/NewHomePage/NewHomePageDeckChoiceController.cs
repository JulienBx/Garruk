using UnityEngine;
using TMPro;

public class NewHomePageDeckChoiceController : MonoBehaviour 
{

	public int id;
	private bool isHovered;
	
	void OnMouseOver()
	{
		if(!this.isHovered)
		{
			this.gameObject.transform.GetComponent<SpriteRenderer>().color=new Color(75f/255f,163f/255f,174f/255f);
			this.gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().color=new Color(75f/255f,163f/255f,174f/255f);
			this.isHovered=true;
		}
	}
	void OnMouseExit()
	{
		if(this.isHovered)
		{
			this.gameObject.transform.GetComponent<SpriteRenderer>().color=new Color(196f/255f,196f/255f,196f/255f);
			this.gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().color=new Color(196f/255f,196f/255f,196f/255f);
			this.isHovered=false;
		}
	}
	void OnMouseDown()
	{
		NewHomePageController.instance.selectDeck (this.id);
		this.gameObject.transform.GetComponent<SpriteRenderer>().color=new Color(196f/255f,196f/255f,196f/255f);
		this.gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().color=new Color(196f/255f,196f/255f,196f/255f);
		this.isHovered = false;
	}
}

