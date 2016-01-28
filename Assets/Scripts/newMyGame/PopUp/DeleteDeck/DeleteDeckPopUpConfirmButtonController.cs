public class DeleteDeckPopUpConfirmButtonController : SimpleButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<DeleteDeckPopUpController> ().deleteDeckHandler ();	
	}
}
