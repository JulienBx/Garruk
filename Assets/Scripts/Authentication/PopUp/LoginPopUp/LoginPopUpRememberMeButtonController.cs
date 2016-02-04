public class LoginPopUpRememberMeButtonController : SpriteButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<LoginPopUpController> ().rememberMeHandler();
	}
}
