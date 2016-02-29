public class ProfileMessagePopUpConfirmButtonController : SimpleButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<ProfileMessagePopUpController> ().exitPopUp();
	}
}
