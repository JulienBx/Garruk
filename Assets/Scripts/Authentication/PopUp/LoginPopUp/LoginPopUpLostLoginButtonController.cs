public class LoginPopUpLostLoginButtonController : TextButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<LoginPopUpController> ().lostLoginHandler();
	}
}
