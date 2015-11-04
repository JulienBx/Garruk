using UnityEngine;
using TMPro;

public class NewHomePageChallengeButtonController : SimpleButtonController 
{
	public override void mainInstruction()
	{
		NewHomePageController.instance.sendInvitationHandler(base.getId());	
	}
}

