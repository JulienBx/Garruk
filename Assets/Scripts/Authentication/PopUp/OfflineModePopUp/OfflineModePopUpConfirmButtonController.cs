
public class OfflineModePopUpConfirmButtonController : SimpleButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<OfflineModePopUpController> ().exitPopUp();
	}
}
