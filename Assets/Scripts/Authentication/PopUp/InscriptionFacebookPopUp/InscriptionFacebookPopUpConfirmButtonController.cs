public class InscriptionFacebookPopUpConfirmButtonController : SimpleButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<InscriptionFacebookPopUpController> ().inscriptionFacebookHandler ();	
	}
}
