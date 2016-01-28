public class DisconnectPopUpConfirmButtonController : SimpleButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<DisconnectPopUpController> ().quitGameHandler ();
	}
}
