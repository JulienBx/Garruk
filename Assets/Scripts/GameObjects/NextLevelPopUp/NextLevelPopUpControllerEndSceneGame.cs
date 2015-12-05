using UnityEngine;
using TMPro;

public class NextLevelPopUpControllerEndSceneGame : NextLevelPopUpController
{
	public override void clickOnAttribute(int index, int newPower, int newLevel)
	{
		StartCoroutine(EndSceneController.instance.upgradeCardAttribute(index, newPower,newLevel));
	}
	public override void resize()
	{
		this.gameObject.transform.localScale = ApplicationDesignRules.nextLevelPopUpScale*ApplicationDesignRules.cardFocusedScale.y;
	}
}

