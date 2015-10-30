using UnityEngine;
using TMPro;

public class MenuUserUsernameController : TextButtonController 
{
	public override void setHoveredState()
	{
		base.setHoveredState ();
		gameObject.transform.parent.gameObject.transform.FindChild("Picture").GetComponent<SpriteRenderer>().color=ApplicationDesignRules.blueColor;
	}
	public override void setInitialState()
	{
		base.setInitialState ();
		gameObject.transform.parent.gameObject.transform.FindChild("Picture").GetComponent<SpriteRenderer>().color=ApplicationDesignRules.whiteSpriteColor;
	}
	public override void OnMouseDown()
	{
		MenuController.instance.profileLink ();
	}
}

