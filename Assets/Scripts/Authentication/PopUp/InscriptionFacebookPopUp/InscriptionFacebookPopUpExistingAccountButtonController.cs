public class InscriptionFacebookPopUpExistingAccountButtonController : TextButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<InscriptionFacebookPopUpController> ().existingAccountHandler();
	}
}
