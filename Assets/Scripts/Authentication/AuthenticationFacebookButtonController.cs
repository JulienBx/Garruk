public class AuthenticationFacebookButtonController : SimpleButtonController
{
	public override void mainInstruction ()
	{
		AuthenticationController.instance.facebookHandler();	
	}
}
