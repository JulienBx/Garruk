public class LoginPopUpConfirmButtonController : SimpleButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<LoginPopUpController> ().loginHandler();
	}
}
