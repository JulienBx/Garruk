using UnityEngine;

public class newMenuErrorPopUpView : NewErrorPopUpView
{
	public override void hideErrorPopUp()
	{
		newMenuController.instance.hideErrorPopUp();
	}
}


