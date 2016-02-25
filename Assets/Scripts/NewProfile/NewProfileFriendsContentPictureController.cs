using UnityEngine;
using TMPro;

public class NewProfileFriendsContentPictureController : SpriteButtonController
{	
	public override void mainInstruction()
	{
		NewProfileController.instance.clickOnFriendsContentProfile (base.getId ());		
	}
	public override void setHoveredState()
	{
		base.setHoveredState ();
		gameObject.transform.parent.gameObject.transform.FindChild ("username").GetComponent<TextMeshPro> ().color = ApplicationDesignRules.blueColor;
		gameObject.transform.parent.gameObject.transform.FindChild("divisionIcon").GetComponent<DivisionIconController>().setHoveredState();
	}
	public override void setInitialState()
	{
		base.setInitialState ();
		gameObject.transform.parent.gameObject.transform.FindChild ("username").GetComponent<TextMeshPro> ().color = ApplicationDesignRules.whiteTextColor;
		gameObject.transform.parent.gameObject.transform.FindChild("divisionIcon").GetComponent<DivisionIconController>().setInitialState();
	}
}

