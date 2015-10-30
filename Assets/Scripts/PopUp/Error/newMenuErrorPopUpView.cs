using UnityEngine;

public class newMenuErrorPopUpView : NewErrorPopUpView
{
	public override void hideErrorPopUp()
	{
		MenuController.instance.hideErrorPopUp();
	}
}


