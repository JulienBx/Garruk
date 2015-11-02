using UnityEngine;

public class MenuNotificationsController : SpriteButtonController 
{
	public override void mainInstruction()
	{
		MenuController.instance.homePageLink ();
	}
}

