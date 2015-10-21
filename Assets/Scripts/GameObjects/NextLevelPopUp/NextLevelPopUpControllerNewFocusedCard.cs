using UnityEngine;
using TMPro;

public class NextLevelPopUpControllerNewFocusedCard : NextLevelPopUpController
{
	public override void clickOnAttribute(int index)
	{
		StartCoroutine(this.gameObject.transform.parent.GetComponent<NewFocusedCardController>().upgradeCardAttribute(index));
	}
}

