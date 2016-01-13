using UnityEngine;
using TMPro;

public class SelectCardTypePopUpPictureController : SpriteButtonController
{
	public int cardType;

	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<SelectCardTypePopUpController> ().selectCardTypeHandler (this.cardType);	
	}
	public override void setForbiddenState()
	{
		gameObject.transform.GetComponent<SpriteRenderer> ().color = ApplicationDesignRules.redColor;
	}
}

