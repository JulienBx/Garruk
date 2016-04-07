using UnityEngine;
using TMPro;

public class NewFocusedFeaturesController : SpriteButtonController
{

	private string toolTipTile;
	private string toolTipDescription;

	public void showPrice(bool value)
	{
		gameObject.transform.FindChild ("Price").gameObject.SetActive (value);
	}
	public override void mainInstruction ()
	{
		if(base.getId()==4 || HelpController.instance.canAccess())
		{
			this.gameObject.transform.parent.GetComponent<NewFocusedCardController>().focusFeaturesHandler(base.getId());
		}
	}
	public override void setHoveredState()
	{
		gameObject.transform.FindChild("Button").GetComponent<SpriteRenderer>().color=ApplicationDesignRules.blueColor;
		gameObject.transform.FindChild("Button").FindChild("Picto").GetComponent<SpriteRenderer>().color=ApplicationDesignRules.blueColor;
		gameObject.transform.FindChild("Price").FindChild("Title").GetComponent<TextMeshPro>().color=ApplicationDesignRules.blueColor;
		gameObject.transform.FindChild("Price").GetComponent<SpriteRenderer>().color=ApplicationDesignRules.blueColor;
	}
	public override void setInitialState()
	{
		gameObject.transform.FindChild("Button").GetComponent<SpriteRenderer>().color=ApplicationDesignRules.whiteSpriteColor;
		gameObject.transform.FindChild("Button").FindChild("Picto").GetComponent<SpriteRenderer>().color=ApplicationDesignRules.whiteSpriteColor;
		gameObject.transform.FindChild("Price").FindChild("Title").GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		gameObject.transform.FindChild("Price").GetComponent<SpriteRenderer>().color=ApplicationDesignRules.whiteSpriteColor;
	}
	public override void setForbiddenState()
	{
		gameObject.transform.FindChild("Button").GetComponent<SpriteRenderer>().color=ApplicationDesignRules.redColor;
		gameObject.transform.FindChild("Button").FindChild("Picto").GetComponent<SpriteRenderer>().color=ApplicationDesignRules.redColor;
		gameObject.transform.FindChild("Price").FindChild("Title").GetComponent<TextMeshPro>().color=ApplicationDesignRules.redColor;
		gameObject.transform.FindChild("Price").GetComponent<SpriteRenderer>().color=ApplicationDesignRules.redColor;
	}
	public override void setIsActive (bool value)
	{
		base.setIsActive (value);
		if(!value)
		{
			this.setForbiddenState();
		}
		else
		{
			this.setInitialState();
		}
	}
	public void setToolTip(string toolTipTile, string toolTipDescription)
	{
		this.toolTipTile=toolTipTile;
		this.toolTipDescription=toolTipDescription;
	}
	public override void showToolTip ()
	{
		BackOfficeController.instance.displayToolTip(this.toolTipTile,this.toolTipDescription);
	}
}

