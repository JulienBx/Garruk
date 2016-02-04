public class LostLoginPopUpConfirmButtonController : SimpleButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<LostLoginPopUpController> ().lostLoginHandler ();	
	}
}
