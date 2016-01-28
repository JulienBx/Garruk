using UnityEngine;
using TMPro;

public class MobileMenuUsernameController : TextButtonController 
{
	public override void setHoveredState()
	{
		base.setHoveredState ();
		gameObject.transform.parent.gameObject.transform.FindChild("MobilePicture").GetComponent<SpriteRenderer>().color=ApplicationDesignRules.blueColor;
	}
	public override void setInitialState()
	{
		base.setInitialState ();
		gameObject.transform.parent.gameObject.transform.FindChild("MobilePicture").GetComponent<SpriteRenderer>().color=ApplicationDesignRules.whiteSpriteColor;
	}
	public override void mainInstruction()
	{
		MenuController.instance.profileLink ();
	}
}

