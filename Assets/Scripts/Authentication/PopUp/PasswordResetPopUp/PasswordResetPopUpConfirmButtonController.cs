public class PasswordResetPopUpConfirmButtonController : SimpleButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<PasswordResetPopUpController> ().exitPopUp();
	}
}
