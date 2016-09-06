using UnityEngine;
using TMPro;

public class MobileMenuNoNetworkController : SpriteButtonController 
{
	public override void mainInstruction()
	{
		MenuController.instance.noNetworkLink ();
	}
	public override void setInitialState()
	{
		this.gameObject.transform.GetComponent<SpriteRenderer> ().color = ApplicationDesignRules.redColor;
	}
}

