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
	public void showPrice(bool value)
	{
		gameObject.transform.FindChild ("Price").gameObject.SetActive (value);
	}
	public void setIsClickable(bool value)
	{
		this.isClickable = value;
		if(value)
		{
			this.setStandardState();
		}
		else
		{
			this.setForbiddenState();
		}
	}
	void OnMouseOver()
	{
		if(!this.isHovering)
		{
			this.isHovering=true;
			if(this.isClickable)
			{
				this.setHoveredState();
			}
		}
	}
	void OnMouseExit()
	{
		if(this.isHovering)
		{
			this.isHovering=false;
			if(this.isClickable)
			{
				this.setStandardState();
			}
			else
			{
				this.setForbiddenState();
			}
		}
	}
	void OnMouseDown()
	{
		if(this.isClickable)
		{
			if(this.id==4 || TutorialObjectController.instance.canAccess())
			{
				this.gameObject.transform.parent.GetComponent<NewFocusedCardController>().focusFeaturesHandler(this.id);
			}
			this.isHovering=false;
		}
	}
	void setHoveredState()
	{
		gameObject.transform.FindChild("Button").GetComponent<SpriteRenderer>().color=ApplicationDesignRules.blueColor;
		gameObject.transform.FindChild("Button").FindChild("Picto").GetComponent<SpriteRenderer>().color=ApplicationDesignRules.blueColor;
		gameObject.transform.FindChild("Price").FindChild("Title").GetComponent<TextMeshPro>().color=ApplicationDesignRules.blueColor;
		gameObject.transform.FindChild("Price").GetComponent<SpriteRenderer>().color=ApplicationDesignRules.blueColor;
	}
	void setStandardState()
	{
		gameObject.transform.FindChild("Button").GetComponent<SpriteRenderer>().color=ApplicationDesignRules.whiteSpriteColor;
		gameObject.transform.FindChild("Button").FindChild("Picto").GetComponent<SpriteRenderer>().color=ApplicationDesignRules.whiteSpriteColor;
		gameObject.transform.FindChild("Price").FindChild("Title").GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		gameObject.transform.FindChild("Price").GetComponent<SpriteRenderer>().color=ApplicationDesignRules.whiteSpriteColor;
	}
	void setForbiddenState()
	{
		gameObject.transform.FindChild("Button").GetComponent<SpriteRenderer>().color=ApplicationDesignRules.redColor;
		gameObject.transform.FindChild("Button").FindChild("Picto").GetComponent<SpriteRenderer>().color=ApplicationDesignRules.redColor;
		gameObject.transform.FindChild("Price").FindChild("Title").GetComponent<TextMeshPro>().color=ApplicationDesignRules.redColor;
		gameObject.transform.FindChild("Price").GetComponent<SpriteRenderer>().color=ApplicationDesignRules.redColor;
	}
}

