public class ConnectionBonusPopUpConfirmButtonController : SimpleButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<ConnectionBonusPopUpController> ().exitPopUp();	
	}
}
