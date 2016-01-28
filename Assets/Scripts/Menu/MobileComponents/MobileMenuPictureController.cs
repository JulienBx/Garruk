using UnityEngine;
using TMPro;

public class MobileMenuPictureController : SpriteButtonController 
{
	
	public override void setHoveredState()
	{
		base.setHoveredState ();
		gameObject.transform.parent.gameObject.transform.FindChild("MobileUsername").GetComponent<TextMeshPro>().color=ApplicationDesignRules.blueColor;
	}
	public override void setInitialState()
	{
		base.setInitialState ();
		gameObject.transform.parent.gameObject.transform.FindChild("MobileUsername").GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
	}
	public override void mainInstruction()
	{
		MenuController.instance.profileLink ();
	}
}

