using UnityEngine;
using TMPro;

public class NewProfileResultsContentUsernameController : TextButtonController
{	
	public override void mainInstruction()
	{
		NewProfileController.instance.clickOnResultsContentProfile (base.getId ());
	}
	public override void setHoveredState()
	{
		base.setHoveredState ();
		gameObject.transform.parent.gameObject.transform.FindChild ("picture").GetComponent<SpriteRenderer> ().color = ApplicationDesignRules.blueColor;
		gameObject.transform.parent.gameObject.transform.FindChild("divisionIcon").GetComponent<DivisionIconController>().setHoveredState();
	}
	public override void setInitialState()
	{
		base.setInitialState ();
		gameObject.transform.parent.gameObject.transform.FindChild ("picture").GetComponent<SpriteRenderer> ().color = ApplicationDesignRules.whiteSpriteColor;
		gameObject.transform.parent.gameObject.transform.FindChild("divisionIcon").GetComponent<DivisionIconController>().setInitialState();
	}
}

