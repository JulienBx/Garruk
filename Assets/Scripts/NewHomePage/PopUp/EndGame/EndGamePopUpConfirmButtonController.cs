public class EndGamePopUpConfirmButtonController : SimpleButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<EndGamePopUpController> ().exitPopUp();	
	}
}
