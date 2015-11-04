using UnityEngine;
using TMPro;

public class NewProfileChallengeButtonController : SimpleButtonController 
{
	public override void mainInstruction()
	{
		NewProfileController.instance.sendInvitationHandler(base.getId());	
	}
}

