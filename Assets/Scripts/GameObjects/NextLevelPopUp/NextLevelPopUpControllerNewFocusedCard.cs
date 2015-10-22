using UnityEngine;
using TMPro;

public class NextLevelPopUpControllerNewFocusedCard : NextLevelPopUpController
{
	public override void clickOnAttribute(int index, int newPower, int newLevel)
	{
		StartCoroutine(this.gameObject.transform.parent.GetComponent<NewFocusedCardController>().upgradeCardAttribute(index, newPower,newLevel));
	}
}

