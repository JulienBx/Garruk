public class EditDeckPopUpConfirmButtonController : SimpleButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<EditDeckPopUpController> ().editDeckHandler ();	
	}
}
