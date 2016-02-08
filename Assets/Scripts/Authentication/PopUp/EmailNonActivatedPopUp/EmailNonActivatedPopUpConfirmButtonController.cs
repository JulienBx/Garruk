public class EmailNonActivatedPopUpConfirmButtonController : SimpleButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<EmailNonActivatedPopUpController> ().emailNonActivatedHandler ();	
	}
}
