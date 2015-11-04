using UnityEngine;
using TMPro;

public class NewProfileResultsContentPictureController : SpriteButtonController
{	
	public override void mainInstruction()
	{
		NewProfileController.instance.clickOnResultsContentProfile (base.getId ());		
	}
	public override void setHoveredState()
	{
		base.setHoveredState ();
		gameObject.transform.parent.gameObject.transform.FindChild ("title").GetComponent<TextMeshPro> ().color = ApplicationDesignRules.blueColor;
	}
	public override void setInitialState()
	{
		base.setInitialState ();
		gameObject.transform.parent.gameObject.transform.FindChild ("title").GetComponent<TextMeshPro> ().color = ApplicationDesignRules.whiteTextColor;
	}
}

