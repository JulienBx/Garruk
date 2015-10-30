using UnityEngine;

public class MenuNotificationsController : SpriteButtonController 
{
	public override void OnMouseDown()
	{
		MenuController.instance.homePageLink ();
	}
}

