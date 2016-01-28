public class CheckPasswordPopUpConfirmButtonController : SimpleButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<CheckPasswordPopUpController> ().checkPasswordHandler ();	
	}
}
