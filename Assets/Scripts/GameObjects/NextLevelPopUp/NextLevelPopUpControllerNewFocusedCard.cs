using UnityEngine;
using TMPro;

public class NextLevelPopUpControllerNewFocusedCard : NextLevelPopUpController
{
	public override void clickOnAttribute(int index, int newPower, int newLevel)
	{
		StartCoroutine(this.gameObject.transform.parent.GetComponent<NewFocusedCardController>().upgradeCardAttribute(index, newPower,newLevel));
	}
	public override void resize()
	{
		this.gameObject.transform.position = new Vector3(ApplicationDesignRules.menuPosition.x+this.gameObject.transform.parent.GetComponent<NewFocusedCardController>().returnFacePosition().x,ApplicationDesignRules.menuPosition.y-ApplicationDesignRules.sceneCameraFocusedCardPosition.y+ApplicationDesignRules.focusedCardPosition.y,-2f);
		this.gameObject.transform.localScale = ApplicationDesignRules.nextLevelPopUpScale;
	}
}

