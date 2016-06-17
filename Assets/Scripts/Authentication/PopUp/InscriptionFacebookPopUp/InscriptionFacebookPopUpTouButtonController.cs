
public class InscriptionFacebookPopUpTouButtonController : SpriteButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<InscriptionFacebookPopUpController> ().touHandler();
	}
}
