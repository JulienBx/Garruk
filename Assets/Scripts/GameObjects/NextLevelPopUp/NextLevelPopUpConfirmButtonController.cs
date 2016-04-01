using UnityEngine;
using TMPro;

public class NextLevelPopUpConfirmButtonController : SimpleButtonController
{
	public override void mainInstruction ()
	{
		this.gameObject.transform.parent.GetComponent<NextLevelPopUpController>().confirmButtonHandler();
	}
	
}

