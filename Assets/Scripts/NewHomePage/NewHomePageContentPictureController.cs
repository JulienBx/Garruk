using UnityEngine;
using TMPro;

public class NewHomePageContentPictureController : SpriteButtonController
{	
	public override void mainInstruction()
	{
		NewHomePageController.instance.clickOnContentProfile (base.getId ());		
	}
	public override void setHoveredState()
	{
		base.setHoveredState ();
		gameObject.transform.parent.gameObject.transform.FindChild ("username").GetComponent<TextMeshPro> ().color = ApplicationDesignRules.blueColor;
	}
	public override void setInitialState()
	{
		base.setInitialState ();
		gameObject.transform.parent.gameObject.transform.FindChild ("username").GetComponent<TextMeshPro> ().color = ApplicationDesignRules.whiteTextColor;
	}
}

