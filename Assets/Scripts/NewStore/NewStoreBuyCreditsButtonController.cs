using UnityEngine;
using TMPro;

public class NewStoreBuyCreditsButtonController : SimpleButtonController
{	
	public override void mainInstruction()
	{
		NewStoreController.instance.displayAddCreditsPopUp ();
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