public class NewCardTypePopUpConfirmButtonController : SimpleButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<NewCardTypePopUpController> ().exitPopUp();	
	}
}


