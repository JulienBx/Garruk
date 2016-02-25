using UnityEngine;
using TMPro;

public class NewLobbyResultsPictureController : SpriteButtonController
{	
	public override void mainInstruction()
	{
		NewLobbyController.instance.clickOnResultsProfile (base.getId ());		
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

