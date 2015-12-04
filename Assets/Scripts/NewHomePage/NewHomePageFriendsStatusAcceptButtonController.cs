using UnityEngine;
using TMPro;

public class NewHomePageFriendsStatusAcceptButtonController : SimpleButtonController 
{
	public override void mainInstruction()
	{
		NewHomePageController.instance.acceptFriendsRequestHandler (base.getId());
	}
}

