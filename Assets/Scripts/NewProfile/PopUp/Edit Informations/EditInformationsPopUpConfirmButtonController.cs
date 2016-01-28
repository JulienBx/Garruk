public class EditInformationsPopUpConfirmButtonController : SimpleButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<EditInformationsPopUpController> ().editInformationsHandler ();	
	}
}
