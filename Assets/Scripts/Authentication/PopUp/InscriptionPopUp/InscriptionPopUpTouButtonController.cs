
public class InscriptionPopUpTouButtonController : SpriteButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<InscriptionPopUpController> ().touHandler();
	}
}
