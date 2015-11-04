using UnityEngine;
using TMPro;

public class NewHomePageContentUsernameController : TextButtonController
{	
	public override void mainInstruction()
	{
		NewHomePageController.instance.clickOnContentProfile (base.getId ());
	}
	public override void setHoveredState()
	{
		base.setHoveredState ();
		gameObject.transform.parent.gameObject.transform.FindChild ("picture").GetComponent<SpriteRenderer> ().color = ApplicationDesignRules.blueColor;
	}
	public override void setInitialState()
	{
		base.setInitialState ();
		gameObject.transform.parent.gameObject.transform.FindChild ("picture").GetComponent<SpriteRenderer> ().color = ApplicationDesignRules.whiteSpriteColor;
	}
}

