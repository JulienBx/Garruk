using UnityEngine;
using TMPro;

public class NextLevelPopUpControllerEndGame : NextLevelPopUpController
{
	public override void clickOnAttribute(int index, int newPower, int newLevel)
	{
		NewEndGameController.instance.upgradeCardAttributeHandler(index, newPower,newLevel);
	}
	public override void resize()
	{
		this.gameObject.transform.localScale = ApplicationDesignRules.nextLevelPopUpScale*ApplicationDesignRules.cardFocusedScale.y;
	}
}

