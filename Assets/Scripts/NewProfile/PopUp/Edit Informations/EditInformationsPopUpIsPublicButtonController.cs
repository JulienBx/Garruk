public class EditInformationsPopUpIsPublicButtonController : SpriteButtonController
{
	public override void mainInstruction ()
	{
		gameObject.transform.parent.GetComponent<EditInformationsPopUpController> ().isPublicHandler();
	}
}
