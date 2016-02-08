public class ExistingAccountPopUpConfirmButtonController : SimpleButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<ExistingAccountPopUpController> ().existingAccountHandler ();	
	}
}
