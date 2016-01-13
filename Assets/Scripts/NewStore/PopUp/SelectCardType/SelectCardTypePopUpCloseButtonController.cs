using UnityEngine;
using TMPro;

public class SelectCardTypePopUpCloseButtonController : SpriteButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<SelectCardTypePopUpController> ().exitPopUp ();	
	}
	public override void setForbiddenState()
	{
		this.gameObject.transform.GetComponent<SpriteRenderer> ().color = ApplicationDesignRules.redColor;
	}
}

