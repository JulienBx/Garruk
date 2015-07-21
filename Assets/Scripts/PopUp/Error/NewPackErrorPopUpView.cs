using UnityEngine;

public class NewPackErrorPopUpView : NewErrorPopUpView
{
	public override void hideErrorPopUp()
	{
		this.gameObject.GetComponent<NewPackController>().hideErrorPopUp();
	}
}


