public class AuthenticationMessagePopUpConfirmButtonController : SimpleButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<AuthenticationMessagePopUpController> ().exitPopUp();
	}
}
