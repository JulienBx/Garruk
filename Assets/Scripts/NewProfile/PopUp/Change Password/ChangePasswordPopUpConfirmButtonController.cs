public class ChangePasswordPopUpConfirmButtonController : SimpleButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<ChangePasswordPopUpController> ().checkPasswordHandler ();	
	}
}
