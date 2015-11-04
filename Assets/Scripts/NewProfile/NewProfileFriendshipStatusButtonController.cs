using UnityEngine;
using TMPro;

public class NewProfileFriendshipStatusButtonController : SimpleButtonController 
{
	public override void mainInstruction()
	{
		NewProfileController.instance.friendshipStateHandler (base.getId());
	}
}

