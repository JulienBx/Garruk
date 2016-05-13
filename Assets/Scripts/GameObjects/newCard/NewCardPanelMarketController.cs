using UnityEngine;
using TMPro;

public class NewCardPanelMarketController : SimpleButtonController
{

	private string toolTipTitle;
	private string toolTipDescription;


	public void setToolTipLabels(string toolTipTitle, string toolTipDescription)
	{
		this.toolTipTitle=toolTipTitle;
		this.toolTipDescription=toolTipDescription;
	}
	public override void setInitialState ()
	{
		gameObject.GetComponent<SpriteRenderer>().color=ApplicationDesignRules.whiteSpriteColor;
		gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		gameObject.transform.FindChild("Cristal").GetComponent<SpriteRenderer>().color=ApplicationDesignRules.whiteSpriteColor;
	}
	public override void setHoveredState ()
	{
		gameObject.GetComponent<SpriteRenderer>().color=ApplicationDesignRules.blueColor;
		gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().color=ApplicationDesignRules.blueColor;
	}
	public override void setForbiddenState ()
	{
		gameObject.GetComponent<SpriteRenderer>().color=ApplicationDesignRules.redColor;
		gameObject.transform.FindChild("Title").GetComponent<TextMeshPro>().color=ApplicationDesignRules.redColor;
		gameObject.transform.FindChild("Cristal").GetComponent<SpriteRenderer>().color=ApplicationDesignRules.redColor;
	}
	public override void mainInstruction ()
	{
		SoundController.instance.playSound(8);
		this.gameObject.transform.parent.GetComponent<NewCardMarketController>().panelMarketHandler();
	}
	public override void showToolTip ()
	{
		BackOfficeController.instance.displayToolTip(this.toolTipTitle,this.toolTipDescription);
	}
	public override void setIsActive (bool value)
	{
		base.setIsActive (value);
		if(value)
		{
			this.setInitialState();
		}
		else
		{
			this.setForbiddenState();
		}
	}
}

