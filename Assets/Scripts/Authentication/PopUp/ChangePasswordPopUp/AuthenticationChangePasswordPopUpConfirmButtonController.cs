public class AuthenticationChangePasswordPopUpConfirmButtonController : SimpleButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<AuthenticationChangePasswordPopUpController> ().changePasswordHandler ();	
	}
}
