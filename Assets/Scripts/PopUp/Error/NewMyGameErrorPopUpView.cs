using UnityEngine;

public class NewMyGameErrorPopUpView : NewErrorPopUpView
{
	public override void hideErrorPopUp()
	{
		newMyGameController.instance.hideErrorPopUp();
	}
}


