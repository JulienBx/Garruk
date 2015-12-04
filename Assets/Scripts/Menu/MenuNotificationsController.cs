using UnityEngine;
using TMPro;

public class MenuNotificationsController : SpriteButtonController 
{
	public override void mainInstruction()
	{
		MenuController.instance.homePageLink ();
	}
	public override void setHoveredState()
	{
		this.gameObject.transform.GetComponent<SpriteRenderer> ().color = ApplicationDesignRules.blueColor;
		this.gameObject.transform.parent.FindChild ("Notifications").GetComponent<TextMeshPro> ().color = ApplicationDesignRules.blueColor;
	}
	public override void setInitialState()
	{
		this.gameObject.transform.GetComponent<SpriteRenderer> ().color = ApplicationDesignRules.redColor;
		this.gameObject.transform.parent.FindChild ("Notifications").GetComponent<TextMeshPro> ().color = ApplicationDesignRules.redColor;
	}
}

