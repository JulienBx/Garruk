using UnityEngine;
using TMPro;

public class NewHomePageSocialButtonController : SpriteButtonController
{	
	public override void mainInstruction()
	{
		NewHomePageController.instance.slideLeft ();
	}
}