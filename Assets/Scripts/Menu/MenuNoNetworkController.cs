using UnityEngine;
using TMPro;

public class MenuNoNetworkController : SpriteButtonController 
{
	public override void mainInstruction()
	{
		MenuController.instance.noNetworkLink ();
	}
	public override void setHoveredState()
	{
		this.gameObject.transform.GetComponent<SpriteRenderer> ().color = ApplicationDesignRules.blueColor;
	}
	public override void setInitialState()
	{
		this.gameObject.transform.GetComponent<SpriteRenderer> ().color = ApplicationDesignRules.redColor;
	}
}