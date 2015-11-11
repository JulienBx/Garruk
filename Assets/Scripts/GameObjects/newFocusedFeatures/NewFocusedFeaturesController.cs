using UnityEngine;
using TMPro;

public class NewFocusedFeaturesController : MonoBehaviour
{
	private bool isClickable;
	private bool isEnabled;
	public int id;

	void Awake()
	{
		this.isClickable = true;
	}
	public void setIsClickable(bool value)
	{
		this.isClickable = value;
	}
	void OnMouseOver()
	{
		if(this.isClickable)
		{
			gameObject.GetComponent<SpriteRenderer>().color=new Color(155f/255f,220f/255f,1f);
			gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().color=new Color(155f/255f,220f/255f,1f);
		}
	}
	void OnMouseExit()
	{
		gameObject.GetComponent<SpriteRenderer>().color=new Color(1f,1f,1f);
		gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().color=new Color(1f,1f,1f);
	}
	void OnMouseDown()
	{
		if(this.isClickable)
		{
			if(this.id==5 || TutorialObjectController.instance.canAccess())
			{
				this.gameObject.transform.parent.GetComponent<NewFocusedCardController>().focusFeaturesHandler(this.id);
			}
			gameObject.GetComponent<SpriteRenderer>().color=new Color(1f,1f,1f);
			gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().color=new Color(1f,1f,1f);
		}
	}
}

