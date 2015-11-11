using UnityEngine;
using TMPro;

public class NewStoreBuyPackButtonController : SimpleButtonController
{	
	public override void mainInstruction()
	{
		if(TutorialObjectController.instance.canAccess(1))
		{
			NewStoreController.instance.buyPackHandler (getId());
		}
	}
	public override void setIsActive(bool value)
	{
		base.setIsActive (value);
		if(value)
		{
			base.setInitialState();
		}
		else
		{
			base.setForbiddenState();
		}
	}
}