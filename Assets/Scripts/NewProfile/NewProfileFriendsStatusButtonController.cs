using UnityEngine;
using TMPro;

public class NewProfileFriendsStatusButtonController : SimpleButtonController 
{
	private bool toAcceptInvitation;
	private bool toDeclineInvitation;
	private bool toCancelInvitation;

	public void setToAcceptInvitation()
	{
		this.toAcceptInvitation = true;
		this.toDeclineInvitation = false;
		this.toCancelInvitation = false;
	}
	public void setToDeclineInvitation()
	{
		this.toDeclineInvitation = true;
		this.toAcceptInvitation = false;
		this.toCancelInvitation = false;
	}
	public void setToCancelInvitation()
	{
		this.toCancelInvitation = true;
		this.toDeclineInvitation = false;
		this.toAcceptInvitation = false;
	}
	public override void mainInstruction()
	{
		if(toAcceptInvitation)
		{
			NewProfileController.instance.acceptFriendsRequestHandler(base.getId ());
		}
		else if(toDeclineInvitation)
		{
			NewProfileController.instance.declineFriendsRequestHandler(base.getId ());
		}
		else if(toCancelInvitation)
		{
			NewProfileController.instance.cancelFriendsRequestHandler(base.getId ());
		}
	}
}

