using UnityEngine;
using TMPro;

public class NewHomePageFriendsStatusDeclineButtonController : SimpleButtonController 
{
	public override void mainInstruction()
	{
		NewHomePageController.instance.declineFriendsRequestHandler (base.getId());
	}
}

