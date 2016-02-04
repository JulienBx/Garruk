public class LoginPopUpInscriptionButtonController : TextButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<LoginPopUpController> ().inscriptionHandler();
	}
}
