using UnityEngine;

public class NewFocusedCardErrorPopUpView : NewErrorPopUpView
{
	public override void hideErrorPopUp()
	{
		this.gameObject.GetComponent<NewFocusedCardController>().hideErrorPopUp();
	}
}


