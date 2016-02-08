public class ExistingAccountPopUpLostLoginButtonController : TextButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<ExistingAccountPopUpController> ().lostLoginHandler();
	}
}
