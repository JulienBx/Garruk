public class WonPackPopUpConfirmButtonController : SimpleButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<WonPackPopUpController> ().exitPopUp();	
	}
}
