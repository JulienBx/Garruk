using UnityEngine;
using TMPro;

public class NewProfileFriendsButtonController : SpriteButtonController
{	
	public override void mainInstruction()
	{
		NewProfileController.instance.slideLeft ();
	}
}