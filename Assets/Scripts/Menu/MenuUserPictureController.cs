using UnityEngine;
using TMPro;

public class MenuUserPictureController : SpriteButtonController 
{
	
	public override void setHoveredState()
	{
		base.setHoveredState ();
		gameObject.transform.parent.gameObject.transform.FindChild("Username").GetComponent<TextMeshPro>().color=ApplicationDesignRules.blueColor;
	}
	public override void setInitialState()
	{
		base.setInitialState ();
		gameObject.transform.parent.gameObject.transform.FindChild("Username").GetComponent<TextMeshPro>().color=ApplicationDesignRules.whiteTextColor;
	}
	public override void OnMouseDown()
	{
		MenuController.instance.profileLink ();
	}
}

