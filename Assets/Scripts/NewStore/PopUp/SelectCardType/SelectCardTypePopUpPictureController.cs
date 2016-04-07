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
	public override void showToolTip ()
	{
		BackOfficeController.instance.displayToolTip(WordingCardTypes.getName(this.cardType),WordingCardTypes.getDescription(this.cardType));
	}
}

