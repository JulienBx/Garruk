using UnityEngine;
using TMPro;

public class NewFocusedFeaturesController : MonoBehaviour
{
	private bool isClickable;
	private bool isEnabled;
	private bool isHovering;
	public int id;

	void Awake()
	{
		this.isClickable = true;
		this.isHovering = false;
	}
	public void setIsClickable(bool value)
	{
		this.isClickable = value;
	}
	void OnMouseOver()
	{
		if(!this.isHovering)
		{
			this.isHovering=true;
			if(this.isClickable)
			{
				gameObject.GetComponent<SpriteRenderer>().color=ApplicationDesignRules.blueColor;
				gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().color=ApplicationDesignRules.blueColor;
			}
		}
	}
	void OnMouseExit()
	{
		if(this.isHovering)
		{
			this.isHovering=false;
			gameObject.GetComponent<SpriteRenderer>().color=ApplicationDesignRules.whiteSpriteColor;
			gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		}
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
			gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
			this.isHovering=false;
		}
	}
}

