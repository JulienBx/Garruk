public class NewDeckPopUpConfirmButtonController : SimpleButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<NewDeckPopUpController> ().newDeckHandler ();	
	}
}
