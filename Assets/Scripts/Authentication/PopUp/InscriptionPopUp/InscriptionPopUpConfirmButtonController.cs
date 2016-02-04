public class InscriptionPopUpConfirmButtonController : SimpleButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<InscriptionPopUpController> ().inscriptionHandler ();	
	}
}
