using UnityEngine;
using TMPro;

public class MenuUserPictureController : SpriteButtonController 
{
	public override void setHoveredState()
	{
		base.setHoveredState ();
		gameObject.transform.parent.gameObject.transform.FindChild("Username").GetComponent<TextMeshPro>().color=ApplicationDesignRules.blueColor;
		gameObject.transform.parent.gameObject.transform.FindChild("DivisionIcon").GetComponent<DivisionIconController>().setHoveredState();
				
	}
	public override void setInitialState()
	{
		base.setInitialState ();
		gameObject.transform.parent.gameObject.transform.FindChild("Username").GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
		gameObject.transform.parent.gameObject.transform.FindChild("DivisionIcon").GetComponent<DivisionIconController>().setInitialState();
	}
	public override void mainInstruction()
	{
		MenuController.instance.profileLink ();
	}
}

