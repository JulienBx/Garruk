public class AccountCreatedPopUpConfirmButtonController : SimpleButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<AccountCreatedPopUpController> ().exitPopUp();
	}
}
