using UnityEngine;
using TMPro;

public class NewLobbySlideRightButtonController : SpriteButtonController
{	
	public override void setHoveredState()
	{
		base.setHoveredState ();
		this.gameObject.transform.FindChild("Picto").GetComponent<SpriteRenderer> ().color = ApplicationDesignRules.blueColor;
	}
	public override void setInitialState()
	{
		base.setInitialState ();
		this.gameObject.transform.FindChild("Picto").GetComponent<SpriteRenderer> ().color = ApplicationDesignRules.whiteSpriteColor;
	}
	public override void mainInstruction()
	{
		NewLobbyController.instance.slideRight ();
	}
}