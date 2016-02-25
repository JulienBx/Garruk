using UnityEngine;
using TMPro;

public class MenuUserUsernameController : TextButtonController 
{
	public override void setHoveredState()
	{
		base.setHoveredState ();
		gameObject.transform.parent.gameObject.transform.FindChild("Picture").GetComponent<SpriteRenderer>().color=ApplicationDesignRules.blueColor;
		gameObject.transform.parent.gameObject.transform.FindChild("DivisionIcon").GetComponent<DivisionIconController>().setHoveredState();
	}
	public override void setInitialState()
	{
		base.setInitialState ();
		gameObject.transform.parent.gameObject.transform.FindChild("Picture").GetComponent<SpriteRenderer>().color=ApplicationDesignRules.whiteSpriteColor;
		gameObject.transform.parent.gameObject.transform.FindChild("DivisionIcon").GetComponent<DivisionIconController>().setInitialState();
	}
	public override void mainInstruction()
	{
		MenuController.instance.profileLink ();
	}
}

