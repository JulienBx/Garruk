using UnityEngine;
using TMPro;

public class NewCardPanelMarketController : MonoBehaviour 
{

	private bool isClickable;


	void Awake()
	{
		this.isClickable = true;
	}

	public void setClickable(bool value)
	{
		this.isClickable = value;
		if(!value)
		{
			gameObject.GetComponent<SpriteRenderer>().color=new Color(166f/255f,31f/255f,28f/255f);
			gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().color=new Color(166f/255f,31f/255f,28f/255f);
			gameObject.transform.FindChild("Cristal").GetComponent<SpriteRenderer>().color=new Color(166f/255f,31f/255f,28f/255f);
		}
		else
		{
			gameObject.GetComponent<SpriteRenderer>().color=new Color(1f,1f,1f);
			gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().color=new Color(1f,1f,1f);
			gameObject.transform.FindChild("Cristal").GetComponent<SpriteRenderer>().color=new Color(1f,1f,1f);
		}
	}

	void OnMouseOver()
	{
		if(isClickable)
		{
			gameObject.GetComponent<SpriteRenderer>().color=new Color(155f/255f,220f/255f,1f);
			gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().color=new Color(155f/255f,220f/255f,1f);
		}
	}
	void OnMouseExit()
	{
		if(isClickable)
		{
			gameObject.GetComponent<SpriteRenderer>().color=new Color(1f,1f,1f);
			gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().color=new Color(1f,1f,1f);
		}
	}
	void OnMouseDown()
	{
		if(isClickable)
		{
			this.gameObject.transform.parent.GetComponent<NewCardController>().displayBuyCardPopUp();
			this.gameObject.transform.parent.GetComponent<NewCardMarketController>().communicateIdCard();
		}
	}
}

